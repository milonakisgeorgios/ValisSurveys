<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="list.aspx.cs" Inherits="ValisManager.manager.emails.knownemails.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        input[id=optedOutEmail],input[id=bouncedEmail],input[id=verifiedEmail] {
            width: 440px;
            padding: 8px;
            margin: 12px;
        }
    </style>
    <script>
        $(document).ready(function ()
        {
            $('#TheOptedOutForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, draggable: true, width: 540, buttons: { 'Addemail': { class: 'lightgreenbutton', text: 'Add Email Address', id: 'AddOptedOutBtn', click: OnAddOptedOutBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelOptedOutBtn', click: function () { $(this).dialog("close"); } } } });
            $('#TheBouncedForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, draggable: true, width: 540, buttons: { 'Addemail': { class: 'lightgreenbutton', text: 'Add Email Address', id: 'AddOptedOutBtn', click: OnAddBouncedBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelOptedOutBtn', click: function () { $(this).dialog("close"); } } } });
            $('#TheVerifiedForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, draggable: true, width: 540, buttons: { 'Addemail': { class: 'lightgreenbutton', text: 'Add Email Address', id: 'AddOptedOutBtn', click: OnAddVerifiedBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelOptedOutBtn', click: function () { $(this).dialog("close"); } } } });

            $grid = $('#theGrid');
            
            var actionsFormatter = function (cellvalue, options, rowObject) {
                var _html = '';
                _html += "<a href=\"javascript:OnGridEdit('" + rowObject.EmailId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                _html += '&nbsp;&nbsp;';
                _html += "<a href=\"javascript:OnGridDelete('" + rowObject.EmailId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/deleteitemred.gif\" alt=\"delete\"/></a>";
                return _html;
            };
            
            var mygrid = $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/KnownEmails/GetPage?ScId=' + theAccessToken,
                datatype: "json", jsonReader: { repeatitems: false, id: "ProfileId" },
                colNames: ['', 'Client', 'EmailAddress', 'RegisterDt', 'IsVerified', 'IsOptedOut', 'IsBounced'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter,search: false },
                        { name: 'ClientName', index: 'Name', width: 300, align: 'center' },
                        { name: 'EmailAddress', index: 'EmailAddress', width: 300, align: 'center' },
                        { name: 'RegisterDt', index: 'RegisterDt', width: 200, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' }, searchoptions: { dataInit: function (el) { $(el).datepicker({ dateFormat: 'dd/mm/yy', onSelect: function (dateText, inst) { mygrid[0].triggerToolbar(); } }); } } },

                        { name: 'IsVerified', index: 'IsVerified', width: 100, align: 'center', formatter: builtinFormatter, stype: 'select', editoptions: { value: ':All;1:Yes;0:No' }  },
                        { name: 'IsOptedOut', index: 'IsOptedOut', width: 100, align: 'center', formatter: builtinFormatter, stype: 'select', editoptions: { value: ':All;1:Yes;0:No' }  },
                        { name: 'IsBounced', index: 'IsBounced', width: 100, align: 'center', formatter: builtinFormatter, stype: 'select', editoptions: { value: ':All;1:Yes;0:No' }  }],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>',page:<%=PageNumber %>,
                rowNum: <%=RowNum %>, rowList: [18, 32, 48], pager: '#thePager', viewrecords: false,
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


            $('#addOptedOutBtn').bind('click', function ()
            {
                $('#TheOptedOutForm').dialog({ position: { my: "bottom", at: "center", of: window } }).dialog("open");
            });
            $('#addBouncedBtn').bind('click', function ()
            {
                $('#TheBouncedForm').dialog({ position: { my: "bottom", at: "center", of: window } }).dialog("open");
            });
            $('#addVerifiedBtn').bind('click', function ()
            {
                $('#TheVerifiedForm').dialog({ position: { my: "bottom", at: "center", of: window } }).dialog("open");
            });
        });

        
        function OnGridDelete(rowId) 
        {
            $.ajax({
                url: theManagerPath + 'services/api/KnownEmails/GetById?ScId=' + theAccessToken + '&emailId=' + rowId, dataType: 'json',
                success: function (selectedEmail) {
                    
                    showDelete('Are you sure you want to delete this email ' + selectedEmail.EmailAddress, function(){                    
                        $.ajax({
                            url: theManagerPath + 'services/api/KnownEmails/Delete?ScId=' + theAccessToken + '&emailId=' + rowId, dataType: 'json',
                            success: function (data) {
                                $('#theGrid').trigger('reloadGrid');
                            }
                        });
                    }, 'Delete this known email!');

                }
            });
        }
        function OnGridEdit(rowId) 
        {
            $.ajax({
                url: theManagerPath + 'services/api/KnownEmails/GetById?ScId=' + theAccessToken + '&emailId=' + rowId, dataType: 'json',
                success: function (selectedEmail) {

                    alert(selectedEmail.EmailAddress);
                
                }
            });
        }

        function OnAddOptedOutBtn()
        {
            var email = document.getElementById('optedOutEmail').value;
            if (validateEmail(email) == false)
            {
                document.getElementById('optedOutEmail').value = '';
                alert("Invalid email format! Please try again.");
                return;
            }

            //ελέγχουμε εάν υπάρχει ήδη


            //το αποθηκεύουμε
            //var _data = 'emailaddress=' + encodeURIComponent(email);
            //$.ajax({
            //    type: "POST", dataType: "json", url: '/services/api/KnownEmails/AddOptedOut?ScId=' + theAccessToken, data: _data,
            //    error: function(jqXHR, textStatus, errorThrown){
            //        alert(errorThrown);
            //    },
            //    success: function (result) {
            //        alert('success');
            //    }
            //});
        }
        function OnAddBouncedBtn()
        {
            var email = document.getElementById('bouncedEmail').value;
            if (validateEmail(email) == false)
            {
                document.getElementById('bouncedEmail').value = '';
                alert("Invalid email format! Please try again.");
                return;
            }

            var _data = 'emailaddress=' + encodeURIComponent(email);


        }
        function OnAddVerifiedBtn()
        {
            var email = document.getElementById('verifiedEmail').value;
            if (validateEmail(email) == false)
            {
                document.getElementById('verifiedEmail').value = '';
                alert("Invalid email format! Please try again.");
                return;
            }

            var _data = 'emailaddress=' + encodeURIComponent(email);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">KnownEmails</span>
        <button class="form-button form-right-button add-button" type="button" id="addOptedOutBtn">
            <span class="ui-button-text">Add OptedOut Email</span>
        </button>
        <button class="form-button form-right-button add-button" type="button" id="addBouncedBtn">
            <span class="ui-button-text">Add Bounced Email</span>
        </button>
        <button class="form-button form-right-button add-button" type="button" id="addVerifiedBtn">
            <span class="ui-button-text">Add Verified Email</span>
        </button>
    </div>
    <div class="grid-wrapper">

        <table id="theGrid"></table>
        <div id="thePager"></div>

    </div>

    <div id="TheOptedOutForm" title="Add OptedOut Email" class="valisInputForm" style="display: none">
        <h2>Enter an Email Address as an system wide OptedOut Email Address:</h2>
        <div class="formWrapper">
            <p>
                <input type="email" name="optedOutEmail" id="optedOutEmail" />
            </p>
        </div>
    </div>
    
    <div id="TheBouncedForm" title="Add Bounced Email" class="valisInputForm" style="display: none">
        <h2>Enter an Email Address as an system wide Bounced Email Address:</h2>
        <div class="formWrapper">
            <p>
                <input type="email" name="bouncedEmail" id="bouncedEmail" />
            </p>
        </div>
    </div>
    
    <div id="TheVerifiedForm" title="Add Verified Email" class="valisInputForm" style="display: none">
        <h2>Enter an Email Address as an system wide Verified Email Address:</h2>
        <div class="formWrapper">
            <p>
                <input type="email" name="verifiedEmail" id="verifiedEmail" />
            </p>
        </div>
    </div>
</asp:Content>
