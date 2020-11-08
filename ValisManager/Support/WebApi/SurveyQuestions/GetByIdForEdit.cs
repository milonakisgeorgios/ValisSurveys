using Newtonsoft.Json;
using System.Text;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class GetByIdForEdit : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var questionId = TryParseInt16(context, "questionId");
            var textsLanguage = TryParseInt16(context, "textsLanguage");


            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            VLSurveyQuestion question = surveyManager.GetQuestionById(surveyId, questionId, textsLanguage);

            if (question != null)
            {
                var options = surveyManager.GetQuestionOptions(question);
                var columns = surveyManager.GetQuestionColumns(question);

                StringBuilder _options = new StringBuilder();
                foreach(var option in options)
                {
                    _options.AppendFormat("{0}\n", option.OptionText);
                } 
                StringBuilder _columns = new StringBuilder();
                foreach (var column in columns)
                {
                    _columns.AppendFormat("{0}\n", column.ColumnText);
                }

                var _question = new
                {
                    question.Survey,
                    question.QuestionId,
                    question.Page,
                    question.MasterQuestion,
                    question.DisplayOrder,
                    QuestionType = question.QuestionType.ToString(),
                    question.CustomType,
                    question.IsRequired,
                    question.RequiredBehavior,
                    question.RequiredMinLimit,
                    question.RequiredMaxLimit,
                    question.AttributeFlags,

                    question.OptionalInputBox,
                    question.RandomizeOptionsSequence,
                    question.DoNotRandomizeLastOption,
                    question.RandomizeColumnSequence,
                    question.OneResponsePerColumn,
                    question.AddResetLink,
                    question.UseDateTimeControls,

                    question.HasSkipLogic,
                    question.ValidationBehavior,

                    question.ValidationField1,
                    question.ValidationField2,
                    question.ValidationField3,

                    question.RegularExpression,

                    question.RandomBehavior,

                    question.OtherFieldType,
                    question.OtherFieldRows,
                    question.OtherFieldChars,

                    question.RangeStart,
                    question.RangeEnd,

                    question.TextsLanguage,
                    //Options = HttpUtility.HtmlEncode(_options.ToString()),
                    //Columns = HttpUtility.HtmlEncode(_columns.ToString()),

                    Options = _options.ToString(),
                    Columns = _columns.ToString(),
                    question.QuestionText,
                    question.Description,
                    question.HelpText,
                    question.FrontLabelText,
                    question.AfterLabelText,
                    question.InsideText,
                    question.RequiredMessage,
                    question.ValidationMessage,
                    question.OtherFieldLabel
                };


                var response = JsonConvert.SerializeObject(_question, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Question with id='{0},{1}'.", surveyId, questionId));
            }

        }
    }
}