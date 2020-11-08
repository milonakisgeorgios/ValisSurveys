using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class ClearResponses : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorId = TryParseInt32(context, "collectorId");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            var collector = manager.ClearResponsesForCollector(collectorId);


            if (collector != null)
            {
                var totalScheduledMessages = manager.GetScheduledMessagesCount(collector.CollectorId);

                var _collector = new
                {
                    collector.CollectorId,
                    collector.Survey,
                    collector.CollectorType,
                    collector.Name,
                    collector.AttributeFlags,
                    collector.Status,
                    collector.Responses,
                    collector.SupportedLanguagesIds,
                    collector.PrimaryLanguage,
                    ScheduledMessages = totalScheduledMessages,
                    CreateDT = collector.CreateDT.ToString(Utilities.DateTime_Format_General),
                    LastUpdateDT = collector.LastUpdateDT.ToString(Utilities.DateTime_Format_General)
                };

                var response = JsonConvert.SerializeObject(_collector, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("ClearResponsesForCollector for Collector with id='{0}', failed.", collectorId));
            }

        }
    }
}