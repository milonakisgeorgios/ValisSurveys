using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var listId = TryParseInt32(context, "listId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            var list = systemManager.GetClientListById(listId);
            if(list == null)
            {
                throw new VLException(string.Format("There is no List with id='{0}'.", listId));
            }

            var _item = new
            {
                list.Client,
                list.ListId,
                list.Name,
                list.TotalContacts,
                list.CreateDT,
                list.LastUpdateDT
            };

            var response = JsonConvert.SerializeObject(_item, Formatting.None);
            context.Response.Write(response);
        }
    }
}