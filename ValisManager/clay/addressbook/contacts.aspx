<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="contacts.aspx.cs" Inherits="ValisManager.clay.addressbook.contacts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        
        .ui-jqgrid tr.ui-row-ltr td { border-bottom-color: #f1f1f1; border-right-color: #f1f1f1;}
        <!--
        .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }
        -->
        .ui-jqgrid .ui-jqgrid-view
        {
            font-size: .82em;
        }
        .ui-jqgrid tr.jqgrow td {font-size:1.12em}

        .rightButton
        {
            float: right;
        }

        .valisInputForm input
        {
            width: 250px;
            padding: 3px;
        }
        .valisInputForm textarea
        {
            width: 250px;
            padding: 3px;
        }
    </style>
    <script>
        
        $(document).ready(function () 
        {
            $('#contactForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 470, height: 500, buttons: { 'SaveBtn': { class: 'greenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Save %>', id: 'SaveBtn', click: OnContactFormOkButton }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelBtn', click: function () { $(this).dialog("close"); } } } });
            $grid = $('#theGrid');

            var actionsFormatter = function (cellvalue, options, rowObject) {
                var _html = '';
                _html += "<a href=\"javascript:OnGridEdit('" + rowObject.ContactId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                _html += "<a href=\"javascript:OnGridDelete('" + rowObject.ContactId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/deleteitemred.gif\" alt=\"delete\"/></a>";
                return _html;
            };
            
            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/ClientLists/Contacts/GetPage?ScId=' + theAccessToken + '&listId=<%=this.ClientListId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "ContactId" },
                colNames: ["Opt-Out","Bounced", "Email", "FirstName", "LastName", "Date Created", "Actions"],
                colModel: [
                        { name: 'IsOptedOut', width: 100, align: 'center', sortable: false, formatter: checkRedFormatter },
                        { name: 'IsBouncedEmail', width: 100, align: 'center', sortable: false, formatter: checkRedFormatter },
                        { name: 'Email', width: 240, align: 'left', sortable: true },
                        { name: 'FirstName', width: 140, align: 'left', sortable: true },
                        { name: 'LastName', width: 140, align: 'left', sortable: true },
                        { name: 'CreateDT', index: 'CreationDT', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } },
                        { name: 'actions', width: 160, sortable: false, align: 'center', formatter: actionsFormatter }
                ],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>', page:<%=PageNumber %>, 
                rowNum: <%=RowNum %>, rowList: [18, 25, 48],pager: "#thePager", viewrecords: true,
                loadui: "block ", hoverrows:false , gridview: false, height:'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });

        });

        function OnGridDelete(contactId)
        {
            $.ajax({
                url: theManagerPath + 'services/api/ClientLists/Contacts/GetById?ScId=' + theAccessToken + '&contactId='+contactId, dataType: 'json', 
                success: function(data)
                {
                    showDelete(
                        'Do You Want to Delete Contact <b>"'+data.LastName+', '+ data.FirstName +'"</b> ?', 
                        function(){
                            $.ajax({
                                url: theManagerPath + 'services/api/ClientLists/Contacts/Delete?ScId=' + theAccessToken + '&contactId='+contactId, dataType: 'json',
                                success: function(data) 
                                { 
                                    ReloadGrid('#theGrid'); 
                                }
                            });
                        },"Delete a contact"
                    );
                }
            });
        }
        function OnGridEdit(contactId)
        {
            $.ajax({
                url: theManagerPath + 'services/api/ClientLists/Contacts/GetById?ScId=' + theAccessToken + '&contactId='+contactId,
                dataType: 'json', 
                success: function(data)
                {
                    OpenContactForm(false, data);
                }
            });
        }
        function OnCreateButton(contactId)
        {
            OpenContactForm(true, null);
        }
        
        function ResetContactForm()
        {   
            $('.formFooter').html('');
            $('#fmOrganization').val('');
            $('#fmTitle').val('');
            $('#fmDepartment').val('');
            $('#fmFirstName').val('');
            $('#fmLastName').val('');
            $('#fmEmail').val('');
            $('#fmComment').val('');
            $('#fmContactId').val('');
        }
        function OpenContactForm(createNew, data)
        {
            $('#contactForm').removeAttr('createNew');
            ResetContactForm();

            
            if (createNew == false)
            {
                $('#fmContactId').val(data.ContactId);
                $('#fmOrganization').val(data.Organization);
                $('#fmTitle').val(data.Title);
                $('#fmDepartment').val(data.Department);
                $('#fmFirstName').val(data.FirstName);
                $('#fmLastName').val(data.LastName);
                $('#fmEmail').val(data.Email);
                $('#fmComment').val(data.Comment);

                $('.formFooter').html('Created at ' + data.CreateDT + '. Updated at ' + data.LastUpdateDT + '.');
                $("#contactForm").dialog("option", "title", 'Update Contact ' + (data.FirstName != null ? data.FirstName : '') + (data.LastName!= null ? ', '+ data.LastName : '') + ':');
            }
            else
            {
                $("#contactForm").dialog("option", "title", 'Create a new Contact:');
            }

            $('#contactForm').dialog({ position: { my: "top", at: "center top", of: window } }).attr("createNew", createNew).dialog("open");
        }
        function OnContactFormOkButton()
        {
            var createNew = $('#contactForm').attr("createNew");
        
            var value = $('#fmEmail').val();
            if (value == '' || value == null || value == undefined)
            {
                alert('You must provide an Email Address for this Contact!');
                return;
            }

            var _data = '';
            _data = _data + '&Organization='+ escape($('#fmOrganization').val());
            _data = _data + '&Title='+ escape($('#fmTitle').val());
            _data = _data + '&Department='+ escape($('#fmDepartment').val());
            _data = _data + '&FirstName='+ escape($('#fmFirstName').val());
            _data = _data + '&LastName='+ escape($('#fmLastName').val());
            _data = _data + '&Email='+ escape($('#fmEmail').val());
            _data = _data + '&Comment='+ escape($('#fmComment').val());
            _data = _data + '&ContactId='+ escape($('#fmContactId').val());
        
            var _url = theManagerPath + 'services/api/ClientLists/Contacts/' + (createNew == "true" ? 'Create' : 'Update') + '?ScId=' + theAccessToken;
        
            $.ajax({
                url: _url, data: _data, async: false,
                success: function (data)
                {
                    $('#contactForm').dialog('close');
                    ReloadGridAndKeepPage($('#theGrid'));
                },
                error: function (jqXHR, textStatus, errorThrown)
                {
                    showException(jqXHR, textStatus, errorThrown);
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">  

    <div class="pageTitle">
        <h1>Contacts for “<%: this.SelectedClientList.Name %>” List</h1>
        <a class="greybutton rightButton" id="returnButton" href="~/clay/addressbook/addressbook.aspx" runat="server">&lt;&lt;&nbsp;Back to Address Book</a>
    </div>
    <div class="pageTools">

    </div>

    
    <table id="theGrid"></table>
    <div id="thePager"></div>


    
    <div id="contactForm" class="valisInputForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow"><label for="fmOrganization">Organization</label><input type="text" name="fmOrganization" id="fmOrganization" /></div>
            <div class="formRow"><label for="fmTitle">Title</label><input type="text" name="fmTitle" id="fmTitle" /></div>
            <div class="formRow"><label for="fmDepartment">Department</label><input type="text" name="fmDepartment" id="fmDepartment" /></div>
            <div class="formRow"><label for="fmFirstName">FirstName</label><input type="text" name="fmFirstName" id="fmFirstName" /></div>
            <div class="formRow"><label for="fmLastName">LastName</label><input type="text" name="fmLastName" id="fmLastName" /></div>
            <div class="formRow"><label for="fmEmail">Email</label><input type="text" name="fmEmail" id="fmEmail" /><%=GetRequiredIcon() %></div>
            <div class="formRow"><label for="fmComment">Comment</label><textarea name="fmComment" id="fmComment"></textarea></div>
        </div>
        <input type="hidden" id="fmContactId" name="fmContactId" />
        <span class="formFooter"></span>
    </div>

</asp:Content>
