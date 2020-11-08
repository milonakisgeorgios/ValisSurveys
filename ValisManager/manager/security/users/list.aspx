<%@ Page Title="The Survey System - SystemUsers" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="ValisManager.manager.security.users.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $grid = $('#theGrid');


            var actionsFormatter = function (cellvalue, options, rowObject)
            {
                _html = '';
                _html += "<a href=\"javascript:OnGridEdit('" + rowObject.UserId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                return _html;
            };

            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/SystemUser/GetSystemUsers?ScId=' + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "UserId" },
                colNames: ['', 'UserId', 'LastName', 'FirstName', 'DefaultLanguage', 'Role', 'IsActive', 'IsLockedOut', 'IsBuiltIn', 'LastLogin', 'CreationDT'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter },
                        { name: 'UserId', index: 'RoleId', width: 60 },
                        { name: 'LastName', index: 'LastName', width: 200 },
                        { name: 'FirstName', index: 'FirstName', width: 200 },
                        { name: 'DefaultLanguageName', sortable: false, width: 160 },
                        { name: 'RoleName', sortable: false, width: 160 },
                        { name: 'IsActive', index: 'IsActive', width: 80, align: 'center', formatter: isActiveFormatter },
                        { name: 'IsLockedOut', index: 'IsLockedOut', width: 80, align: 'center', formatter: isActiveFormatter },
                        { name: 'IsBuiltIn', index: 'IsBuiltIn', width: 80, align: 'center', formatter: builtinFormatter },
                        { name: 'LastLoginDate', index: 'LastLoginDate', width: 120, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } },
                        { name: 'CreateDT', index: 'CreateDT', width: 120, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } }],
                sortname: "LastName", sortorder: 'asc',
                rowNum: 10, rowList: [6, 10, 16], pager: '#thePager', viewrecords: false,
                loadui: "block", multiselect: false, height: 250,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });

            $('#createNewBtn').bind('click', function () {
                window.location.href = "add.aspx";
            });


        });

        function OnGridEdit(rowId) {
            window.location.href = "edit.aspx?UserId=" + rowId;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">System Users</span>
        <button class="form-button form-right-button add-button" type="button" id="createNewBtn">
            <span class="ui-button-text">Create new user</span>
        </button>
    </div>
    <div class="grid-wrapper">

        <table id="theGrid"></table>
        <div id="thePager"></div>

    </div>
</asp:Content>
