using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists.Contacts
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var contactId = TryParseInt32(context, "contactId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            systemManager.DeleteContact(contactId);

            //empty json object
            context.Response.Write("{}");
        }
    }
}