using log4net;
using System;
using System.Threading;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.clay.mysurveys
{
    public partial class New_Survey : ManagerPage
    {
        public ILog Logger = LogManager.GetLogger(typeof(New_Survey));

        protected override void OnLoad(EventArgs e)
        {
            if (this.IsPostBack == false)
            {
                //δίνουμε τις γλώσσες:
                this.surveyLanguageCreate.Items.Clear();
                this.surveyLanguageCreate.Items.Add(new ListItem(BuiltinLanguages.Invariant.EnglishName, BuiltinLanguages.Invariant.LanguageId.ToString()));
                this.surveyLanguageCreate.Items.Add(new ListItem(BuiltinLanguages.Greek.EnglishName, BuiltinLanguages.Greek.LanguageId.ToString()));
                this.surveyLanguageCreate.Items.Add(new ListItem(BuiltinLanguages.English.EnglishName, BuiltinLanguages.English.LanguageId.ToString()));
                this.surveyLanguageCreate.Items.Add(new ListItem(BuiltinLanguages.French.EnglishName, BuiltinLanguages.French.LanguageId.ToString()));
                this.surveyLanguageCreate.Items.Add(new ListItem(BuiltinLanguages.German.EnglishName, BuiltinLanguages.German.LanguageId.ToString()));


                //this.surveyLanguageCopy.Items.Clear();
                //this.surveyLanguageCopy.Items.Add(new ListItem(BuiltinLanguages.Invariant.EnglishName, BuiltinLanguages.Invariant.LanguageId.ToString()));
                //this.surveyLanguageCopy.Items.Add(new ListItem(BuiltinLanguages.Greek.EnglishName, BuiltinLanguages.Greek.LanguageId.ToString()));
                //this.surveyLanguageCopy.Items.Add(new ListItem(BuiltinLanguages.English.EnglishName, BuiltinLanguages.English.LanguageId.ToString()));
                //this.surveyLanguageCopy.Items.Add(new ListItem(BuiltinLanguages.French.EnglishName, BuiltinLanguages.French.LanguageId.ToString()));
                //this.surveyLanguageCopy.Items.Add(new ListItem(BuiltinLanguages.German.EnglishName, BuiltinLanguages.German.LanguageId.ToString()));

                //Γεμίζουμε και το surveyList
                var surveys = SurveyManager.GetSurveys();
                this.surveyList.Items.Clear();
                this.surveyList.Items.Add(new ListItem("Choose a survey to copy from...", "0"));
                foreach (var item in surveys)
                {
                    this.surveyList.Items.Add(new ListItem(item.Title, item.SurveyId.ToString()));
                }
            }
        }

        protected void btnAddSrvy_Click(object sender, EventArgs e)
        {
            try
            {
                string surveyCreate = this.Request.Params["SrvyCreate"];
                if (string.Equals(surveyCreate, "rdbtnScratch"))
                {
                    /*
                     * Το survey που θα δημιουργηθεί ανήκει στον τρέχοντα χρήστη που είναι logged-in
                     */
                    if (Globals.UserToken.PrincipalType == PrincipalType.SystemUser)
                    {
                        throw new NotSupportedException("Α systemUser cannot create surveys!");
                    }
                    var clientUser = SystemManager.GetClientUserById(Globals.UserToken.Principal);
                    if (clientUser == null)
                    {
                        throw new VLException(string.Format("Cannot find a clientUser with UserId={0}!", Globals.UserToken.Principal));
                    }

                    var newSurvey = new VLSurvey();
                    newSurvey.Client = clientUser.Client;
                    newSurvey.Title = this.surveyTitleCreate.Text;
                    newSurvey = SurveyManager.CreateSurvey(newSurvey, Int16.Parse(this.surveyLanguageCreate.SelectedValue));

                    Response.Redirect(string.Format("~/clay/mysurveys/Design_Survey.aspx?surveyid={0}&language={1}", newSurvey.SurveyId, newSurvey.TextsLanguage), false);
                    this.Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    /*
                     * Το survey που θα δημιουργηθεί ανήκει στον τρέχοντα χρήστη που είναι logged-in
                     */
                    if (Globals.UserToken.PrincipalType == PrincipalType.SystemUser)
                    {
                        throw new NotSupportedException("Α systemUser cannot create surveys!");
                    }

                    Int32 sourceSurveyId = Int32.Parse(this.surveyList.SelectedValue);
                    string newTitle = this.surveyTitleCopy.Text;
                    Int16 languageId = Int16.Parse(this.Request.Params[this.surveyLanguageCopy.UniqueID]);

                    var newSurvey = SurveyManager.CopySurvey(sourceSurveyId, newTitle, languageId);
                    Response.Redirect(string.Format("~/clay/mysurveys/Design_Survey.aspx?surveyid={0}&language={1}", newSurvey.SurveyId, newSurvey.TextsLanguage), false);
                    this.Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                this.ErrorMessage = ex.Message;
            }
        }

    }
}