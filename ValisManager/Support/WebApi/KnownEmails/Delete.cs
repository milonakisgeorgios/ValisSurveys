using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.KnownEmails
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var emailId = TryParseInt32(context, "emailId");


            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            systemManager.DeleteKnownEmail(emailId);

            //empty json object
            context.Response.Write("{}");
        }
    }
}