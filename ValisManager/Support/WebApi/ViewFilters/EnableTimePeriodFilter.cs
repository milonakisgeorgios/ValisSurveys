using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class EnableTimePeriodFilter : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var viewId = TryParseGuid(context, "viewId");

                //Φτιάχνουμε ένα surveyManager:
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                //Βρισκουμε την επιλεγμένη όψη:
                var selectedview = surveyManager.GetViewById(viewId);

                selectedview = surveyManager.EnableFilteringByTimePeriod(selectedview);

                var _view = new
                {
                    selectedview.Client,
                    selectedview.UserId,
                    selectedview.Survey,
                    selectedview.ViewId,
                    selectedview.Name,
                    selectedview.IsDefaultView,
                    selectedview.PartialShowInUse,
                    selectedview.EnablePartialShow,
                    selectedview.FilteringByCollectorInUse,
                    selectedview.EnableFilteringByCollector,
                    selectedview.FilteringByTimePeriodInUse,
                    selectedview.EnableFilteringByTimePeriod,
                    selectedview.FilteringByResponseTimeInUse,
                    selectedview.EnableFilteringByResponseTime,
                    selectedview.FilteringByQuestionInUse,
                    selectedview.TimePeriodStart,
                    selectedview.TimePeriodEnd,
                    selectedview.TotalResponseTime,
                    selectedview.TotalResponseTimeUnit,
                    selectedview.TotalResponseTimeOperator,
                    selectedview.NumberOfQuestionFilters
                };


                var response = JsonConvert.SerializeObject(_view, Formatting.None);
                context.Response.Write(response);
            }
            catch
            {
                throw;
            }
        }
    }
}