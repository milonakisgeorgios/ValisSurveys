<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="create.aspx.cs" Inherits="ValisManager.manager.customers.create" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {

        });
    </script>
    <style type="text/css">

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="form-header">
        <span class="form-header-title">Add A New Customer</span>
        <a class="form-header-link" href=<%=_UrlSuffix("list.aspx") %>">(back to customers list)</a>
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>

    <div class="form-wrapper">
        <div class="form-line">
            <asp:Label ID="Label1" runat="server" Text="Code" AssociatedControlID="Code"></asp:Label><asp:TextBox ID="Code" runat="server"></asp:TextBox></div>
        <div class="form-line">
            <asp:Label ID="Label2" runat="server" Text="Name" AssociatedControlID="Name"></asp:Label><asp:TextBox ID="Name" runat="server" required="required"></asp:TextBox><%=GetRequiredIcon() %></div>
        <div class="form-line">
            <asp:Label ID="Label13" runat="server" Text="cmProfile" AssociatedControlID="cmProfile"></asp:Label><asp:DropDownList ID="cmProfile" runat="server"></asp:DropDownList><%=GetRequiredIcon() %></div>
        <div class="form-line">
            <asp:Label ID="Label3" runat="server" Text="Profession" AssociatedControlID="Profession"></asp:Label><asp:TextBox ID="Profession" runat="server"></asp:TextBox></div>
        <div class="form-line">
            <asp:Label ID="Label4" runat="server" Text="Country" AssociatedControlID="Country"></asp:Label><asp:DropDownList ID="Country" runat="server" EnableViewState="false"></asp:DropDownList><%=GetRequiredIcon() %></div>
        <div class="form-line">
            <asp:Label ID="Label4a" runat="server" Text="TimeZone" AssociatedControlID="TimeZone"></asp:Label><asp:DropDownList ID="TimeZone" runat="server" EnableViewState="false"></asp:DropDownList><%=GetRequiredIcon() %></div>
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
        <div class="form-line"></div>
    </div>  

</asp:Content>
