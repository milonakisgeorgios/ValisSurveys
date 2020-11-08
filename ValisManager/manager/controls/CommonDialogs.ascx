<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CommonDialogs.ascx.cs" Inherits="ValisManager.manager.controls.CommonDialogs" %>
<script type="text/javascript">
    var _errorBand_timeout = null;
    var _infoBand_timeout = null;

    function showErrorBand(message) {
        if (_errorBand_timeout != null) {
            clearTimeout(_errorBand_timeout);
            $('div#showErrorBand').remove();
            _errorBand_timeout = null;
        }
        $('div#header').prepend('<div id=\'showErrorBand\'>' + message + '</div>');
        _errorBand_timeout = setTimeout(function () {
            $('div#showErrorBand').fadeOut('slow', function () { $(this).remove(); });
        }, 3400);
    }
    function showInfoBand(message) {
        if (_infoBand_timeout != null) {
            clearTimeout(_infoBand_timeout);
            $('div#showInfoBand').remove();
            _infoBand_timeout = null;
        }
        $('div#header').prepend('<div id=\'showInfoBand\'>' + message + '</div>');
        _infoBand_timeout = setTimeout(function () {
            $('div#showInfoBand').fadeOut('slow', function () { $(this).remove(); });
        }, 2500);
    }

    function showInfo(htmlMessage, title) {
        if (title == null)
            $("#infoDialog").dialog("option", "title", '<span class="commonDialogTitle">Surveys Manager Information:<span>');
        else
            $("#infoDialog").dialog("option", "title", '<span class="commonDialogTitle">' + title + '<span>');

        $('#infoDialog_Message').html(htmlMessage);
        $('#infoDialog').dialog("open");
    }
    function showWarning(htmlMessage, title) {
        if (title == null)
            $("#warningDialog").dialog("option", "title", '<span class="commonDialogTitle">Surveys Manager Warning:<span>');
        else
            $("#warningDialog").dialog("option", "title", '<span class="commonDialogTitle">' + title + '<span>');

        $('#warningDialog_Message').html(htmlMessage);
        $('#warningDialog').dialog("open");
    }
    function showError(htmlMessage, title) {
        if (title == null)
            $("#errorDialog").dialog("option", "title", '<span class="commonDialogTitle">Surveys Manager Error:<span>');
        else
            $("#errorDialog").dialog("option", "title", '<span class="commonDialogTitle">' + title + '<span>');

        $('#errorDialog_Message').html(htmlMessage);
        $('#errorDialog').dialog("open");
    }
    function showQuestion(htmlMessage, callback, title) {
        if (title == null)
            $("#confirmDialog").dialog("option", "title", '<span class="commonDialogTitle">Surveys Manager Question:<span>');
        else
            $("#confirmDialog").dialog("option", "title", '<span class="commonDialogTitle">' + title + '<span>');

        $("#confirmDialog").dialog("option", "buttons", { 'Yes': { text: '<%=Resources.Global.CommonDialogs_Btn_YES %>', id: 'cnfrmDlgOkBtn', click: function () { callback(); $(this).dialog("close"); } }, 'No': { text: '<%=Resources.Global.CommonDialogs_Btn_NO %>', id: 'formCancelButton', click: function () { $(this).dialog("close"); } } });

        $('#confirmDialog_Message').html(htmlMessage);
        $('#confirmDialog').dialog("open");
    }
    function showDelete(htmlMessage, callback, title) {
        if (title == null)
            $("#deleteDialog").dialog("option", "title", '<span class="commonDialogTitle">Surveys Manager Delete:<span>');
        else
            $("#deleteDialog").dialog("option", "title", '<span class="commonDialogTitle">' + title + '<span>');

        $("#deleteDialog").dialog("option", "buttons", { 'Yes': { class: 'redbutton', text: '<%=Resources.Global.CommonDialogs_Btn_YES %>', id: 'cnfrmDlgOkBtn', click: function () { callback(); $(this).dialog("close"); } }, 'No': { text: '<%=Resources.Global.CommonDialogs_Btn_NO %>', id: 'formCancelButton', click: function () { $(this).dialog("close"); } } });

        $('#deleteDialog_Message').html(htmlMessage);
        $('#deleteDialog').dialog("open");
    }

    function showAjaxError(evt, jqXHR, ajaxSettings, thrownError) {
        $('#status').text(jqXHR.status);
        $('#statusText').text(jqXHR.statusText);
        $('#responseText').html(jqXHR.responseText);

        $("#exceptionDialog").dialog("option", "title", '<span class="commonDialogTitle">Ajax Exception:<span>');
        $('#exceptionDialog').dialog("open");
    }
    function showException(jqXHR, textStatus, errorThrown) {
        alert('showException');
        $('#status').text(jqXHR.status);
        $('#statusText').text(jqXHR.statusText);
        $('#responseText').html(jqXHR.responseText);

        $("#exceptionDialog").dialog("option", "title", '<span class="commonDialogTitle">System Exception:<span>');
        $('#exceptionDialog').dialog("open");
    }

    $(document).ready(function () 
    {
        $("#exceptionDialog").dialog({ dialogClass: 'errorDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 500, height: 260, buttons: { 'OK': { text: '<%=Resources.Global.CommonDialogs_Btn_OK %>', id: 'excDlgOkBtn', click: function () { $(this).dialog("close"); } } } });
        $("#infoDialog").dialog({ dialogClass: 'infoDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 440, height: 220, buttons: { 'OK': { text: '<%=Resources.Global.CommonDialogs_Btn_OK %>', id: 'gnrlDlgOkBtn', click: function () { $(this).dialog("close"); } } } });
        $("#warningDialog").dialog({ dialogClass: 'warningDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 440, height: 220, buttons: { 'OK': { text: '<%=Resources.Global.CommonDialogs_Btn_OK %>', id: 'gnrlDlgOkBtn', click: function () { $(this).dialog("close"); } } } });
        $("#errorDialog").dialog({ dialogClass: 'errorDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 440, height: 220, buttons: { 'OK': { text: '<%=Resources.Global.CommonDialogs_Btn_OK %>', id: 'gnrlDlgOkBtn', click: function () { $(this).dialog("close"); } } } });
        $("#confirmDialog").dialog({ dialogClass: 'questionDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 400, height: 200 });
        $("#deleteDialog").dialog({ dialogClass: 'deleteDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 400, height: 200 });

        //We need to override the undocumented _title function, according to Bug #6016 to ensure that the title attribute is not escaped. 
        //var dialog = $("#exceptionDialog").dialog();
        //dialog.data("ui.dialog")._title = function (title) {
        //    title.html(this.options.title);
        //};
        //dialog = $("#infoDialog").dialog();
        //dialog.data("ui.dialog")._title = function (title) {
        //    title.html(this.options.title);
        //};
        //dialog = $("#confirmDialog").dialog();
        //dialog.data("ui.dialog")._title = function (title) {
        //    title.html(this.options.title);
        //};
        /*
            NOTE: Αλλαξα των κωδικα μέσα στο jquery-2.1.0.min.js, jquery-2.1.0.js
        */
    });
</script>
<div id="exceptionDialog" style="display:none;z-index: 10002;">
    <span id="statusLine">status: (<span id="status"></span>) <span id="statusText"></span></span>
    <div id="responseText"></div>
</div>
<div id="infoDialog" style="display:none;z-index: 10002;">
    <span id="infoDialog_Message"></span>
</div>
<div id="warningDialog" style="display:none;z-index: 10002;">
    <span id="warningDialog_Message"></span>
</div>
<div id="errorDialog" style="display:none;z-index: 10002;">
    <span id="errorDialog_Message"></span>
</div>
<div id="deleteDialog" style="display:none;z-index: 10002;">
    <span id="deleteDialog_Message"></span>
</div>
<div id="confirmDialog" style="display:none;font-size: 11pt;z-index: 10002;">
    <span id="confirmDialog_Message"></span>
</div>
<div id="fuzz" class="ui-widget-overlay"><img alt="fuzz_indicator" id="fuzz_indicator" src="<%=IndicatorImage %>" /></div> 