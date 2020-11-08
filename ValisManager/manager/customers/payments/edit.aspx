<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="ValisManager.manager.customers.payments.edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {

            $('#tabs').tabs({ });


            var resourceTypeFrmtr = function (cellvalue, options, rowObject) {
                if (cellvalue == '0')
                    return 'None';
                if (cellvalue == '1')
                    return 'emails';
                if (cellvalue == '2')
                    return 'responses';
                if (cellvalue == '3')
                    return 'clicks';
                return '??';
            };
            var collectorStatusFrmtr = function (cellvalue, options, rowObject) {
                if (cellvalue == '0')
                    return 'New';
                if (cellvalue == '1')
                    return 'Open';
                if (cellvalue == '2')
                    return 'Close';
                return '??';
            };
            var collectorTypeFrmtr = function (cellvalue, options, rowObject) {
                if (cellvalue == '0')
                    return 'WebLink';
                if (cellvalue == '1')
                    return 'Email';
                if (cellvalue == '2')
                    return 'Website';
                return '??';
            };
            var actionsFrmtr = function (cellvalue, options, rowObject) {
                return '';
            };

            $('#theChargesGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Payments/GetCharges?ScId=' + theAccessToken + '&paymentId=<%= this.SelectedPayment.PaymentId%>',
                datatype: "json",
                jsonReader: { repeatitems: false, id: "CollectorPaymentId" },
                colNames: ['', 'Survey', 'Collector', 'CollectorType', 'Status', 'Responses', 'Χρέωση με', 'CreationDT', 'QuantityReserved', 'QuantityUsed'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFrmtr },

                        { name: 'SurveyTitle', sortable: false, width: 200, align: 'center' },
                        { name: 'CollectorTitle', sortable: false, width: 220, align: 'center' },
                        { name: 'CollectorType', sortable: false, width: 80, align: 'center', formatter: collectorTypeFrmtr },
                        { name: 'Status', sortable: false, width: 80, align: 'center', formatter: collectorStatusFrmtr },
                        { name: 'Responses', sortable: false, width: 70, align: 'center' },
                        { name: 'CreditType', sortable: false, width: 80, align: 'center', formatter: resourceTypeFrmtr },
                        { name: 'CreationDT', sortable: false, width: 160, align: 'center', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y H:i' } },
                        { name: 'QuantityReserved', sortable: false, width: 100, align: 'center' },
                        { name: 'QuantityUsed', sortable: false, width: 100, align: 'center' },
                ],
                sortname: "UseOrder", sortorder: 'asc',
                rowNum: 10, pager: '#theChargesPager', viewrecords: false,
                loadui: "enable", multiselect: false, height: 250,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theChargesGrid', _xml, ts, er); }
            });


        });


        function OnDelete() {
            showDelete("Are you sure you want to delete this payment?", function () {
                <%=GetDeleteButtonHandler%>;
            }, "Are you sure?");

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="form-header">
        <span class="form-header-title">Edit Payment</span>
        <a class="form-header-link" href="../edit.aspx?ClientId=<%=this.SelectedPayment.Client %>&<%=this.UrlSuffix %>">(back to customer details)</a>
        <asp:Button ID="deleteBtn" runat="server" CssClass="form-button form-right-button delete-button" Text="Delete" OnClick="deleteBtn_Click" OnClientClick="return OnDelete();" formnovalidate="formnovalidate" />
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
        
    </div>

    <div class="form-wrapper">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Payment</a></li>
                <li><a href="#tabs-2">Charges</a></li>
            </ul>
            <div id="tabs-1">
                <p class="help">
                    Εδώ έχουμε τα στοιχεία της Πληρωμής, και το σύστημα, μας επιτρέπει να αλλάξουμε κάποια στοιχεία αναλόγως με το εάν η Πληρωμή έχει χρησιμοποιηθεί ή όχι.
                </p>
                <div class="form-line">
                    <asp:Label ID="Label1" runat="server" Text="PaymentDate" AssociatedControlID="PaymentDate"></asp:Label><asp:TextBox ID="PaymentDate" runat="server" required="required" aria-required=”true” MaxLength="10"></asp:TextBox><%=GetRequiredIcon() %>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label2" runat="server" Text="Comment" AssociatedControlID="Comment"></asp:Label><asp:TextBox ID="Comment" runat="server" Height="150px" TextMode="MultiLine" Width="450px"></asp:TextBox>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label3" runat="server" Text="CustomCode1" AssociatedControlID="CustomCode1"></asp:Label><asp:TextBox ID="CustomCode1" runat="server" Width="450px" ></asp:TextBox>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label6" runat="server" Text="CustomCode2" AssociatedControlID="CustomCode2"></asp:Label><asp:TextBox ID="CustomCode2" runat="server" Width="450px" ></asp:TextBox>
                </div>
        
                <div class="form-line">
                    <asp:Label ID="Label4" runat="server" Text="ResourceType" AssociatedControlID="frmResourceType"></asp:Label><asp:DropDownList ID="frmResourceType" runat="server" Width="160px">
                        <asp:ListItem Value="1">EmailType</asp:ListItem>
                        <asp:ListItem Value="2">ResponseType</asp:ListItem>
                        <asp:ListItem Value="3">ClickType</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label7" runat="server" Text="QuantityUsed" AssociatedControlID="QuantityUsed"></asp:Label><asp:TextBox ID="QuantityUsed" runat="server" required="required" aria-required=”true” MaxLength="8" ReadOnly="True" BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#666666"></asp:TextBox>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label5" runat="server" Text="Quantity" AssociatedControlID="Quantity"></asp:Label><asp:TextBox ID="Quantity" runat="server" required="required" aria-required=”true” MaxLength="8"></asp:TextBox><%=GetRequiredIcon() %>
                </div>
            </div>
            <div id="tabs-2">
                <p class="help">
                    Στο παρακάτω πίνακα παρουσιάζονται οι χρεώσεις που έχουν πραγματοποιηθεί με την συγκεκριμένη Πληρωμή. Οι πληρωμές χρησιμοποιούνται απο τον Πελάτη όταν δημιουργεί ένα νέον Collector. Η δημιουργία ενός collector απαιτεί να συνδεθεί με μία η περισσότερες πηρωμές έτσι ώστε το σύστημα να μπορέσει να χρεώσει τον Πελάτη για τους πόρους που καταλώνει η ερευνα που διεξάγει.
                </p>
                <table id="theChargesGrid"></table>
                <div id="theChargesPager"></div>
            </div>
        </div>
    </div>

</asp:Content>
