using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorId = TryParseInt32(context, "collectorId");
            var textsLanguage = TryParseInt16(context, "textsLanguage");


            var manager = VLSurveyManager.GetAnInstance(accessToken);
            var sysmanager = VLSystemManager.GetAnInstance(accessToken);
            var collector = manager.GetCollectorById(collectorId, textsLanguage);
            var hasUsedPayments = false;
            if (collector != null)
            {
                var totalScheduledMessages = manager.GetScheduledMessagesCount(collector.CollectorId);
                var totalResponses = manager.GetResponsesCountForCollector(collector.Survey, collector.CollectorId);
                var totalPayments = sysmanager.GetCollectorPayments(collectorId);
                foreach(var tp in totalPayments)
                {
                    if(tp.IsUsed)
                    {
                        hasUsedPayments = true;
                        break;
                    }
                }

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
                    TotalResponses = totalResponses,
                    HasUsedPayments = hasUsedPayments,
                    CreateDT = collector.CreateDT.ToString(Utilities.DateTime_Format_General),
                    LastUpdateDT = collector.LastUpdateDT.ToString(Utilities.DateTime_Format_General)
                };

                var response = JsonConvert.SerializeObject(_collector, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Collector with id='{0},{1}'.", collectorId, textsLanguage));
            }

        }
    }
}