using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Recipients
{
    public class GetSurveyRuntimeURL : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            var recipientId = TryParseInt32(context, "recipientId");


            var manager = VLSurveyManager.GetAnInstance(accessToken);

            var recipient = manager.GetRecipientById(recipientId);
            if (recipient != null)
            {
                var collector = manager.GetCollectorById(recipient.Collector);
                if (collector != null)
                {
                    var survey = manager.GetSurveyById(collector.Survey, collector.TextsLanguage);
                    if (survey != null)
                    {
                        var _url = Utility.GetSurveyRuntimeURL(survey, collector, recipient, true);

                        var _data = new
                        {
                            url = _url
                        };

                        var response = JsonConvert.SerializeObject(_data, Formatting.None);
                        context.Response.Write(response);
                    }
                    else
                    {
                        throw new VLException(string.Format("There is no Survey with id='{0}'.", collector.Survey));
                    }
                }
                else
                {
                    throw new VLException(string.Format("There is no Collector with id='{0}'.", recipient.Collector));
                }
            }
            else
            {
                throw new VLException(string.Format("There is no Recipient with id='{0}'.", recipientId));
            }

        }
    }
}