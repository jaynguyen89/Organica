<?php
declare(strict_types=1);

namespace App\Controller;

class AvatarController extends AppController {

    private const IMAGE_SIZE = 3000000; //3MB

    public function saveAvatar() {
        //$this->autoRender = false;
        //$this->request->allowMethod(['post']);

        $image = array();
        if (array_key_exists('image', $_FILES))
            $image = $_FILES['image'];

        $response = $this->response;
        $response = $response->withType('application/json');

        $message = array();
        if (!empty($image))
            $message = $this->checkImageExif($image, self::IMAGE_SIZE);
        if (!empty($message)) {
            $response = $response->withStringBody(json_encode($message));
            return $response;
        }

        $hidrogenianId = array_key_exists('hidrogenianId', $_REQUEST) ? $_REQUEST['hidrogenianId'] : null;

        if (!empty($image) && $hidrogenianId != null) {
            $message = $this->saveImageToDisk($image);
            if (!empty($message) && array_key_exists('error', $message)) {
                $response = $response->withStringBody(json_encode($message));
                return $response;
            }

            $avatar_newName = $message['imageName'];
            if ($image['type'] != 'image/gif')
                $this->reduceImageSize(WWW_ROOT . 'files' . DS . 'avatars' . DS . $avatar_newName);

            $this->persistImageData($avatar_newName, $hidrogenianId, WWW_ROOT.'files'.DS.'avatars'.DS, true);
            $message = [
                'error' => false,
                'message' => $avatar_newName
            ];
        }
        else
            $message = [
                'error' => true,
                'message' => 'Unable to process your request due to missing data.'
            ];

        $response = $response->withStringBody(json_encode($message));
        //return $response;
        $this->set(compact('image', 'message'));
    }


    public function replaceAvatar() {
        //$this->autoRender = false;
        //$this->request->allowMethod(['post']);

        $currentAvatar = array_key_exists('current', $_REQUEST) ? $_REQUEST['current'] : null;
        $hidrogenianId = array_key_exists('hidrogenianId', $_REQUEST) ? $_REQUEST['hidrogenianId'] : null;
        $newAvatar = array_key_exists('replaceBy', $_FILES) ? $_FILES['replaceBy'] : null;

        $response = $this->response;
        $response = $response->withType('application/json');

        if ($currentAvatar != null && $newAvatar != null && $hidrogenianId != null) {
            $message = $this->removeImageData($currentAvatar);
            if (!empty($message)) {
                $response = $response->withStringBody(json_encode($message));
                return $response;
            }

            $message = $this->checkImageExif($newAvatar, self::IMAGE_SIZE);
            if (!empty($message)) {
                $response = $response->withStringBody(json_encode($message));
                return $response;
            }

            $message = $this->saveImageToDisk($newAvatar);
            if (!empty($message) && array_key_exists('error', $message)) {
                $response = $response->withStringBody(json_encode($message));
                return $response;
            }

            $avatar_newName = $message['imageName'];
            if ($newAvatar['type'] != 'image/gif')
                $this->reduceImageSize(WWW_ROOT.'files'.DS.'avatars'.DS.$avatar_newName);

            $this->persistImageData($avatar_newName, $hidrogenianId, WWW_ROOT.'files'.DS.'avatars'.DS, true);
            $message = [
                'error' => false,
                'message' => $avatar_newName
            ];
        }
        else
            $message = [
                'error' => true,
                'message' => 'Unable to process your request due to missing data.'
            ];

        $response = $response->withStringBody(json_encode($message));
        //return $response;
        $this->set(compact('currentAvatar', 'newAvatar', 'message'));
    }


    public function removeAvatar() {
        //$this->autoRender = false;
        //$this->request->allowMethod(['delete']);

        $imageName = array_key_exists('image', $_REQUEST) ? $_REQUEST['image'] : null;

        $response = $this->response;
        $response = $response->withType('application/json');

        if ($imageName != null) {
            $message = $this->removeImageData($imageName, true);
            if (!empty($message)) {
                $response = $response->withStringBody(json_encode($message));
                return $response;
            }

            $message = ['error' => false];
        }
        else
            $message = [
                'error' => true,
                'message' => 'No data to process your request.'
            ];

        $response = $response->withStringBody(json_encode($message));
        //return $response;
        $this->set(compact('message', 'imageName'));
    }
}
