<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="create.aspx.cs" Inherits="ValisManager.manager.customers.payments.create" ClientIDMode="Static" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {

            $('#PaymentDate').datepicker({
                buttonImage: "/content/images/datepicker-20.png",
                showOn: "both",
                buttonImageOnly: true,
                dateFormat: "yy-mm-dd"
            });
            $('#Quantity').onlyUnSignedIntegers();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Add A New Payment for <%: this.SelectedClient.Name %></span>
        <a class="form-header-link" href="../edit.aspx?ClientId=<%=this.SelectedClient.ClientId %>&<%=this.UrlSuffix %>">(back to customer details)</a>
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>

    <div class="form-wrapper">
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
            <asp:Label ID="Label4" runat="server" Text="CreditType" AssociatedControlID="frmResourceType"></asp:Label><asp:DropDownList ID="frmResourceType" runat="server" Width="160px">
                <asp:ListItem Value="1">EmailType</asp:ListItem>
                <asp:ListItem Value="2">ResponseType</asp:ListItem>
                <asp:ListItem Value="3">ClickType</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-line">
            <asp:Label ID="Label5" runat="server" Text="Quantity" AssociatedControlID="Quantity"></asp:Label><asp:TextBox ID="Quantity" runat="server" required="required" aria-required=”true” MaxLength="8"></asp:TextBox><%=GetRequiredIcon() %>
        </div>

    </div>
</asp:Content>
