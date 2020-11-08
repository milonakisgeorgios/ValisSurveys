using System.Web;

namespace ValisManager.Support.WebApi.KnownEmails
{
    public class AddVerified : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            base.ProcessPostRequestWrapped(accessToken, context);
        }
    }
}