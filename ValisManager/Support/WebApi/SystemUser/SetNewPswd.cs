using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SystemUser
{
    public class SetNewPswd : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            //we will need an instance of a LrSystem:
            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            //gather required values:
            var userId = TryParseInt32(context, "UserId", true);
            var newPswdToken = TryParseString(context, "npt", true);

            systemManager.SetNewPassword(PrincipalType.SystemUser, userId, newPswdToken);

            //empty json object
            context.Response.Write("{}");
        }
    }
}