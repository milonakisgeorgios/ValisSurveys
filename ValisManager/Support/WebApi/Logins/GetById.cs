using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Logins
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var loginId = TryParseInt32(context, "loginId");

            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            var login = systemManager.GetLoginById(loginId);

            if (login != null)
            {

                var response = JsonConvert.SerializeObject(login, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Login with id='{0}'.", loginId));
            }
        }
    }
}