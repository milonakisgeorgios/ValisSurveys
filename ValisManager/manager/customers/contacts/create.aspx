<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="create.aspx.cs" Inherits="ValisManager.manager.customers.contacts.create" %>
<%@ Register src="contactsform.ascx" tagname="contactsform" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Add A New Contact to '<%:this.SelectedClient.Name %>'</span>
        <a class="form-header-link" href="../edit.aspx?ClientId=<%=this.SelectedClient.ClientId %>&<%=this.UrlSuffix %>">(back to customer details)</a>
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>

    <uc1:contactsform ID="contactsform1" runat="server" />
</asp:Content>
