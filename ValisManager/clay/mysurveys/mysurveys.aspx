<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="mysurveys.aspx.cs" Inherits="ValisManager.clay.mysurveys.mysurveys" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        
        div.mysurveys-wrapper
        {
            padding: 12px;
            margin-left: auto;
            margin-right: auto;
            width: 1115px;
        }

        div.pageTitle {
            background-image: url(/content/images/surveys.png);
            background-position: 0px 0px;
            background-repeat: no-repeat;
            padding-left: 44px;
        }
        a.actionLinks {
            text-decoration: none;
            margin-left: 6px;
            margin-right: 8px;
            color: #0077b5;
        }

        .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }
        .ui-jqgrid tr.ui-row-ltr td { border-bottom-style: none; border-right-style: none;}

        
        .rightButton
        {
            float: right;
        }
    </style>
    <script>

        /*Αυτός είναι ο πίνακας των υποστηριζόμενων γλωσσών:*/
        <%=LanguagesArray%>

        function OnEdit(surveyId, textsLanguage)
        {
            var url = theManagerPath +"clay/mysurveys/Design_Survey.aspx?surveyid="+surveyId +'&language='+textsLanguage;
            window.location = url;
        }
        function OnResults(surveyId, textsLanguage)
        {
            var url = theManagerPath +AddJqGridParamsToURLSuffix("SurveysGrid", "clay/mysurveys/analysis/summary.aspx?surveyid="+surveyId);
            window.location = url;
        }
        function OnOpenSelectorsList(surveyId)
        {
            var url = theManagerPath +AddJqGridParamsToURLSuffix("SurveysGrid", "clay/mysurveys/collectors/list.aspx?surveyid="+surveyId);
            window.location = url;
        }
        function OnAddSelector(surveyId)
        {
            var url = theManagerPath +AddJqGridParamsToURLSuffix("SurveysGrid", "clay/mysurveys/collectors/addCollector.aspx?surveyid="+surveyId);
            window.location = url;
        }
        function GetTargetLanguageOptions(selectedSurvey)
        {
            var options = '';
            var tokens = selectedSurvey.SupportedLanguagesIds.split(","); 

            for(var i=0; i< languages.length; i++)
            {
                var find = false;
                for(var j=0; j< tokens.length; j++)
                {
                    if(languages[i].id == tokens[j])
                    {
                        find = true;
                        break;
                    }
                }

                if(find == false)
                {
                    options+='<option value="'+languages[i].id+'">'+languages[i].name+'</option>'
                }
            }

            return options;
        }
        function OnTranslate(surveyId, textsLanguage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/Surveys/GetById?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (selectedSurvey) 
                {
                    var _options = GetTargetLanguageOptions(selectedSurvey);
                    if(_options == null || _options == '')
                    {
                        showInfo('<b>There are no available languages for translation!</b><br /><br />The survey has been translated to all available languages!','Survey Translation');
                        return;
                    }
                    $('#targetLanguage').html(_options);

                    $('#surveyId', '#theTranslationForm').val(selectedSurvey.SurveyId);
                    $('#sourceLanguage', '#theTranslationForm').val(selectedSurvey.TextsLanguage);
                    $("#theTranslationForm").dialog("option", "title", 'Translate survey').dialog({ position: { my: "bottom", at: "center", of: window } }).dialog("open");
                }
            });
        }
        function OnTranslationFormOkButton()
        {
            var surveyId = $('#surveyId', '#theTranslationForm').val();
            var sourceLanguage = $('#sourceLanguage', '#theTranslationForm').val();
            var targetLanguage = $('#targetLanguage', '#theTranslationForm').val();

            $.ajax({
                url: theManagerPath + 'services/api/Surveys/AddLanguage?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&sourceLanguage=' + sourceLanguage +'&targetLanguage='+targetLanguage, dataType: 'json',
                success: function (data) {
                    $('#SurveysGrid').trigger('reloadGrid');
                    $('#theTranslationForm').dialog('close');
                }
            });
        }
        function OnDelete(surveyId, textsLanguage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/Surveys/GetById?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (selectedSurvey) {

                    if(selectedSurvey.OpenCollectors > 0)
                    {
                        //Δεν μποούμε να διαγράψουμε το survey, έχει ανοιχτούς collectors:                        
                        showWarning('<div style="background-color: orange; color: #fff; padding: 6px; font-size: 1.2em;"><B>Warning</B> This survey has open collectors actively collecting responses. You must close all open collectors before this survey can be deleted.</div>', 'Delete the survey and responses');
                        return;
                    }
                    else
                    {
                        var _surveyTitle = (selectedSurvey.ShowTitle != null ? selectedSurvey.ShowTitle : selectedSurvey.Title);
                        showDelete('Are you sure you want to delete the survey <b>"' + _surveyTitle +'"</b>?', function(){
                    
                            $.ajax({
                                url: theManagerPath + 'services/api/Surveys/Delete?ScId=' + theAccessToken + '&surveyId=' + surveyId + '&textsLanguage=' + textsLanguage, dataType: 'html',
                                success: function (data) {
                                    $('#SurveysGrid').trigger('reloadGrid');
                                }
                            });
                        }, 'Delete the survey and responses.');
                    }
                }
            });
        }

        function actionsFormatter(cellvalue, options, rowObject)
        {
            var html = '';
            //html += '<a class="actionLinks" href="javascript:OnEdit(\''+rowObject.SurveyId+'\',\''+rowObject.TextsLanguage+'\');"><img width="20" height="20" src="'+theManagerPath + 'content/images/design.png" alt="Design" title="Design your survey."/></a>';
            
            if(rowObject.HasCollectors)
            {
                html += '<a class="actionLinks" href="javascript:OnOpenSelectorsList(\''+rowObject.SurveyId+'\');"><img width="20" height="20" src="'+theManagerPath + 'content/images/collectors.png" alt="Selectors" title="Manage your collectors for this survey."/></a>';
            }
            else
            {
                html += '<a class="actionLinks" href="javascript:OnAddSelector(\''+rowObject.SurveyId+'\');"><img width="20" height="20" src="'+theManagerPath + 'content/images/collectorsgray.png" alt="Selectors" title="Make collectors for this survey."/></a>';
            }
            if(rowObject.HasResponses)
            {
                html += '<a class="actionLinks" href="javascript:OnResults(\''+rowObject.SurveyId+'\',\''+rowObject.TextsLanguage+'\');"><img width="20" height="20" src="'+theManagerPath + 'content/images/results.png" alt="Analyze" title="View the responses of this survey."/></a>';
            }
            else
            {
                html += '<a class="actionLinks" href="javascript:void()"><img width="20" height="20" src="'+theManagerPath + 'content/images/results-gray.png" alt="Analyze" title="No responses for this survey yet."/></a>';
            }
            
            
            if(rowObject.IsBuiltIn)
            {
                html += '<a class="actionLinks"><img src="'+theManagerPath + 'content/images/Translate-16-gray.gif" alt="Translate" title="Cannot translate this survey."/></a>';            
                html += '<a class="actionLinks"><img src="'+theManagerPath + 'content/images/deleteGrayIcon.gif" alt="Delete" title="Cannot delete this survey."/></a>';
            }
            else
            {
                if(rowObject.PrimaryLanguage == /*Invariant*/0)
                {
                    //Δεν μεταφράζεται:
                    html += '<a class="actionLinks" href="javascript:void();"><img src="'+theManagerPath + 'content/images/Translate-16-gray.gif" alt="Translate" title="Survey does not support translation"/></a>';
                }
                else
                {
                    html += '<a class="actionLinks" href="javascript:OnTranslate(\''+rowObject.SurveyId+'\',\''+rowObject.TextsLanguage+'\');"><img src="'+theManagerPath + 'content/images/Translate-16.png" alt="Translate" title="Translate this survey."/></a>';
                }
                
                html += '<a class="actionLinks" href="javascript:OnDelete(\''+rowObject.SurveyId+'\',\''+rowObject.TextsLanguage+'\');"><img src="'+theManagerPath + 'content/images/deleteRedIcon.png" alt="Delete" title="Delete this survey."/></a>';
            }

            return html;
        }
        function showTitleformatter(cellvalue, options, rowObject)
        {
            var html = '<a class="actionLinks" style=\"font-size: 16px;\" href="javascript:OnEdit(\''+rowObject.SurveyId+'\',\'' + rowObject.PrimaryLanguage + '\');">' + cellvalue + '</a>';

            return html;
        }
        function supportedLanguagesFormatter(cellvalue, options, rowObject)
        {
            if(rowObject.PrimaryLanguage != /*Invariant*/0)
            {
                var tokens = cellvalue.split(",");
                var html = '';
                for(var i=0; i< tokens.length; i++)
                {
                    if(tokens[i] == '')
                        continue;
                    html += '<a title="Design the '+GetLanguageName(tokens[i])+' version of '+rowObject.Title+'" href="javascript:OnEdit(\''+rowObject.SurveyId+'\',\''+tokens[i]+'\');"><img alt="'+GetLanguageName(tokens[i])+'" src="'+theManagerPath + 'content/' + GetLanguageIcon(tokens[i])+'" /></a>&nbsp;&nbsp;';
                }
                return html;
            }
            else
            {
                return '<a title="Design the '+rowObject.Title+'" href="javascript:OnEdit(\''+rowObject.SurveyId+'\',\'0\');"><img alt="'+GetLanguageName(0)+'" src="'+theManagerPath + 'content/' + GetLanguageIcon(0)+'" /></a>&nbsp;&nbsp;';
            }
        }
        function supportedLanguagesCellAttributes(rowId, val, rowObject)
        {
            return ' title="The supported languages of '+rowObject.Title+'!"';
        }

        $(document).ready(function () {
            $('#theTranslationForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 380, height: 235, buttons: { 'Translate': { class: 'greenbutton', text: 'Translate', id: 'formSaveButton', click: OnTranslationFormOkButton }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'saveCancelBtn', click: function () { $(this).dialog("close"); } } } });
            var emptyMsgDiv = $('<div style="font-size: 18px; padding: 18px; color: #777;">No surveys have been created yet. Click the "Create Survey" button above to get started.</div>');
            var grid = $('#SurveysGrid');

            grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + "services/api/Surveys/ClientGetAll?ScId=" + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "SurveyId" },
                colNames: ["Title","Modified","Responses","design","actions"],
                colModel: [
                        { name: 'Title', width: 520, sortable: true, formatter: showTitleformatter },
                        { name: 'LastUpdateDT', width: 180, align: 'center', sortable: true },
                        { name: 'RecordedResponses', width: 70, sortable: false, align: 'center' },
                        { name: 'SupportedLanguagesIds', width: 140, sortable: false, align: 'center', formatter: supportedLanguagesFormatter, cellattr : supportedLanguagesCellAttributes },
                        { name: 'Actions1', width: 180, align: 'center', sortable: false, formatter: actionsFormatter }
                ],
                pager: "#SurveysPager", page:<%=PageNumber %>, sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>', rowNum: <%=RowNum %>, rowList: [],
                loadui: "block ", viewrecords: true,hoverrows:false , gridview: false, pginput: false, height: 380,
                emptyrecords: 'No surveys have been created yet. Click the "Create Survey" button above to get started.',
                loadError: function (xhr, status, error) {
                    OnjqGridLoadError('#SurveysGrid', xhr, status, error)
                },
                beforeSelectRow: function(rowid, e) {
                    return false;
                },
                loadComplete: function(){
                    var count = grid.getGridParam();
                    var ts = grid[0];
                    if (ts.p.reccount === 0) {
                        grid.hide();
                        emptyMsgDiv.show();
                    } else {
                        grid.show();
                        emptyMsgDiv.hide();
                    }
                }
            });
            emptyMsgDiv.insertAfter(grid.parent());

        });
        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="mysurveys-wrapper">

        <div class="pageTitle">
            <h1>My Surveys</h1>
            <a title="create a survey" class="greenbutton rightButton" id="createSurvey" href="New_Survey.aspx">+ Create Survey</a>
        </div>
        <div class="pageTools">
        </div>
        <table id="SurveysGrid"></table>
        <div id="SurveysPager"></div>

    
        <div id="theTranslationForm" class="valisInputForm" style="display: none">
            <div class="formWrapper">
                <div class="formRow" id="PositionWrapper">
                        <div style="padding: 4px; margin: 2px 2px 8px 2px; ">Choose a new language for the 'new-survey-01':</div>
                        <select name="targetLanguage" id="targetLanguage">

                        </select>
                </div>
            </div>
            <input type="hidden" id="surveyId" name="surveyId" />
            <input type="hidden" id="sourceLanguage" name="sourceLanguage" />
        </div>

    </div>

</asp:Content>
