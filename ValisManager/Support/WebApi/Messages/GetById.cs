using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Messages
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var messageId = TryParseInt32(context, "messageId");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            var message = manager.GetMessageById(messageId);

            if (message != null)
            {
                var _message = new
                {
                    message.Collector,
                    message.MessageId,
                    message.Sender,
                    message.Status,
                    message.IsDeliveryMethodOK,
                    message.IsSenderOK,
                    message.IsContentOK,
                    message.IsScheduleOK,
                    message.ScheduledAt,
                    message.SentCounter,
                    message.DeliveryMethod,
                    message.Subject
                };

                var response = JsonConvert.SerializeObject(_message, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Message with id='{0}'.", messageId));
            }

        }
    }
}