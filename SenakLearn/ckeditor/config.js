/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.contentsLangDirection = 'rtl';
    config.language = 'fa';
    config.filebrowserImageUploadUrl = window.location.origin +'/CKEDITORConfig/UploadImage';
    //config.filebrowserBrowseUrl= '/browser/browse.php';
    //config.filebrowserImageBrowseUrl= '/browser/browse.php?type=Images';
    //config.filebrowserUploadUrl= '/uploader/upload.php';
    //config.filebrowserImageUploadUrl= '/uploader/upload.php?type=Images';

    //config.toolbar = 'MyToolbar';

    //config.toolbar_MyToolbar =
	//[
	//	{ name: 'document', items: ['NewPage', 'Preview'] },
	//	{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
	//	{ name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
	//	{
	//	    name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'
    //               , 'Iframe']
	//	},
    //            '/',
	//	{ name: 'styles', items: ['Styles', 'Format'] },
	//	{ name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat'] },
	//	{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
	//	{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	//	{ name: 'tools', items: ['Maximize', '-', 'About'] }
	//];
};
