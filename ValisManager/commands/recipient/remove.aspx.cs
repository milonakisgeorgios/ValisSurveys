using System;
using System.Globalization;
using Valis.Core;

namespace ValisManager.commands.recipient
{
    public partial class remove : System.Web.UI.Page
    {
        #region Support Stuff
        static System.Object m_loginMutex = new object();
        static VLAccessToken m_accessToken = null;

        VLSystemManager m_systemManager = null;
        VLSurveyManager m_surveyManager = null;

        string LogOnToken
        {
            get
            {
                return ValisSystem.Manager.LogOnToken;
            }
        }

        string PswdToken
        {
            get
            {
                return ValisSystem.Manager.PswdToken;
            }
        }

        VLAccessToken AccessToken
        {
            get
            {
                if (m_accessToken == null)
                {
                    lock (m_loginMutex)
                    {
                        if (m_accessToken == null)
                        {
                            ValisSystem system = new ValisSystem();
                            m_accessToken = system.LogOnUser(LogOnToken, PswdToken);
                            if (m_accessToken == null) throw new VLException("Invalid manager-account credentials");
                        }
                    }
                }
                return m_accessToken;
            }
        }
        VLSurveyManager SurveyManager
        {
            get
            {
                if (m_surveyManager == null)
                {
                    m_surveyManager = VLSurveyManager.GetAnInstance(AccessToken);
                }
                return m_surveyManager;
            }
        }
        VLSystemManager SystemManager
        {
            get
            {
                if (m_systemManager == null)
                {
                    m_systemManager = VLSystemManager.GetAnInstance(AccessToken);
                }
                return m_systemManager;
            }
        }
        #endregion


        protected string RecipientKey
        {
            get
            {
                var _obj = this.ViewState["RecipientKey"] as string;

                return _obj;
            }
            set
            {
                this.ViewState["RecipientKey"] = value;
            }
        }

        protected string PublicId
        {
            get
            {
                var _obj = this.ViewState["PublicId"] as string;

                return _obj;
            }
            set
            {
                this.ViewState["PublicId"] = value;
            }
        }

        protected Int32 CollectorId
        {
            get
            {
                Object _obj = this.ViewState["CollectorId"];
                if (_obj == null) return -1;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["CollectorId"] = value;
            }
        }

        protected Int16 LanguageId
        {
            get
            {
                Object _obj = this.ViewState["LanguageId"];
                if (_obj == null) return BuiltinLanguages.PrimaryLanguage.LanguageId;
                return (Int16)_obj;
            }
            set
            {
                this.ViewState["LanguageId"] = value;
            }
        }


        protected VLRecipient Recipient { get; set; }
        protected VLCollector Collector { get; set; }
        protected VLSurvey Survey { get; set; }
        protected string ErrorMessage { get; set; }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            this.QuestionPanel.Visible = false;
            this.ResultPanel.Visible = false;
            this.ErrorPanel.Visible = false;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if(this.IsPostBack == false)
                {
                    #region διαβάζουμε recipientKey, publicId, collectorId & language
                    //Περιμένουμε στις παραμέτρους της κλήσης ένα rkey (recipient.RecipientKey)
                    if (string.IsNullOrEmpty(Request.Params["rkey"]))
                    {
                        throw new VLException("RecipientKey does not exist. Invalid Call!");
                    }
                    this.RecipientKey = Request.Params["rkey"];

                    //Περιμένουμε στις παραμέτρους της κλήσης ένα pid (survey.PublicId)
                    if (string.IsNullOrEmpty(Request.Params["pid"]))
                    {
                        throw new VLException("PublicId does not exist. Invalid Call!");
                    }
                    this.PublicId = Request.Params["pid"];

                    //Περιμένουμε στις παραμέτρους της κλήσης ένα cid (collector.CollectorId)
                    if (string.IsNullOrEmpty(Request.Params["cid"]))
                    {
                        throw new VLException("CollectorId does not exist. Invalid Call!");
                    }
                    try
                    {
                        this.CollectorId = Int32.Parse(Request.Params["cid"], CultureInfo.InvariantCulture);
                    }
                    catch(Exception ex)
                    {
                        throw new VLException(string.Format("Invalid CollectorId. {0}!", ex.Message));
                    }

                    //Περιμένουμε στις παραμέτρους της κλήσης ένα lang 
                    if (!string.IsNullOrEmpty(Request.Params["lang"]))
                    {
                        try
                        {
                            this.LanguageId = Int16.Parse(Request.Params["lang"], CultureInfo.InvariantCulture);
                            if(this.LanguageId != BuiltinLanguages.PrimaryLanguage.LanguageId && this.LanguageId != BuiltinLanguages.DefaultLanguage.LanguageId)
                            {
                                if (BuiltinLanguages.GetLanguageById(this.LanguageId) == null)
                                {
                                    throw new VLException("Invalid Language.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new VLException(string.Format("Invalid Language. {0}!", ex.Message));
                        }
                    }
                    #endregion

                }

                #region Διαβάζουμε απο την βάση μας Recipient, Collector και Survey
                this.Recipient = SurveyManager.GetRecipientByKey(this.RecipientKey);
                if (this.Recipient == null)
                {
                    throw new VLException("Recipient is invalid!");
                }

                this.Collector = SurveyManager.GetCollectorById(this.CollectorId, this.LanguageId);
                if (this.Collector == null)
                {
                    throw new VLException("Collector is invalid!");
                }

                this.Survey = SurveyManager.GetSurveyByPublicId(this.PublicId, this.LanguageId);
                if (this.Survey == null)
                {
                    throw new VLException("Survey is invalid!");
                }

                //Now all the above entities must be compatible:
                if(this.Recipient.Collector != this.Collector.CollectorId)
                {
                    throw new VLException("Recipient and Collector are invalid!");
                }
                if(this.Collector.Survey != this.Survey.SurveyId)
                {
                    throw new VLException("Collector and Survey are invalid!");
                }
                #endregion

                this.lnkSurvey.NavigateUrl = Utility.GetSurveyRuntimeURL(this.Survey, this.Collector, this.Recipient, false);

                this.QuestionPanel.Visible = true;
                this.ResultPanel.Visible = false;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                this.QuestionPanel.Visible = false;
                this.ResultPanel.Visible = false;
                this.ErrorPanel.Visible = true;
            }
        }

        protected void btnLocalOptOut_Click(object sender, EventArgs e)
        {
            try
            {
                var updated_recipient = SurveyManager.OptOutRecipient(this.RecipientKey, this.CollectorId, this.PublicId);


                this.QuestionPanel.Visible = false;
                this.ResultPanel.Visible = true;
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
                this.QuestionPanel.Visible = false;
                this.ResultPanel.Visible = false;
                this.ErrorPanel.Visible = true;
            }
        }
    }
}