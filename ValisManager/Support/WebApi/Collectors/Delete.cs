using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var collectorId = TryParseInt32(context, "collectorId");

                var manager = VLSurveyManager.GetAnInstance(accessToken);

                manager.DeleteCollector(collectorId);
                //empty json object
                context.Response.Write("{}");
            }
            catch
            {
                throw;
            }
        }
    }
}