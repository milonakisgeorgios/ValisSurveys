<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="add.aspx.cs" Inherits="ValisManager.manager.security.roles.add" %>
<%@ Register src="rolesform.ascx" tagname="rolesform" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

    </script>
    <style type="text/css">

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Add A New Role</span>
        <a class="form-header-link" href="list.aspx">(back to roles list)</a>
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>

    <uc1:rolesform ID="rolesform1" runat="server" />
    
</asp:Content>
