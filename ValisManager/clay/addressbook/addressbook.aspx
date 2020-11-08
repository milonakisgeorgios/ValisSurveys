<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="addressbook.aspx.cs" Inherits="ValisManager.clay.addressbook.addressbook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        
        div.addressbook-wrapper
        {
            padding: 12px;
            margin-left: auto;
            margin-right: auto;
            width: 1095px;
        }
        div.pageTitle {
            background-image: url(/content/images/contacts.png);
            background-position: 0px 6px;
            background-repeat: no-repeat;
            padding-left: 38px;
        }
        a.actionLinks 
        {
            text-decoration: none;
            margin-left: 6px;
            margin-right: 8px;
            color: #0077b5;
            font-size: .86em;
        }
        a.actionLinks2 
        {
            text-decoration: none;
            margin-left: 6px;
            margin-right: 8px;
            color: #0077b5;
            font-size: 1em;
        }
        span.actionLinks
        {
            margin-left: 6px;
            margin-right: 8px;
            color: #999999;
            font-size: .86em;
        }
        .ui-jqgrid tr.ui-row-ltr td { border-bottom-style: none; border-right-style: none;}
        <!--
        .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }
        -->
        .ui-jqgrid .ui-jqgrid-view
        {
            font-size: .87em;
        }
        .ui-jqgrid tr.jqgrow td {font-size:1.2em}
        
        div.valisInputForm label
        {
            width: auto;
        }

        .rightButton
        {
            float: right;
        }
    </style>
    <script>

        

        
        function nameformatter(cellvalue, options, rowObject)
        {
            if(rowObject.TotalContacts == 0)
            {
                return '<a class="actionLinks2" href="javascript:OnAddList(\''+rowObject.ListId+'\');">'+rowObject.Name+'</a>';
            }
            else
            {
                return '<a class="actionLinks2" href="contacts.aspx?listId='+rowObject.ListId+'">'+rowObject.Name+'</a>';
            }
        }
        function actionsFormatter(cellvalue, options, rowObject)
        {
            var _html = '';
            _html += '<a class="actionLinks" href="javascript:OnAddList(\''+rowObject.ListId+'\');">Add</a>';
            if(rowObject.TotalContacts>0)
            {
                _html += '<a class="actionLinks" href="javascript:OnRemoveList(\''+rowObject.ListId+'\');">Remove</a>';
                _html += '<a class="actionLinks" href="javascript:OnDownloadList(\''+rowObject.ListId+'\');">Download</a>';
            }
            else
            {
                _html += '<span class="actionLinks">Remove</span>';
                _html += '<span class="actionLinks">Download</span>';
            }
            _html += '<a class="actionLinks" href="javascript:OnDeleteList(\''+rowObject.ListId+'\');">Delete</a>';

            return _html;
        }

        $(document).ready(function () {
            $('#newListForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 400, height: 235, buttons: { 'CreateNewList': { class: 'greenbutton', text: 'Create New List', id: 'CreateNewList', click: OnCreateNewListButton }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'saveCancelBtn', click: function () { $(this).dialog("close"); } } } });
            
            $('#ListsGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + "services/api/ClientLists/GetPage?ScId=" + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "ListId" },
                colNames: ["List Name","TotalContacts","Date Created", "Actions"],
                colModel: [
                        { name: 'Name', width: 485, align: 'left', sortable: true, formatter: nameformatter },
                        { name: 'TotalContacts', width: 160, align: 'center', sortable: true },
                        { name: 'CreationDT', index: 'CreationDT', align: 'center', width: 160, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } },
                        { name: 'actions', width: 270, sortable: false, align: 'center', formatter: actionsFormatter }
                ],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>', page:<%=PageNumber %>, 
                rowNum: <%=RowNum %>, rowList: [],pager: "#ListsPager", viewrecords: false,
                loadui: "block ", hoverrows:false , gridview: false, pginput: false,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#ListsGrid', _xml, ts, er); }
            });



        });

        function OnCreateNewList()
        {
            $("#newListForm").dialog({ position: { my: "center", at: "center", of: window } }).dialog("open");
        }
        function OnCreateNewListButton()
        {
            var value = $('#listName').val();
            if (value == '' || value == null || value == undefined) {
                //alert('You must provide "Answer Choices" for this question!');
                //$("#QuestionChoices").fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);
                $("#listName").animateHighlight("#fd2525", 500);
                $('#listName').focus();
                return false;
            }

            var _data = 'listName=' + escape($('#listName').val());
            $.ajax({
                url: theManagerPath + 'services/api/ClientLists/Create?ScId=' + theAccessToken, data: _data, dataType: 'json',
                success: function (data) {
                    OnAddList(data.ListId);
                }
            });
        }

        function OnAddList(listId)
        {
            var url = theManagerPath +"clay/addressbook/add.aspx?listId="+listId;
            window.location = url;
        }
        function OnRemoveList(listId)
        {
            var url = theManagerPath +"clay/addressbook/remove.aspx?listId="+listId;
            window.location = url;
        }
        function OnDownloadList(listId)
        {
            var url = theManagerPath +"clay/addressbook/export.aspx?listId="+listId;
            window.location = url;
        }
        function OnDeleteList(listId)
        {
            $.ajax({
                url: theManagerPath + 'services/api/ClientLists/GetById?ScId=' + theAccessToken + '&listId=' + listId, dataType: 'json',
                success: function (selectedList) {
                    if(selectedList.TotalContacts == 0)
                    {
                        //empty list
                        showDelete('Are you sure you want to delete the list "' + selectedList.Name+'"?', function(){
                            $.ajax({
                                url: theManagerPath + 'services/api/ClientLists/Delete?ScId=' + theAccessToken + '&listId=' + listId, dataType: 'json',
                                success: function (data) {
                                    $('#ListsGrid').trigger('reloadGrid');
                                }
                            });
                        }, 'Delete an empty list.');
                    }
                    else
                    {
                        //non empty list
                        showDelete('<h2>The list "'+selectedList.Name+'" <b>contains '+selectedList.TotalContacts+' contacts</b>.</h2><div>Are you sure you want to delete the list?</div>', function(){
                            $.ajax({
                                url: theManagerPath + 'services/api/ClientLists/Delete?ScId=' + theAccessToken + '&listId=' + listId, dataType: 'json',
                                success: function (data) {
                                    $('#ListsGrid').trigger('reloadGrid');
                                }
                            });
                        }, 'Delete this list and contacts');
                    }
                }
            });
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="addressbook-wrapper">

        <div class="pageTitle">
            <h1>Address Book</h1>
            <a title="create a list" class="greenbutton rightButton" id="createList" href="javascript:OnCreateNewList()">+ Add a new List</a>
        </div>
        <div class="pageTools">

        </div>

        <table id="ListsGrid"></table>
        <div id="ListsPager"></div>

    
        <div id="newListForm" class="valisInputForm" title="Create New List:" style="display: none">
            <div class="formWrapper">
                <div class="formRow" id="PositionWrapper">
                        <label for="listName">New List Name:</label><input type="text" name="listName" id="listName" required/><img class="required-image" alt="required" src="/content/images/requiredIcon1.gif">
                </div>
            </div>
        </div>
    </div>

</asp:Content>
