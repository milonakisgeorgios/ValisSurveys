using System;
using Valis.Core;

namespace ValisManager.clay.mysurveys.analysis.controls
{
    public partial class analysisTabs : ManagerUserControl
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

        protected string SummaryTabClass
        {
            get
            {
                if (this.Request.RawUrl.Contains("summary.aspx"))
                    return "tab selected";
                return "tab noselected";
            }
        }
        protected string SummaryLink
        {
            get
            {
                if (this.Request.RawUrl.Contains("summary.aspx"))
                    return "#";
                return string.Format("summary.aspx?surveyid={0}&textslanguage={1}", this.Surveyid, this.TextsLanguage);
            }
        }

        protected string ResponsesTabClass
        {
            get
            {
                if (this.Request.RawUrl.Contains("responses.aspx"))
                    return "tab selected";
                return "tab noselected";
            }
        }
        protected string ResponsesLink
        {
            get
            {
                if (this.Request.RawUrl.Contains("responses.aspx"))
                    return "#";
                return string.Format("responses.aspx?surveyid={0}&textslanguage={1}", this.Surveyid, this.TextsLanguage);
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


                if (Request.Params["textslanguage"] != null)
                    this.TextsLanguage = Int16.Parse(Request.Params["textslanguage"]);
            }
        }
    }
}