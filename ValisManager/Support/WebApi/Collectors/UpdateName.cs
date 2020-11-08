using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class UpdateName: WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorId = TryParseInt32(context, "collectorId");
            var textsLanguage = TryParseInt16(context, "textsLanguage");

            var manager = VLSurveyManager.GetAnInstance(accessToken);
            var collector = manager.GetCollectorById(collectorId, textsLanguage);

            if (collector != null)
            {
                collector.Name = TryParseString(context, "collectorName", true);
                collector = manager.UpdateCollector(collector);

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

        }
    }
}