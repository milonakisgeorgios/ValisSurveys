using System;
using System.Text;
using Valis.Core;

namespace ValisManager.clay.mysurveys.analysis.controls
{
    /// <summary>
    /// Εξαρτάται (για την λειτουργία του) απο την μέθοδο GetJsQuestionsArray(), που καλείται απο την AnalysisBasePage
    /// </summary>
    public partial class analysisToolBox : ManagerUserControl
    {
        VLView m_selectedView;


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

        public VLSurvey SelectedSurvey
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


        public Guid ViewId
        {
            get
            {
                Object _obj = this.ViewState["ViewId"];
                if (_obj == null) return default(Guid);
                return (Guid)_obj;
            }
            set
            {
                this.ViewState["ViewId"] = value;
            }
        }

        public VLView SelectedView
        {
            get
            {
                if (m_selectedView == null)
                {
                    m_selectedView = SurveyManager.GetViewById(this.ViewId);
                }
                return m_selectedView;
            }
        }



        protected string GetSurveyQuestionsOptions()
        {
            StringBuilder options = new StringBuilder();

            options.Append("<option value=\"0\">Choose..</option>");

            var questions = SurveyManager.GetQuestionsForSurvey(this.Surveyid, this.TextsLanguage);
            foreach (var q in questions)
            {
                options.AppendFormat("<option value=\"{0}\" questiontype=\"{1}\" >Q{0}: {2}</option>", q.QuestionId, q.QuestionType, Server.HtmlEncode(q.QuestionText));
            }

            return options.ToString();
        }

        protected string GetCollectorsCheckBoxes()
        {
            StringBuilder html = new StringBuilder();

            var collectors = SurveyManager.GetCollectors(this.Surveyid, textsLanguage: this.TextsLanguage);
            foreach (var c in collectors)
            {
                html.Append("<div class=\"filterRow\">");

                html.AppendFormat("<input type=\"checkbox\" id=\"fltrCollector{0}\" name=\"fltrCollector{0}\"><label for=\"fltrCollector{0}\">{1}</label>", c.CollectorId, Server.HtmlEncode(c.Name));

                html.Append("</div>");
            }

            return html.ToString();
        }


        protected string GetFilters()
        {
            StringBuilder sb = new StringBuilder();

            #region QnA filters
            var filters = SurveyManager.GetViewFilters(this.ViewId);
            foreach (var filter in filters)
            {
                sb.Append("<div class=\"filter-summary-wrapper\">");
                sb.AppendFormat("<div class=\"filter-summary\" id=\"filter_{0}\">", filter.FilterId);
                sb.AppendFormat("<div class=\"filter-status\" onclick=\"OnFilterStatusClick(this,'{0}')\">&nbsp;</div>", filter.FilterId);
                sb.AppendFormat("<div class=\"filter-name\" onclick=\"OnFilterNameClick(this,'{0}')\">", filter.FilterId);

                if (string.IsNullOrWhiteSpace(filter.Name))
                {
                    sb.AppendFormat("FILTER: Q{0}: ", filter.Question);
                    short dcounter = 0;
                    foreach (var fd in filter.FilterDetails)
                    {
                        if (filter.QuestionType.Value == QuestionType.DropDown || filter.QuestionType.Value == QuestionType.OneFromMany || filter.QuestionType.Value == QuestionType.ManyFromMany)
                        {
                            if (dcounter++ > 0)
                                sb.Append(", ");
                            if (fd.SelectedOption.HasValue)
                                sb.AppendFormat("Option {0}", fd.SelectedOption);
                        }
                        else if (filter.QuestionType.Value == QuestionType.MatrixOnePerRow || filter.QuestionType.Value == QuestionType.MatrixManyPerRow)
                        {
                            if (dcounter++ > 0)
                                sb.Append(", ");
                            if (fd.SelectedOption.HasValue)
                                sb.AppendFormat("Row{0}:Col{1}", fd.SelectedOption, fd.SelectedColumn);
                        }
                        else if (filter.QuestionType.Value == QuestionType.Integer || filter.QuestionType.Value == QuestionType.Decimal || filter.QuestionType.Value == QuestionType.Date)
                        {
                            if (fd.Operator == ComparisonOperator.Between)
                            {
                                sb.AppendFormat("between {0} and {1}", Server.HtmlEncode(fd.UserInput1), Server.HtmlEncode(fd.UserInput2));
                            }
                            else
                            {
                                sb.AppendFormat("{0} {1}", fd.Operator.ToString(), Server.HtmlEncode(fd.UserInput1));
                            }
                        }
                    }
                }
                else
                {
                    sb.AppendFormat("FILTER: {0}", Server.HtmlEncode(filter.Name));
                }
                sb.Append("</div>");

                sb.AppendFormat("<div class=\"filter-actions\" onclick=\"OnFilterDeleteClick(this,'{0}')\" ><img width=\"30\" height=\"28\" src=\"{1}\" alt=\"Delete\" title=\"Delete the filter\" /></div>", filter.FilterId, this.ResolveClientUrl("~/content/images/close-filter.png"));
                sb.Append("<div class=\"filter-clear\"></div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            #endregion

            #region Collector filter:
            if (this.SelectedView.FilteringByCollectorInUse)
            {
                short dcounter = 0;
                var collectors = SurveyManager.GetViewCollectors(this.ViewId);
                sb.AppendFormat("<div class=\"filter-summary-wrapper\">");
                sb.AppendFormat("<div class=\"filter-summary\" id=\"filter_{0}\">", "collectors");
                sb.AppendFormat("<div class=\"filter-status\" onclick=\"OnFilterStatusClick(this,'{0}')\">&nbsp;</div>", "collectors");
                sb.AppendFormat("<div class=\"filter-name\" onclick=\"OnFilterNameClick(this,'{0}')\">", "collectors");

                sb.Append("FILTER: ");
                foreach (var item in collectors)
                {
                    if (item.IncludeResponses)
                    {
                        if (dcounter++ > 0)
                            sb.Append(", ");
                        sb.AppendFormat("Col-{0}", item.Collector);
                    }
                }
                sb.Append("</div>");


                sb.AppendFormat("<div class=\"filter-actions\" onclick=\"OnFilterDeleteClick(this,'{0}')\" ><img width=\"30\" height=\"28\" src=\"{1}\" alt=\"Delete\" title=\"Delete the filter\" /></div>", "collectors", this.ResolveClientUrl("~/content/images/close-filter.png"));
                sb.Append("<div class=\"filter-clear\"></div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            #endregion

            #region ResponseTimeFilter
            if (this.SelectedView.FilteringByResponseTimeInUse)
            {
                sb.AppendFormat("<div class=\"filter-summary-wrapper\">");
                sb.AppendFormat("<div class=\"filter-summary\" id=\"filter_{0}\">", "responsetime");
                sb.AppendFormat("<div class=\"filter-status\" onclick=\"OnFilterStatusClick(this,'{0}')\">&nbsp;</div>", "responsetime");
                sb.AppendFormat("<div class=\"filter-name\" onclick=\"OnFilterNameClick(this,'{0}')\">", "responsetime");

                sb.AppendFormat("FILTER: ResponseTime ");
                if (SelectedView.TotalResponseTimeOperator.Value == ResponseTimeOperator.Greater)
                    sb.Append("> ");
                else if (SelectedView.TotalResponseTimeOperator.Value == ResponseTimeOperator.GreaterOrEqual)
                    sb.Append(">= ");
                else if (SelectedView.TotalResponseTimeOperator.Value == ResponseTimeOperator.Less)
                    sb.Append("< ");
                else if (SelectedView.TotalResponseTimeOperator.Value == ResponseTimeOperator.LessOrEqual)
                    sb.Append("<= ");

                sb.Append(SelectedView.TotalResponseTime.Value);

                if (this.SelectedView.TotalResponseTimeUnit.Value == ResponseTimeUnit.Second)
                    sb.Append("sec");
                else if (this.SelectedView.TotalResponseTimeUnit.Value == ResponseTimeUnit.Minute)
                    sb.Append("min");
                else if (this.SelectedView.TotalResponseTimeUnit.Value == ResponseTimeUnit.Hour)
                    sb.Append("h");
                else if (this.SelectedView.TotalResponseTimeUnit.Value == ResponseTimeUnit.Day)
                    sb.Append("d");



                sb.Append("</div>");


                sb.AppendFormat("<div class=\"filter-actions\" onclick=\"OnFilterDeleteClick(this,'{0}')\" ><img width=\"30\" height=\"28\" src=\"{1}\" alt=\"Delete\" title=\"Delete the filter\" /></div>", "responsetime", this.ResolveClientUrl("~/content/images/close-filter.png"));
                sb.Append("<div class=\"filter-clear\"></div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            #endregion

            #region TimePeriod
            if (this.SelectedView.FilteringByTimePeriodInUse)
            {
                sb.AppendFormat("<div class=\"filter-summary-wrapper\">");
                sb.AppendFormat("<div class=\"filter-summary\" id=\"filter_{0}\">", "timeperiod");
                sb.AppendFormat("<div class=\"filter-status\" onclick=\"OnFilterStatusClick(this,'{0}')\">&nbsp;</div>", "timeperiod");
                sb.AppendFormat("<div class=\"filter-name\" onclick=\"OnFilterNameClick(this,'{0}')\">", "timeperiod");

                var timePeriodStart = string.Format("{0:00}/{1:00}/{2:0000}", SelectedView.TimePeriodStart.Value.Month, SelectedView.TimePeriodStart.Value.Day, SelectedView.TimePeriodStart.Value.Year);
                var timePeriodEnd = string.Format("{0:00}/{1:00}/{2:0000}", SelectedView.TimePeriodEnd.Value.Month, SelectedView.TimePeriodEnd.Value.Day, SelectedView.TimePeriodEnd.Value.Year);

                sb.AppendFormat("FILTER: TimePeriod from {0} to {1}", timePeriodStart, timePeriodEnd);



                sb.Append("</div>");


                sb.AppendFormat("<div class=\"filter-actions\" onclick=\"OnFilterDeleteClick(this,'{0}')\" ><img width=\"30\" height=\"28\" src=\"{1}\" alt=\"Delete\" title=\"Delete the filter\" /></div>", "timeperiod", this.ResolveClientUrl("~/content/images/close-filter.png"));
                sb.Append("<div class=\"filter-clear\"></div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            #endregion

            return sb.ToString();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                if (string.IsNullOrEmpty(Request.Params["surveyid"]))
                    throw new ArgumentNullException("surveyid");
                this.Surveyid = Int32.Parse(Request.Params["surveyid"]);


                if (Request.Params["textslanguage"] != null)
                    this.TextsLanguage = Int16.Parse(Request.Params["textslanguage"]);

                if (Request.Params["viewId"] != null)
                    this.ViewId = Guid.Parse(Request.Params["viewId"]);
                else
                {
                    this.m_selectedView = SurveyManager.GetDefaultView(this.Surveyid);
                    this.ViewId = this.SelectedView.ViewId;
                }
            }
        }

    }
}