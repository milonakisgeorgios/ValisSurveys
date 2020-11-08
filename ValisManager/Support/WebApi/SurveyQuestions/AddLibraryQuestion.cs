using Newtonsoft.Json;
using System;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class AddLibraryQuestion : WebApiHandler
    {
        
        protected override void ProcessPostRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            ProcessGetRequestWrapped(accessToken, context);
        }

        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var libraryQuestionId = TryParseInt32(context, "questionId");
                var surveyId = TryParseInt32(context, "surveyId");
                var pageId = TryParseInt16(context, "pageId");
                InsertPosition position = (InsertPosition)TryParseInt16(context, "position");
                Int16? referingQuestionId = TryParseInt16(context, "referingQuestionId", false, null);
                var textsLanguage = TryParseInt16(context, "textsLanguage");

                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                VLSurveyQuestion question = null;
                if (referingQuestionId.HasValue)
                {
                    if (position == InsertPosition.Before)
                    {
                        question = surveyManager.AddLibraryQuestionBefore(surveyId, pageId, referingQuestionId.Value, libraryQuestionId, textsLanguage);
                    }
                    else if (position == InsertPosition.After)
                    {
                        question = surveyManager.AddLibraryQuestionAfter(surveyId, pageId, referingQuestionId.Value, libraryQuestionId, textsLanguage);
                    }
                    else
                    {
                        question = surveyManager.AddLibraryQuestion(surveyId, pageId, libraryQuestionId, textsLanguage);
                    }
                }
                else
                {
                    question = surveyManager.AddLibraryQuestion(surveyId, pageId, libraryQuestionId, textsLanguage);
                }


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
            catch
            {
                throw;
            }
        }


    }
}