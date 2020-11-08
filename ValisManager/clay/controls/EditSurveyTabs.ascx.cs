using System;

namespace ValisManager.clay.controls
{
    public partial class EditSurveyTabs : ManagerUserControl
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
                if (_obj == null) return -1;
                return (Int16)_obj;
            }
            set
            {
                this.ViewState["textslanguage"] = value;
            }
        }

        protected string DesignTabClass
        {
            get
            {
                if (this.Request.RawUrl.Contains("Design_Survey.aspx"))
                    return "tab selected";
                return "tab";
            }
        }
        protected string DesignLink
        {
            get
            {
                return string.Format("Design_Survey.aspx?surveyid={0}&language={1}", this.Surveyid, this.TextsLanguage);
            }
        }

        protected string EditTabClass
        {
            get
            {
                if (this.Request.RawUrl.Contains("Edit_Survey.aspx"))
                    return "tab selected";
                return "tab";
            }
        }
        protected string EditLink
        {
            get
            {
                return string.Format("Edit_Survey.aspx?surveyid={0}&language={1}", this.Surveyid, this.TextsLanguage);
            }
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
    }
}