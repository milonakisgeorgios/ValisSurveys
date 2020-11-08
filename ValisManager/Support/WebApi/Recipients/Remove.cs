using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Recipients
{
    public class Remove : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var recipientId = TryParseInt32(context, "recipientId");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            manager.RemoveRecipient(recipientId);

            //empty json object
            context.Response.Write("{}");
        }
    }
}