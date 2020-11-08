using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class GetQuestionsForPage : WebApiHandler
    {
        class _Question
        {
            public System.Int32 Survey { get; set; }
            public System.Int16 QuestionId { get; set; }
            public System.Int16 Page { get; set; }
            public System.Int16? MasterQuestion { get; set; }
            public System.Int16 DisplayOrder { get; set; }
            public QuestionType QuestionType { get; set; }
            public QuestionType? CustomType { get; set; }
            public System.Boolean IsRequired { get; set; }
            public System.Boolean HasSkipLogic { get; set; }
            public System.String QuestionText { get; set; }
            public System.String OptionTitle { get; set; }
        }
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var pageId = TryParseInt16(context, "pageId");
            var textsLanguage = TryParseInt16(context, "textsLanguage", required: false, defValue: 0);


            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var questions = surveyManager.GetQuestionsForPage(surveyId, pageId, textsLanguage);

            _Question[] rows = new _Question[questions.Count];
            for(int i=0; i< questions.Count; i++)
            {
                _Question t = new _Question();
                t.Survey = questions[i].Survey;
                t.QuestionId = questions[i].QuestionId;
                t.Page = questions[i].Page;
                t.MasterQuestion = questions[i].MasterQuestion;
                t.DisplayOrder = questions[i].DisplayOrder;
                t.QuestionType = questions[i].QuestionType;
                t.CustomType = questions[i].CustomType;
                t.IsRequired = questions[i].IsRequired;
                t.HasSkipLogic = questions[i].HasSkipLogic;
                t.QuestionText = questions[i].QuestionText;

                if(questions[i].QuestionText.Length < 64)
                {
                    t.OptionTitle = HttpUtility.HtmlEncode(string.Format("Q {0}: {1}", questions[i].DisplayOrder, questions[i].QuestionText));
                }
                else
                {
                    t.OptionTitle = HttpUtility.HtmlEncode(string.Format("Q {0}: {1}", questions[i].DisplayOrder, questions[i].QuestionText)).Substring(0, 64) + "...";
                }

                rows[i] = t;
            }


            var response = JsonConvert.SerializeObject(rows, Formatting.None);
            context.Response.Write(response);
        }
    }
}