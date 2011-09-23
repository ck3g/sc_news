/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function(config) {
    // Define changes to default configuration here. For example:
    config.language = 'ru';
    //config.uiColor = '#035A85';
    config.height = 500;

    config.toolbar =
    [
        ['Source'],
        ['Bold', 'Italic', 'Underline', 'Strike'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Image', 'Flash', 'Table', 'HorizontalRule']
    ];
};
