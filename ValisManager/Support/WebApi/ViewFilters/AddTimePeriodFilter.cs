using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class AddTimePeriodFilter : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var viewId = TryParseGuid(context, "viewId");
                var _timePeriodStart = TryParseString(context, "timePeriodStart");
                var _timePeriodEnd = TryParseString(context, "timePeriodEnd");

                //Φτιάχνουμε ένα surveyManager:
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                //Βρισκουμε την επιλεγμένη όψη:
                var selectedview = surveyManager.GetViewById(viewId);

                //ΣΤΑ RESPONSES ΟΙ ΗΜΕΡΟΜΗΝΙΕΣ ΕΙΝΑΙ ΠΑΝΤΑ MM/DD/YYYY:
                var timePeriodStart = DateTime.ParseExact(_timePeriodStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var timePeriodEnd = DateTime.ParseExact(_timePeriodEnd, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                selectedview = surveyManager.AddTimePeriodFilter(selectedview, timePeriodStart, timePeriodEnd);
                
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