using System;
using System.Text;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.clay.controls
{
    public partial class CollectorNavigation : ManagerUserControl
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            StringBuilder sb = new StringBuilder();
            if (SelectedCollector.CollectorType == CollectorType.WebLink)
            {
                //Overview
                if (this.Request.RawUrl.Contains("details-weblink.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"details-weblink.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"details-weblink.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);

                //Charges
                if(Globals.UseCredits)
                {
                    //Change Settings
                    if (this.Request.RawUrl.Contains("charges.aspx"))
                        sb.AppendFormat("<li><a class=\"selected\" href=\"charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Payments</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                    else
                        sb.AppendFormat("<li><a href=\"charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Payments</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);                
                }
                //Change Settings
                if (this.Request.RawUrl.Contains("settings.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"settings.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Settings</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"settings.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Settings</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                
                //Change Restrictions
                if (this.Request.RawUrl.Contains("restrictions.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"restrictions.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Restrictions</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"restrictions.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Restrictions</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                
                //Manual Data Entry
                if (this.SelectedCollector.Status == CollectorStatus.Open)
                {
                    sb.AppendFormat("<li><a href=\"javascript:OnManualwebLinkDataEntry({0}, {1});\">Manual Data Entry</a></li>", this.CollectorId, this.TextsLanguage);
                }
                else
                {
                    sb.Append("<li><span class=\"disabled\">Manual Data Entry</span></li>");
                }

                //Close Collector Now
                if (this.SelectedCollector.Status == CollectorStatus.Open)
                {
                    sb.AppendFormat("<li><a href=\"javascript:OnCloseCollector({0}, {1});\">Close Collector Now</a></li>", this.CollectorId, this.TextsLanguage);
                }
                else
                {
                    //sb.Append("<li><span class=\"disabled\">Close Collector Now</span></li>");
                    sb.AppendFormat("<li><a href=\"javascript:OnOpenCollector({0}, {1});\">Open Collector Now</a></li>", this.CollectorId, this.TextsLanguage);
                }
            }
            else if (SelectedCollector.CollectorType == CollectorType.Email)
            {
                //Overview
                if (this.Request.RawUrl.Contains("details-email.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"details-email.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"details-email.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                
                //Recipients
                if (this.Request.RawUrl.Contains(@"/recipients.aspx") || this.Request.RawUrl.Contains(@"/recipientsAdd.aspx") || this.Request.RawUrl.Contains(@"/recipientsDownload.aspx") || this.Request.RawUrl.Contains(@"/recipientsRemove.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"recipients.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Recipients</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"recipients.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Recipients</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                //Messages
                if (this.Request.RawUrl.Contains(@"/messages.aspx") || this.Request.RawUrl.Contains(@"/editMessage_") || this.Request.RawUrl.Contains(@"/message_preview"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"messages.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Messages</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"messages.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Messages</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);

                //Charges
                if (Globals.UseCredits)
                {
                    //Change Settings
                    if (this.Request.RawUrl.Contains("charges.aspx"))
                        sb.AppendFormat("<li><a class=\"selected\" href=\"charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Payments</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                    else
                        sb.AppendFormat("<li><a href=\"charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Payments</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                }
                //Change Settings
                if (this.Request.RawUrl.Contains("settings.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"settings.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Settings</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"settings.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Settings</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                //Change Restrictions
                if (this.Request.RawUrl.Contains("restrictions.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"restrictions.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Restrictions</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"restrictions.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Restrictions</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);


                //Close Collector Now
                if (this.SelectedCollector.TotalMessages > 0 || this.SelectedCollector.HasSentEmails)
                {
                    if (this.SelectedCollector.Status == CollectorStatus.Open)
                    {
                        sb.AppendFormat("<li><a href=\"javascript:OnCloseCollector({0}, {1});\">Close Collector Now</a></li>", this.CollectorId, this.TextsLanguage);
                    }
                    else
                    {
                        //sb.Append("<li><span class=\"disabled\">Close Collector Now</span></li>");
                        sb.AppendFormat("<li><a href=\"javascript:OnOpenCollector({0}, {1});\">Open Collector Now</a></li>", this.CollectorId, this.TextsLanguage);
                    }
                }
            }
            else if (SelectedCollector.CollectorType == CollectorType.Website)
            {
                //Survey Configuration
                if (this.Request.RawUrl.Contains("configuration-website.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"configuration-website.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"configuration-website.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                //Website Collector
                if (this.Request.RawUrl.Contains("details-website.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"details-website.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"details-website.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Overview</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);

                //Charges
                if (Globals.UseCredits)
                {
                    //Change Settings
                    if (this.Request.RawUrl.Contains("charges.aspx"))
                        sb.AppendFormat("<li><a class=\"selected\" href=\"charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Payments</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                    else
                        sb.AppendFormat("<li><a href=\"charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Payments</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                }
                //Change Settings
                if (this.Request.RawUrl.Contains("settings.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"settings.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Settings</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"settings.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Settings</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                //Change Restrictions
                if (this.Request.RawUrl.Contains("restrictions.aspx"))
                    sb.AppendFormat("<li><a class=\"selected\" href=\"restrictions.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Restrictions</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                else
                    sb.AppendFormat("<li><a href=\"restrictions.aspx?surveyid={0}&collectorId={1}&textslanguage={2}\">Change Restrictions</a></li>", this.Surveyid, this.CollectorId, this.TextsLanguage);
                
                //Manual Data Entry
                if (this.SelectedCollector.Status == CollectorStatus.Open)
                {
                    sb.Append("<li><a href=\"#\">Manual Data Entry</a></li>");
                }
                else
                {
                    sb.Append("<li><span class=\"disabled\">Manual Data Entry</span></li>");
                }

                //Close Collector Now
                if (this.SelectedCollector.Status == CollectorStatus.Open)
                {
                    sb.AppendFormat("<li><a href=\"javascript:OnCloseCollector({0}, {1});\">Close Collector Now</a></li>", this.CollectorId, this.TextsLanguage);
                }
                else
                {
                    sb.Append("<li><span class=\"disabled\">Close Collector Now</span></li>");
                }
            }

            //Collectors Return
            sb.AppendFormat("<li><a href=\"list.aspx?surveyid={0}\">Return</a></li>", this.Surveyid);

            this.PlaceHolder1.Controls.Add(new LiteralControl(sb.ToString()));
        }
    }
}