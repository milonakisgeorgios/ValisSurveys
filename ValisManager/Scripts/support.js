/*
    CKEDITOR Configurations
*/
valis_RICH =
[
	{ name: 'document', items: ['Source'] },
	{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
	{ name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
	{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
    { name: 'styles', items: ['Font', 'FontSize'] }
];
valis_HTML =
[
	{ name: 'document', items: ['Source'] },
    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
	'/',
	{ name: 'links', items: ['Image','Link', 'Unlink', 'Anchor', 'HorizontalRule', 'SpecialChar'] },
	{ name: 'colors', items: ['TextColor', 'BGColor'] },
	{ name: 'tools', items: ['Maximize', 'ShowBlocks'] },
	{ name: 'styles', items: ['Font', 'FontSize'] }
];

function textEdit(id, mode, required, scId, clientId, surveyId)
{
    var config = {};

    config["language"] = 'en';
    config["entities"] = false;
    config["entities_greek"] = false;
    config["entities_latin"] = false;
    //config["skin"] = 'silver-master';
    config["uiColor"] = required ? '#272727' : '#999999';
    config["indentOffset"] = 2;
    config["customConfig"] = '';
    config["htmlEncodeOutput"] = true;
    config["removePlugins"] = 'elementspath';

    if (mode == 1 /*RichText = 1*/) {
        config["toolbar"] = valis_RICH;
        config["startupShowBorders"] = false;
        config["resize_enabled"] = false;
        config["toolbarStartupExpanded"] = false;
        config["enterMode"] = CKEDITOR.ENTER_BR;
        config["shiftEnterMode"] = CKEDITOR.ENTER_P;
        config["autoParagraph"] = false;
    }
    else if (mode == 2 /*HTMLText = 2*/) {

        config["toolbar"] = valis_HTML;
        config["resize_enabled"] = true;
        config["resize_minHeight"] = 300;
        config["resize_minWidth"] = 780;
        config["resize_maxHeight"] = 800;
        config["resize_maxWidth"] = 1100;
        config["toolbarStartupExpanded"] = true;
        config["enterMode"] = CKEDITOR.ENTER_BR;
        config["shiftEnterMode"] = CKEDITOR.ENTER_P;
        config["autoParagraph"] = true;
         
        config["filebrowserBrowseUrl"] = '/clay/fileman/surveyFiles.html?scId=' + scId + '&client=' + clientId + '&survey=' + surveyId;
        config["filebrowserUploadUrl"] = '/clay/fileman/surveyFiles.html?scId=' + scId + '&client=' + clientId + '&survey=' + surveyId;
        config["filebrowserImageBrowseUrl"] = '/clay/fileman/surveyFiles.html?scId=' + scId + '&client=' + clientId + '&survey=' + surveyId + '&type=Images';
        config["filebrowserImageUploadUrl"] = '/clay/fileman/surveyFiles.html?scId=' + scId + '&client=' + clientId + '&survey=' + surveyId + '&type=Images';
        config["filebrowserWindowWidth"] = 800;
        config["filebrowserWindowHeight"] = 480;
        //config["extraPlugins"] = 'filebrowser,popup';
    }

    config["on"] =
    {
        instanceReady: function (ev) {
            // The character sequence to use for every indentation step
            this.dataProcessor.writer.indentationChars = '\t';
            // The way to close self closing tags, like <br />.
            this.dataProcessor.writer.selfClosingEnd = ' />';
            // The character sequence to be used for line breaks.
            this.dataProcessor.writer.lineBreakChars = '\n';
            // Output paragraphs as <p>Text</p>.
            this.dataProcessor.writer.setRules('p',
                    {
                        indent: false,
                        // Inserts a line break before the <p> opening tag.
                        breakBeforeOpen: false,
                        // Inserts a line break after the <p> opening tag.
                        breakAfterOpen: false,
                        // Inserts a line break before the </p> closing tag.
                        breakBeforeClose: false,
                        // Inserts a line break after the </p> closing tag.
                        breakAfterClose: false
                    });
        }
    };

    CKEDITOR.replace(id, config);
}




/*
Κεντράρει ένα element στο παράθυρο του browser
*/
$.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", (($(window).height() - this.outerHeight()) / 2) + $(window).scrollTop() + "px");
    this.css("left", (($(window).width() - this.outerWidth()) / 2) + $(window).scrollLeft() + "px");
    return this;
}

$.fn.animateHighlight = function (highlightColor, duration) {
    var highlightBg = highlightColor || "#FFFF9C";
    var animateMs = duration || 1500;
    var originalBg = this.css("backgroundColor");
    this.stop().css("background-color", highlightBg).animate({ backgroundColor: originalBg }, animateMs);
};


/*
Επιτρέπει εισαγωγή δεκαδικών αριθμών.
Επιτρέπει την τελεία σαν διαχωριστικό των δεκαδικών ψηφίων
*/
$.fn.onlyUnSignedNumbers = function () {
    return this.each(function () {
        $(this).keydown(function (evt) {
            /*το κόμμα είναι το 188*/
            if (evt.keyCode == 190/*., keyboard*/ || evt.keyCode == 110/*., numeric*/) {
                var _value = $(evt.target).val();
                if (_value.indexOf(".") != -1)
                    evt.preventDefault();
                return;
            }
            if (evt.keyCode == 37 || evt.keyCode == 39)/*arrows*/ {
                return;
            }
            if (evt.keyCode == 8/*Backspace*/ || evt.keyCode == 9/*TAB*/ || evt.keyCode == 35/*End*/ || evt.keyCode == 36 /*Home*/ || evt.keyCode == 46/*Delete*/) {
                return;
            }
            //number from keypad
            if (evt.keyCode <= 105 && evt.keyCode >= 96) {
                return;
            }
            //number from keyboard
            if (evt.keyCode <= 57 && evt.keyCode >= 48) {
                return;
            }
            //Allow: Ctrl+A
            if (evt.keyCode == 65 && evt.ctrlKey === true)
                return;
            evt.preventDefault();
        });
    });
}
/*
Επιτρέπει εισαγωγή δεκαδικών αριθμών
Επιτρέπει την τελεία σαν διαχωριστικό των δεκαδικών ψηφίων
*/
$.fn.onlySignedNumbers = function () {
    return this.each(function () {
        $(this).keydown(function (evt) {
            /*το κόμμα είναι το 188*/
            if (evt.keyCode == 190/*., keyboard*/ || evt.keyCode == 110/*., numeric*/) {
                var _value = $(evt.target).val();
                if (_value.indexOf(".") != -1)
                    evt.preventDefault();
                return;
            }
            if (evt.keyCode == 173 || evt.keyCode == 189/*-,keyboard*/ || evt.keyCode == 109)/*-, numeric*/ {
                var _value = $(evt.target).val();
                if (_value.length > 0)
                    evt.preventDefault();
                if (_value.indexOf("-") != -1)
                    evt.preventDefault();
                return;
            }
            if (evt.keyCode == 37 || evt.keyCode == 39)/*arrows*/ {
                return;
            }
            if (evt.keyCode == 8/*Backspace*/ || evt.keyCode == 9/*TAB*/ || evt.keyCode == 35/*End*/ || evt.keyCode == 36 /*Home*/ || evt.keyCode == 46/*Delete*/) {
                return;
            }
            //number from keypad
            if (evt.keyCode <= 105 && evt.keyCode >= 96) {
                return;
            }
            //number from keyboard
            if (evt.keyCode <= 57 && evt.keyCode >= 48) {
                return;
            }
            //Allow: Ctrl+A
            if (evt.keyCode == 65 && evt.ctrlKey === true)
                return;
            evt.preventDefault();
        });
    });
}

/*
Επιτρέπει εισαγωγή ακέραιων αριθμών
*/
$.fn.onlyUnSignedIntegers = function () {
    return this.each(function () {
        $(this).keydown(function (evt) {

            if (evt.keyCode == 37 || evt.keyCode == 39)/*arrows*/ {
                return;
            }
            if (evt.keyCode == 8/*Backspace*/ || evt.keyCode == 9/*TAB*/ || evt.keyCode == 35/*End*/ || evt.keyCode == 36 /*Home*/ || evt.keyCode == 46/*Delete*/) {
                return;
            }
            //number from keypad
            if (evt.keyCode <= 105 && evt.keyCode >= 96) {
                return;
            }
            //number from keyboard
            if (evt.keyCode <= 57 && evt.keyCode >= 48) {
                return;
            }
            //Allow: Ctrl+A
            if (evt.keyCode == 65 && evt.ctrlKey === true)
                return;
            evt.preventDefault();
        });
    });
}
/*
Επιτρέπει εισαγωγή ακέραιων αριθμών
*/
$.fn.onlySignedIntegers = function () {
    return this.each(function () {
        $(this).keydown(function (evt) {

            if (evt.keyCode == 173 || evt.keyCode == 189/*-,keyboard*/ || evt.keyCode == 109)/*-, numeric*/ {
                var _value = $(evt.target).val();
                if (_value.length > 0)
                    evt.preventDefault();
                if (_value.indexOf("-") != -1)
                    evt.preventDefault();
                return;
            }
            if (evt.keyCode == 37 || evt.keyCode == 39)/*arrows*/ {
                return;
            }
            if (evt.keyCode == 8/*Backspace*/ || evt.keyCode == 9/*TAB*/ || evt.keyCode == 35/*End*/ || evt.keyCode == 36 /*Home*/ || evt.keyCode == 46/*Delete*/) {
                return;
            }
            //number from keypad
            if (evt.keyCode <= 105 && evt.keyCode >= 96) {
                return;
            }
            //number from keyboard
            if (evt.keyCode <= 57 && evt.keyCode >= 48) {
                return;
            }
            //Allow: Ctrl+A
            if (evt.keyCode == 65 && evt.ctrlKey === true)
                return;
            evt.preventDefault();
        });
    });
}

/*
Περιορίζει το πλήθος των χαρακτήρων
*/
$.fn.maxLength = function (maxChars) {
    return this.each(function () {
        $(this).keydown(function (evt) {
            if (evt.keyCode == 46 || evt.keyCode == 8) {
                return;
            }
            var _value = $(this).val();
            if (_value.length < maxChars)
                return;
            evt.preventDefault();
        });
    });
}


function validateEmail(email)
{
    if (email == undefined || email == null || email == '')
    {
        return false;
    }
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    return re.test(email);
}



/*
αντιστοιχεί στον πίνακα Languages
*/
function GetLanguageName(languageId) {
    if (languageId == 0 || languageId == '0')
        return 'Invariant Language';
    if (languageId == 19 || languageId == '19')
        return 'Bulgarian';
    if (languageId == 33 || languageId == '33')
        return 'English';
    if (languageId == 38 || languageId == '38')
        return 'French';
    if (languageId == 42 || languageId == '42')
        return 'German';
    if (languageId == 43 || languageId == '43')
        return 'Greek';
    if (languageId == 101 || languageId == '101')
        return 'Russian';
    return '???';
}
function GetLanguageIcon(languageId) {
    if (languageId == 0 || languageId == '0')
        return 'flags/flag0000.gif';
    if (languageId == 19 || languageId == '19')
        return 'flags/flag0402.gif';
    if (languageId == 33 || languageId == '33')
        return 'flags/flag0809.gif';
    if (languageId == 38 || languageId == '38')
        return 'flags/flag040c.gif';
    if (languageId == 42 || languageId == '42')
        return 'flags/flag0407.gif';
    if (languageId == 43 || languageId == '43')
        return 'flags/flag0408.gif';
    if (languageId == 101 || languageId == '101')
        return 'flags/flag0419.gif';
    return 'flags/flag0000.gif';
}


/*
    Περνάει στο pageURL το τρέχων state του οριζόμενου JqGrid

*/
function AddJqGridParamsToURLSuffix(gridID, pageUrl) {
    var sortname = $('#' + gridID).getGridParam('sortname');
    var sortorder = $('#' + gridID).getGridParam('sortorder');
    var page = $('#' + gridID).getGridParam('page');
    var rowNum = $('#' + gridID).getGridParam('rowNum');

    var data = "pageno=" + page + "&sortname=" + sortname + "&sortorder=" + sortorder + "&rowNum=" + rowNum;
    if (pageUrl.indexOf("?") != -1) {
        pageUrl = pageUrl + '&' + data;
    }
    else {
        pageUrl = pageUrl + '?' + data;
    }

    return pageUrl;
}
function OnJqGridLoadError(gridId, _XMLHttpRequest, textStatus, errorThrown) {
    //$(gridId).GridUnload();
    var msg = '<tr><td><div class="jqGridError">';
    msg += '<div id="jqGridErrorStatus"><span class="errorIcon">status: (' + _XMLHttpRequest.status + ') ' + _XMLHttpRequest.statusText + '</span></div>';
    msg += '<div id="jqGridErrorResponse"' + _XMLHttpRequest.responseText + '</div>';
    msg += '</div></td></tr>';
    $(gridId).html(msg);
}
function ReloadGrid(gridId) {
    $(gridId).jqGrid('setGridParam', { page: 1 }).trigger("reloadGrid");
}
function ReloadGridAndKeepPage(gridId) {
    var _pageNo = $(gridId).getGridParam('page');
    $(gridId).jqGrid('setGridParam', { page: _pageNo }).trigger("reloadGrid");
}

var builtinFormatter = function (cellvalue, options, rowObject) {
    if (cellvalue == true) {
        return "<img border=\"0\" src=\"" + theManagerPath + "content/images/isBuiltin.gif\" alt=\"isBuiltin\"/>";
    }
    return '';
};
var checkRedFormatter = function (cellvalue, options, rowObject) {
    if (cellvalue == true) {
        return "<img border=\"0\" src=\"" + theManagerPath + "content/images/chk_red.gif\" alt=\"On\"/>";
    }
    
    return "<img border=\"0\" src=\"" + theManagerPath + "content/images/chk_off.gif\" alt=\"Off\"/>";
};
var checkGreenFormatter = function (cellvalue, options, rowObject) {
    if (cellvalue == true) {
        return "<img border=\"0\" src=\"" + theManagerPath + "content/images/chk_green.gif\" alt=\"On\"/>";
    }

    return "<img border=\"0\" src=\"" + theManagerPath + "content/images/chk_off.gif\" alt=\"Off\"/>";
};
var isActiveFormatter = function (cellvalue, options, rowObject) {
    if (cellvalue == true) {
        return "<img border=\"0\" src=\"" + theManagerPath + "content/images/isActive.gif\" alt=\"isActive\"/>";
    }
    return '';
};



function ShowFuzz() {
    $("#fuzz").css("height", $(document).height());
    $('#fuzz_indicator').center();
    $("#fuzz").show();
}
function HideFuzz() {
    $("#fuzz").hide();
}

var _globalFuzzTimer01 = null;

$(document).ready(function ()
{

    $(document).ajaxSend(function (evt, jqXHR, ajaxOptions) {
        console.log('ajaxSend');
        var _showGlobalAjaxFuzz = ajaxOptions.showGlobalAjaxFuzz != null ? ajaxOptions.showGlobalAjaxFuzz : false;
        if (_showGlobalAjaxFuzz) {
            if (_globalFuzzTimer01 == null) {
                _globalFuzzTimer01 = setTimeout(ShowFuzz(), 400, null);
            }
        }
    });

    $(document).ajaxStop(function (evt, request, settings) {
        console.log('ajaxStop');
        if (_globalFuzzTimer01 != null) {
            clearTimeout(_globalFuzzTimer01);
            _globalFuzzTimer01 = null;
            HideFuzz();
        }
    });

    $(document).ajaxError(function (evt, jqXHR, ajaxOptions, thrownError) {
        var _showGlobalAjaxError = ajaxOptions.showGlobalAjaxError != null ? ajaxOptions.showGlobalAjaxError : true;
        if (_showGlobalAjaxError && showAjaxError) {
            showAjaxError(evt, jqXHR, ajaxOptions, thrownError);
        }
    });


});