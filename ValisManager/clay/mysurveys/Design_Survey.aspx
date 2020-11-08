<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="Design_Survey.aspx.cs" Inherits="ValisManager.clay.mysurveys.Design_Survey" %>

<%@ Register Assembly="Valis.Core.HtmlRenderers" Namespace="Valis.Core.Html.WebControls" TagPrefix="valis" %>

<%@ Register src="../controls/EditSurveyTabs.ascx" tagname="EditSurveyTabs" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/scripts/jquery.form.min.js"></script>
    <link href="/scripts/ms-Dropdown-master/dd.css" rel="stylesheet" />
    <script src="/scripts/ms-Dropdown-master/jquery.dd.min.js"></script>
    <link href="/content/theme00.css" rel="stylesheet" />
    <style type="text/css">

        div.pageTitle {
            background-image: url(/content/images/survey.png);
            background-position: 0px 4px;
            background-repeat: no-repeat;
            padding-left: 36px;
        }
        a.previewLink
        {
            background: url("/content/images/btnMain3.gif") repeat-x scroll 0 0 #777777;
            font-size: 14px;
            font-weight: normal;

            padding: 0px 6px 0px 6px;
            cursor: pointer;
            float: right;
        }
        /*#region surveyPage
        -------------------------------------------*/
        div.pageTools
        {
            margin: 0px;
        }
        div.surveyPage
        {
            margin: 0px 0px 0px 0px;
            font-size: 1em;
            padding: 24px 0px 32px 12px;
            /*border: 0px solid red;*/
            height: 48px;
            background-color: #f1f1f1;
        }
        div.surveyPage select
        {
            font-size: 1em;
            margin: 0px 24px 0px 12px;
            min-width: 28em;
        }
        div.surveyPage a.pageButtons
        {
            font-size: 12px;
            min-width: 80px;
        }
        /*#endregion*/


        /*#region surveyQuestions
        -------------------------------------------*/
        div.surveyQuestions
        {
            background-color: #f1f1f1;
            padding: 12px;
            text-align: center;
            /*border: 1px dashed #c7c7c7;*/
        }
        div.questionBox
        {
            text-align: left;
            margin: 12px;
            border: 1px dashed #999999;
            background-color: #f1f1f1;
            min-height: 100px;
            position: relative;
        }
        
        div.questionHeader
        {
            padding-top: 12px;
            font-size: 1.1em;
            line-height: 1.4em;
        }
        img.branchedMarker
        {
            float: left;
            margin-left: 8px;
        }

        div.questionHeaderTools 
        {
            position: absolute;
            top: -10px;
            right: 0px;
        }

        div.questionbuttons 
        {
            display: inline-block;
            border: 0px solid green;
            clear: both;
            height: 22px;
            text-align: center;
            margin: 0px auto 0px auto;
            width: 252px;
        }
        div.surveyQuestions a.questionAddLink, div.surveyQuestions a.libraryQuestionAddLink
        {
            float: left;
            background: url("/content/images/btnMain2.gif") repeat-x scroll 0 0 #b6c55d;
            font-size: 12px;
            font-weight: normal;
            line-height: 1.8em;
            color: #1a1a1a;
            text-decoration: none;
            text-align: center;
            vertical-align: middle;
            padding: 0px 8px 0px 8px;
            border-radius: 3px;
            cursor: pointer;
            margin: 0px 8px;
        }
        div.surveyQuestions a.libraryQuestionAddLink 
        {    
            background: url("/content/images/btnMain2a.gif") repeat-x scroll 0 0 #b6c55d;
        }
        div.surveyQuestions a.questionEditLink, div.surveyQuestions a.questionMoveLink , div.surveyQuestions a.questionLogicLink 
        {
            background: url("/content/images/btnMain3b.gif") repeat-x scroll 0 0 #959495;
        }
        div.surveyQuestions a.questionDeleteLink 
        {
            background: url("/content/images/btnMain4b.gif") repeat-x scroll 0 0 #d87272;
        }
        div.surveyQuestions a.questionEditLink,div.surveyQuestions a.questionDeleteLink,div.surveyQuestions a.questionMoveLink,div.surveyQuestions a.questionLogicLink
        {
            font-size: 11px;
            font-weight: normal;
            line-height: 1.72em;
            color: #F1F1F1;
            min-width: 50px;
        }
        div.questionEditButton, div.questionDeleteButton, div.questionMoveButton, div.questionLogicButton
        {
            float: right; margin-right: 8px;
        }
        /*#endregion*/


        
        
        
        /*#region TheQuestionForm
        -------------------------------------------*/
        input#pgShowTitle
        {
            width: 24em;
        }
        div#AddPageForm #pgDescription
        {
            width: 24em;
            height: 4.4em;
        }
        /*#endregion*/

        
        
        /*#region TheQuestionForm
        -------------------------------------------*/
        div.theQuestionForm .formWrapper
        {
            margin: 0;
            padding: 2px;
        }
        div.theQuestionForm .formRow
        {
            margin: 4px;
            padding: 2px;
        }
        div.theQuestionForm label
        {
            display: inline-block;
            width: auto;
            text-align: left;
            margin-right: 8px;
        }
        
        div.theQuestionForm input[type=checkbox]
        {
            margin: 6px;
        }
        div.QuestionTypeAreaWrapper
        {
            background-color: #c7c7c7;
            width: 540px;
            margin: 4px;
            padding: 2px;
        }
        div#QuestionTypeAttributesPlaceHolder {
            padding: 4px 0px 16px 16px;
            color: #0077b5;
        }

        div#QuestionTypeAttributesPlaceHolder textarea
        {
            font-size: 0.78em;
            line-height: 1.5em;
            font-family: Verdana, sans-serif;
        }




        textarea#QuestionText, textarea#QuestionDescription
        {
            width: 540px;
        }

        div.requiredMessageWrapper
        {
            padding: 6px 0px 8px 12px;
        }
        div.requiredMessageWrapper label {
            font-size: .8em;
            color: #0077b5;
        }
        div.requiredMessageWrapper textarea {
             width: 540px;
        }
        /*#endregion*/

        
        /*#region DeletePageForm
        -------------------------------------------*/
        #DeletePageForm h1{
            font-size: 1.3em;
            margin-bottom: 18px;
            font-weight: bold;
        }
        #DeletePageForm h2{
            font-size: 1.2em;
            margin-bottom: 8px;
            font-weight: bold;
        }
        #DeletePageForm div.warning{
            color: red;
            font-size: 0.86em;
            margin: 4px 0px 0px 24px;
            font-weight: bold;
        }
        /*#endregion*/

        /*#region DeletePageForm
        -------------------------------------------*/
        #ThePagelogicForm h2{
            font-size: 1.2em;
            margin-bottom: 8px;
            font-weight: bold;
        }
        #ThePagelogicForm select#skipPage
        {
            font-size: 1em;
            min-width: 350px;
        }
        /*#endregion*/

        
        /*#region DeletePageForm
        -------------------------------------------*/
        #TheQuestionlogicForm div.panel{
            padding: 4px;
            margin: 8px 0px 12px 0px;
        }
        #TheQuestionlogicForm div.panel h2{
            font-size: 1.2em;
            margin-bottom: 8px;
            font-weight: bold;
            background-color: #c7c7c7;
        }
        #TheQuestionlogicForm p#lg_QuestionText
        {
            font-size: 1.2em;
        }
        #TheQuestionlogicForm p#lg_help
        {
            font-size: 1.1em;
            padding: 6px 0px 8px 0px;
        }

        #TheQuestionlogicForm table { border-collapse: collapse; border: 1px solid #777; width: 100%;}
        #TheQuestionlogicForm th, #TheQuestionlogicForm td { padding: .65em; }
        #TheQuestionlogicForm th, #TheQuestionlogicForm thead { background: #000; color: #fff; border: 1px solid #000; }
        #TheQuestionlogicForm tr:nth-child(odd) { background: #ccc; } 
        #TheQuestionlogicForm td { border-right: 1px solid #777; }
        #TheQuestionlogicForm td.col1{
            width: 380px;
        }#TheQuestionlogicForm td.col2{
             width: 450px;
        }#TheQuestionlogicForm td.col3{
             width: 70px;
             vertical-align: central;
        }
        #TheQuestionlogicForm select {
            margin: 4px; min-width: 400px;
        }
        /*#endregion*/

        .pageForm .ui-dialog-titlebar
        {
            background-image: url(../../content/images/btnMain.gif);   
        }
        .fixed-dialog{
          position: fixed;
          top: 50px;
          left: 50px;
        }



        div.qLibrary {
            width: 100%;
            overflow-y: auto;
            overflow-x:hidden;
            background: none repeat scroll 0 0 #9e9b90;
        }
        ul.qLibrary-ul{
            list-style: outside none none;
            margin: 0;
            padding: 0 2.75% 45px 4.5%;
        }
        li.qLibrary-li{
            margin: 0 0 3px;
        }
        div.qLibrary-p{
            background: none repeat scroll 0 0 #d4d4ce;
            color: #777;
            padding: 9px 32px 12px 8px;
            border-radius: 3px;
            box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.25);
            color: #444;
            cursor: pointer;
            display: block;
            font-size: 12px;
            font-weight: bold;
            line-height: 20px;
        }
        div.qLibrary-p:hover {
            background: none repeat scroll 0 0 #d0d38b;
        }
    </style>
    <script>
        $(document).ready(function () {
            $("#pagesSelector").msDropDown();

            $('#AddPageForm').dialog({ dialogClass: 'pageForm fixed-dialog', autoOpen: false, modal: true, resizable: false, width: 500, height: 309, buttons: { 'Save': { class: 'greenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Save %>', id: 'formSaveButton', click: OnFormOkButton }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'saveCancelBtn', click: function () { $(this).dialog("close"); } } } });
            $('#AddPageForm').find('#pgShowTitle').maxLength(128);
            $('#AddPageForm').find('#pgDescription').maxLength(2048);

            $('#TheQuestionForm').dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, resizable: false, draggable: false, width: 600, buttons: { 'Create': { class: 'lightgreenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Create %>', id: 'AddQuestionButton', click: OnCreateOrUpdateQuestionBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelAddQuestionButton', click: function () { $(this).dialog("close"); } } } });
            
            $('#QuestionType').bind('change', OnQuestionTypeChange);
            $('#QuestionType').on('keyup', function (event)
            {
                if (event.which == $.ui.keyCode.UP || event.which == $.ui.keyCode.DOWN)
                {
                    $('#QuestionType').trigger("change");
                }
            });
            $('#IsRequired').bind('change', function () {
                if($(this).is(':checked'))
                {
                    $('#requiredMessageWrapper').show();
                }
                else
                {
                    $('#requiredMessageWrapper').hide();
                }
            });


            $("#DeletePageForm").dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 500, height: 280, buttons: { 'OK': { 'class': 'redbutton', text: 'Yes, Delete Page', id: 'DeletePageButton', click: OnButtonDeletePage }, 'Cancel': { text: 'No, Cancel', id: 'CancelDeletePageButton', click: function () { $(this).dialog("close"); } } } });

            $('#ThePagelogicForm').dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, draggable: false, width: 500, buttons: { 'SaveLogic': { class: 'lightgreenbutton', text: 'Save Logic', id: 'SavePageLogicBtn', click: OnSavePageLogicBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelPageLogicBtn', click: function () { $(this).dialog("close"); } } } });
            $('#TheQuestionlogicForm').dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, draggable: false, width: 900, buttons: { 'SaveLogic': { class: 'lightgreenbutton', text: 'Save Logic', id: 'SaveQuestionLogicBtn', click: OnSaveQuestionLogicBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelQuestionLogicBtn', click: function () { $(this).dialog("close"); } } } });


            $('#TheLibraryQuestionForm').dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, resizable: false, draggable: false, width: 440, height: 500, buttons: { 'Close': { text: '<%=Resources.Global.CommonDialogs_Btn_Close %>', id: 'CloseAddLibraryQuestionButton', click: function () { $(this).dialog("close"); } } } });
            $('div.qLibrary-p').bind('click', OnLibraryQuestionClick);
        });

        //#region SurveyPage
        function OnDeletePage() 
        {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyPages/GetDeleteOptions?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId + '&textsLanguage=' + selectedTextsLanguage,
                dataType: 'json',
                success: function (data)
                {
                    if (data.CanBeDeleted == true)
                    {
                        if (data.HasQuestions)
                        {
                            if (data.HasPreviousPage) $('#DeleteQuestionsBehavior_1_Wrapper').show(); else $('#DeleteQuestionsBehavior_1_Wrapper').hide();
                            if (data.HasNextPage) $('#DeleteQuestionsBehavior_2_Wrapper').show(); else $('#DeleteQuestionsBehavior_2_Wrapper').hide();
                            if (data.SurveyHasResponses) $('#responsesWarning').show(); else $('#responsesWarning').hide();
                            $('#DeleteQuestionsBehavior_0').prop('checked', true);


                            $('#DeletePageForm').removeAttr('HasNextPage');
                            $('#DeletePageForm').removeAttr('NextPageId');
                            $('#DeletePageForm').removeAttr('HasPreviousPage');
                            $('#DeletePageForm').removeAttr('PreviousPageId');
                            $('#DeletePageForm').attr('HasNextPage', data.HasNextPage);
                            $('#DeletePageForm').attr('NextPageId', data.NextPageId);
                            $('#DeletePageForm').attr('HasPreviousPage', data.HasPreviousPage);
                            $('#DeletePageForm').attr('PreviousPageId', data.PreviousPageId);
                            $('#DeletePageForm').dialog("open");
                        }
                        else
                        {
                            showDelete('Are you sure you want to delete the page "' + data.ShowTitle + '"?', function () {

                                $.ajax({
                                    url: theManagerPath + 'services/api/SurveyPages/Delete?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId, dataType: 'html',
                                    success: function (data) {
                                        window.location.href = theManagerPath + "clay/mysurveys/Design_Survey.aspx?surveyid=" + selectedSurveyId + "&language=" + selectedTextsLanguage;
                                    }
                                });
                            }, 'Delete the page.');
                        }
                    }
                    else
                    {
                        showWarning('<div style="background-color: orange; color: #fff; padding: 6px; font-size: 1.2em;"><B>Warning</B> You cannot delete this Page', 'Delete the page</div>');
                        return;
                    }

                }
            });

        }
        function OnButtonDeletePage()
        {
            var NextPageId = $('#DeletePageForm').attr('NextPageId');
            var PreviousPageId = $('#DeletePageForm').attr('PreviousPageId');
            var questionsDeleteBehavior = /*DeleteQuestionsBehavior.DeleteAll*/0;


            if ($('#DeleteQuestionsBehavior_0').is(':checked'))
                questionsDeleteBehavior = /*DeleteQuestionsBehavior.DeleteAll*/0;
            else if ($('#DeleteQuestionsBehavior_1').is(':checked'))
                questionsDeleteBehavior = /*DeleteQuestionsBehavior.MoveAbove*/1;
            else if ($('#DeleteQuestionsBehavior_2').is(':checked'))
                questionsDeleteBehavior = /*DeleteQuestionsBehavior.MoveBellow*/2;

            $.ajax({
                url: theManagerPath + 'services/api/SurveyPages/Delete?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId + '&questionsDeleteBehavior=' + questionsDeleteBehavior, dataType: 'html',
                success: function (data) {
                    $('#DeletePageForm').dialog('close');
                    if (questionsDeleteBehavior == /*DeleteQuestionsBehavior.MoveAbove*/1)
                        window.location.href = theManagerPath + "clay/mysurveys/Design_Survey.aspx?surveyid=" + selectedSurveyId + '&pageId=' + PreviousPageId + "&language=" + selectedTextsLanguage;
                    else if (questionsDeleteBehavior == /*DeleteQuestionsBehavior.MoveBellow*/2)
                        window.location.href = theManagerPath + "clay/mysurveys/Design_Survey.aspx?surveyid=" + selectedSurveyId + '&pageId=' + NextPageId + "&language=" + selectedTextsLanguage;
                    else
                        window.location.href = theManagerPath + "clay/mysurveys/Design_Survey.aspx?surveyid=" + selectedSurveyId + "&language=" + selectedTextsLanguage;
                }
            });
        }

        function OnAddPage() {
            OpenForm(true, null);
        }
        function OnEditPage() {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyPages/GetById?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId + '&textsLanguage=' + selectedTextsLanguage,
                dataType: 'json',
                success: function (data) {
                    OpenForm(false, data);
                }
            });
        }
        function ResetForm() {
            $('#AddPageForm').clearForm();
            $('.formFooter').html('');
            $('#pgpPageId').val('');
            $('#PositionWrapper').show();
        }
        function OpenForm(createNew, data)
        {
            ResetForm();
            $('#AddPageForm').removeAttr('createNew');

            if (createNew == false) {
                $('#PositionWrapper').hide();
                $('#pgpPageId').val(data.PageId);
                $('#pgpTextsLanguage').val(data.TextsLanguage);
                $('#pgShowTitle').val(data.ShowTitle);
                $('#pgDescription').val(data.Description);

                $('.formFooter').html('Created at ' + data.CreateDT + '. Updated at ' + data.LastUpdateDT + '.');
                $("#AddPageForm").dialog("option", "title", 'Update SurveyPage #' + data.DisplayOrder + ':');
            }
            else
            {
                $("#AddPageForm").dialog("option", "title", 'Create a new page:');
                $('#pgPosition').val('0');
                $('#pgpTextsLanguage').val(selectedTextsLanguage);
            }

            $('#AddPageForm').dialog({ position: { my: "center", at: "center", of: window } }).attr("createNew", createNew).dialog("open");
        }
        function OnFormOkButton()
        {
            var createNew = $('#AddPageForm').attr("createNew");

            var showTitle = $('#pgShowTitle').val();
            if (showTitle == '' || showTitle == null || showTitle == undefined) {
                alert('You must provide a ShowTitle for this Page!');
                return;
            }

            var _data = 'ShowTitle=' + escape($('#pgShowTitle').val());
            _data = _data + '&Description=' + escape($('#pgDescription').val());
            _data = _data + '&Position=' + escape($('#pgPosition').val());
            _data = _data + '&ReferingPage=' + selectedSurveyPageId;
            _data = _data + '&SurveyId=' + selectedSurveyId;
            _data = _data + '&PageId=' + escape($('#pgpPageId').val());
            _data = _data + '&TextsLanguage=' + escape($('#pgpTextsLanguage').val());

            var _url = theManagerPath + 'services/api/SurveyPages/' + (createNew == "true" ? 'Create' : 'Update') + '?ScId=' + theAccessToken;



            $.ajax({
                url: _url, dataType: 'json', data: _data, async: false,
                success: function (data, textStatus, jqXHR) {
                    $('#AddPageForm').dialog('close');
                    window.location.href = theManagerPath + "clay/mysurveys/Design_Survey.aspx?surveyid=" + data.Survey + '&pageId=' + data.PageId + "&language=" + data.TextsLanguage;
                }
            });
        }
        //#endregion

        function OnAddPageLogic()
        {
            OpenPageLogicForm(null);
        } 
        function OnEditPageLogic()
        {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyPages/GetById?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId + '&textsLanguage=' + selectedTextsLanguage,
                dataType: 'json',
                success: function (data)
                {    
                    OpenPageLogicForm(data);
                }
            });
        }
        function OpenPageLogicForm(bPage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyPages/GetCandidateSkipToPages?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId + '&textsLanguage=' + selectedTextsLanguage,
                dataType: 'json',
                success: function (data)
                {
                    if (data.length == 0)
                    {
                        alert('There are not enough pages!');
                        return;
                    }
                    
                    var _value = 0;
                    if (bPage != null && bPage.HasSkipLogic)
                    {
                        if (bPage.SkipTo == 1)//AnotherPage
                            _value = bPage.SkipToPage;
                        else if (bPage.SkipTo == /*EndSurvey*/2)
                            _value = -1;
                        else if (bPage.SkipTo == /*GoodbyePage*/3)
                            _value = -2;
                        else if (bPage.SkipTo == /*DisqualificationPage*/4)
                            _value = -3;
                    }

                    var _options = '';
                    if(_value == 0)
                        _options = '<option selected>--select a page to jump to--</option>';
                    else
                        _options = '<option>--select a page to jump to--</option>';
                    for(var i=0; i< data.length; i++)
                    {
                        if (_value == data[i].PageId)
                            _options += '<option value=\'' + data[i].PageId + '\' selected>' + data[i].OptionTitle + '</option>';
                        else
                            _options += '<option value=\'' + data[i].PageId + '\'>' + data[i].OptionTitle + '</option>';
                    }
                    $('#skipPage').html(_options);

                    $('#ThePagelogicForm').dialog({ position: { my: "bottom", at: "center", of: window } }).attr("createNew", false).dialog("open");
                }
            });

        }
        function OnSavePageLogicBtn()
        {
            var selOption = $('#skipPage option:selected').get(0);
            if (selOption != null && selOption != undefined) {
                var skipToPage = $('#skipPage').val();
                if (skipToPage == null || skipToPage == undefined || skipToPage == '')
                    return;

                var _url = theManagerPath + 'services/api/SurveyPages/SetSkipLogic?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + selectedSurveyPageId + '&textsLanguage=' + selectedTextsLanguage + '&skipToPage=' + skipToPage;

                $.ajax({
                    url: _url, type: 'GET', async: false,
                    success: function (data) {
                        $('#ThePagelogicForm').dialog('close');
                        window.location = "Design_Survey.aspx?surveyid=<%=this.Surveyid %>&language=<%=this.TextsLanguage%>&pageId=<%=this.SurveyPageId%>";
                    }
                });
            }
        }

        function ResetQuestionForm()
        {
            $('#TheQuestionForm').find('*[name]').clearFields();

            $('#TheQuestionForm').removeAttr('createNew');
            $('#TheQuestionForm').removeAttr('surveyId');
            $('#TheQuestionForm').removeAttr('questionId');
            $('#TheQuestionForm').removeAttr('pageId');
            $('#TheQuestionForm').removeAttr('position');
            $('#TheQuestionForm').removeAttr('referingQuestionId');
            $('#TheQuestionForm').removeAttr('textsLanguage');

            $("#QuestionType").prop("disabled", false);

            $('#requiredMessageWrapper').hide();
            $('#requiredMessage').val('This question requires an answer.');
        }
        function OnQuestionAdd(surveyId, pageId, position, referingQuestionId, textsLanguage)
        {
            ResetQuestionForm();
            $('#QuestionType').val('SingleLine');
            LoadQuestionTypePartialHtml('SingleLine', null);

            $('#TheQuestionForm').attr("surveyId", surveyId);
            $('#TheQuestionForm').attr("pageId", pageId);
            $('#TheQuestionForm').attr("position", position);
            $('#TheQuestionForm').attr("referingQuestionId", referingQuestionId);
            $('#TheQuestionForm').attr("textsLanguage", textsLanguage);

            /*we open the TheQuestionForm:*/
            $('#AddQuestionButton').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Create %>');
            $("#TheQuestionForm").dialog("option", "title", 'Add Question:').dialog({ position: { my: "top", at: "top", of: window } }).attr("createNew", true).dialog("open");
        }
        function OnQuestionEdit(surveyId, questionId, textsLanguage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyQuestions/GetByIdForEdit?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&questionId=' + questionId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (selectedQuestion)
                {
                    ResetQuestionForm();
                    $('#QuestionType').val(selectedQuestion.QuestionType);
                    $("#QuestionType").prop("disabled", true);
                    LoadQuestionTypePartialHtml(selectedQuestion.QuestionType, selectedQuestion);

                    $('#TheQuestionForm').attr("surveyId", selectedQuestion.Survey);
                    $('#TheQuestionForm').attr("pageId", selectedQuestion.Page);
                    $('#TheQuestionForm').attr("questionId", selectedQuestion.QuestionId);
                    $('#TheQuestionForm').attr("textsLanguage", selectedQuestion.TextsLanguage);

                    $('#QuestionText').val(selectedQuestion.QuestionText);
                    $('#QuestionDescription').val(selectedQuestion.Description);
                    if (selectedQuestion.IsRequired)
                    {
                        $('#IsRequired').prop('checked', true);
                        $('#requiredMessage').val(selectedQuestion.RequiredMessage);
                        $('#requiredMessageWrapper').show();
                    }

                    /*we open the TheQuestionForm:*/
                    $('#AddQuestionButton').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Update %>');
                    $("#TheQuestionForm").dialog("option", "title", 'Edit Question:').dialog({ position: { my: "top", at: "top", of: window } }).attr("createNew", false).dialog("open");
                }
            });
        }
        function OnQuestionTypeChange(evt)
        {
            //$('#QuestionTypeAttributesPlaceHolder').html('');
            initializeFieldAttributes = function () { };
            var selOption = $('#QuestionType option:selected').get(0);
            if (selOption != null && selOption != undefined)
            {
                var questionType = $('#QuestionType').val();
                if (questionType == null || questionType == undefined || questionType == '')
                    return;

                LoadQuestionTypePartialHtml(questionType, null);
            }
        }
        function LoadQuestionTypePartialHtml(questionType, data)
        {

                if (questionType == /*QuestionType*/ 'DescriptiveText')
                {
                    <%-- Αφήνουμε μόνο ένα textarea ανοιχτό στο οποίο βάζουμε τον τίτλο 'Descriptive Text' και το μεγαλώνουμε:--%>
                    $('#QuestionDescriptionLbl').hide();
                    $('#QuestionDescription').hide();
                    $('#requiredPanel').hide();
                    $('#QuestionTextLbl').html('Descriptive Text');
                    $('#QuestionText').attr('rows', '8');
                }
                else
                {
                    <%-- Επαναφέρουμε τα controls της φόρμας σην default κατάσταση--%>
                    $('#QuestionDescriptionLbl').show();
                    $('#QuestionDescription').show();
                    $('#requiredPanel').show();
                    $('#QuestionText').attr('rows', '2');
                    $('#QuestionTextLbl').html('QuestionText');
                }



            var _url = theManagerPath + 'clay/mysurveys/QuestionHelpers/' + questionType + 'Helper.aspx?SurveyId=<%=this.Surveyid %>&PageId=<%=this.SurveyPageId%>&Language=<%=this.TextsLanguage%>';

            //if (questionType == 'XXXXXXXX' || questionType == 'YYYYYYY')
            //{
                /*Προσοχή εδώ: 
                Ο Browser cachαρει τις html αποκρίσεις. Αυτό δεν μας πειράζει, αλλά μερικά controls θέλουμε να κάνουν πάντα
                refresh απο τον Server. Για αυτά τα controls αλλάζουμε συνεχώς το requested url, προσθέτοντας την ώρα!*/
                //_url += '&t=' + (new Date()).getTime();
            //}
            $.ajax({
                url: _url, dataType: 'html', showGlobalAjaxFuzz: false, async: false,
                success: function (partialHtml)
                {
                    $('#QuestionTypeAttributesPlaceHolder').html(partialHtml);
                    initializeFieldAttributes(data);
                }
            });
        }
        function OnCreateOrUpdateQuestionBtn()
        {
            var value = $('#QuestionText').val();
            if (value == '' || value == null || value == undefined) {
                $("#QuestionText").animateHighlight("#fd2525", 400);
                $('#QuestionText').focus();
                return;
            }
            if ($('#IsRequired').is(':checked') == true)
            {
                value = $('#requiredMessage').val();
                if (value == '' || value == null || value == undefined) {
                    $("#requiredMessage").animateHighlight("#fd2525", 400);
                    $('#requiredMessage').focus();
                    return;
                }
            }
            if (!validateFieldAttributes())
                return;


            var createNew = $('#TheQuestionForm').attr("createNew");

            var _data = 'questionText=' + escape($('#QuestionText').val());
            _data = _data + '&description=' + escape($('#QuestionDescription').val());
            _data = _data + '&questionType=' + escape($('#QuestionType').val());
            _data = _data + getFieldAttributesData();
            _data = _data + '&isRequired=' + ($('#IsRequired').is(':checked') == true ? 'on' : 'off');
            _data = _data + '&requiredMessage=' + escape($('#requiredMessage').val());
            _data = _data + '&surveyId=' + $('#TheQuestionForm').attr("surveyId");
            _data = _data + '&pageId=' + $('#TheQuestionForm').attr("pageId");
            _data = _data + '&questionId=' + $('#TheQuestionForm').attr("questionId");
            _data = _data + '&position=' + $('#TheQuestionForm').attr("position");
            _data = _data + '&referingQuestionId=' + $('#TheQuestionForm').attr("referingQuestionId");
            _data = _data + '&textsLanguage=' + $('#TheQuestionForm').attr("textsLanguage");

            var _url = theManagerPath + 'services/api/SurveyQuestions/' + (createNew == "true" ? 'Create' : 'Update') + '?ScId=' + theAccessToken;

            $.ajax({
                url: _url, data: _data, type: 'POST', async: false,
                success: function (data) {
                    $('#TheQuestionForm').dialog('close');
                        window.location = "Design_Survey.aspx?surveyid=<%=this.Surveyid %>&language=<%=this.TextsLanguage%>&pageId=<%=this.SurveyPageId%>";
                }
            });
        }


        function OnQuestionDelete(surveyId, questionId, textsLanguage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyQuestions/GetDeleteOptions?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&questionId=' + questionId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (data)
                {
                    if (data.CanBeDeleted == true)
                    {
                        var message = '';
                        if (data.SurveyHasResponses)
                        {
                            message = '<div style=""><B>Warning</B> Deletion of this question will permanently remove any and all responses for this question! </div> <br/><p>Are you sure you want to delete this question?</p>';
                        }
                        else
                        {
                            message = 'Are you sure you want to delete the question ' + data.QuestionText + '?';
                        }

                        showDelete(message, function () {

                            $.ajax({
                                url: theManagerPath + 'services/api/SurveyQuestions/Delete?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&questionId=' + questionId, dataType: 'html',
                                success: function (data) {
                                    window.location = "Design_Survey.aspx?surveyid=<%=this.Surveyid %>&language=<%=this.TextsLanguage%>&pageId=<%=this.SurveyPageId%>";
                                }
                            });
                        }, 'Delete question confirmation.');

                    }
                    else
                    {
                        showWarning('<div style="background-color: orange; color: #fff; padding: 6px; font-size: 1.2em;"><B>Warning</B> You cannot delete this Question', 'Delete question</div>');
                        return;
                    }
                }
            });
        }
        function OnQuestionMove(surveyId, questionId) {
            alert('OnQuestionMove( surveyid=' + surveyId + ', questionId=' + questionId);
        }

        function OnQuestionLogicBtn(surveyId, questionId, textsLanguage, isEdit)
        {
            $.ajax({
                url: theManagerPath + 'services/api/SurveyQuestions/GetByIdForSkipLogic?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&questionId=' + questionId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (selectedQuestion)
                {
                    OpenQuestionlogicForm(selectedQuestion, isEdit);
                }
            });
        }
        function OpenQuestionlogicForm(selectedQuestion, isEdit)
        {
            /*τα Options της σελίδας είναι μέσα στο selectedQuestion:*/
            var options = selectedQuestion.Options;

            //Τώρα θέλουμε τις πιθανές σελίδες για skipping:
            $.ajax({
                url: theManagerPath + 'services/api/SurveyQuestions/GetCandidateSkipToPages?ScId=' + theAccessToken + '&surveyId=' + selectedQuestion.Survey + '&questionId=' + selectedQuestion.QuestionId + '&textsLanguage=' + selectedQuestion.TextsLanguage, dataType: 'json',
                success: function (candidatePages)
                {
                    var _tbody = '';
                    for (var j = 0; j < options.length; j++)
                    {
                        var _value = options[j].SkipToPage;
                        if (options[j].SkipTo == /*EndSurvey*/2)
                            _value = -1;
                        else if (options[j].SkipTo == /*GoodbyePage*/3)
                            _value = -2;
                        else if (options[j].SkipTo == /*DisqualificationPage*/4)
                            _value = -3;

                        _tbody += '<tr><td class="col1">' + options[j].OptionText + '</td>';

                        _tbody += '<td class="col2">';
                        _tbody += '<div><select class="opSkipPage" optionid="' + options[j].OptionId + '" id="opSkipPage_' + options[j].OptionId + '">';
                        _tbody += '<option value="">Choose page....</option>';
                        for (var k = 0; k < candidatePages.length; k++) {
                            if (_value == candidatePages[k].PageId)
                                _tbody += '<option value=\'' + candidatePages[k].PageId + '\' selected>' + candidatePages[k].OptionTitle + '</option>';
                            else
                                _tbody += '<option value=\'' + candidatePages[k].PageId + '\'>' + candidatePages[k].OptionTitle + '</option>';
                        }

                        _tbody += '</select></div>';
                        _tbody += '<div><select class="opSkipQuestion" optionid="' + options[j].OptionId + '" id="opSkipQuestion_' + options[j].OptionId + '">';
                        _tbody += '<option value="">--</option>';
                        _tbody += '</select></div>';
                        _tbody += '</td>';

                        _tbody += '<td class="col3">';
                        _tbody += '<a href="#">clear</a>';
                        _tbody += '</td></tr>';
                    }
                    $('#TheQuestionlogicForm').find('#lg_OptionsTable tbody').html(_tbody);
                    $('#TheQuestionlogicForm').find('#lg_QuestionText').html(selectedQuestion.QuestionText);

                    $('#TheQuestionlogicForm').find('select.opSkipPage').bind('change', OnQuestionLogicPageChange);
                    $('#TheQuestionlogicForm').find('select.opSkipQuestion').prop('disabled', true);

                    $("#TheQuestionlogicForm").dialog({ position: { my: "top", at: "top", of: window } }).attr("questionId", selectedQuestion.QuestionId).dialog("open");

                    /*Προκαλούμε την εκτέλεση του change event handler για όλα τα select.opSkipPage elements, έτσι ώστε να συγχρονιστούν τα select.opSkipQuestion:*/
                    $('#TheQuestionlogicForm').find('select.opSkipPage').trigger('change', selectedQuestion);
                }
            });
        }

        function OnQuestionLogicPageChange(evt, selectedQuestion/*είναι ορισμένο μόνο με manul invoke του handler!*/)
        {
            var optionid = $(evt.target).attr('optionid');      //Το optionId υπάρχει επάνω στο select.opSkipPage
            var skipPage = $(evt.target).val();                 //βρίσκουμε την επιλογή που έχει κάνει ο χρήστης επάνω στο select.opSkipPage
            if (skipPage == null || skipPage == undefined)
                return;

            if (skipPage == '' || skipPage == '-1' || skipPage == '-2' || skipPage == '-3')
            {
                $('#opSkipQuestion_' + optionid).html('<option value="">--</option>');
                $('#opSkipQuestion_' + optionid).prop('disabled', true);
            }
            else
            {
                //Εάν έχει οριστεί η selectedQuestion, βρίσκουμε τότε το instance του option έτσι όπως μας έχει έρθςι απο το σύστημα:
                var selectedOption = null;
                if (selectedQuestion != null)
                {
                    for (var j = 0; j < selectedQuestion.Options.length; j++)
                    {
                        if(selectedQuestion.Options[j].OptionId == optionid)
                        {
                            selectedOption = selectedQuestion.Options[j];
                            break;
                        }
                    }
                }

                //τραβάμε τις ερωτήσεις που υπάρχουν στο σύστημα για την σελίδα που επέλεξε ο χρήστης:
                $.ajax({
                    url: theManagerPath + 'services/api/SurveyQuestions/GetQuestionsForPage?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&pageId=' + skipPage + '&textsLanguage=' + selectedTextsLanguage, dataType: 'json',
                    success: function (candidateQuestions) {
                        var _html = '<option value="">Top of page</option>';
                        for (var i = 0; i < candidateQuestions.length; i++)
                        {
                            if (selectedOption != null && selectedOption.SkipToQuestion == candidateQuestions[i].QuestionId)
                                _html += '<option value="' + candidateQuestions[i].QuestionId + '" selected>' + candidateQuestions[i].OptionTitle + '</option>';
                            else
                                _html += '<option value="' + candidateQuestions[i].QuestionId + '">' + candidateQuestions[i].OptionTitle + '</option>';
                        }

                        $('#opSkipQuestion_' + optionid).html(_html);
                        $('#opSkipQuestion_' + optionid).prop('disabled', false);
                    }
                });
            }
        }
        function OnSaveQuestionLogicBtn()
        {
            var questionId = $('#TheQuestionlogicForm').attr('questionId');

            var _data = 'data=';
            var _addCm = false;
            $('#TheQuestionlogicForm').find('select.opSkipPage').each(function (index, elem) {
                var optionid = $(this).attr('optionid');
                var skipPage = $(this).val();
                var skipQuestion = $('#opSkipQuestion_' + optionid).val();

                if (_addCm)
                    _data += ',';
                _data += 'op:' + optionid + '|sp:' + skipPage + '|sq:' + skipQuestion;
                _addCm = true;
            });


            var _url = theManagerPath + 'services/api/SurveyQuestions/SetSkipLogic?ScId=' + theAccessToken + '&surveyId=' + selectedSurveyId + '&questionId=' + questionId + '&textsLanguage=' + selectedTextsLanguage;


            $.ajax({
                url: _url, type: 'GET', async: false, data: _data,
                success: function (data) {
                    $('#TheQuestionlogicForm').dialog('close');
                    window.location = "Design_Survey.aspx?surveyid=<%=this.Surveyid %>&language=<%=this.TextsLanguage%>&pageId=<%=this.SurveyPageId%>";
                }
            });

        }


        function OnLibraryQuestionAdd(surveyId, pageId, position, referingQuestionId, textsLanguage)
        {
            $('#TheLibraryQuestionForm').attr("surveyId", surveyId);
            $('#TheLibraryQuestionForm').attr("pageId", pageId);
            $('#TheLibraryQuestionForm').attr("position", position);
            $('#TheLibraryQuestionForm').attr("referingQuestionId", referingQuestionId);
            $('#TheLibraryQuestionForm').attr("textsLanguage", textsLanguage);

            /*we open the TheLibraryQuestionForm:*/
            $("#TheLibraryQuestionForm").dialog("option", "title", 'Add Library Question:').dialog({ position: { my: "top", at: "top", of: window } }).dialog("open");
        }
        function OnLibraryQuestionClick()
        {
            //The user clicked a 'div.qLibrary-p' element! We need to find the corresponding questionId!
            var questionid = $(this).attr('questionId');

            var _data = 'questionId=' + escape(questionid);
            _data = _data + '&surveyId=' + $('#TheLibraryQuestionForm').attr("surveyId");
            _data = _data + '&pageId=' + $('#TheLibraryQuestionForm').attr("pageId");
            _data = _data + '&position=' + $('#TheLibraryQuestionForm').attr("position");
            _data = _data + '&referingQuestionId=' + $('#TheLibraryQuestionForm').attr("referingQuestionId");
            _data = _data + '&textsLanguage=' + $('#TheLibraryQuestionForm').attr("textsLanguage");

            var _url = theManagerPath + 'services/api/SurveyQuestions/AddLibraryQuestion?ScId=' + theAccessToken;

            $.ajax({
                url: _url, data: _data, type: 'POST', async: false,
                success: function (data) {
                    //$('#TheLibraryQuestionForm').dialog('close');
                    window.location = "Design_Survey.aspx?surveyid=<%=this.Surveyid %>&language=<%=this.TextsLanguage%>&pageId=<%=this.SurveyPageId%>";
                }
            });
        }

        function OnPreview(surveyPublicId, languageName)
        {
            window.open('<%=GetPreviewLink%>', '_blank');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
     <div class="pageTitle">
        <h1><a class="back-link" href='<%=_UrlSuffix("mysurveys.aspx") %>'><%=this.Server.HtmlEncode(this.SelectedSurvey.Title) %></a></h1>
         <%if(this.SelectedSurvey.TextsLanguage != 0)
          {
              Response.Write(this.GetTextsLanguageThumbnail());
          }%>
         <a class="greenbutton previewLink" id="previewButton" onclick="OnPreview('<%=this.SelectedSurvey.PublicId %>', '<%= Valis.Core.BuiltinLanguages.GetTwoLetterISOCode(this.SelectedSurvey.TextsLanguage) %>');">Preview Survey</a>
    </div>
    <div class="pageTools">
        <uc1:EditSurveyTabs ID="EditSurveyTabs2" runat="server" />
    </div>
    <div class="surveyPage">
        <div>    
            <valis:VLDropDownList ID="pagesSelector" runat="server" AutoPostBack="True" OnSelectedIndexChanged="pagesSelector_SelectedIndexChanged" ClientIDMode="Static"></valis:VLDropDownList>
            <a title="Add a survey Page" class="greenbutton pageButtons" href="javascript:OnAddPage()">Add Page</a>
            <a title="Edit the survey Page" class="greenbutton pageButtons" href="javascript:OnEditPage()">Edit Page</a>
            <%if(this.SelectedPage.HasSkipLogic){ %>
                <a title="Edit Page Logic" class="greenbutton pageButtons" href="javascript:OnEditPageLogic()">Edit Page Logic</a>
            <%} else { %>
                <a title="Add Page Logic" class="greybutton pageButtons" href="javascript:OnAddPageLogic()">Add Page Logic</a>
            <%} %>
            <a title="Delete the survey Page" class="redbutton pageButtons" href="javascript:OnDeletePage()">Delete Page</a>
        </div>
    </div>
    <div class="surveyQuestions">
        <%=this.GetQuestionsHtml() %>
    </div>

    <div id="AddPageForm" class="valisInputForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow" id="PositionWrapper"><label for="pgPosition">Position</label>
                    <select name="pgPosition" id="pgPosition">
                        <option value="0">Add Page After</option>
                        <option value="1">Add Page Before</option>
                    </select>
            </div>
            <div class="formRow"><label for="pgShowTitle">ShowTitle</label><input type="text" name="pgShowTitle" id="pgShowTitle" /></div>
            <div class="formRow"><label for="pgDescription">Description</label><textarea name="pgDescription" id="pgDescription"></textarea></div>
        </div>
        <input type="hidden" id="pgpPageId" name="pgpPageId" />
        <input type="hidden" id="pgpTextsLanguage" name="pgpTextsLanguage" />
        <span class="formFooter"></span>
    </div>

    <div id="DeletePageForm" title="Delete this Page?" class="valisInputForm" style="display: none;">
        <h2>What Should Happen to the Questions on This Page?</h2>
        <div class="formWrapper">
            <input type="radio" name="DeleteQuestionsBehavior" id="DeleteQuestionsBehavior_0" value="DeleteQuestionsBehavior_0"/><label style="width: 300px; text-align: left;" for="DeleteQuestionsBehavior_0">Delete all questions on this page.</label>
            <br />
            <div class="warning" id="responsesWarning">(NOTE: Any responses collected for questions on this page will be deleted.)</div>
        </div>
        <div class="formWrapper" id="DeleteQuestionsBehavior_1_Wrapper"><input type="radio" name="DeleteQuestionsBehavior" id="DeleteQuestionsBehavior_1" value="DeleteQuestionsBehavior_1"/><label style="width: 300px; text-align: left;" for="DeleteQuestionsBehavior_1">Move all questions to the page above.</label></div>
        <div class="formWrapper" id="DeleteQuestionsBehavior_2_Wrapper"><input type="radio" name="DeleteQuestionsBehavior" id="DeleteQuestionsBehavior_2" value="DeleteQuestionsBehavior_2"/><label style="width: 300px; text-align: left;" for="DeleteQuestionsBehavior_2">Move all questions to the page below.</label></div>

    </div>

    <div id="TheQuestionForm" class="valisInputForm theQuestionForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow"><label id="QuestionTextLbl" for="QuestionText">QuestionText</label><br /><textarea rows="2" id="QuestionText" name="QuestionText" class="required"></textarea><span class="requiredFieldAsterisk">*</span></div>
            <div class="formRow"><label id="QuestionDescriptionLbl" for="QuestionDescription">Description</label><br /><textarea rows="3" id="QuestionDescription" name="QuestionDescription"></textarea></div>
            <div class="QuestionTypeAreaWrapper">
                <div class="formRow"><label for="QuestionType">QuestionType</label><br /><select id="QuestionType" name="QuestionType"><%=this.GetQuestionTypeOptions()%></select></div>
                <div id="QuestionTypeAttributesPlaceHolder">

                </div>
            </div>
            <div class="formRow" id="requiredPanel"><input type="checkbox" id="IsRequired" name="IsRequired" /><label for="IsRequired">Require an answer to this question</label>
                <div class="requiredMessageWrapper" id="requiredMessageWrapper" style="display:none">
                    <div><label for="requiredMessage">When the question is not answered, display this error message:</label></div>
                    <div><textarea name="requiredMessage" id="requiredMessage" rows="3" cols="60"></textarea></div>
                </div>
            </div>
        </div>
    </div>

    <div id="TheLibraryQuestionForm" class="valisInputForm theLibraryQuestionForm" style="display: none;background: none repeat scroll 0 0 #9e9b90;">
        <div class="qLibrary">
            <%
                Response.Write("<ul class=\"qLibrary-ul\">");
                var questions = this.LibraryManager.GetLibraryQuestions(Valis.Core.BuiltinLibraryQuestionCategories.CommonQuestions.CategoryId);
                foreach(var q in questions)
                {
                    Response.Write("<li class=\"qLibrary-li\">");
                    Response.Write(string.Format("<div class=\"qLibrary-p\" questionid=\"{0}\">", q.QuestionId));

                    Response.Write(HttpUtility.HtmlEncode(q.QuestionText));
                    
                    Response.Write("</div>");
                    Response.Write("</li>");
                }
                Response.Write("</ul>");
            %>
        </div>
    </div>
    <div id="ThePagelogicForm" title="Choose a Page to Jump To..." class="valisInputForm" style="display: none">
        <h2>When the respondent leaves the current page, they will automatically jump to the page you select below.</h2>
        <div class="formWrapper">
                <p>
                    <select id="skipPage"><option value="0">-- select a page to jump to --</option></select>
                </p>
        </div>
    </div>

    <div id="TheQuestionlogicForm" title="Add question Logic" class="valisInputForm" style="display: none">
        <div class="panel">
            <h2>Question</h2>
            <p id="lg_QuestionText">questionText</p>
        </div>
        <div class="panel">
            <h2>Skip Logic</h2>
            <p id="lg_help">To add skip logic to one or more answer choice, choose the destination page and question under 'Skip to'</p>
            <table id="lg_OptionsTable" class="logicOptionsTable">
                <thead>
                    <tr><td>If answer is</td><td>Skip to</td><td>Clear All</td></tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="col1">option #2,223</td>
                        <td class="col2">
                            <div><select><option>Choose page....</option></select></div>
                            <div><select><option>Top of page</option></select></div>                            
                        </td>
                        <td class="col3">
                            <a href="#">clear</a>
                        </td>
                    </tr>
                    <tr>
                        <td>option #2,223</td>
                        <td>
                            <div><select><option>Choose page....</option></select></div>
                            <div><select><option>Top of page</option></select></div>                              
                        </td>
                        <td>
                            <a href="#">clear</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
