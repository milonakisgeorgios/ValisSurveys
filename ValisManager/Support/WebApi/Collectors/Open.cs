using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class Open : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorId = TryParseInt32(context, "collectorId");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            manager.OpenCollector(collectorId);
            //empty json object
            context.Response.Write("{}");
        }
    }
}