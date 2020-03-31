<?php

use Cake\Http\Middleware\CsrfProtectionMiddleware;
use Cake\Routing\Route\DashedRoute;
use Cake\Routing\RouteBuilder;

$routes->setRouteClass(DashedRoute::class);

$routes->scope('/', function (RouteBuilder $builder) {

    $builder->registerMiddleware('csrf', new CsrfProtectionMiddleware([
        'httpOnly' => true,
    ]));

    // $builder->applyMiddleware('csrf');
    // $builder->setExtensions(['json']);

    // $builder->resources('Image');

    $builder->connect('avatar/save-avatar', ['controller' => 'Avatar', 'action' => 'saveAvatar']);
    $builder->connect('avatar/replace-avatar', ['controller' => 'Avatar', 'action' => 'replaceAvatar']);
    $builder->connect('avatar/remove-avatar', ['controller' => 'Avatar', 'action' => 'removeAvatar']);

    $builder->connect('photo/save-photos', ['controller' => 'Photo', 'action' => 'savePhotos']);
    $builder->connect('photo/replace-photos', ['controller' => 'Photo', 'action' => 'replacePhotos']);
    $builder->connect('photo/remove-photos', ['controller' => 'Photo', 'action' => 'removePhotos']);

    $builder->fallbacks();
});
