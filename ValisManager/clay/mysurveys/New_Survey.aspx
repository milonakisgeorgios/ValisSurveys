<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="New_Survey.aspx.cs" Inherits="ValisManager.clay.mysurveys.New_Survey" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

        div.squaretable {
            background-color: #ffffff;
            padding: 4px;
            width: 700px;
            position: relative;
            margin-top: 12px;
        }

        div.leftColumn fieldset{
            padding: 12px;
            margin: 8px;
            border: 1px solid #aaaaaa;
        }
        div.leftColumn input, div.leftColumn select
        {
            margin: 8px;
        }
        div.leftColumn input[type=text], div.leftColumn select{
            min-width: 240px;
        }
        label.frontLabel {
            display: inline-block;
            width: 120px;
            text-align: right;
        }

        a.CancelBtn
        {
            text-decoration: none;
            display: inline-block;
            border: 1px solid #999999;
            font-size: 1em;
            padding: 4px 5px 4px 5px;
            margin-right: 5px;
            color: #222222;
            background: url("/content/images/bg-grygrad.gif") repeat-x scroll 0 0 #777777;
        }
        input.btnAddSrvy
        {
            background: url("/content/images/btnMain.gif") repeat-x scroll 0 0 #2eacaa;
            border: 1px solid #999999;
            color: #FFFFFF;
            font-size: 1em;
            font-weight: bold;
            padding: 4px 5px;
        }

        div.leftFooter
        {
            margin: 18px 0px 0px 12px;
        }

        div.row1
        {
            display: block;
        }
        div.row2
        {
            display: none;
        }
        div.row3
        {
            display: none;
        }
        #surveyTitleCreate::-moz-placeholder, #surveyTitleCopy::-moz-placeholder {
            font-style:italic;
            color: #888;
        }
        #surveyTitleCreate::-webkit-input-placeholder, #surveyTitleCopy::-webkit-input-placeholder {
            font-style:italic;
            color: #888;
        }
        #surveyTitleCreate:-ms-input-placeholder , #surveyTitleCopy:-ms-input-placeholder {
            font-style:italic;
            color: #999;
        }
    </style>
    <script>
    
        $(document).ready(function () {

            $('#rdbtnScratch').bind('change', function () {
                $('div.row1').slideDown();
                $('div.row2').slideUp();
                $('div.row3').slideUp();
            });
            $('#rdbtnImport').bind('change', function () {
                $('div.row1').slideUp();
                $('div.row2').slideDown();
                $('div.row3').slideUp();
            });
            $('#rdbtnTemplate').bind('change', function () {
                $('div.row1').slideUp();
                $('div.row2').slideUp();
                $('div.row3').slideDown();
            });

            $('#surveyList').bind('change', function ()
            {
                var selOption = $('#surveyList option:selected').get(0);
                if (selOption != null && selOption != undefined)
                {
                    var surveyId = $(selOption).attr('value');

                    if (surveyId == 0 || surveyId == '0' || surveyId == '')
                    {
                        $('#surveyLanguageCopy').html('');
                        return;
                    }

                    $.ajax({
                        url: theManagerPath + 'services/api/Surveys/GetById?ScId=' + theAccessToken + '&surveyId=' + surveyId, dataType: 'json',
                        success: function (selectedSurvey)
                        {
                            if(selectedSurvey.PrimaryLanguage != /*Invariant*/0)
                            {
                                var tokens = selectedSurvey.SupportedLanguagesIds.split(",");
                                if((tokens.length-1) <= 1)
                                {
                                    var _languageOptions = '<option value=\'' + selectedSurvey.PrimaryLanguage + '\'>' + GetLanguageName(selectedSurvey.PrimaryLanguage) + '</option>';
                                }
                                else
                                {
                                    var _languageOptions = '<option value=\'-4\'>All Languages</option>';
                                    for (var i = 0; i < tokens.length; i++) {
                                        if (tokens[i] == '')
                                            continue;
                                        _languageOptions += '<option value=\'' + tokens[i] + '\'>' + GetLanguageName(tokens[i]) + '</option>';
                                    }
                                }
                            }
                            else
                            {
                                var _languageOptions = '<option value=\'0\'>Invariant Language</option>';
                            }

                            $('#surveyLanguageCopy').html(_languageOptions);
                        }
                    });

                }
            });

        });

        function OnSurveyAdd() {
            if ($('#rdbtnScratch').is(':checked'))
            {
                var value = $('#surveyTitleCreate').val();
                if (value == '' || value == null || value == undefined) {
                    //alert('You must provide a Title for this Survey!');
                    $("#surveyTitleCreate").animateHighlight("#fd2525", 500);
                    return false;
                }
                return true;
            }
            else if ($('#rdbtnImport').is(':checked'))
            {
                var value = $('#surveyTitleCopy').val();
                if (value == '' || value == null || value == undefined) {
                    //alert('You must provide a Title for this Survey!');
                    $("#surveyTitleCopy").animateHighlight("#fd2525", 500);
                    return false;
                }
                return true;
            }
            else if ($('#rdbtnTemplate').is(':checked'))
            {
                //TO DO!
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1>Create Survey</h1>
    </div>

    <div class="squaretable">
        <div class="leftColumn">
            <fieldset class="firstWay">
                    <input name="SrvyCreate" id="rdbtnScratch" type="radio" value="rdbtnScratch" checked="checked"/>
                    <label class="large-label" for="rdbtnScratch">Build a New Survey from Scratch </label>
                <div class="row1">
                    <div><label class="frontLabel">Title:</label><asp:TextBox ID="surveyTitleCreate" runat="server" ClientIDMode="Static" Width="300px" placeholder="Give your survey a name."></asp:TextBox><img class="required-image" alt="required" src="/content/images/requiredIcon1.gif"></div>
                    <div><label class="frontLabel">Language:</label><asp:DropDownList ID="surveyLanguageCreate" runat="server" ClientIDMode="Static"></asp:DropDownList></div>
                </div>

            </fieldset>
            <fieldset class="secondWay">
                    <input name="SrvyCreate" id="rdbtnImport" type="radio" value="rdbtnImport"/>
                    <label class="large-label" for="rdbtnImport">Copy an existing survey</label>
                <div class="row2">
                    <div><label class="frontLabel">Copy:</label><asp:DropDownList ID="surveyList" runat="server" ClientIDMode="Static"></asp:DropDownList></div>
                    <div><label class="frontLabel">Title:</label><asp:TextBox ID="surveyTitleCopy" runat="server" ClientIDMode="Static" Width="300px"></asp:TextBox><img class="required-image" alt="required" src="/content/images/requiredIcon1.gif"></div>
                    <div><label class="frontLabel">Language:</label><asp:DropDownList ID="surveyLanguageCopy" runat="server" ClientIDMode="Static"></asp:DropDownList></div>
                </div>
            </fieldset>
            <!--
            <fieldset class="thirdWay">
                    <input name="SrvyCreate" id="rdbtnTemplate" type="radio" value="rdbtnTemplate"/>
                    <label class="large-label" for="rdbtnTemplate">Use an expert survey template</label>
                <div class="row3">
                </div>
            </fieldset>
            -->
        </div>
        <div class="leftFooter">
            <a class="CancelBtn" id="btnCancel" href="~/clay/mysurveys/mysurveys.aspx" runat="server">Cancel</a>
            <asp:Button ID="btnAddSrvy" runat="server" Text="Continue" CssClass="btnAddSrvy" OnClick="btnAddSrvy_Click" OnClientClick="return OnSurveyAdd();" />
        </div>
    </div>
</asp:Content>
