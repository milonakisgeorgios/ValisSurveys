<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="list.aspx.cs" Inherits="ValisManager.manager.libraries.questions.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        
        /*#region TheQuestionForm
        -------------------------------------------*/
        div.theQuestionForm .formWrapper
        {
            margin: 0;
            padding: 2px;
            font-size: 14px;
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
            margin: 12px 8px 4px 4px;
        }
        textarea#QuestionText
        {
            width: 540px;
            height: 128px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('#TheQuestionForm').dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, resizable: false, draggable: false, width: 600, buttons: { 'Create': { class: 'lightgreenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Create %>', id: 'AddQuestionButton', click: OnSaveQuestionBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelAddQuestionButton', click: function () { $(this).dialog("close"); } } } });
            $grid = $('#theGrid');

            var qtypeFormatter = function (cellvalue, options, rowObject) {
                if(rowObject.QuestionType == 1) return "SingleLine";
                else if(rowObject.QuestionType == 1) return "SingleLine";
                else if(rowObject.QuestionType == 2) return "MultipleLine";
                else if(rowObject.QuestionType == 3) return "Integer";
                else if(rowObject.QuestionType == 4) return "Decimal";
                else if(rowObject.QuestionType == 7) return "DateTime";
                else if(rowObject.QuestionType == 10) return "OneFromMany";
                else if(rowObject.QuestionType == 12) return "DropDown";
                else if(rowObject.QuestionType == 16) return "DescriptiveText";
                else if(rowObject.QuestionType == 22) return "Range";
                else if(rowObject.QuestionType == 21) return "Slider";
                else if(rowObject.QuestionType == 31) return "MatrixOnePerRow";
                else 
                    return cellvalue;
            }
            var actionsFormatter = function (cellvalue, options, rowObject) {
                var _html = '';
                _html += "<a href=\"javascript:OnGridEdit('" + rowObject.QuestionId + "')\" ><img style=\"float: left;\" border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                return _html;
            };

            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/LibraryQuestions/GetQuestions?ScId=' + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "QuestionId" },
                colNames: ['', 'QuestionType', 'QuestionText', 'CreationDT'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter },
                        { name: 'QuestionType', index: 'QuestionType', width: 200, align: 'left', formatter: qtypeFormatter },
                        { name: 'QuestionText', index: 'QuestionText', width: 700, align: 'left' },
                        { name: 'CreateDT', sortable: false, width: 140, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } }],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>',page:<%=PageNumber %>,
                rowNum: <%=RowNum %>, rowList: [6, 10, 16], pager: '#thePager', viewrecords: false,
                loadui: "block", multiselect: false, height: 250,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });

            
            $('#createNewBtn').bind('click', OnCreateNewQuestion);
        });

        function OnGridEdit(questionId)
        {
            window.location = "/manager/libraries/questions/edit.aspx?QuestionId=" + questionId+"&language=0";
        }

        function OnCreateNewQuestion()
        {
            $('#QuestionText').val('');
            $('#QuestionType').val('SingleLine');


            $('#AddQuestionButton').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Create %>');
            $("#TheQuestionForm").dialog("option", "title", 'Add Library Question:').dialog({ position: { my: "top", at: "top", of: window } }).attr("createNew", true).dialog("open");
        }
        function OnSaveQuestionBtn()
        {
            var value = $('#QuestionText').val();
            if (value == '' || value == null || value == undefined) {
                $("#QuestionText").animateHighlight("#fd2525", 400);
                $('#QuestionText').focus();
                return;
            }

            var _data = 'questionText=' + encodeURIComponent($('#QuestionText').val());
            _data = _data + '&questionType=' + encodeURIComponent($('#QuestionType').val());
            _data = _data + '&category=<%: Valis.Core.BuiltinLibraryQuestionCategories.CommonQuestions.CategoryId%>'
            
            var _url = theManagerPath + 'services/api/LibraryQuestions/CreateQuestion?ScId=' + theAccessToken;
            
            $.ajax({
                url: _url, data: _data, type: 'POST', async: false,
                success: function (data) {
                    //$('#TheQuestionForm').dialog('close');
                    window.location = "/manager/libraries/questions/edit.aspx?QuestionId=" + data.QuestionId+"&language=0";
                }
            });

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Library Questions</span>
        <button class="form-button form-right-button add-button" type="button" id="createNewBtn">
            <span class="ui-button-text">Add new</span>
        </button>
    </div>
    <p class="help">
        Απο αυτό το σημείο διαχειριζόμαστε την βιβλιοθήκη των προκατασεκυασμένων ερωτήσεων. Οι ερωτήσεις αυτές χρησιμοποιούνται για την γρήγορη δημιουργία των surveys των πελατων.
        Το σύστημα μας επιτρέπει για αυτές τις ερωτήσεις να ορίσουμε τα κείμενά τους σε όλες τις υποστηριζόμενες γλώσσες.
    </p>
    
    <div class="grid-wrapper">

        <table id="theGrid"></table>
        <div id="thePager"></div>

    </div>

    <div id="TheQuestionForm" class="valisInputForm theQuestionForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow"><label for="QuestionType">QuestionType</label><br /><select id="QuestionType" name="QuestionType"><%=this.GetQuestionTypeOptions()%></select></div>
            <div class="formRow"><label id="QuestionTextLbl" for="QuestionText">QuestionText</label><br /><textarea rows="2" id="QuestionText" name="QuestionText" class="required"></textarea><span class="requiredFieldAsterisk">*</span></div>
        </div>
    </div>
</asp:Content>
