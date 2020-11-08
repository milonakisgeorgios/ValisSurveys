using System;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.security.roles
{
    public partial class add : ManagerPage
    {
        protected string GetSaveButtonHandler
        {
            get
            {
                PostBackOptions myPostBackOptions = new PostBackOptions(this);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.RequiresJavaScriptProtocol = false;
                myPostBackOptions.PerformValidation = true;
                myPostBackOptions.Argument = "saveBtn";
                myPostBackOptions.ClientSubmit = true;

                return Page.ClientScript.GetPostBackEventReference(myPostBackOptions);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var newRole = new VLRole();
                this.rolesform1.GetValues(newRole);

                newRole = SystemManager.CreateRole(newRole);

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