using System;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    /// <summary>
    /// Αυτή είναι η base class την οποία κληρονοούν όλες οι σελίδες που ασχολούνται με τους collectors
    /// </summary>
    public class CollectorsPage : ManagerPage
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
            set
            {
                this.Context.Items["SelectedCollector"] = value;
            }
        }

        protected string StatusHtml
        {
            get
            {
                if(this.SelectedCollector.Status == CollectorStatus.New)
                {
                    return "<span class=\"collectorstatus\"><span class=\"StatLabel\">STATUS:</span><span class=\"StatNew\">NOT CONFIGURED</span></span>";
                }
                else if(this.SelectedCollector.Status == CollectorStatus.Open)
                {
                    return "<span class=\"collectorstatus\"><span class=\"StatLabel\">STATUS:</span><span class=\"StatOpen\">OPEN</span></span>";
                }
                else
                {
                    return "<span class=\"collectorstatus\"><span class=\"StatLabel\">STATUS:</span><span class=\"StatClosed\">CLOSED</span></span>";
                }
            }
        }


        protected string PaymentMethod
        {
            get
            {
                if (this.SelectedCollector.CreditType.HasValue)
                {
                    if (this.SelectedCollector.CreditType.Value == CreditType.ClickType)
                    {
                        return "by each CLICK on the web link";
                    }
                    else if (this.SelectedCollector.CreditType.Value == CreditType.EmailType)
                    {
                        return "by each EMAIL you send";
                    }
                    else if (this.SelectedCollector.CreditType.Value == CreditType.ResponseType)
                    {
                        return "by each RESPONSE you receive";
                    }
                }
                return string.Empty;
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            try
            {
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
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}