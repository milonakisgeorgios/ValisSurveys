using System;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class settings : CollectorsPage
    {

        #region SaveIPAddressOrEmail support
        protected string SaveIPAddressOrEmail_Title
        {
            get
            {
                if(this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Email)
                {
                    return "Save Email Address in Results?";
                }
                else if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.WebLink)
                {
                    return "Save IP Address in Results?";
                }
                else if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Website)
                {
                    return "Save IP Address in Results?";
                }

                return string.Empty;
            }
        }
        protected string SaveIPAddressOrEmail_Option0
        {
            get
            {
                if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Email)
                {
                    return "<b>No</b>, the respondent's email address will not be stored in the survey results.";
                }
                else if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.WebLink)
                {
                    return "<b>No</b>, the respondent's IP address will not be stored in the survey results.";
                }
                else if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Website)
                {
                    return "<b>No</b>, the respondent's IP address will not be stored in the survey results.";
                }

                return string.Empty;
            }
        }
        protected string SaveIPAddressOrEmail_Option1
        {
            get
            {
                if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Email)
                {
                    return "<b>Yes</b>, the respondent's email address will be stored in the survey results.";
                }
                else if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.WebLink)
                {
                    return "<b>Yes</b>, the respondent's IP address will be stored in the survey results.";
                }
                else if (this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Website)
                {
                    return "<b>Yes</b>, the respondent's IP address will be stored in the survey results.";
                }

                return string.Empty;
            }
        }
        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(this.IsPostBack == false)
            {
                if (this.SelectedCollector.AllowMultipleResponsesPerComputer)
                {
                    this.AllowMultipleResponses_0.Checked = false;
                    this.AllowMultipleResponses_1.Checked = true;
                }
                else
                {
                    this.AllowMultipleResponses_0.Checked = true;
                    this.AllowMultipleResponses_1.Checked = false;
                }

                if(this.SelectedCollector.EditResponseMode == Valis.Core.EditResponseModes.NotAllowed)
                {
                    this.EditResponseMode_0.Checked = true;
                    this.EditResponseMode_1.Checked = false;
                    this.EditResponseMode_2.Checked = false;
                }
                else if (this.SelectedCollector.EditResponseMode == Valis.Core.EditResponseModes.AllowedBetween)
                {
                    this.EditResponseMode_0.Checked = false;
                    this.EditResponseMode_1.Checked = true;
                    this.EditResponseMode_2.Checked = false;
                }
                else if (this.SelectedCollector.EditResponseMode == Valis.Core.EditResponseModes.AllowedAlways)
                {
                    this.EditResponseMode_0.Checked = false;
                    this.EditResponseMode_1.Checked = false;
                    this.EditResponseMode_2.Checked = true;
                }

                if(this.SelectedCollector.DisplayInstantResults)
                {
                    this.DisplayInstantResults_0.Checked = false;
                    this.DisplayInstantResults_1.Checked = true;
                }
                else
                {
                    this.DisplayInstantResults_0.Checked = true;
                    this.DisplayInstantResults_1.Checked = false;
                }
                this.DisplayNumberOfResponses.Checked = this.SelectedCollector.DisplayNumberOfResponses;


                if (this.SelectedCollector.OnCompletionMode == Valis.Core.EndSurveyMode.CloseWindow)
                {
                    CompletionMode_0.Checked = true;
                }
                else if (this.SelectedCollector.OnCompletionMode == Valis.Core.EndSurveyMode.GoToUrl)
                {
                    CompletionMode_1.Checked = true;
                    OnCompletionURL.Text = this.SelectedCollector.OnCompletionURL;
                }

                if(this.SelectedCollector.UseSSL)
                {
                    this.UseSSL_1.Checked = true;
                }
                else
                {
                    this.UseSSL_0.Checked = true;
                }

                if(this.SelectedCollector.SaveIPAddressOrEmail)
                {
                    this.SaveIPAddressOrEmail_1.Checked = true;
                }
                else
                {
                    this.SaveIPAddressOrEmail_0.Checked = true;
                }

            }
        }

        protected void saveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.SelectedCollector.CollectorType != Valis.Core.CollectorType.Email)
                {
                    if (this.AllowMultipleResponses_0.Checked)
                        this.SelectedCollector.AllowMultipleResponsesPerComputer = false;
                    else
                        this.SelectedCollector.AllowMultipleResponsesPerComputer = true;
                }

                if (this.EditResponseMode_0.Checked)
                    this.SelectedCollector.EditResponseMode = Valis.Core.EditResponseModes.NotAllowed;
                else if (this.EditResponseMode_1.Checked)
                    this.SelectedCollector.EditResponseMode = Valis.Core.EditResponseModes.AllowedBetween;
                else if (this.EditResponseMode_2.Checked)
                    this.SelectedCollector.EditResponseMode = Valis.Core.EditResponseModes.AllowedAlways;


                if(this.DisplayInstantResults_0.Checked)
                {
                    this.SelectedCollector.DisplayInstantResults = false;
                    this.SelectedCollector.DisplayNumberOfResponses = false;
                }
                else
                {
                    this.SelectedCollector.DisplayInstantResults = true;
                    this.SelectedCollector.DisplayNumberOfResponses = this.DisplayNumberOfResponses.Checked;
                }

                if (this.CompletionMode_0.Checked)
                {
                    this.SelectedCollector.OnCompletionMode = Valis.Core.EndSurveyMode.CloseWindow;
                    this.SelectedCollector.OnCompletionURL = null;
                }
                else if (this.CompletionMode_1.Checked)
                {
                    this.SelectedCollector.OnCompletionMode = Valis.Core.EndSurveyMode.GoToUrl;
                    this.SelectedCollector.OnCompletionURL = this.OnCompletionURL.Text;
                }

                if(this.UseSSL_0.Checked)
                {
                    this.SelectedCollector.UseSSL = false;
                }
                else
                {
                    this.SelectedCollector.UseSSL = true;
                }

                if(this.SaveIPAddressOrEmail_0.Checked)
                {
                    this.SelectedCollector.SaveIPAddressOrEmail = false;
                }
                else
                {
                    this.SelectedCollector.SaveIPAddressOrEmail = true;
                }

                this.SelectedCollector = SurveyManager.UpdateCollector(this.SelectedCollector);
                this.InfoMessage = "Settings saved succesfully!";
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}