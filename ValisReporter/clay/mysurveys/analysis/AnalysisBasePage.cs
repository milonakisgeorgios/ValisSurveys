using System;
using System.Globalization;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using Valis.Core;

namespace ValisReporter
{
    public class AnalysisBasePage : ManagerPage
    {
        public Int32 Surveyid
        {
            get
            {
                Object _obj = this.ViewState["surveyid"];
                if (_obj == null) return -1;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["surveyid"] = value;
            }
        }
        public Int16 TextsLanguage
        {
            get
            {
                Object _obj = this.ViewState["textslanguage"];
                if (_obj == null) return BuiltinLanguages.PrimaryLanguage.LanguageId;
                return (Int16)_obj;
            }
            set
            {
                this.ViewState["textslanguage"] = value;
            }
        }

        protected VLSurvey SelectedSurvey
        {
            get
            {
                if (this.Context.Items["SelectedSurvey"] == null)
                {
                    this.Context.Items["SelectedSurvey"] = SurveyManager.GetSurveyById(this.Surveyid, this.TextsLanguage);
                }
                return (VLSurvey)this.Context.Items["SelectedSurvey"];
            }
        }


        /// <summary>
        /// Κάνει render ένα Javascript array με όλες τις ερωτήσεις του συγκεκριμένου survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        string GetJsQuestionsArray(Int32 surveyId, Int16 textsLanguage)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("var questions = [");
            var questions = SurveyManager.GetQuestionsForSurvey(surveyId, textsLanguage);
            foreach (var q in questions)
            {
                sb.Append("{");
                {
                    sb.AppendFormat("Id:{0}, ", q.QuestionId);
                    sb.AppendFormat("type:\"{0}\", ", q.QuestionType);
                    sb.AppendFormat("text:{0}, ", Utilities.EncodeJsString(q.QuestionText));
                    if (q.QuestionType == QuestionType.Range)
                    {
                        sb.AppendFormat("rangeStart:{0}, ", q.RangeStart);
                        sb.AppendFormat("rangeEnd:{0}, ", q.RangeEnd);
                    }
                    if(q.OptionalInputBox)
                    {
                        sb.Append("OptionalInputBox:1, ");
                    }

                    var options = SurveyManager.GetQuestionOptions(q);
                    if (q.OptionalInputBox)
                    {
                        /*Τοποθετούμε ένα virtual 'other' Option:*/
                        options.Add(new VLQuestionOption() { OptionId = 0, OptionText = "Other" });
                    }
                    if (options.Count > 0)
                    {
                        sb.Append("options: [");
                        {
                            foreach (var op in options)
                            {
                                sb.AppendFormat("{{Id:{0}, text:{1}}},", op.OptionId, Utilities.EncodeJsString(op.OptionText));
                            }
                        }
                        sb.Append("],");
                    }
                    else
                    {
                        if (q.QuestionType == QuestionType.Range)
                        {
                            sb.Append("options: [");
                            for (Int32 i = q.RangeStart.Value; i <= q.RangeEnd.Value; i++)
                            {
                                sb.AppendFormat("{{Id:{0}, text:{1}}},", i, i.ToString(CultureInfo.InvariantCulture));
                            }
                            sb.Append("],");
                        }
                        else
                        {
                            sb.Append("options: [],");
                        }
                    }

                    var columns = SurveyManager.GetQuestionColumns(q);
                    if (columns.Count > 0)
                    {
                        sb.Append("columns: [");
                        {
                            foreach (var col in columns)
                            {
                                sb.AppendFormat("{{Id:{0}, text:{1}}},", col.ColumnId, Utilities.EncodeJsString(col.ColumnText));
                            }
                        }
                        sb.Append("],");
                    }
                    else
                    {
                        sb.Append("columns: []");
                    }
                }
                sb.AppendLine("},");
            }
            sb.AppendLine("];");

            return sb.ToString();
        }
        /// <summary>
        /// Κάνει render το ViewSummary για του συγκεκριμένου survey σαν ένα Javascript Object.
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        protected string GetJsSummaryArray(Guid viewId)
        {
            var serializer = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("var summary = ");

            var summary = SurveyManager.GetViewSummaryEx(viewId);
            serializer.Serialize(summary, sb);

            sb.AppendLine(";");
            return sb.ToString();
        }



        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //try
            //{
                if (this.IsPostBack == false)
                {
                    if (string.IsNullOrEmpty(Request.Params["surveyid"]))
                        throw new ArgumentNullException("surveyid");
                    this.Surveyid = Int32.Parse(Request.Params["surveyid"]);


                    if (Request.Params["textslanguage"] != null)
                        this.TextsLanguage = Int16.Parse(Request.Params["textslanguage"]);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "JsQuestionsArray", GetJsQuestionsArray(this.Surveyid, this.TextsLanguage), true);
            //}
            //catch (Exception ex)
            //{
            //    this.ErrorMessage = ex.Message;
            //}
        }
    }
}