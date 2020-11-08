<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="edit.aspx.cs" Inherits="ValisManager.manager.libraries.questions.edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        div.flag-wrapper {
            float: left;
            margin: 6px;
            width: 16px;
            height: 16px;
            padding: 4px;            
        }
        div.flag-wrapper img{
            float: left;
        }
        div.flag-active {
            background-color: #ffc525;
            cursor: default;
        }
        div.flag-inactive {
            background-color: transparent;
            cursor: pointer;
        }

        div.toolbar
        {    
            background-color: #f1f1f1;
            border-bottom: 2px solid #c7c7c7;
            font-size: 1em;
            font-weight: bold;
            height: 96px;
            line-height: 42px;
            padding-left: 18px;
        }
        div.toolbar h2{
            font-size: 1.4em;
            color: #808080;
            line-height: 26px;
            clear: both;
        }
        div.toolbar h1{
            font-size: 1.6em;
            color: #01073c;
            line-height: 30px;
            clear: both;
        }

        div.formRow
        {
            margin: 12px 4px 4px 4px;
        }
        div.formRow label{
            display: inline-block;
            width: auto;
            text-align: left;
            font-weight: bold;
            font-size: 1.2em;
            padding: 0px 0px 0px 4px;
        }
        div.requiredMessageWrapper
        {
            margin: 8px 0px 0px 24px;
        }
        div.formRow label.sublabel{
            font-weight: normal;
            font-size: 1em;
        }
        #requiredMessage
        {
            width: 440px;
            height: 96px;
        }


        #QuestionText
        {
            width: 724px;
            height: 164px;
            font-size: 16px;
            line-height: 22px;
            padding: 6px;
        }
        #requiredMessage
        {
            width: 600px;
            height: 96px;
            font-size: 14px;
            line-height: 18px;
            padding: 4px;
        }
        #OptionText
        {
            width: 460px;
            height: 96px;
        }
        #ColumnText
        {
            width: 460px;
            height: 96px;
        }
        #QuestionType
        {
            background-color: #dddddd;
            border: 1px solid #808080;
            color: #010333;
        }

        
        .ui-jqgrid .ui-jqgrid-view {
            font-size: 14px;
        }
        .ui-jqgrid .ui-jqgrid-view img{
            margin-top: 6px;
        }
        .ui-jqgrid tr.jqgrow td {
            height: 16px;
        }

    </style>
    <script type="text/javascript">
        var selectedQuestion = <%=this.SelectedQuestion.QuestionId%>;
        var selectedLanguage = <%=this.SelectedLanguage.LanguageId%>;
        var haschanges = false;


        $(document).ready(function ()
        {
            $('#TheOptionForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, draggable: true, width: 500, buttons: { 'Create': { class: 'lightgreenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Create %>', id: 'FormOptionSaveBtn', click: OnFormOptionSaveBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'FormOptionCancelBtn', click: function () { $(this).dialog("close"); } } } });
            $('#TheColumnForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, draggable: true, width: 500, buttons: { 'Create': { class: 'lightgreenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Create %>', id: 'FormColumnSaveBtn', click: OnFormColumnSaveBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'FormColumnCancelBtn', click: function () { $(this).dialog("close"); } } } });

            $('#tabs').tabs({
                activate: function (event, ui)
                {
                    $.cookie("libraryquestions.edit", $("#tabs").tabs("option", "active"));
                },
                active: $.cookie("libraryquestions.edit")
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

            <%if(this.SelectedQuestion.IsRequired){%>
                $('#requiredMessageWrapper').show();
            <%}%>


            
            var optionsActionFormatter = function (cellvalue, options, rowObject) {
                var _html = '';
                _html += "<a  href=\"javascript:OnOptionsGridEdit('" + rowObject.OptionId + "')\" ><img style=\"float: left;\" border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                _html += "<a  href=\"javascript:OnOptionsGridDelete('" + rowObject.OptionId + "')\" ><img style=\"float: left;\" border=\"0\" src=\"" + theManagerPath + "content/images/deleteitemred.gif\" alt=\"delete\"/></a>";
                return _html;
            };


            $('#theOptionsGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/LibraryQuestions/GetOptions?ScId=' + theAccessToken+'&question='+selectedQuestion+'&language='+selectedLanguage,
                datatype: "json", jsonReader: { repeatitems: false, id: "OptionId" },
                colNames: ['', 'DisplayOrder', 'OptionText'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: optionsActionFormatter },
                        { name: 'DisplayOrder', sortable: false, width: 120, align: 'center' },
                        { name: 'OptionText', sortable: false, width: 700, align: 'left' }],
                    sortname: 'DisplayOrder', sortorder: 'asc',page:1,
                rowNum: -1, viewrecords: false,
                loadui: "block", multiselect: false, height: 400,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theOptionsGrid', _xml, ts, er); }
            });


            
            var columnsActionFormatter = function (cellvalue, options, rowObject) {
                var _html = '';
                _html += "<a href=\"javascript:OnColumnsGridEdit('" + rowObject.ColumnId + "')\" ><img style=\"float: left;\" border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                _html += "<a href=\"javascript:OnColumnsGridDelete('" + rowObject.ColumnId + "')\" ><img style=\"float: left;\" border=\"0\" src=\"" + theManagerPath + "content/images/deleteitemred.gif\" alt=\"delete\"/></a>";
                return _html;
            };

            
            $('#theColumnsGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/LibraryQuestions/GetColumns?ScId=' + theAccessToken+'&question='+selectedQuestion+'&language='+selectedLanguage,
                datatype: "json", jsonReader: { repeatitems: false, id: "ColumnId" },
                colNames: ['', 'DisplayOrder', 'ColumnText'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: columnsActionFormatter },
                        { name: 'DisplayOrder', sortable: false, width: 120, align: 'center' },
                        { name: 'ColumnText', sortable: false, width: 700, align: 'left' }],
                sortname: 'DisplayOrder', sortorder: 'asc',page:1,
                rowNum: -1, viewrecords: false,
                loadui: "block", multiselect: false, height: 400,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theColumnsGrid', _xml, ts, er); }
            });
        });


        function OnDelete()
        {
            showDelete("Are you sure you want to delete this Question?", function ()
            {
                <%=GetDeleteButtonHandler%>;
            }, "Are you sure?");

            return false;
        }

        function ChangeLanguage(languageId)
        {
            window.location = "/manager/libraries/questions/edit.aspx?QuestionId=" + selectedQuestion + "&language=" + languageId;
        }

        
        function OnOptionsGridDelete(rowId) 
        {
            var _params = 'ScId=' + theAccessToken+'&QuestionId='+selectedQuestion+'&language='+selectedLanguage +'&optionId=' + rowId;
            
            $.ajax({
                url: theManagerPath + 'services/api/LibraryQuestions/GetOptionById?' + _params ,
                dataType: 'json', 
                success: function(data)
                {                    
                    showQuestion('Are you sure you want to delete the option '+ data.OptionText +'?', function () {
                        $.ajax({
                            url: theManagerPath + 'services/api/LibraryQuestions/DeleteOption?' + _params, 
                            dataType: 'json',
                            success: function (data) {
                                ReloadGrid('#theOptionsGrid'); 
                            }
                        });
                    }, 'Delete Option');
                }
            });
        }

        function OnOptionsGridEdit(rowId) 
        {
            $.ajax({
                url: theManagerPath + 'services/api/LibraryQuestions/GetOptionById?ScId=' + theAccessToken+'&QuestionId='+selectedQuestion+'&language='+selectedLanguage +'&optionId=' + rowId ,
                dataType: 'json', 
                success: function(data)
                {
                    OpenOptionForm(false, data);
                }
            });
        }
        function OpenOptionForm(createNew, data)
        {
            $('#OptionText').val('');
            $('#OptionId').val('');
            $('#TheOptionForm').removeAttr('createNew');
            
            if(createNew == false)
            {
                $('#OptionId').val(data.OptionId);
                $('#OptionText').val(data.OptionText);

                $("#TheOptionForm").dialog("option", "title", 'Update Option:');
                $('#FormOptionSaveBtn').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Update %>');
            }
            else
            {
                $("#TheOptionForm").dialog("option", "title", 'Create a new Option:');
                $('#FormOptionSaveBtn').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Create %>');
            }

            $("#TheOptionForm").attr("createNew", createNew).dialog("open");
        }
        function OnFormOptionSaveBtn()
        {
            var createNew = $('#TheOptionForm').attr("createNew");

            var value = $('#OptionText').val();
            if (value == '' || value == null || value == undefined) {
                $("#OptionText").animateHighlight("#fd2525", 400);
                $('#OptionText').focus();
                return;
            }
            
            var _data = 'optionText=' + encodeURIComponent($('#OptionText').val());
            _data = _data + '&QuestionId=' + selectedQuestion;
            _data = _data + '&language=' + selectedLanguage;
            _data = _data + '&optionId=' + encodeURIComponent($('#OptionId').val());

            var _url = theManagerPath + 'services/api/LibraryQuestions/'+ (createNew == "true" ? 'CreateOption' : 'UpdateOption') +'?ScId=' + theAccessToken;            
            
            $.ajax({
                url: _url, data: _data, type: 'POST', async: false,
                success: function (data) {
                    $('#TheOptionForm').dialog('close');
                    ReloadGrid('#theOptionsGrid'); 
                }
            });
        }


        function OnColumnsGridDelete(rowId) 
        {
            var _params = 'ScId=' + theAccessToken+'&QuestionId='+selectedQuestion+'&language='+selectedLanguage +'&columnId=' + rowId;
            
            $.ajax({
                url: theManagerPath + 'services/api/LibraryQuestions/GetColumnById?' + _params ,
                dataType: 'json', 
                success: function(data)
                {                    
                    showQuestion('Are you sure you want to delete the column '+ data.ColumnText +'?', function () {
                        $.ajax({
                            url: theManagerPath + 'services/api/LibraryQuestions/DeleteColumn?' + _params, 
                            dataType: 'json',
                            success: function (data) {
                                ReloadGrid('#theColumnsGrid'); 
                            }
                        });
                    }, 'Delete Column');
                }
            });
        }
        function OnColumnsGridEdit(rowId) 
        {
            $.ajax({
                url: theManagerPath + 'services/api/LibraryQuestions/GetColumnById?ScId=' + theAccessToken+'&QuestionId='+selectedQuestion+'&language='+selectedLanguage +'&columnId=' + rowId ,
                dataType: 'json', 
                success: function(data)
                {
                    OpenColumnForm(false, data);
                }
            });
        }
        function OpenColumnForm(createNew, data)
        {
            $('#ColumnText').val('');
            $('#ColumnId').val('');
            $('#TheColumnForm').removeAttr('createNew');
            
            if(createNew == false)
            {
                $('#ColumnId').val(data.ColumnId);
                $('#ColumnText').val(data.ColumnText);

                $("#TheColumnForm").dialog("option", "title", 'Update Column:');
                $('#FormColumnSaveBtn').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Update %>');
            }
            else
            {
                $("#TheColumnForm").dialog("option", "title", 'Create a new Column:');
                $('#FormColumnSaveBtn').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Create %>');
            }

            $("#TheColumnForm").attr("createNew", createNew).dialog("open");
        }
        function OnFormColumnSaveBtn()
        {
            var createNew = $('#TheColumnForm').attr("createNew");

            var value = $('#ColumnText').val();
            if (value == '' || value == null || value == undefined) {
                $("#ColumnText").animateHighlight("#fd2525", 400);
                $('#ColumnText').focus();
                return;
            }
            
            var _data = 'columnText=' + encodeURIComponent($('#ColumnText').val());
            _data = _data + '&QuestionId=' + selectedQuestion;
            _data = _data + '&language=' + selectedLanguage;
            _data = _data + '&columnId=' + encodeURIComponent($('#ColumnId').val());

            var _url = theManagerPath + 'services/api/LibraryQuestions/'+ (createNew == "true" ? 'CreateColumn' : 'UpdateColumn') +'?ScId=' + theAccessToken;            
            
            $.ajax({
                url: _url, data: _data, type: 'POST', async: false,
                success: function (data) {
                    $('#TheColumnForm').dialog('close');
                    ReloadGrid('#theColumnsGrid'); 
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Edit Library Question: <span style="color: #000a56; font-size: .92em;"><%: this.QuestionTitle %></span></span>
        <a class="form-header-link" href='<%=_UrlSuffix("list.aspx") %>'>(back to library list)</a>
        <asp:Button ID="deleteBtn" runat="server" CssClass="form-button form-right-button delete-button" Text="Delete" OnClick="deleteBtn_Click" OnClientClick="return OnDelete();" formnovalidate="formnovalidate" />
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Update" OnClick="saveBtn_Click" />
    </div>
    
    <div class="toolbar">
        <h2>Choose your editing language:</h2>
        <%foreach (var language in Valis.Core.BuiltinLanguages.Languages)
          {
              if (this.SelectedLanguage.LanguageId == language.LanguageId)
              {
                  Response.Write("<div class=\"flag-wrapper flag-active\">");
                  Response.Write(string.Format("<img src=\"/content/flags/{0}\" alt=\"edit question in {1} language!\" />", Valis.Core.BuiltinLanguages.GetLanguageThumbnail(language.LanguageId), language.EnglishName));
                  Response.Write("</div>");
              }
              else
              {
                  Response.Write(string.Format("<div class=\"flag-wrapper flag-inactive\" onclick=\"ChangeLanguage({0})\">", language.LanguageId));
                  Response.Write(string.Format("<img src=\"/content/flags/{0}\" alt=\"edit question in {1} language!\" />", Valis.Core.BuiltinLanguages.GetLanguageThumbnail(language.LanguageId), language.EnglishName));
                  Response.Write("</div>");
              }
          } %>
          <h1>Editing Language: <%=this.SelectedLanguage.EnglishName %></h1>
    </div>

    <div class="form-wrapper">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Question</a></li>
                <%if(Valis.Core.VLSurveyManager.SupportOptions(this.SelectedQuestion.QuestionType)){ %>
                <li><a href="#tabs-2">Options</a></li>
                <%} %>
                <%if(Valis.Core.VLSurveyManager.SupportColumns(this.SelectedQuestion.QuestionType)){ %>
                <li><a href="#tabs-3">Columns</a></li>
                <%} %>
            </ul>
            <div id="tabs-1">
                <div class="formWrapper">
                    <div class="formRow">
                        <label id="QuestionTypeLbl" for="QuestionType">QuestionType<span class="requiredFieldAsterisk">*</span></label><br />
                        <asp:TextBox ID="QuestionType" CssClass="required" runat="server" ClientIDMode="Static" TextMode="SingleLine" ReadOnly="True"></asp:TextBox>
                    </div>
                    <div class="formRow">
                        <label id="QuestionTextLbl" for="QuestionText">QuestionText<span class="requiredFieldAsterisk">*</span></label><br />
                        <asp:TextBox ID="QuestionText" CssClass="required" runat="server" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="formRow" id="requiredPanel">
                        <asp:CheckBox ID="IsRequired" runat="server" ClientIDMode="Static" /><label for="IsRequired">Require an answer to this question</label>
                        <div class="requiredMessageWrapper" id="requiredMessageWrapper" style="display:none">
                            <div><label class="sublabel" for="requiredMessage">When the question is not answered, display this error message:</label></div>
                            <asp:TextBox ID="requiredMessage" CssClass="required" runat="server" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <%if(Valis.Core.VLSurveyManager.SupportOptions(this.SelectedQuestion.QuestionType)){ %>
            <div id="tabs-2">

                <div>                    
                    <input type="button" value="Add Option" onclick="OpenOptionForm(true,null)"/>
                </div>
                <div class="grid-wrapper">

                    <table id="theOptionsGrid"></table>

                </div>
            </div>
            <%} %>
            <%if(Valis.Core.VLSurveyManager.SupportColumns(this.SelectedQuestion.QuestionType)){ %>
            <div id="tabs-3">
                
                <div>                    
                    <input type="button" value="Add Column" onclick="OpenColumnForm(true,null)"/>
                </div>
                <div class="grid-wrapper">

                    <table id="theColumnsGrid"></table>

                </div>
            </div>
            <%} %>
        </div>
    </div>

    
    <div id="TheOptionForm" class="valisInputForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow">
                <label id="OptionTextLbl" for="OptionText">OptionText</label><br />
                <textarea rows="2" id="OptionText" name="OptionText" class="required"></textarea>
            </div>
        </div>
        <input type="hidden" id="OptionId" name="OptionId" />
    </div>
    
    <div id="TheColumnForm" class="valisInputForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow">
                <label id="ColumnTextLbl" for="ColumnText">ColumnText</label><br />
                <textarea rows="2" id="ColumnText" name="ColumnText" class="required"></textarea>
            </div>
        </div>
        <input type="hidden" id="ColumnId" name="ColumnId" />
    </div>

</asp:Content>
