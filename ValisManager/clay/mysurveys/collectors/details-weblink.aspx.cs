using System;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class details_weblink : CollectorsPage
    {
        protected string ManualSurveyRuntimeURL
        {
            get
            {
                return Utility.GetSurveyRuntimeURL(this.SelectedSurvey, this.SelectedCollector, true);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                //EmailLink & HtmlCode
                var surveyUrl = Utility.GetSurveyRuntimeURL(this.SelectedSurvey, this.SelectedCollector);

                this.txtEmailLink.Text = surveyUrl;
                this.txtHtmlCode.Text = string.Format("<a href=\"{0}\">Click here to take survey</a>", surveyUrl);
            }
        }


    }
}