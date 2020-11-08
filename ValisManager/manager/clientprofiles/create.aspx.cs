using System;
using Valis.Core;


namespace ValisManager.manager.clientprofiles
{
    public partial class create : ManagerPage
    {


        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var profile = new VLClientProfile();
                profile.Name = this.Name.Text;
                profile.Comment = this.Comment.Text;
                profile.UseCredits = this.UseCredits.Checked;
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfUsers.Text))
                    profile.MaxNumberOfUsers = Int32.Parse(this.MaxNumberOfUsers.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfSurveys.Text))
                    profile.MaxNumberOfSurveys = Int32.Parse(this.MaxNumberOfSurveys.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfLists.Text))
                    profile.MaxNumberOfLists = Int32.Parse(this.MaxNumberOfLists.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfRecipientsPerList.Text))
                    profile.MaxNumberOfRecipientsPerList = Int32.Parse(this.MaxNumberOfRecipientsPerList.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfRecipientsPerMessage.Text))
                    profile.MaxNumberOfRecipientsPerMessage = Int32.Parse(this.MaxNumberOfRecipientsPerMessage.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfCollectorsPerSurvey.Text))
                    profile.MaxNumberOfCollectorsPerSurvey = Int32.Parse(this.MaxNumberOfCollectorsPerSurvey.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmailsPerDay.Text))
                    profile.MaxNumberOfEmailsPerDay = Int32.Parse(this.MaxNumberOfEmailsPerDay.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmailsPerWeek.Text))
                    profile.MaxNumberOfEmailsPerWeek = Int32.Parse(this.MaxNumberOfEmailsPerWeek.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmailsPerMonth.Text))
                    profile.MaxNumberOfEmailsPerMonth = Int32.Parse(this.MaxNumberOfEmailsPerMonth.Text);
                if (!string.IsNullOrWhiteSpace(this.MaxNumberOfEmails.Text))
                    profile.MaxNumberOfEmails = Int32.Parse(this.MaxNumberOfEmails.Text);

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

                profile = SystemManager.CreateClientProfile(profile);


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