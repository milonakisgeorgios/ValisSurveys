using System;
using System.Diagnostics;
using System.Web;
using Valis.Core;

namespace ValisReporter
{
    public class ValisHttpApplication : HttpApplication
    {
        static System.Object m_loginMutex = new object();
        static System.Boolean m_initializationOK = false;
        VLSurveyManager m_surveyManager;
        VLSystemManager m_systemManager;


        public static VLAccessToken AccessToken { get; private set; }

        public VLSurveyManager SurveyManager
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
        public VLSystemManager SystemManager
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


        public override void Init()
        {
            base.Init();

            try
            {
                if (!m_initializationOK)
                {
                    lock (m_loginMutex)
                    {
                        if (!m_initializationOK)
                        {
                            ValisSystem system = new ValisSystem();

                            AccessToken = system.LogOnUser("r2p0rt3rUs3r", "v@l1$D@#M)NP@$$)");
                            if (AccessToken == null) throw new VLException("Invalid account credentials");

                            Debug.WriteLine(string.Format("ValisHttpApplication:: AccessToken for {0} acquired!", AccessToken.LogOnToken));

                            m_initializationOK = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("ValisHttpApplication:: (An exception occured!): {0}", ex.Message));
                throw new HttpException(ex.Message);
            }
        }


        #region
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        #endregion
    }
}