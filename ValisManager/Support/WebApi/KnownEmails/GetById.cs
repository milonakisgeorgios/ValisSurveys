using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.KnownEmails
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var emailId = TryParseInt32(context, "emailId");


            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            var email = systemManager.GetKnownEmailById(emailId);

            if (email != null)
            {

                var _item = new
                {
                    email.Client,
                    email.EmailId,
                    email.EmailAddress,
                    email.LocalPart,
                    email.DomainPart,
                    email.RegisterDt,
                    email.IsDomainOK,
                    email.IsVerified,
                    email.IsOptedOut,
                    email.IsBounced,
                    email.VerifiedDt,
                    email.OptedOutDt
                };

                var response = JsonConvert.SerializeObject(_item, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no KnownEmail with id='{0}'.", emailId));
            }
        }
    }
}