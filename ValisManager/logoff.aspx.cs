using System;
using System.Web.Security;

namespace ValisManager
{
    public partial class logoff : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            /*
             *  we destroy the session, the user will get a new
             *  session key
            */
            Session.Abandon();
            /*
             * Removes the forms-authentication ticket from the browser.
             */
            FormsAuthentication.SignOut();
            /*
             */
            Response.Redirect(Globals.LoginPage, false);
            this.Context.ApplicationInstance.CompleteRequest();
        }
    }
}