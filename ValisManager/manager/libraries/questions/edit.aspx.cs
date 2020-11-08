using System;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.libraries.questions
{
    public partial class edit : ManagerPage
    {
        protected string QuestionTitle
        {
            get
            {
                if (this.SelectedQuestion == null)
                    return String.Empty;
                if (string.IsNullOrWhiteSpace(this.SelectedQuestion.QuestionText))
                    return string.Empty;

                if (this.SelectedQuestion.QuestionText.Length <= 92)
                    return this.SelectedQuestion.QuestionText;

                return this.SelectedQuestion.QuestionText.Substring(0, 89) + "...";
            }
        }
        protected VLLibraryQuestion SelectedQuestion
        {
            get
            {
                return this.ViewState["SelectedQuestion"] as VLLibraryQuestion;
            }
            private set
            {
                this.ViewState["SelectedQuestion"] = value;
            }
        }
        protected VLLanguage SelectedLanguage
        {
            get
            {
                return this.ViewState["SelectedLanguage"] as VLLanguage;
            }
            private set
            {
                this.ViewState["SelectedLanguage"] = value;
            }
        }

        protected string GetDeleteButtonHandler
        {
            get
            {
                PostBackOptions myPostBackOptions = new PostBackOptions(this.deleteBtn);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.RequiresJavaScriptProtocol = false;
                myPostBackOptions.PerformValidation = false;
                myPostBackOptions.ClientSubmit = true;

                return Page.ClientScript.GetPostBackEventReference(myPostBackOptions);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα language
                    string _value = this.Request.Params["language"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var languageId = Int16.Parse(_value);
                        this.SelectedLanguage = BuiltinLanguages.GetLanguageById(languageId);
                    }
                    else
                    {
                        this.SelectedLanguage = BuiltinLanguages.Invariant;
                    }
                    if (this.SelectedLanguage == null)
                    {
                        throw new VLException("language is invalid!");
                    }


                    //Πρέπει στο url να υπάρχει ένα QuestionId
                    _value = this.Request.Params["QuestionId"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var questionId = Int32.Parse(_value);
                        this.SelectedQuestion = LibraryManager.GetLibraryQuestionById(questionId, this.SelectedLanguage.LanguageId);
                    }
                    if (this.SelectedQuestion == null)
                    {
                        throw new VLException("QuestionId is invalid!");
                    }


                    SetValues(this.SelectedQuestion);
                }
            }
            catch (Exception ex)
            {
                this.Response.Redirect("list.aspx");
            }
        }

        void SetValues(VLLibraryQuestion question)
        {
            this.QuestionType.Text = question.QuestionType.ToString();
            this.QuestionText.Text = question.QuestionText;
            this.IsRequired.Checked = question.IsRequired;
            this.requiredMessage.Text = question.RequiredMessage;

        }
        void GetValues(VLLibraryQuestion question)
        {
            question.QuestionText = this.QuestionText.Text;
            question.IsRequired = this.IsRequired.Checked;
            question.RequiredMessage = this.requiredMessage.Text;
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GetValues(this.SelectedQuestion);

                this.SelectedQuestion = LibraryManager.UpdateLibraryQuestion(this.SelectedQuestion);
                SetValues(this.SelectedQuestion);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                LibraryManager.DeleteLibraryQuestion(this.SelectedQuestion.QuestionId);

                this.Response.Redirect(_UrlSuffix("list.aspx"), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }


    }
}