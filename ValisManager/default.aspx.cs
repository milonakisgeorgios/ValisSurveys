using System;

namespace ValisManager
{
    public partial class _default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            Response.Redirect(Globals.MySurveysPage, false);
            this.Context.ApplicationInstance.CompleteRequest();
        }
    }
}