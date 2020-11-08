using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Recipients
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var recipientId = TryParseInt32(context, "recipientId");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            var recipient = manager.GetRecipientById(recipientId);

            if (recipient != null)
            {
                var _recipient = new
                {
                    recipient.RecipientId,
                    recipient.Collector,
                    recipient.RecipientKey,
                    recipient.Email,
                    recipient.FirstName,
                    recipient.LastName,
                    recipient.Title,
                    recipient.Status,
                    recipient.IsSentEmail,
                    recipient.IsOptedOut,
                    recipient.IsBouncedEmail,
                    recipient.HasPartiallyResponded,
                    recipient.HasResponded,
                    recipient.HasManuallyAdded
                };

                var response = JsonConvert.SerializeObject(_recipient, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Recipient with id='{0}'.", recipientId));
            }
        }
    }
}