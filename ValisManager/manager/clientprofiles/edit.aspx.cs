using System;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.clientprofiles
{
    public partial class edit : ManagerPage
    {
        protected VLClientProfile SelectedProfile
        {
            get
            {
                return this.ViewState["SelectedProfile"] as VLClientProfile;
            }
            set
            {
                this.ViewState["SelectedProfile"] = value;
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
                if(!this.IsPostBack)
                {
                    //Πρέπει στο URL να υπάρχει ένα ProfileId
                    string _value = this.Request.Params["ProfileId"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var profileId = Int32.Parse(_value);
                        this.SelectedProfile = SystemManager.GetClientProfileById(profileId);
                    }
                    if (this.SelectedProfile == null)
                        throw new VLException("ProfileId is invalid!");

                    SetValues(this.SelectedProfile);
                }
            }
            catch (Exception ex)
            {
                this.Response.Redirect("list.aspx");
            }
        }

        private void SetValues(VLClientProfile profile)
        {
            this.Name.Text = profile.Name;
            this.Comment.Text = profile.Comment;
            this.MaxNumberOfUsers.Text = profile.MaxNumberOfUsers.ToString();
            this.MaxNumberOfSurveys.Text = profile.MaxNumberOfSurveys.ToString();
            this.MaxNumberOfLists.Text = profile.MaxNumberOfLists.ToString();
            this.MaxNumberOfRecipientsPerList.Text = profile.MaxNumberOfRecipientsPerList.ToString();
            this.MaxNumberOfRecipientsPerMessage.Text = profile.MaxNumberOfRecipientsPerMessage.ToString();
            this.MaxNumberOfCollectorsPerSurvey.Text = profile.MaxNumberOfCollectorsPerSurvey.ToString();
            this.MaxNumberOfEmailsPerDay.Text = profile.MaxNumberOfEmailsPerDay.ToString();
            this.MaxNumberOfEmailsPerWeek.Text = profile.MaxNumberOfEmailsPerWeek.ToString();
            this.MaxNumberOfEmailsPerMonth.Text = profile.MaxNumberOfEmailsPerMonth.ToString();
            this.MaxNumberOfEmails.Text = profile.MaxNumberOfEmails.ToString();

            this.UseCredits.Checked = profile.UseCredits;
            this.CanTranslateSurveys.Checked = profile.CanTranslateSurveys;
            this.CanUseSurveyTemplates.Checked = profile.CanUseSurveyTemplates;
            this.CanUseQuestionTemplates.Checked = profile.CanUseQuestionTemplates;
            this.CanCreateWebLinkCollectors.Checked = profile.CanCreateWebLinkCollectors;
            this.CanCreateEmailCollectors.Checked = profile.CanCreateEmailCollectors;
            this.CanCreateWebsiteCollectors.Checked = profile.CanCreateWebsiteCollectors;
            this.CanUseSkipLogic.Checked = profile.CanUseSkipLogic;
            this.CanExportData.Checked = profile.CanExportData;
            this.CanExportReport.Checked = profile.CanExportReport;
            this.CanUseWebAPI.Checked = profile.CanUseWebAPI;
        }
        private void GetValues(VLClientProfile profile)
        {
            profile.Name = this.Name.Text;
            profile.Comment = this.Comment.Text;
            profile.UseCredits = this.UseCredits.Checked;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfUsers.Text))
                profile.MaxNumberOfUsers = Int32.Parse(this.MaxNumberOfUsers.Text);
            else
                profile.MaxNumberOfUsers = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfSurveys.Text))
                profile.MaxNumberOfSurveys = Int32.Parse(this.MaxNumberOfSurveys.Text);
            else
                profile.MaxNumberOfSurveys = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfLists.Text))
                profile.MaxNumberOfLists = Int32.Parse(this.MaxNumberOfLists.Text);
            else
                profile.MaxNumberOfLists = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfRecipientsPerList.Text))
                profile.MaxNumberOfRecipientsPerList = Int32.Parse(this.MaxNumberOfRecipientsPerList.Text);
            else
                profile.MaxNumberOfRecipientsPerList = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfRecipientsPerMessage.Text))
                profile.MaxNumberOfRecipientsPerMessage = Int32.Parse(this.MaxNumberOfRecipientsPerMessage.Text);
            else
                profile.MaxNumberOfRecipientsPerMessage = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfCollectorsPerSurvey.Text))
                profile.MaxNumberOfCollectorsPerSurvey = Int32.Parse(this.MaxNumberOfCollectorsPerSurvey.Text);
            else
                profile.MaxNumberOfCollectorsPerSurvey = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmailsPerDay.Text))
                profile.MaxNumberOfEmailsPerDay = Int32.Parse(this.MaxNumberOfEmailsPerDay.Text);
            else
                profile.MaxNumberOfEmailsPerDay = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmailsPerWeek.Text))
                profile.MaxNumberOfEmailsPerWeek = Int32.Parse(this.MaxNumberOfEmailsPerWeek.Text);
            else
                profile.MaxNumberOfEmailsPerWeek = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmailsPerMonth.Text))
                profile.MaxNumberOfEmailsPerMonth = Int32.Parse(this.MaxNumberOfEmailsPerMonth.Text);
            else
                profile.MaxNumberOfEmailsPerMonth = null;
            if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmails.Text))
                profile.MaxNumberOfEmails = Int32.Parse(this.MaxNumberOfEmails.Text);
            else
                profile.MaxNumberOfEmails = null;

            profile.CanTranslateSurveys = this.CanTranslateSurveys.Checked;
            profile.CanUseSurveyTemplates = this.CanUseSurveyTemplates.Checked;
            profile.CanUseQuestionTemplates = this.CanUseQuestionTemplates.Checked;
            profile.CanCreateWebLinkCollectors = this.CanCreateWebLinkCollectors.Checked;
            profile.CanCreateEmailCollectors = this.CanCreateEmailCollectors.Checked;
            profile.CanCreateWebsiteCollectors = this.CanCreateWebsiteCollectors.Checked;
            profile.CanUseSkipLogic = this.CanUseSkipLogic.Checked;
            profile.CanExportData = this.CanExportData.Checked;
            profile.CanExportReport = this.CanExportReport.Checked;
            profile.CanUseWebAPI = this.CanUseWebAPI.Checked;
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GetValues(this.SelectedProfile);

                SystemManager.UpdateClientProfile(this.SelectedProfile);

                this.Response.Redirect(_UrlSuffix("list.aspx"), false);
                this.Context.ApplicationInstance.CompleteRequest();
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
                SystemManager.DeleteClientProfile(this.SelectedProfile.ProfileId);

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