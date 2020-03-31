<?php
declare(strict_types=1);

namespace App\Controller;

use Cake\Datasource\ConnectionManager;
use Cake\Http\Response;
use Cake\Filesystem\Folder;
use Cake\Filesystem\File;
use Cake\I18n\Time;
use Cake\ORM\TableRegistry;

class PhotoController extends AppController {

    private const PHOTO_SIZE = 10000000; //10MB

    public function savePhotos() {
        //$this->autoRender = false;
        //$this->request->allowMethod(['post']);

        $hidrogenianId = array_key_exists('hidrogenianId', $_REQUEST) ? $_REQUEST['hidrogenianId'] : null;
        $coverImage = array_key_exists('coverImage', $_REQUEST) ? $_REQUEST['coverImage'] : null;
        $images = array_key_exists('images', $_FILES) ? $_FILES['images'] : null;

        if ($images != null && $hidrogenianId != null && $coverImage != null) {
            $images = $this->reprocessMultipleImagesTempData($images);

            $dbImageNames = array();
            $oversizedImages = array();
            $failedImages = array();

            foreach ($images as $image) {
                $message = $this->checkImageExif($image, self::PHOTO_SIZE);
                if (!empty($message)) {
                    array_push($oversizedImages, $image['name']);
                    continue;
                }

                $message = $this->saveImageToDisk($image);
                if (!empty($message) && array_key_exists('error', $message)) {
                    array_push($failedImages, $image['name']);
                    continue;
                }

                $photo_newName = $message['imageName'];
                if ($image['type'] != 'image/gif')
                    $this->reduceImageSize(WWW_ROOT . 'files' . DS . 'avatars' . DS . $photo_newName);

                $this->persistImageData($photo_newName, $hidrogenianId, false, $coverImage == $image['name']);
                array_push($dbImageNames, $photo_newName);
            }

            $message = [
                'error' => !(!empty($dbImageNames) && empty($oversizedImages) && empty($failedImages)),
                'imageNames' => $dbImageNames,
                'fails' => $failedImages,
                'oversizes' => $oversizedImages
            ];
        }
        else
            $message = [
                'error' => true,
                'message' => 'Unable to process request due to missing data.'
            ];

        $response = $this->response;
        $response = $response->withType('application/json');
        $response = $response->withStringBody(json_encode($message));
        //return $response;
        $this->set(compact('images', 'message'));
    }

    public function removePhotos() {
        //$this->autoRender = false;
        //$this->request->allowMethod(['delete']);

        $hidrogenianId = array_key_exists('hidrogenianId', $_REQUEST) ? $_REQUEST['hidrogenianId'] : null;
        $removals = array_key_exists('removals', $_REQUEST) ? $_REQUEST['removals'] : null;

        $response = $this->response;
        $response = $response->withType('application/json');

        $message = array();
        $failedRemovals = array();
        $unknownRemovals = array();

        if ($hidrogenianId != null && $removals != null) {
            $dbConnection = ConnectionManager::get('default');

            foreach ($removals as $removal) {
                $counter = $dbConnection->execute('
                SELECT COUNT(*) AS PCount
                FROM Photos AS p, Userphotos AS u
                WHERE p.Id == u.PhotoId
                    AND u.HidrogenianId == ?
                    AND p.PhotoName == ?
                ', [$hidrogenianId, $removal])->fetch('assoc');

                if ($counter['PCount'] == 1) {
                    $message = $this->removeImageData($removal);
                    if (!empty($message)) array_push($failedRemovals, $removal);
                }
                else array_push($unknownRemovals, $removal);
            }
        }
        else
            $message = [
                'error' => true,
                'message' => 'Unable to process request due to missing data.'
            ];

        $message = (!empty($message)) ? $message : [
            'error' => false,
            'fails' => $failedRemovals,
            'unknowns' => $unknownRemovals
        ];
        $response = $response->withStringBody(json_encode($message));
        //return $response;
        $this->set(compact('removals', 'message'));
    }
}
