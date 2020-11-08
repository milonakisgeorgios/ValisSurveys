using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Analysis
{
    public class ToggleZeroResponseOptionsVisibility : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var ViewId = TryParseGuid(context, "ViewId", true);
            var SurveyId = TryParseInt32(context, "SurveyId", true);
            var QuestionId = TryParseInt16(context, "QuestionId", true);

            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            VLViewQuestion question = surveyManager.ToggleZeroResponseOptionsVisibility(ViewId, SurveyId, QuestionId);

            if (question != null)
            {
                var _question = new
                {
                    question.ViewId,
                    question.Survey,
                    question.Question,
                    question.ShowResponses,
                    question.ShowChart,
                    question.ShowDataTable,
                    question.ShowDataInTheChart,
                    question.HideZeroResponseOptions,
                    question.SwapRowsAndColumns,
                    question.ChartType,
                    question.LabelType,
                    question.AxisScale,
                    question.ScaleMaxPercentage,
                    question.ScaleMaxAbsolute,
                    question.SummaryTotalAnswered,
                    question.SummaryTotalSkipped
                };

                var response = JsonConvert.SerializeObject(_question, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no ViewQuestion (ViewId='{0}', SurveyId='{1}', QuestionId='{2}'", ViewId, SurveyId, QuestionId));
            }
        }
    }
}