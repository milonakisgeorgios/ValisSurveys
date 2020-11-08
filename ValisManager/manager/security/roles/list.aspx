<%@ Page Title="The Survey System - Roles" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="list.aspx.cs" Inherits="ValisManager.manager.security.roles.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $grid = $('#theGrid');


            var actionsFormatter = function (cellvalue, options, rowObject) {
                _html = '';
                //if (rowObject.IsBuiltIn == true)
                //    _html += "<img border=\"0\" src=\"" + theManagerPath + "content/images/edititemoff.gif\" alt=\"edit\"/>";
                //else
                    _html += "<a href=\"javascript:OnGridEdit('" + rowObject.RoleId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                return _html;
            };

            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Roles/GetRoles?ScId=' + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "RoleId" },
                colNames: ['', 'RoleId', 'Name', 'IsBuiltIn', 'IsClientRole', 'CreationDT'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter },
                        { name: 'RoleId', index: 'RoleId', width: 70 },
                        { name: 'Name', index: 'Name', width: 340 },
                        { name: 'IsBuiltIn', index: 'IsBuiltIn', width: 80, align: 'center', formatter: builtinFormatter },
                        { name: 'IsClientRole', index: 'IsClientRole', width: 80, align: 'center', formatter: checkGreenFormatter },
                        { name: 'CreateDT', index: 'CreateDT', width: 120, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } }],
            sortname: "Name", sortorder: 'asc',
            rowNum: 10, rowList: [6, 10, 16], pager: '#thePager', viewrecords: false,
            loadui: "block", multiselect: false, height: 250,
            beforeSelectRow: function (id) { return false; },
            loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
        });

            $('#createNewBtn').bind('click', function () {
                window.location.href = "add.aspx";
            });
        });

        function OnGridEdit(rowId)
        {
            window.location.href = "edit.aspx?roleid="+rowId;
        }
    </script>
    <style>



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="form-header">
        <span class="form-header-title">Roles</span>
        <button class="form-button form-right-button add-button" type="button" id="createNewBtn">
            <span class="ui-button-text">Create new role</span>
        </button>
    </div>
    <div class="grid-wrapper">
        
        <table id="theGrid"></table>
        <div id="thePager"></div>

    </div>

</asp:Content>
