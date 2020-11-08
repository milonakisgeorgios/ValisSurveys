using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Valis.Core;
using Valis.Core.Html;

namespace ValisManager.clay.mysurveys
{
    public partial class Design_Survey : ManagerPage
    {
        VLSurvey m_selectedSurvey;
        VLSurveyPage m_selectedSurveyPage;

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
                if (_obj == null) return -1;
                return (Int16)_obj;
            }
            set
            {
                this.ViewState["textslanguage"] = value;
            }
        }

        public Int16 SurveyPageId
        {
            get
            {
                Object _obj = this.ViewState["pageId"];
                if (_obj == null) return -1;
                return (Int16)_obj;
            }
            set
            {
                this.ViewState["pageId"] = value;
            }
        }

        public VLSurvey SelectedSurvey
        {
            get
            {
                if (m_selectedSurvey == null)
                {
                    m_selectedSurvey = SurveyManager.GetSurveyById(this.Surveyid, textsLanguage: this.TextsLanguage);
                }
                return m_selectedSurvey;
            }
        }

        public VLSurveyPage SelectedPage
        {
            get
            {
                return m_selectedSurveyPage;
            }
        }


        protected string GetTextsLanguageThumbnail()
        {
            return this.ResolveClientUrl(string.Format("<img src=\"{0}/{1}\" alt=\"{2}\"/>", this.ResolveClientUrl("~/content/flags/"), BuiltinLanguages.GetLanguageThumbnail(this.TextsLanguage), BuiltinLanguages.GetLanguageById(this.TextsLanguage).EnglishName));
        }
        protected string GetTextsLanguageStrip()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var lang in this.SelectedSurvey.SupportedLanguages)
            {
                if (lang.LanguageId == BuiltinLanguages.Invariant.LanguageId)
                    continue;
                var _url = string.Format("~/clay/mysurveys/Design_Survey.aspx?surveyid={0}&language={1}", this.SelectedSurvey.SurveyId, lang.LanguageId); 
                var _src = string.Format("~/content/flags/{0}", BuiltinLanguages.GetLanguageThumbnail(lang.LanguageId) );
                sb.AppendFormat("<a title=\"Design the {0} version of the survey!\" href=\"{1}\"><img alt=\"{2}\" src=\"{3}\"></a>", HttpUtility.HtmlEncode(lang.EnglishName), this.ResolveClientUrl(_url), HttpUtility.HtmlEncode(lang.EnglishName), this.ResolveClientUrl(_src));
                sb.Append("&nbsp;&nbsp;");
            }

            return sb.ToString();
        }

        protected string GetQuestionTypeOptions()
        {
            StringBuilder options = new StringBuilder();

            //SingleLine
            options.AppendFormat("<option value=\"{0}\">SingleLine</option>", QuestionType.SingleLine);
            //MultipleLine
            options.AppendFormat("<option value=\"{0}\">MultipleLine</option>", QuestionType.MultipleLine);
            //Integer
            options.AppendFormat("<option value=\"{0}\">Integer</option>", QuestionType.Integer);
            //Decimal
            options.AppendFormat("<option value=\"{0}\">Decimal</option>", QuestionType.Decimal);
            //Date
            options.AppendFormat("<option value=\"{0}\">Date</option>", QuestionType.Date);
            //Time
            //options.AppendFormat("<option value=\"{0}\">Time</option>", QuestionType.Time);
            //DateTime
            //options.AppendFormat("<option value=\"{0}\">DateTime</option>", QuestionType.DateTime);
            //OneFromMany
            options.AppendFormat("<option value=\"{0}\">OneFromMany</option>", QuestionType.OneFromMany);
            //ManyFromMany
            options.AppendFormat("<option value=\"{0}\">ManyFromMany</option>", QuestionType.ManyFromMany);
            //DropDown
            options.AppendFormat("<option value=\"{0}\">DropDown</option>", QuestionType.DropDown);
            //DescriptiveText
            options.AppendFormat("<option value=\"{0}\">DescriptiveText</option>", QuestionType.DescriptiveText);
            //Slider
            //options.AppendFormat("<option value=\"{0}\">Slider</option>", QuestionType.Slider);
            //Range
            options.AppendFormat("<option value=\"{0}\">Range</option>", QuestionType.Range);
            //MatrixOnePerRow
            options.AppendFormat("<option value=\"{0}\">MatrixOnePerRow</option>", QuestionType.MatrixOnePerRow);
            //MatrixManyPerRow
            options.AppendFormat("<option value=\"{0}\">MatrixManyPerRow</option>", QuestionType.MatrixManyPerRow);
            //MatrixManyPerRowCustom
            //options.AppendFormat("<option value=\"{0}\">MatrixManyPerRowCustom</option>", QuestionType.MatrixManyPerRowCustom);
            //Composite
            //options.AppendFormat("<option value=\"{0}\">Composite</option>", QuestionType.Composite);

            return options.ToString();
        }

        protected string GetQuestionsHtml()
        {
            var _html = new StringBuilder();
            var writer = new HtmlTextWriter(new StringWriter(_html));

            //εάν δεν υπάρχει επιλεγμένη σελίδα, τότε δεν δημιουργούμε αυτό το section:
            if (this.SurveyPageId == -1)
            {
                #region No Pages
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                HttpUtility.HtmlEncode("Το ερωτηματολόγιό σας, δεν περιέχει καμμία σελίδα ερωτήσεων.", writer);
                writer.RenderEndTag();
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                HttpUtility.HtmlEncode("Προσθέστε παρακαλώ, μια σελίδα!", writer);
                writer.RenderEndTag();
                #endregion
                return _html.ToString();
            }

            var questions = SurveyManager.GetQuestionsForPage(this.Surveyid, this.SurveyPageId, this.TextsLanguage);
            //StringBuilder sb = new StringBuilder();

            foreach (var question in questions)
            {
                AddQuestionButtons(writer, question);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionBox");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {

                    if (question.QuestionType == QuestionType.DescriptiveText)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionHeaderTransparent");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionHeader");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    if (question.HasSkipLogic)
                    {
                        writer.Write("<img class=\"branchedMarker\" src=\"/content/images/branch-page-icon.png\" />");
                    }
                    if (question.IsRequired)
                    {
                        writer.Write("<div class=\"requiredMarker\">*</div>");
                    }
                    if (SelectedSurvey.UseQuestionNumbering)
                    {
                        writer.Write(string.Format("{0}. ", question.DisplayOrder));
                    }

                    HttpUtility.HtmlEncode(question.QuestionText, writer);
                    if (!string.IsNullOrWhiteSpace(question.Description))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionDescription");
                        writer.RenderBeginTag(HtmlTextWriterTag.P);
                        HttpUtility.HtmlEncode(question.Description, writer);
                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();//Div.questionHeader
                }
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionHeaderTools");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    DeleteQuestionButton(writer, question);
                    //MoveQuestionButton(writer, question);
                    LogicQuestionButton(writer, question);
                    EditQuestionButton(writer, question);

                    writer.RenderEndTag();//Div.questionHeaderTools
                }

                var renderer = HtmlRenderers.GetQuestionRenderer(SurveyManager, writer, null, question.QuestionType);
                renderer.RenderQuestion(SelectedSurvey, m_selectedSurveyPage, question);

                writer.RenderEndTag();//Div.questionBox
            }
            AddQuestionButtons(writer, null);


            return _html.ToString();
        }

        protected string GetPreviewLink
        {
            get
            {
                return Utility.GetSurveyPreviewURL(this.SelectedSurvey);
            }
        }


        void FillPageSelector()
        {
            var pages = SurveyManager.GetSurveyPages(this.SelectedSurvey);
            
            //Διαβάζουμε τις σελίδες
            this.pagesSelector.Items.Clear();
            foreach(var page in pages)
            {
                if (m_selectedSurveyPage == null)
                {
                    m_selectedSurveyPage = page;
                    this.SurveyPageId = page.PageId;
                }

                var newItem = new ListItem();
                newItem.Text = "Page " + page.DisplayOrder.ToString()+". "+ this.Server.HtmlEncode(page.ShowTitle);
                newItem.Value = page.PageId.ToString();
                if(page.HasSkipLogic)
                {
                    newItem.Attributes.Add("data-image", "/content/images/branch-page-icon.png");
                }

                if(this.SurveyPageId == page.PageId)
                {
                    newItem.Selected = true;
                }

                this.pagesSelector.Items.Add(newItem);
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                if (this.IsPostBack == false)
                {
                    if (string.IsNullOrEmpty(Request.Params["surveyid"]))
                        throw new ArgumentNullException("surveyid");
                    if (string.IsNullOrEmpty(Request.Params["language"]))
                        throw new ArgumentNullException("language");

                    this.Surveyid = Int32.Parse(Request.Params["surveyid"]);
                    this.TextsLanguage = Int16.Parse(Request.Params["language"]);
                    /*we check the validity of the language:*/
                    if(BuiltinLanguages.GetLanguageById(this.TextsLanguage) == null)
                    {
                        throw new ArgumentException(string.Format("Invalid languageId {0}!", this.TextsLanguage));
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(Request.Params["pageId"]))
                        {
                            this.SurveyPageId = Int16.Parse(Request.Params["pageId"]);
                            this.m_selectedSurveyPage = SurveyManager.GetSurveyPageById(this.Surveyid, this.SurveyPageId, this.TextsLanguage);
                        }
                    }
                    catch
                    {
                        this.SurveyPageId = default(Int16);
                        this.m_selectedSurveyPage = null;
                    }

                    FillPageSelector();
                }
            }
            catch
            {
                Response.Redirect("~/clay/mysurveys/mysurveys.aspx", true);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "localVariables1", string.Format("var selectedSurveyId = '{0}';var selectedSurveyPageId = {1};var selectedTextsLanguage = {2};", this.Surveyid, this.SurveyPageId, this.TextsLanguage), true);

            base.OnPreRender(e);
        }

        void AddQuestionButton(HtmlTextWriter writer, VLSurveyQuestion question)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionAddButton");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionAddLink");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnQuestionAdd({0}, {1}, {2}, {3},{4})", this.Surveyid, this.SurveyPageId, (Int16)InsertPosition.Before, question != null ? question.QuestionId.ToString() : "null", this.TextsLanguage));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            HttpUtility.HtmlEncode("Add Question", writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
        void AddLibraryQuestionButton(HtmlTextWriter writer, VLSurveyQuestion question)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "libraryQuestionAddButton");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "libraryQuestionAddLink");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnLibraryQuestionAdd({0}, {1}, {2}, {3},{4})", this.Surveyid, this.SurveyPageId, (Int16)InsertPosition.Before, question != null ? question.QuestionId.ToString() : "null", this.TextsLanguage));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            HttpUtility.HtmlEncode("Add Library Question", writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
        void AddQuestionButtons(HtmlTextWriter writer, VLSurveyQuestion question)
        {

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionbuttons");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                AddQuestionButton(writer, question);
                AddLibraryQuestionButton(writer, question);
            }
            writer.RenderEndTag();
        }
        
        void LogicQuestionButton(HtmlTextWriter writer, VLSurveyQuestion question)
        {
            if (question.QuestionType == QuestionType.OneFromMany || question.QuestionType == QuestionType.DropDown)
            {
                if(question.HasSkipLogic)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionLogicButton");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionLogicLink greenbutton");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnQuestionLogicBtn({0}, {1}, {2}, true)", question.Survey, question.QuestionId, question.TextsLanguage));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    HttpUtility.HtmlEncode("Edit Question Logic", writer);
                    writer.RenderEndTag();

                    writer.RenderEndTag();
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionLogicButton");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionLogicLink greybutton");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnQuestionLogicBtn({0}, {1}, {2}, false)", question.Survey, question.QuestionId, question.TextsLanguage));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    HttpUtility.HtmlEncode("Add Question Logic", writer);
                    writer.RenderEndTag();

                    writer.RenderEndTag();
                }
            }
        }
        void EditQuestionButton(HtmlTextWriter writer, VLSurveyQuestion question)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionEditButton");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionEditLink greybutton");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnQuestionEdit({0}, {1}, {2})", question.Survey, question.QuestionId, question.TextsLanguage));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            HttpUtility.HtmlEncode("Edit", writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
        void DeleteQuestionButton(HtmlTextWriter writer, VLSurveyQuestion question)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionDeleteButton");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionDeleteLink redbutton");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnQuestionDelete({0}, {1}, {2})", question.Survey, question.QuestionId, question.TextsLanguage));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            HttpUtility.HtmlEncode("Delete", writer);
            writer.RenderEndTag();

            writer.RenderEndTag();

        }
        void MoveQuestionButton(HtmlTextWriter writer, VLSurveyQuestion question)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionMoveButton");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionMoveLink greybutton");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("javascript:OnQuestionMove({0}, {1})", question.Survey, question.QuestionId));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            HttpUtility.HtmlEncode("Move", writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        protected void pagesSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SurveyPageId = Int16.Parse(this.pagesSelector.SelectedValue);
            this.m_selectedSurveyPage = SurveyManager.GetSurveyPageById(this.Surveyid, this.SurveyPageId, this.TextsLanguage);
        }


    }
}