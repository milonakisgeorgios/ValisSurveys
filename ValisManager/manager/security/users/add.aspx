<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="ValisManager.manager.security.users.add" %>
<%@ Register src="usersform.ascx" tagname="usersform" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Add A New User</span>
        <a class="form-header-link" href="list.aspx">(back to users list)</a>
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>

    <uc1:usersform ID="usersform1" runat="server" />
</asp:Content>
