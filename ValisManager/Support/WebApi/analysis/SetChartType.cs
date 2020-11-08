using Newtonsoft.Json;
using System;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Analysis
{
    public class SetChartType : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var ViewId = TryParseGuid(context, "ViewId", true);
            var SurveyId = TryParseInt32(context, "SurveyId", true);
            var QuestionId = TryParseInt16(context, "QuestionId", true);
            var chartType = TryParseString(context, "chartType", true);

            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            VLViewQuestion question = null;

            if (string.Equals(chartType, "2"/*Pie*/, StringComparison.OrdinalIgnoreCase))
            {
                question = surveyManager.SetChartType(ViewId, SurveyId, QuestionId, ChartType.Pie);
            }
            else if (string.Equals(chartType, "0"/*HorizontalBar*/, StringComparison.OrdinalIgnoreCase))
            {
                question = surveyManager.SetChartType(ViewId, SurveyId, QuestionId, ChartType.HorizontalBar);
            }
            else if (string.Equals(chartType, "1"/*VerticalBar*/, StringComparison.OrdinalIgnoreCase))
            {
                question = surveyManager.SetChartType(ViewId, SurveyId, QuestionId, ChartType.VerticalBar);
            }

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