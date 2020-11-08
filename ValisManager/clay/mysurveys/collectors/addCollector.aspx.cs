using System;
using System.Threading;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class addCollector : ManagerPage
    {
        VLSurvey m_selectedSurvey;

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

        public VLSurvey SelectedSurvey
        {
            get
            {
                if (m_selectedSurvey == null)
                {
                    m_selectedSurvey = SurveyManager.GetSurveyById(this.Surveyid);
                }
                return m_selectedSurvey;
            }
        }

        /// <summary>
        /// Μπορεί να έχουμε ε΄ρθει απο δύο σημεία εδώ
        /// </summary>
        protected string CancelLink
        {
            get
            {
                if (this.SelectedSurvey.HasCollectors)
                {
                    return _UrlSuffix(string.Format("list.aspx?surveyid={0}", this.Surveyid));
                }
                else
                {
                    //Μας έχουν καλέσει απο mysurveys/mysurveys.aspx
                    return _UrlSuffix("../mysurveys.aspx");
                }
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
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                bool hadCollectors = this.SelectedSurvey.HasCollectors;
                VLCollector collector = null;

                //Αναλόγως του τύπου Collector που επιλέχτηκε, δημιοργούμε έναν:
                if (this.rdbtnWebLink.Checked)
                {
                    if (this.UseCredits)
                    {
                        if (this.ShowCreditTypeSelector)
                        {
                            //Ο πελάτης επιλέγει το CreditType:
                            CreditType _resourceType = (CreditType)Enum.Parse(typeof(CreditType), this.frmCreditType.SelectedValue);
                            collector = SurveyManager.CreateCollector(this.Surveyid, CollectorType.WebLink, this.collectorName.Text, creditType: _resourceType);
                        }
                        else
                        {
                            //Επιλέουμε εμείς αυτόματα το CreditType:
                            CreditType _resourceType = CreditType.ClickType;
                            collector = SurveyManager.CreateCollector(this.Surveyid, CollectorType.WebLink, this.collectorName.Text, creditType: _resourceType);
                        }
                    }
                    else
                    {
                        collector = SurveyManager.CreateCollector(this.Surveyid, CollectorType.WebLink, this.collectorName.Text);
                    }
                }
                else if (this.rdbtnEmailList.Checked)
                {
                    if (this.UseCredits)
                    {
                        if (this.ShowCreditTypeSelector)
                        {
                            //Ο πελάτης επιλέγει το CreditType:
                            CreditType _resourceType = (CreditType)Enum.Parse(typeof(CreditType), this.frmCreditType.SelectedValue);
                            collector = SurveyManager.CreateCollector(this.Surveyid, CollectorType.Email, this.collectorName.Text, creditType: _resourceType);
                        }
                        else
                        {
                            //Επιλέουμε εμείς αυτόματα το CreditType:
                            CreditType _resourceType = CreditType.EmailType;
                            collector = SurveyManager.CreateCollector(this.Surveyid, CollectorType.Email, this.collectorName.Text, creditType: _resourceType);
                        }
                    }
                    else
                    {
                        collector = SurveyManager.CreateCollector(this.Surveyid, CollectorType.Email, this.collectorName.Text);
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }


                //Μετά την δημιουργία πάμε κατευθείαν στην αντίστοιχη edit οθόνη:
                if (collector.CollectorType == CollectorType.WebLink)
                {
                    Response.Redirect(string.Format("details-weblink.aspx?surveyid={0}&collectorId={1}&textslanguage={2}", this.Surveyid, collector.CollectorId, collector.TextsLanguage), false);
                    this.Context.ApplicationInstance.CompleteRequest();
                }
                else if (collector.CollectorType == CollectorType.Email)
                {
                    Response.Redirect(string.Format("details-email.aspx?surveyid={0}&collectorId={1}&textslanguage={2}", this.Surveyid, collector.CollectorId, collector.TextsLanguage), false);
                    this.Context.ApplicationInstance.CompleteRequest();
                }
                else if (collector.CollectorType == CollectorType.Website)
                {
                    Response.Redirect(string.Format("details-website.aspx?surveyid={0}&collectorId={1}&textslanguage={2}", this.Surveyid, collector.CollectorId, collector.TextsLanguage), false);
                    this.Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    throw new NotSupportedException();
                }

            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}