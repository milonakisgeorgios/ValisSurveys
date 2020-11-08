using System;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class CollectorDetails : ManagerMasterPage
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
        public Int32 CollectorId
        {
            get
            {
                Object _obj = this.ViewState["collectorId"];
                if (_obj == null) return -1;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["collectorId"] = value;
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
        protected VLCollector SelectedCollector
        {
            get
            {
                if (this.Context.Items["SelectedCollector"] == null)
                {
                    this.Context.Items["SelectedCollector"] = SurveyManager.GetCollectorById(this.CollectorId, this.TextsLanguage);
                }
                return (VLCollector)this.Context.Items["SelectedCollector"];
            }
        }


        protected string StatusHtml
        {
            get
            {
                if (this.SelectedCollector.Status == CollectorStatus.New)
                {
                    return "<span class=\"collectorstatus\"><span class=\"StatLabel\">STATUS:</span><span class=\"StatNew\">NOT CONFIGURED</span></span>";
                }
                else if (this.SelectedCollector.Status == CollectorStatus.Open)
                {
                    return "<span class=\"collectorstatus\"><span class=\"StatLabel\">STATUS:</span><span class=\"StatOpen\">OPEN</span></span>";
                }
                else
                {
                    return "<span class=\"collectorstatus\"><span class=\"StatLabel\">STATUS:</span><span class=\"StatClosed\">CLOSED</span></span>";
                }
            }
        }

        protected string GetTextsLanguageThumbnail()
        {
            return this.ResolveClientUrl(string.Format("<img src=\"{0}/{1}\" alt=\"{2}\"/>", this.ResolveClientUrl("~/content/flags/"), BuiltinLanguages.GetLanguageThumbnail(this.TextsLanguage), BuiltinLanguages.GetLanguageById(this.TextsLanguage).EnglishName));
        }



        protected string ManualSurveyRuntimeURL
        {
            get
            {
                if(this.SelectedCollector.CollectorType == CollectorType.WebLink)
                    return Utility.GetSurveyRuntimeURL(this.SelectedSurvey, this.SelectedCollector, true);

                return string.Empty;
            }
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                if (string.IsNullOrEmpty(Request.Params["surveyid"]))
                    throw new ArgumentNullException("surveyid");
                this.Surveyid = Int32.Parse(Request.Params["surveyid"]);

                if (string.IsNullOrEmpty(Request.Params["collectorId"]))
                    throw new ArgumentNullException("collectorId");
                this.CollectorId = Int32.Parse(Request.Params["collectorId"]);

                if (Request.Params["textslanguage"] != null)
                    this.TextsLanguage = Int16.Parse(Request.Params["textslanguage"]);
            }
        }
    }
}