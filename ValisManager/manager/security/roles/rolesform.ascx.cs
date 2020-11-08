using System.Web.UI;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.manager.security.roles
{
    public partial class rolesform : ManagerUserControl
    {
        
        public void SetValues(VLRole role)
        {
            this.Name.Text = role.Name;
            this.Description.Text = role.Description;
            this.IsBuiltIn.Checked = role.IsBuiltIn;
            this.IsClientRole.Checked = role.IsClientRole;

            this.perm_ManageSystem.Checked = (role.Permissions & VLPermissions.ManageSystem) == VLPermissions.ManageSystem;
            this.perm_Developer.Checked = (role.Permissions & VLPermissions.Developer) == VLPermissions.Developer;
            this.perm_SystemService.Checked = (role.Permissions & VLPermissions.SystemService) == VLPermissions.SystemService;
            this.perm_EnumerateSecurity.Checked = (role.Permissions & VLPermissions.EnumerateSecurity) == VLPermissions.EnumerateSecurity;
            this.perm_ManageSecurity.Checked = (role.Permissions & VLPermissions.ManageSecurity) == VLPermissions.ManageSecurity;
            this.perm_EnumerateSystemParameters.Checked = (role.Permissions & VLPermissions.EnumerateSystemParameters) == VLPermissions.EnumerateSystemParameters;
            this.perm_ManageSystemParameters.Checked = (role.Permissions & VLPermissions.ManageSystemParameters) == VLPermissions.ManageSystemParameters;
            this.perm_EnumerateBuildingBlocks.Checked = (role.Permissions & VLPermissions.EnumerateBuildingBlocks) == VLPermissions.EnumerateBuildingBlocks;
            this.perm_ManageBuidingBlocks.Checked = (role.Permissions & VLPermissions.ManageBuidingBlocks) == VLPermissions.ManageBuidingBlocks;
            this.perm_EnumerateThemes.Checked = (role.Permissions & VLPermissions.EnumerateThemes) == VLPermissions.EnumerateThemes;
            this.perm_ManageThemes.Checked = (role.Permissions & VLPermissions.ManageThemes) == VLPermissions.ManageThemes;
            this.perm_EnumerateRenders.Checked = (role.Permissions & VLPermissions.EnumerateRenders) == VLPermissions.EnumerateRenders;
            this.perm_ManageRenders.Checked = (role.Permissions & VLPermissions.ManageRenders) == VLPermissions.ManageRenders;
            this.perm_EnumerateClients.Checked = (role.Permissions & VLPermissions.EnumerateClients) == VLPermissions.EnumerateClients;
            this.perm_ManageClients.Checked = (role.Permissions & VLPermissions.ManageClients) == VLPermissions.ManageClients;

            this.perm_ClientFullControl.Checked = (role.Permissions & VLPermissions.ClientFullControl) == VLPermissions.ClientFullControl;
            this.perm_ClientUnlimitedQuota.Checked = (role.Permissions & VLPermissions.ClientUnlimitedQuota) == VLPermissions.ClientUnlimitedQuota;
            this.perm_ClientEnumerateUsers.Checked = (role.Permissions & VLPermissions.ClientEnumerateUsers) == VLPermissions.ClientEnumerateUsers;
            this.perm_ClientManageUsers.Checked = (role.Permissions & VLPermissions.ClientManageUsers) == VLPermissions.ClientManageUsers;
            this.perm_ClientEnumerateLists.Checked = (role.Permissions & VLPermissions.ClientEnumerateLists) == VLPermissions.ClientEnumerateLists;
            this.perm_ClientManageLists.Checked = (role.Permissions & VLPermissions.ClientManageLists) == VLPermissions.ClientManageLists;
            this.perm_ClientImportLists.Checked = (role.Permissions & VLPermissions.ClientImportLists) == VLPermissions.ClientImportLists;
            this.perm_ClientEnumerateSurveys.Checked = (role.Permissions & VLPermissions.ClientEnumerateSurveys) == VLPermissions.ClientEnumerateSurveys;
            this.perm_ClientPreviewSurveys.Checked = (role.Permissions & VLPermissions.ClientPreviewSurveys) == VLPermissions.ClientPreviewSurveys;
            this.perm_ClientCreateSurveys.Checked = (role.Permissions & VLPermissions.ClientCreateSurveys) == VLPermissions.ClientCreateSurveys;
            this.perm_ClientEditSurveys.Checked = (role.Permissions & VLPermissions.ClientEditSurveys) == VLPermissions.ClientEditSurveys;
            this.perm_ClientDeleteSurveys.Checked = (role.Permissions & VLPermissions.ClientDeleteSurveys) == VLPermissions.ClientDeleteSurveys;
            this.perm_ClientRunSurveys.Checked = (role.Permissions & VLPermissions.ClientRunSurveys) == VLPermissions.ClientRunSurveys;
            this.perm_ClientEnumerateCollectors.Checked = (role.Permissions & VLPermissions.ClientEnumerateCollectors) == VLPermissions.ClientEnumerateCollectors;
            this.perm_ClientManageCollectors.Checked = (role.Permissions & VLPermissions.ClientManageCollectors) == VLPermissions.ClientManageCollectors;
            this.perm_ClientEnumerateAnswers.Checked = (role.Permissions & VLPermissions.ClientEnumerateAnswers) == VLPermissions.ClientEnumerateAnswers;
            this.perm_ClientManageAnswers.Checked = (role.Permissions & VLPermissions.ClientManageAnswers) == VLPermissions.ClientManageAnswers;

            if(role.IsBuiltIn)
            {
                foreach(Control ctrl in this.Controls)
                {
                    if (ctrl is WebControl)
                        ((WebControl)ctrl).Enabled = false;
                }
            }
        }

        public void GetValues(VLRole role)
        {
            role.Name = this.Name.Text;
            role.Description = this.Description.Text;
            role.IsClientRole = this.IsClientRole.Checked;

            role.Permissions = VLPermissions.None;
            if (this.perm_ManageSystem.Checked) role.Permissions = role.Permissions | VLPermissions.ManageSystem;
            if (this.perm_Developer.Checked) role.Permissions = role.Permissions | VLPermissions.Developer;
            if (this.perm_SystemService.Checked) role.Permissions = role.Permissions | VLPermissions.SystemService;
            if (this.perm_EnumerateSecurity.Checked) role.Permissions = role.Permissions | VLPermissions.EnumerateSecurity;
            if (this.perm_ManageSecurity.Checked) role.Permissions = role.Permissions | VLPermissions.ManageSecurity;
            if (this.perm_EnumerateSystemParameters.Checked) role.Permissions = role.Permissions | VLPermissions.EnumerateSystemParameters;
            if (this.perm_ManageSystemParameters.Checked) role.Permissions = role.Permissions | VLPermissions.ManageSystemParameters;
            if (this.perm_EnumerateBuildingBlocks.Checked) role.Permissions = role.Permissions | VLPermissions.EnumerateBuildingBlocks;
            if (this.perm_ManageBuidingBlocks.Checked) role.Permissions = role.Permissions | VLPermissions.ManageBuidingBlocks;
            if (this.perm_EnumerateThemes.Checked) role.Permissions = role.Permissions | VLPermissions.EnumerateThemes;
            if (this.perm_ManageThemes.Checked) role.Permissions = role.Permissions | VLPermissions.ManageThemes;
            if (this.perm_EnumerateRenders.Checked) role.Permissions = role.Permissions | VLPermissions.EnumerateRenders;
            if (this.perm_ManageRenders.Checked) role.Permissions = role.Permissions | VLPermissions.ManageRenders;
            if (this.perm_EnumerateClients.Checked) role.Permissions = role.Permissions | VLPermissions.EnumerateClients;
            if (this.perm_ManageClients.Checked) role.Permissions = role.Permissions | VLPermissions.ManageClients;

            if (this.perm_ClientFullControl.Checked) role.Permissions = role.Permissions | VLPermissions.ClientFullControl;
            if (this.perm_ClientUnlimitedQuota.Checked) role.Permissions = role.Permissions | VLPermissions.ClientUnlimitedQuota;
            if (this.perm_ClientEnumerateUsers.Checked) role.Permissions = role.Permissions | VLPermissions.ClientEnumerateUsers;
            if (this.perm_ClientManageUsers.Checked) role.Permissions = role.Permissions | VLPermissions.ClientManageUsers;
            if (this.perm_ClientEnumerateLists.Checked) role.Permissions = role.Permissions | VLPermissions.ClientEnumerateLists;
            if (this.perm_ClientManageLists.Checked) role.Permissions = role.Permissions | VLPermissions.ClientManageLists;
            if (this.perm_ClientImportLists.Checked) role.Permissions = role.Permissions | VLPermissions.ClientImportLists;
            if (this.perm_ClientEnumerateSurveys.Checked) role.Permissions = role.Permissions | VLPermissions.ClientEnumerateSurveys;
            if (this.perm_ClientPreviewSurveys.Checked) role.Permissions = role.Permissions | VLPermissions.ClientPreviewSurveys;
            if (this.perm_ClientCreateSurveys.Checked) role.Permissions = role.Permissions | VLPermissions.ClientCreateSurveys;
            if (this.perm_ClientEditSurveys.Checked) role.Permissions = role.Permissions | VLPermissions.ClientEditSurveys;
            if (this.perm_ClientDeleteSurveys.Checked) role.Permissions = role.Permissions | VLPermissions.ClientDeleteSurveys;
            if (this.perm_ClientRunSurveys.Checked) role.Permissions = role.Permissions | VLPermissions.ClientRunSurveys;
            if (this.perm_ClientEnumerateCollectors.Checked) role.Permissions = role.Permissions | VLPermissions.ClientEnumerateCollectors;
            if (this.perm_ClientManageCollectors.Checked) role.Permissions = role.Permissions | VLPermissions.ClientManageCollectors;
            if (this.perm_ClientEnumerateAnswers.Checked) role.Permissions = role.Permissions | VLPermissions.ClientEnumerateAnswers;
            if (this.perm_ClientManageAnswers.Checked) role.Permissions = role.Permissions | VLPermissions.ClientManageAnswers;

        }
    }
}