<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="rolesform.ascx.cs" Inherits="ValisManager.manager.security.roles.rolesform" %>
    <script>
        $(document).ready(function () {
            $("#tabs").tabs();
            $("#perm-tabs").tabs();

        });
    </script>
<style type="text/css">
    table.permsTable td
    {
        padding: 2px 4px 2px 4px;
    }
</style>
    <div class="form-wrapper">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">General Info</a></li>
                <li><a href="#tabs-2">System-Permissions</a></li>
                <li><a href="#tabs-3">Client-Permissions</a></li>
                <li><a href="#tabs-4">Survey-Permissions</a></li>
                <li><a href="#tabs-5">Other-Permissions</a></li>
            </ul>
            <div id="tabs-1">
                <div class="form-line">
                    <asp:Label ID="Label1" runat="server" Text="Name:" AssociatedControlID="Name"></asp:Label><asp:TextBox ID="Name" runat="server" placeholder="Enter the role's name" autofocus="autofocus" required="required"></asp:TextBox>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label2" runat="server" Text="Description:" AssociatedControlID="Description"></asp:Label><asp:TextBox ID="Description" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="form-line">
                    <asp:Label ID="Label3" runat="server" Text="Builtin Role:" AssociatedControlID="IsBuiltIn"></asp:Label><asp:CheckBox ID="IsBuiltIn" runat="server" Enabled="False" />
                </div>
                <div class="form-line">
                    <asp:Label ID="Label4" runat="server" Text="IsClient Role:" AssociatedControlID="IsClientRole"></asp:Label><asp:CheckBox ID="IsClientRole" runat="server" />
                </div>
            </div>
            <div id="tabs-2">
                <table class="permsTable">
                    <tr>
                        <td><asp:CheckBox ID="perm_ManageSystem" runat="server" Text="ManageSystem" Enabled="False" /></td>
                        <td><asp:CheckBox ID="perm_Developer" runat="server" Text="Developer" /></td>
                        <td><asp:CheckBox ID="perm_SystemService" runat="server" Text="SystemService" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_EnumerateSecurity" runat="server" Text="EnumerateSecurity" /></td>
                        <td><asp:CheckBox ID="perm_ManageSecurity" runat="server" Text="ManageSecurity" /></td>
                        <td><asp:CheckBox ID="perm_EnumerateSystemParameters" runat="server" Text="EnumerateSystemParameters" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_ManageSystemParameters" runat="server" Text="ManageSystemParameters" /></td>
                        <td><asp:CheckBox ID="perm_EnumerateBuildingBlocks" runat="server" Text="EnumerateBuildingBlocks" /></td>
                        <td><asp:CheckBox ID="perm_ManageBuidingBlocks" runat="server" Text="ManageBuidingBlocks" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_EnumerateThemes" runat="server" Text="EnumerateThemes" /></td>
                        <td><asp:CheckBox ID="perm_ManageThemes" runat="server" Text="ManageThemes" /></td>
                        <td><asp:CheckBox ID="perm_EnumerateRenders" runat="server" Text="EnumerateRenders" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_ManageRenders" runat="server" Text="ManageRenders" /></td>
                        <td><asp:CheckBox ID="perm_EnumerateClients" runat="server" Text="EnumerateClients" /></td>
                        <td><asp:CheckBox ID="perm_ManageClients" runat="server" Text="ManageClients" /></td>
                    </tr>
                </table>
            </div>
            <div id="tabs-3">
                <table class="permsTable">
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientFullControl" runat="server" Text="ClientFullControl" /></td>
                        <td><asp:CheckBox ID="perm_ClientUnlimitedQuota" runat="server" Text="ClientUnlimitedQuota" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientEnumerateUsers" runat="server" Text="ClientEnumerateUsers" /></td>
                        <td><asp:CheckBox ID="perm_ClientManageUsers" runat="server" Text="ClientManageUsers" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientEnumerateLists" runat="server" Text="ClientEnumerateLists" /></td>
                        <td><asp:CheckBox ID="perm_ClientManageLists" runat="server" Text="ClientManageLists" /></td>
                        <td><asp:CheckBox ID="perm_ClientImportLists" runat="server" Text="ClientImportLists" /></td>
                    </tr>
                </table>
            </div>
            <div id="tabs-4">
                <table class="permsTable">
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientEnumerateSurveys" runat="server" Text="ClientEnumerateSurveys" /></td>
                        <td><asp:CheckBox ID="perm_ClientPreviewSurveys" runat="server" Text="ClientPreviewSurveys" /></td>
                        <td><asp:CheckBox ID="perm_ClientCreateSurveys" runat="server" Text="ClientCreateSurveys" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientEditSurveys" runat="server" Text="ClientEditSurveys" /></td>
                        <td><asp:CheckBox ID="perm_ClientDeleteSurveys" runat="server" Text="ClientDeleteSurveys" /></td>
                        <td><asp:CheckBox ID="perm_ClientRunSurveys" runat="server" Text="ClientRunSurveys" /></td>
                    </tr>
                </table>
            </div>
            <div id="tabs-5">
                <table class="permsTable">
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientEnumerateCollectors" runat="server" Text="ClientEnumerateCollectors" /></td>
                        <td><asp:CheckBox ID="perm_ClientManageCollectors" runat="server" Text="ClientManageCollectors" /></td>
                        <td><asp:CheckBox ID="perm_ClientEnumerateAnswers" runat="server" Text="ClientEnumerateAnswers" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="perm_ClientManageAnswers" runat="server" Text="ClientManageAnswers" /></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>