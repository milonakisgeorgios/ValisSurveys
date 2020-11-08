using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Payments
{
    public class RemoveFromCollector : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorPaymentId = TryParseInt32(context, "collectorPaymentId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            systemManager.RemovePaymentFromCollector(collectorPaymentId);


            //empty json object
            context.Response.Write("{}");
        }
    }
}