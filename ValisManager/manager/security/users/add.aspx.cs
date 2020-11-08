using System;
using Valis.Core;

namespace ValisManager.manager.security.users
{
    public partial class add : ManagerPage
    {


        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string logOnToken = null;
                string pswdToken = null;
                var temporalUser = new VLSystemUser();
                this.usersform1.GetValues(temporalUser, ref logOnToken, ref pswdToken);

                var account = SystemManager.CreateSystemAccount(temporalUser.FirstName, temporalUser.LastName, temporalUser.Role, temporalUser.Email, logOnToken, pswdToken);
                if(string.IsNullOrWhiteSpace(temporalUser.Notes ) == false || account.IsActive != temporalUser.IsActive)
                {
                    account.Notes = temporalUser.Notes;
                    account.IsActive = temporalUser.IsActive;
                    account = SystemManager.UpdateSystemUser(account);
                }

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

            this.Response.Redirect("list.aspx", false);
            this.Context.ApplicationInstance.CompleteRequest();
        }

    }
}