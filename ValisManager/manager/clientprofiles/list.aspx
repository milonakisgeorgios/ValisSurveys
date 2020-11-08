<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="list.aspx.cs" Inherits="ValisManager.manager.clientprofiles.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $grid = $('#theGrid');

            var actionsFormatter = function (cellvalue, options, rowObject) {
                var _html = '';
                _html += "<a href=\"javascript:OnGridEdit('" + rowObject.ProfileId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                return _html;
            };

            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Profiles/GetProfiles?ScId=' + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "ProfileId" },
                colNames: ['', 'Name', 'UseCredits', 'MaxUsers', 'MaxSurveys', 'MaxLists', 'MaxEmailsPerDay','MaxEmailsPerWeek','CanExportData','IsBuiltIn', 'CreationDT'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter },
                        { name: 'Name', index: 'Name', width: 200, align: 'center' },
                        { name: 'UseCredits', sortable: false, width: 100, align: 'center' },
                        { name: 'MaxNumberOfUsers', sortable: false, width: 80, align: 'center' },
                        { name: 'MaxNumberOfSurveys', sortable: false, width: 80, align: 'center' },
                        { name: 'MaxNumberOfLists', sortable: false, width: 80, align: 'center' },
                        { name: 'MaxNumberOfEmailsPerDay', sortable: false, width: 130, align: 'center' },
                        { name: 'MaxNumberOfEmailsPerWeek', sortable: false, width: 130, align: 'center' },
                        { name: 'CanExportData', sortable: false, width: 100, align: 'center' },
                        { name: 'IsBuiltIn', index: 'IsBuiltIn', width: 80, align: 'center', formatter: builtinFormatter },
                        { name: 'CreateDT', index: 'CreateDT', width: 120, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } }],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>',page:<%=PageNumber %>,
                rowNum: <%=RowNum %>, rowList: [6, 10, 16], pager: '#thePager', viewrecords: false,
                loadui: "block", multiselect: false, height: 250,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });


            $('#createNewBtn').bind('click', function () {
                window.location.href = AddJqGridParamsToURLSuffix("theGrid","create.aspx");
            });
        });

        
        function OnGridEdit(rowId) {
            window.location.href = AddJqGridParamsToURLSuffix("theGrid", "edit.aspx?ProfileId=" + rowId);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Profiles</span>
        <button class="form-button form-right-button add-button" type="button" id="createNewBtn">
            <span class="ui-button-text">Add new</span>
        </button>
    </div>
    <div class="grid-wrapper">

        <table id="theGrid"></table>
        <div id="thePager"></div>

    </div>

</asp:Content>
