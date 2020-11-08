using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists
{
    public class Create : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var listName = TryParseString(context, "listName", true);


            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            var list = systemManager.CreateClientList(accessToken.ClientId.Value, listName);

            var _item = new
            {
                list.Client,
                list.ListId,
                list.Name,
                list.CreateDT,
                list.LastUpdateDT
            };

            var response = JsonConvert.SerializeObject(_item, Formatting.None);
            context.Response.Write(response);
        }
    }
}