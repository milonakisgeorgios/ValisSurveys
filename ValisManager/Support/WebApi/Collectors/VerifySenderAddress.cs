using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class VerifySenderAddress : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var messageId = TryParseInt32(context, "messageId");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            var message = manager.GetMessageById(messageId);
            if(message == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", messageId));
            }

            /*
             * Τώρα στέλνουμε το verification sender email για αυτό το message: 
             */
            VLSystemEmail verify_email = manager.SendVerifySenderEmail(message);


            //empty json object
            context.Response.Write("{}");
        }
    }
}