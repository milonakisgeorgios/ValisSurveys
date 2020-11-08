using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class AddCollectorsFilter : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var viewId = TryParseGuid(context, "viewId");
                var conc_collectors = TryParseString(context, "collectors");

                //Φτιάχνουμε ένα surveyManager:
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                //Βρισκουμε την επιλεγμένη όψη:
                var selectedview = surveyManager.GetViewById(viewId);

                var _collectors = conc_collectors.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                Collection<Int32> collectors = new Collection<int>();
                foreach (var item in _collectors)
                {
                    collectors.Add(Int32.Parse(item.Substring(13)));
                }
                selectedview = surveyManager.AddCollectorFilter(selectedview, collectors);

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