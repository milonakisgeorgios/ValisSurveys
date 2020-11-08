using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class SetSkipLogic : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var questionId = TryParseInt16(context, "questionId");
            var textsLanguage = TryParseInt16(context, "textsLanguage");
            var data = TryParseString(context, "data");


            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            /*Σπάμε τα data:*/
            Collection<VLQuestionOptionHelper> options = new Collection<VLQuestionOptionHelper>();
            var lines = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var segments = line.Split(new char[]{'|'});
                var _op = segments[0].Substring(3);     //optionId
                var _sp = segments[1].Substring(3);     //skip to pageId
                var _sq = segments[2].Substring(3);     //skip to questionId

                var _option = new VLQuestionOptionHelper();
                _option.Survey = surveyId;
                _option.Question = questionId;
                _option.OptionId = Byte.Parse(segments[0].Substring(3));
                if(!string.IsNullOrWhiteSpace(_sp))
                {
                    _option.skipToPage = Int16.Parse(_sp);
                }
                if (!string.IsNullOrWhiteSpace(_sq))
                {
                    _option.SkipToQuestion = Int16.Parse(_sq);
                }
                options.Add(_option);
            }

            var question = surveyManager.SetQuestionSkipLogic(surveyId, questionId, options, textsLanguage);


            var _question = new
            {
                question.Survey,
                question.QuestionId,
                question.Page,
                question.MasterQuestion,
                question.DisplayOrder,
                question.QuestionType,
                question.CustomType,
                question.IsRequired,
                question.AttributeFlags,
                question.ValidationBehavior,
                question.RegularExpression,
                question.HasSkipLogic,
                question.TextsLanguage,

                question.QuestionText,
                question.Description,
                question.HelpText,
                question.FrontLabelText,
                question.AfterLabelText,
                question.InsideText,
                question.RequiredMessage,
                question.ValidationMessage
            };


            var response = JsonConvert.SerializeObject(_question, Formatting.None);
            context.Response.Write(response);

        }
    }
}