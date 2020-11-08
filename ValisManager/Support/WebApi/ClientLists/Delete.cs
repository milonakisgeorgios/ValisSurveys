using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var listId = TryParseInt32(context, "listId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            systemManager.DeleteClientList(listId);

            //empty json object
            context.Response.Write("{}");
        }
    }
}