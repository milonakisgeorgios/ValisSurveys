<%@ Page Title="The Survey System - Logins" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="list.aspx.cs" Inherits="ValisManager.manager.security.logins.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $grid = $('#theGrid');

            var principalTypeformatter = function (cellvalue, options, rowObject) {
                if (rowObject.PrincipalType == 0)
                    return 'SystemUser';
                if (rowObject.PrincipalType == 1)
                    return 'ClientUser';
                return '??';
            }
            var userNameformatter = function (cellvalue, options, rowObject) {
                return rowObject.LastName + ', ' + rowObject.FirstName;
            }
            var actionsFormatter = function (cellvalue, options, rowObject) {
                _html = '';
                return _html;
            }

            var mygrid = $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Logins/GetLogins?ScId=' + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "LoginId" },
                colNames: ['', 'Type', 'Client', 'User', 'LogOnToken', 'EnterDt', 'LeaveDt'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter,search: false },
                        { name: 'PrincipalType', index: 'PrincipalType', align: 'center', width: 100, formatter: principalTypeformatter, stype: 'select', editoptions: { value: ':All;0:SystemUser;1:ClientUser' } },
                        { name: 'ClientName', index: 'ClientName', align: 'center', width: 340 },
                        { name: 'User', index: 'User', align: 'left', width: 300, formatter: userNameformatter },
                        { name: 'LogOnToken', index: 'LogOnToken', align: 'center', width: 200 },
                        { name: 'EnterDt', index: 'EnterDt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' }, searchoptions: { dataInit: function (el) { $(el).datepicker({ dateFormat: 'dd/mm/yy', onSelect: function (dateText, inst) { mygrid[0].triggerToolbar(); } }); } } },
                        { name: 'LeaveDt', index: 'LeaveDt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' }, search: false },
                            ],
                sortname: "EnterDt", sortorder: 'desc',
                rowNum: 18, rowList: [18, 32, 48], pager: '#thePager', viewrecords: true,
                loadui: "block", multiselect: false, height: 'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });
            $grid.jqGrid('navGrid', '#thePager', { edit: false, add: false, del: false, search: false, refresh: false });
            $grid.jqGrid('filterToolbar', { autosearch: true, searchOnEnter: true });

            $grid.jqGrid('navButtonAdd', "#thePager", {
                caption: "Clear Filters", title: "Clear Search", buttonicon: 'ui-icon-refresh',
                onClickButton: function () {
                    mygrid[0].clearToolbar();
                }
            });

            $grid.jqGrid('navButtonAdd', "#thePager", {
                caption: "Refresh Data", title: "Refresh Data On Grid", buttonicon: 'ui-icon-arrowrefresh-1-s',
                onClickButton: function () {
                    mygrid[0].triggerToolbar();
                }
            });

        });


    </script>
    <style>


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Logins</span>
    </div>

    <div class="grid-wrapper">
        
        <table id="theGrid"></table>
        <div id="thePager"></div>

    </div>

</asp:Content>
