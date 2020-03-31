<?php
declare(strict_types=1);
namespace App\Controller;

use Cake\Controller\Controller;
use Cake\ORM\TableRegistry;
use Exception;

class AppController extends Controller {

    public function initialize(): void {
        parent::initialize();

        $this->loadComponent('RequestHandler');
        $this->loadComponent('Flash');
    }

    /**
     * Turns the array of uploaded images'info into an array of images.
     * @param $tempImages
     * @return array
     */
    public function reprocessMultipleImagesTempData($tempImages) {
        $images = array();
        $imageCount = count($tempImages['name']);

        for ($i = 0; $i < $imageCount; $i++) {
            $anImage = array();
            $anImage['name'] = $tempImages['name'][$i];
            $anImage['type'] = $tempImages['type'][$i];
            $anImage['tmp_name'] = $tempImages['tmp_name'][$i];
            $anImage['error'] = $tempImages['error'][$i];
            $anImage['size'] = $tempImages['size'][$i];

            array_push($images, $anImage);
        }

        return $images;
    }

    /**
     * Check image type and size. For gif, limit size to a fixed 3MB.
     * @param $image
     * @param $size
     * @return array
     */
    public function checkImageExif($image, $size) {
        $allowTypes = ['image/jpg', 'image/jpeg', 'image/png', 'image/gif'];
        $message = array();

        if (!in_array($image['type'], $allowTypes))
            $message = [
                'error' => true,
                'message' => 'The photo is not of expected type. Expected: JPG, JPEG, PNG, GIF.'
            ];

        if ($image['type'] == 'image/gif') {
            if ($image['size'] > 3000000)
                $message = [
                    'error' => true,
                    'message' => 'The GIF photo is too big. Max size allowed: 3MB.'
                ];
        }
        else {
            if ($image['size'] > $size)
                $message = [
                    'error' => true,
                    'message' => 'The photo is too big. Max size allowed: ' . $size . 'MB.'
                ];
        }

        return $message;
    }

    /**
     * Compress images (except GIF) by 50% size
     * @param $imagePath
     * @return bool
     */
    public function reduceImageSize($imagePath) {
        $type = getimagesize($imagePath);
        $image = null;

        if ($type['mime'] == 'image/jpg' || $type['mime'] == 'image/jpeg')
            $image = imagecreatefromjpeg($imagePath);

        if ($type['mime'] == 'image/png')
            $image = imagecreatefrompng($imagePath);

        return imagejpeg($image, $imagePath, 50);
    }

    /**
     * Save the uploaded image from temp folder to disk
     * @param $avatar
     * @return array
     */
    public function saveImageToDisk($avatar) {
        $image_newName = '';
        try {
            $imageName = strtolower($avatar['name']);
            $tokens = explode('.', $imageName);
            $imageExtension = end($tokens);

            $image_newName = md5($imageName) . '_' . time() . '.' . $imageExtension;
            move_uploaded_file($avatar['tmp_name'], WWW_ROOT . 'files' . DS . 'avatars' . DS . $image_newName);
            chmod(WWW_ROOT . 'files' . DS . 'avatars' . DS . $image_newName, 0755);
        } catch (Exception $e) {
            $message = [
                'error' => true,
                'message' => 'An error occurred while attempting to save new avatar. Please try again.'
            ];
        }

        $message = !empty($message) ? $message : ['imageName' => $image_newName];
        return $message;
    }

    /**
     * Insert database entries for Photos and Userphotos tables
     * @param $avatarName
     * @param $userId
     * @param bool $isAvatar
     * @param bool $isCover
     */
    public function persistImageData($avatarName, $userId, $isAvatar = false, $isCover = false) {
        $dbPhoto = TableRegistry::getTableLocator()->get('Photos')->newEmptyEntity();
        $dbPhoto->PhotoName = $avatarName;
        TableRegistry::get('photos')->save($dbPhoto);

        $dbUserPhoto = TableRegistry::getTableLocator()->get('Userphotos')->newEmptyEntity();
        $dbUserPhoto->PhotoId = $dbPhoto->Id;
        $dbUserPhoto->HidrogenianId = intval($userId);
        $dbUserPhoto->IsAvatar = $isAvatar;
        $dbUserPhoto->IsCover = $isCover;
        TableRegistry::getTableLocator()->get('Userphotos')->save($dbUserPhoto);
    }

    /**
     * Remove database entry associated with the image, then delete image on disk
     * @param $avatarName
     * @return array
     */
    public function removeImageData($avatarName) {
        $message = array();
        try {
            $currentDbAvatar = TableRegistry::getTableLocator()->get('Photos')->find()->where(['PhotoName' => $avatarName])->first();
            TableRegistry::getTableLocator()->get('Photos')->delete($currentDbAvatar);

            unlink(WWW_ROOT.'files'.DS.'avatars'.DS.$avatarName);
        } catch (Exception $e) {
            $message = [
                'error' => true,
                'message' => 'An error occurred while attempting to replace your avatar. Please try again.'
            ];
        }

        return $message;
    }
}
