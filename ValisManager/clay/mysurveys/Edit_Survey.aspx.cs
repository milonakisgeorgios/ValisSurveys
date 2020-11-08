using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.clay.mysurveys
{
    public partial class Edit_Survey : ManagerPage
    {
        VLSurvey m_selectedSurvey;

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

        protected string GetTextsLanguageThumbnail()
        {
            return this.ResolveClientUrl(string.Format("<img src=\"{0}/{1}\" alt=\"{2}\"/>", this.ResolveClientUrl("~/content/flags/"), BuiltinLanguages.GetLanguageThumbnail(this.TextsLanguage), BuiltinLanguages.GetLanguageById(this.TextsLanguage).EnglishName));
        }

        /// <summary>
        /// Εάν το survey υποστηρίζει πολλές γλώσσες, τότε επιστρέφει το εικονίδια 'Translatable.png'
        /// </summary>
        /// <returns></returns>
        protected string CheckAndGetTranslatableIcon()
        {
            if (this.SelectedSurvey.TextsLanguage != 0)
            {
                return this.GetTranslatableIcon();
            }

            return string.Empty;
        }



        protected override void OnLoad(EventArgs e)
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
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnPreLoad(e);
            if(this.IsPostBack == false)
            {
                /*TAB1*/
                {
                    this.ShowLanguageSelector.Checked = SelectedSurvey.ShowLanguageSelector;
                    /*Page and Question Numbering*/
                    this.UsePageNumbering.Checked = SelectedSurvey.UsePageNumbering;
                    this.UseQuestionNumbering.Checked = SelectedSurvey.UseQuestionNumbering;
                    if (SelectedSurvey.QuestionNumberingType == QuestionNumberingType.NumberingEntireSurvey)
                        this.NumberingEntireSurvey.Checked = true;
                    else
                        this.NumberingPerPage.Checked = true;
                    /*Progress Bar Settings*/
                    this.UseProgressBar.Checked = SelectedSurvey.UseProgressBar;
                    this.ProgressBarPosition.SelectedValue = SelectedSurvey.ProgressBarPosition.ToString();
                    /*Navigation Buttons*/
                    this.StartButton.Text = SelectedSurvey.StartButton;
                    this.PreviousButton.Text = SelectedSurvey.PreviousButton;
                    this.NextButton.Text = SelectedSurvey.NextButton;
                    this.DoneButton.Text = SelectedSurvey.DoneButton;
                    /*Required Question Highlight*/
                    if (SelectedSurvey.RequiredHighlightType == RequiredHighlightType.None)
                        this.RequiredHighlightType_0.Checked = true;
                    else
                        this.RequiredHighlightType_1.Checked = true;
                }
                //TAB2 - Title
                {
                    this.SurveyTitle.Text = SelectedSurvey.Title;
                    this.HeaderHtml.Text = SelectedSurvey.HeaderHtml;

                    this.ShowSurveyTitle.Checked = SelectedSurvey.ShowSurveyTitle;
                    this.ShowPageTitles.Checked = SelectedSurvey.ShowPageTitles;
                }
                //TAB3 - Welcome Page
                {
                    this.ShowWelcomePage.Checked = SelectedSurvey.ShowWelcomePage;
                    this.WelcomeHtml.Text = SelectedSurvey.WelcomeHtml;
                }
                //TAB4 - Goodbye Page
                {
                    this.ShowGoodbyePage.Checked = SelectedSurvey.ShowGoodbyePage;
                    this.GoodbyeHtml.Text = SelectedSurvey.GoodbyeHtml;
                }
                //TAB5 - DisqualificationHtml
                {
                    this.OnDisqualificationMode.SelectedValue = ((byte)SelectedSurvey.OnDisqualificationMode).ToString(CultureInfo.InvariantCulture);
                    this.DisqualificationUrl.Text = SelectedSurvey.DisqualificationUrl;
                    this.DisqualificationHtml.Text = SelectedSurvey.DisqualificationHtml;
                }
                //TAB6 - Footer
                {
                    this.ShowCustomFooter.Checked = SelectedSurvey.ShowCustomFooter;
                    this.FooterHtml.Text = SelectedSurvey.FooterHtml;
                }
                //TAB7
                {
                    this.OnCompletionMode.SelectedValue = ((byte)SelectedSurvey.OnCompletionMode).ToString(CultureInfo.InvariantCulture);
                    this.OnCompletionUrl.Text = SelectedSurvey.OnCompletionUrl;
                }
            }

            if (this.IsPostBack && Request.Form["__EVENTARGUMENT"] == "OnSaveButton")
            {
                OnSaveButton(this, EventArgs.Empty);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.SaveButton1.HRef = this.ClientScript.GetPostBackClientHyperlink(this.SaveButton1, "OnSaveButton", true);
            base.Render(writer);
        }

        protected void OnSaveButton(object sender, EventArgs e)
        {
            try
            {
                //TAB1
                {
                    SelectedSurvey.ShowLanguageSelector = this.ShowLanguageSelector.Checked;
                    /*Page and Question Numbering*/
                    SelectedSurvey.UsePageNumbering = this.UsePageNumbering.Checked;
                    SelectedSurvey.UseQuestionNumbering = this.UseQuestionNumbering.Checked;
                    if (this.NumberingEntireSurvey.Checked)
                        SelectedSurvey.QuestionNumberingType = QuestionNumberingType.NumberingEntireSurvey;
                    else
                        SelectedSurvey.QuestionNumberingType = QuestionNumberingType.NumberingPerPage;
                    /*Progress Bar Settings*/
                    SelectedSurvey.UseProgressBar = this.UseProgressBar.Checked;
                    SelectedSurvey.ProgressBarPosition = (ProgressBarPosition)Byte.Parse(this.ProgressBarPosition.SelectedValue);
                    /*Navigation Buttons*/
                    SelectedSurvey.StartButton = this.StartButton.Text;
                    SelectedSurvey.PreviousButton = this.PreviousButton.Text;
                    SelectedSurvey.NextButton = this.NextButton.Text;
                    SelectedSurvey.DoneButton = this.DoneButton.Text;
                    /*Required Question Highlight*/
                    if (this.RequiredHighlightType_0.Checked)
                        SelectedSurvey.RequiredHighlightType = RequiredHighlightType.None;
                    else
                        SelectedSurvey.RequiredHighlightType = RequiredHighlightType.UseAsterisk;
                }
                //TAB2 - Title
                {
                    SelectedSurvey.Title = this.SurveyTitle.Text;
                    SelectedSurvey.HeaderHtml = System.Net.WebUtility.HtmlDecode(this.HeaderHtml.Text);

                    SelectedSurvey.ShowSurveyTitle = this.ShowSurveyTitle.Checked;
                    SelectedSurvey.ShowPageTitles = this.ShowPageTitles.Checked;
                }
                //TAB3 - Welcome Page
                {
                    SelectedSurvey.ShowWelcomePage = this.ShowWelcomePage.Checked;
                    SelectedSurvey.WelcomeHtml = System.Net.WebUtility.HtmlDecode(this.WelcomeHtml.Text);
                }
                //TAB4 - Goodbye Page
                {
                    SelectedSurvey.ShowGoodbyePage = this.ShowGoodbyePage.Checked;
                    SelectedSurvey.GoodbyeHtml = System.Net.WebUtility.HtmlDecode(this.GoodbyeHtml.Text);
                }
                //TAB5 - DisqualificationHtml
                {
                    SelectedSurvey.OnDisqualificationMode = (DisqualificationMode)Enum.Parse(typeof(DisqualificationMode), this.OnDisqualificationMode.SelectedValue);
                    SelectedSurvey.DisqualificationUrl = this.DisqualificationUrl.Text;
                    SelectedSurvey.DisqualificationHtml = System.Net.WebUtility.HtmlDecode(this.DisqualificationHtml.Text);
                }
                //TAB6 - Footer
                {
                    SelectedSurvey.ShowCustomFooter = this.ShowCustomFooter.Checked;
                    SelectedSurvey.FooterHtml = System.Net.WebUtility.HtmlDecode(this.FooterHtml.Text);
                }
                //TAB7
                {
                    SelectedSurvey.OnCompletionMode = (EndSurveyMode)Enum.Parse(typeof(EndSurveyMode), this.OnCompletionMode.SelectedValue);
                    SelectedSurvey.OnCompletionUrl = this.OnCompletionUrl.Text;
                }


                SurveyManager.UpdateSurvey(SelectedSurvey);
                Response.Redirect(string.Format("~/clay/mysurveys/Edit_Survey.aspx?surveyid={0}&language={1}", this.Surveyid, this.TextsLanguage), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }



    }
}