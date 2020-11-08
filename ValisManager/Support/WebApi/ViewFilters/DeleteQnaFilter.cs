using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class DeleteQnaFilter : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var filterId = TryParseInt32(context, "filterId");

                var manager = VLSurveyManager.GetAnInstance(accessToken);
                var filter = manager.GetViewFilterById(filterId);
                if (filter != null)
                {
                    manager.DeleteViewFilter(filter);


                    //empty json object
                    context.Response.Write("{}");
                }
                else
                {
                    throw new VLException(string.Format("There is no Filter with id='{0}'.", filterId));
                }
            }
            catch
            {
                throw;
            }
        }
    }
}