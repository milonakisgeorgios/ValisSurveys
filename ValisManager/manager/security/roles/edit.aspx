<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="false" CodeBehind="edit.aspx.cs" Inherits="ValisManager.manager.security.roles.edit" %>
<%@ Register src="rolesform.ascx" tagname="rolesform" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnDelete()
        {
            showDelete("Are you sure you want to delete this item?", function () {
                <%=GetDeleteButtonHandler%>;
            }, "Are you sure?");

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Edit '<%: this.SelectedRole != null ? this.SelectedRole.Name : string.Empty %>' Role</span>
        <a class="form-header-link" href="list.aspx">(back to roles list)</a>
        <asp:Button ID="deleteBtn" runat="server" CssClass="form-button form-right-button delete-button" Text="Delete" OnClick="deleteBtn_Click" OnClientClick="return OnDelete();" formnovalidate="formnovalidate" />
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
    </div>

    <uc1:rolesform ID="rolesform1" runat="server" />

</asp:Content>
