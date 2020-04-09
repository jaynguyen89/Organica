<?php
/*
 * Local configuration file to provide any overrides to your app.php configuration.
 * Copy and save this file as app_local.php and make changes as required.
 * Note: It is not recommended to commit files with credentials such as app_local.php
 * into source code version control.
 */
return [
    'debug' => filter_var(env('DEBUG', true), FILTER_VALIDATE_BOOLEAN),
    'Security' => [
        'salt' => env('SECURITY_SALT', '7e94f38cd2ccd07998cd57f0f77e13c6b7cc51be75c2599a2e57d5410280072c'),
    ],
    'Datasources' => [
        'default' => [
            'host' => '69.90.66.140',
            'port' => 3306,
            'username' => 'jayde421_uwater',
            'password' => 'ThanhPhuc311211!',
            'database' => 'jayde421_water',
            'url' => env('DATABASE_URL', null),
        ],
        'test' => [
            'host' => '69.90.66.140',
            'username' => 'jayde421_uwater',
            'password' => 'ThanhPhuc311211!',
            'database' => 'water'
        ],
    ],
    'EmailTransport' => [
        'default' => [
            'host' => 'localhost',
            'port' => 25,
            'username' => null,
            'password' => null,
            'client' => null,
            'url' => env('EMAIL_TRANSPORT_DEFAULT_URL', null),
        ],
    ],
];
