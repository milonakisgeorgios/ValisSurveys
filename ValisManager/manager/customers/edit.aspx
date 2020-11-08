<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="edit.aspx.cs" Inherits="ValisManager.manager.customers.edit" %>
<%@ Register src="../../clay/controls/balanceStatistics.ascx" tagname="balanceStatistics" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ui-subgrid div.tablediv
        {
            padding: 8px;
        }
        .ui-subgrid div.tablediv td
        {
            text-align: center;
            padding: 4px;
            color: #001727;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('#tabs').tabs({
                activate: function (event, ui) {
                    $.cookie("customers.edit", $("#tabs").tabs("option", "active"));
                },
                active: $.cookie("customers.edit")
            });



            var actionsFrmtr1 = function (cellvalue, options, rowObject) {
                _html = '';
                _html += "<a href=\"javascript:OnContactEdit('" + rowObject.UserId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                return _html;
            };

            $('#theGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Clients/GetClientUsers?ScId=' + theAccessToken + '&ClientId=<%= this.SelectedClient.ClientId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "UserId" },
                colNames: ['', 'FirstName', 'LastName', 'Email', 'Role', 'IsActive', 'IsLockedOut', 'LastLogin', 'CreationDT'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFrmtr1 },
                        { name: 'FirstName', sortable: false, width: 140 },
                        { name: 'LastName', sortable: false, width: 160 },
                        { name: 'Email', sortable: false, width: 140 },
                        { name: 'RoleName', sortable: false, width: 120 },
                        { name: 'IsActive', index: 'IsActive', width: 80, align: 'center', formatter: isActiveFormatter },
                        { name: 'IsLockedOut', index: 'IsLockedOut', width: 80, align: 'center', formatter: isActiveFormatter },
                        { name: 'LastLoginDate', index: 'LastLoginDate', width: 120, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } },
                        { name: 'CreateDT', index: 'CreateDT', width: 120, formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } }],
                sortname: "LastName", sortorder: 'asc',
                rowNum: 10, pager: '#thePager', viewrecords: false,
                loadui: "enable", multiselect: false, height: 250,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });


            var actionsFrmtr2 = function (cellvalue, options, rowObject) {
                _html = '';
                _html += "<a href=\"javascript:OnEditPayment('" + rowObject.PaymentId + "')\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
                return _html;
            };
            var restCreditsFormatter = function (cellvalue, options, rowObject) {
                
                return '<b style="font-size: 1.12em;">' + cellvalue + '</b>';
            }

            <%if(this.SelectedClient.UseCredits) {%>
            var OnLoadCompletePaymentGrid = function()
            {
                var cRows = this.rows.length;
                for (var iRow = 0; iRow < cRows; iRow++) {
                    var row = this.rows[iRow];
                    if ($(row).hasClass("jqgrow"))
                    {
                        /*Η εβδομη (7) κολώνα είναι η IsActive, που μας δείχνει εάν υπάρχει υπόλοιπο στην συγκεκριμένη πληρωμή*/
                        var rest = $(row.cells[7]).text();
                        if(rest !='0')
                        {
                            $(row).css('background', '#bfcfb5');
                        }
                        else
                        {

                        }
                    }
                }
            }

            $('#thePaymentsGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Payments/GetPaymentsView1?ScId=' + theAccessToken + '&ClientId=<%= this.SelectedClient.ClientId%>',
                datatype: "json",
                jsonReader: {
                    repeatitems: false,
                    id: "PaymentId",
                    subgrid: {
                                root: "rows", 
                                repeatitems: false,
                                cell: "cell"
                            }
                },
                colNames: ['', 'PaymentDate', 'TotalCredits', 'Reserved', 'Used', 'Balance', 'IsActive', 'Responses'],
                colModel: [
                        { name: 'actions', width: 60, sortable: false, align: 'center', formatter: actionsFrmtr2 },
                        { name: 'PaymentDate', index: 'PaymentDate', width: 140, align: 'center', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y' } },
                        { name: 'TotalCredits', sortable: false, align: 'center', width: 160, },
                        { name: 'QuantityReserved', sortable: false, align: 'center', width: 140 },
                        { name: 'UsedCredits', sortable: false, align: 'center', width: 140 },
                        { name: 'RestCredits', sortable: false, align: 'center', width: 140, formatter: restCreditsFormatter },
                        { name: 'IsActive', sortable: false, align: 'center', width: 140, hidden: true, formatter: isActiveFormatter },
                        { name: 'Responses', sortable: false, align: 'center', width: 140 }
                ],
                sortname: "PaymentDate", sortorder: 'asc',
                rowNum: 10, pager: '#thePaymentsPager', viewrecords: false,
                loadui: "enable", multiselect: false, height: 250,
                subGrid: true,
                subGridUrl: theManagerPath + 'services/api/Payments/GetChargesForSubgrid?ScId=' + theAccessToken,
                subGridModel: [{
                    name: ['Collector', 'Reserved', 'Used', 'FirstCharge', 'LastCharge','Responses'],
                    width: [340, 80, 80,80,120,120],
                    align: ['left', 'right', 'center', 'center', 'center', 'center'],
                    mapping: ['CollectorName', 'QuantityReserved', 'QuantityUsed', 'FirstChargeDt', 'LastChargeDt','Responses']
                }],
                loadComplete: OnLoadCompletePaymentGrid,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#thePaymentsGrid', _xml, ts, er); }
            });
            <%} %>
        });

        function OnAddNewContact()
        {
            window.location.href = "contacts/create.aspx?ClientId=<%= this.SelectedClient.ClientId%>&<%=this.UrlSuffix %>";
        }

        function OnContactEdit(rowId) {
            window.location.href = "contacts/edit.aspx?ClientId=<%= this.SelectedClient.ClientId%>&UserId=" + rowId + "&<%=this.UrlSuffix %>";
        }

        function OnDelete() {
            showDelete("Are you sure you want to delete this item?", function () {
                <%=GetDeleteButtonHandler%>;
            }, "Are you sure?");

            return false;
        }

        function OnAddNewPayment()
        {
            window.location.href = "payments/create.aspx?ClientId=<%= this.SelectedClient.ClientId%>&<%=this.UrlSuffix %>";
        }
        function OnEditPayment(rowId)
        {
            window.location.href = "payments/edit.aspx?ClientId=<%= this.SelectedClient.ClientId%>&PaymentId=" + rowId + "&<%=this.UrlSuffix %>";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="form-header">
        <span class="form-header-title">Edit Customers Details - <%: this.SelectedClient != null ? this.SelectedClient.Name : string.Empty %></span>
        <a class="form-header-link" href='<%=_UrlSuffix("list.aspx") %>'>(back to clients list)</a>
        <asp:Button ID="deleteBtn" runat="server" CssClass="form-button form-right-button delete-button" Text="Delete" OnClick="deleteBtn_Click" OnClientClick="return OnDelete();" formnovalidate="formnovalidate" />
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>
    <div class="form-wrapper">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Customer Info</a></li>
                <li><a href="#tabs-2">Customer Users</a></li>
                <li><a href="#tabs-3">Credits</a></li>
            </ul>
            <div id="tabs-1">
                <p class="help">
                    Εδω βλέπουμε τα γενικά στοιχεία του πελάτη μας, και μπορούμε να τα αλλάξουμε.
                </p>
                <div class="form-line">
                    <asp:Label ID="Label1" runat="server" Text="Code" AssociatedControlID="Code"></asp:Label><asp:TextBox ID="Code" runat="server"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label2" runat="server" Text="Name" AssociatedControlID="Name"></asp:Label><asp:TextBox ID="Name" runat="server" required="required"></asp:TextBox><%=GetRequiredIcon() %></div>
                <div class="form-line">
                    <asp:Label ID="Label13" runat="server" Text="cmProfile" AssociatedControlID="cmProfile"></asp:Label><asp:DropDownList ID="cmProfile" runat="server"></asp:DropDownList><%=GetRequiredIcon() %></div>
                <div class="form-line">
                    <asp:Label ID="Label3" runat="server" Text="Profession" AssociatedControlID="Profession"></asp:Label><asp:TextBox ID="Profession" runat="server"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label4" runat="server" Text="Country" AssociatedControlID="Country"></asp:Label><asp:DropDownList ID="Country" runat="server" EnableViewState="False"></asp:DropDownList><%=GetRequiredIcon() %></div>
                <div class="form-line">
                    <asp:Label ID="Label4a" runat="server" Text="TimeZone" AssociatedControlID="TimeZone"></asp:Label><asp:DropDownList ID="TimeZone" runat="server" EnableViewState="False"></asp:DropDownList><%=GetRequiredIcon() %></div>
                <div class="form-line">
                    <asp:Label ID="Label5" runat="server" Text="Prefecture" AssociatedControlID="Prefecture"></asp:Label><asp:TextBox ID="Prefecture" runat="server"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label6" runat="server" Text="Town" AssociatedControlID="Town"></asp:Label><asp:TextBox ID="Town" runat="server"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label7" runat="server" Text="Address" AssociatedControlID="Address"></asp:Label><asp:TextBox ID="Address" runat="server"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label8" runat="server" Text="Zip" AssociatedControlID="Zip"></asp:Label><asp:TextBox ID="Zip" runat="server"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label9" runat="server" Text="Telephone1" AssociatedControlID="Telephone1"></asp:Label><asp:TextBox ID="Telephone1" runat="server" TextMode="Phone"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label10" runat="server" Text="Telephone2" AssociatedControlID="Telephone2"></asp:Label><asp:TextBox ID="Telephone2" runat="server" TextMode="Phone"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label11" runat="server" Text="WebSite" AssociatedControlID="WebSite"></asp:Label><asp:TextBox ID="WebSite" runat="server" TextMode="Url"></asp:TextBox></div>
                <div class="form-line">
                    <asp:Label ID="Label12" runat="server" Text="Comment" AssociatedControlID="Comment"></asp:Label><asp:TextBox ID="Comment" runat="server" TextMode="MultiLine"></asp:TextBox></div>
            </div>
            <div id="tabs-2">
                <p class="help">
                    Όι χρήστες που ανήκουν στον Πελάτη μας, και μπορούν να κάνουν login στο σύστημά μας και να εργάζονται.
                </p>
                <table id="theGrid"></table>
                <div id="thePager"></div>

                <div class="under-grid-buttons">
                    <input type="button" class="form-button" value="Add new Contact" onclick="OnAddNewContact();" />
                </div>
            </div>
            <div id="tabs-3">
                <%if(this.SelectedClient.UseCredits) {%>
                <p class="help">
                    Ο παρακάτω πίνακας περιέχει τις καταχωρημένες πληρωμές του Πελάτη στο σύστημά μας. Υπάρχουν τριών ειδών πληρωμών: emails, responses και clicks. Μετά τη δημιουργία ενός survey ο Πελάτης πρέπει να δημιουργήσει και έναν τρόπο κοινοποίησης (collector) του ερωτηματολογίου στους αποδέκτες του. Τότε πρέπει ο Πελατης να επιλέξει το είδος της χρέωσης που επιθυμεί. Για να γίνει αυτό πρέπει πρώτα να υπάρχουν στο σύστημα καταχωρημένες πληρωμές για τον Πελάτη.
                </p>

                <div style="width: 1024px; margin: 24px 0px 36px 0px;">
                    <uc1:balanceStatistics ID="balanceStatistics1" runat="server" />
                </div>

                <table id="thePaymentsGrid"></table>
                <div id="thePaymentsPager">
                    
                </div>

                <div class="under-grid-buttons">
                    <input type="button" class="form-button" value="Add new Payment" onclick="OnAddNewPayment();" />
                </div>
                <%} else {%>
                <p class="notice">
                    Το profile του πελάτη δεν υποστηρίζει πληρωμές!
                </p>
                <%} %>
            </div>
        </div>
    </div>



</asp:Content>
