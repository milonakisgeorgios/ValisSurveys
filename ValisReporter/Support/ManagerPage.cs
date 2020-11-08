using System;
using Valis.Core;

namespace ValisReporter
{
    public class ManagerPage : System.Web.UI.Page
    {
        protected VLAccessToken UserToken
        {
            get
            {
                return ValisHttpApplication.AccessToken;
            }
        }
        protected VLSystemManager SystemManager
        {
            get
            {
                return ((ValisHttpApplication)this.Context.ApplicationInstance).SystemManager;
            }
        }
        protected VLSurveyManager SurveyManager
        {
            get
            {
                return ((ValisHttpApplication)this.Context.ApplicationInstance).SurveyManager;
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "clientGlobalVariables1", string.Format("var theManagerPath = '{0}';var theAccessToken = {1};var isSysadmin = {2};", this.ResolveUrl("~/"), this.UserToken.AccessTokenId, this.UserToken.IsSysAdmin ? "true" : "false"), true);

            base.OnPreRenderComplete(e);
        }
    }
}