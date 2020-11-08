using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SystemUser
{
    public class ChangePassword : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var logontoken = TryParseString(context, "logontoken", true);
            var oldPass = TryParseString(context, "oldPass", true);
            var newPass = TryParseString(context, "newPass", true);


            //we will need an instance of a VLSystemManager:
            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            systemManager.ChangePassword(logontoken, oldPass, newPass);

            //empty json object
            context.Response.Write("{}");
        }
    }
}