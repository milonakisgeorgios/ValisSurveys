using System;
using Valis.Core;

namespace ValisApplicationService
{
    internal class UtilityActiveObject : ActiveObject
    {
        protected SrvLogger logger;
        VLSurveyManager m_surveyManager;
        VLSystemManager m_systemManager;

        protected VLSurveyManager SurveyManager
        {
            get
            {
                if (m_surveyManager == null)
                {
                    m_surveyManager = VLSurveyManager.GetAnInstance(Globals.AccessToken);
                }
                return m_surveyManager;
            }
        }
        protected VLSystemManager SystemManager
        {
            get
            {
                if (m_systemManager == null)
                {
                    m_systemManager = VLSystemManager.GetAnInstance(Globals.AccessToken);
                }
                return m_systemManager;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadName"></param>
        public UtilityActiveObject(string threadName)
            : base(threadName)
        {
            logger = new SrvLogger(this.GetType());
        }


        #region Debug Logging
        protected void Debug(object message)
        {
            logger.Debug(message);
        }
        protected void Debug(object message, Exception exception)
        {
            logger.Debug(message, exception);
        }
        protected void DebugFormat(string format, object arg0)
        {
            logger.DebugFormat(format, arg0);
        }
        protected void DebugFormat(string format, object arg0, object arg1)
        {
            logger.DebugFormat(format, arg0, arg1);
        }
        protected void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.DebugFormat(format, arg0, arg1, arg2);
        }
        protected void DebugFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }
        #endregion

        #region Info Logging
        protected void Info(object message)
        {
            logger.Info(message);
        }
        protected void Info(object message, Exception exception)
        {
            logger.Info(message, exception);
        }
        protected void InfoFormat(string format, object arg0)
        {
            logger.InfoFormat(format, arg0);
        }
        protected void InfoFormat(string format, object arg0, object arg1)
        {
            logger.InfoFormat(format, arg0, arg1);
        }
        protected void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.InfoFormat(format, arg0, arg1, arg2);
        }
        protected void InfoFormat(string format, params object[] args)
        {
            logger.InfoFormat(format, args);
        }
        #endregion

        #region Warn Logging
        protected void Warn(object message)
        {
            logger.Warn(message);
        }
        protected void Warn(object message, Exception exception)
        {
            logger.Warn(message, exception);
        }
        protected void WarnFormat(string format, object arg0)
        {
            logger.WarnFormat(format, arg0);
        }
        protected void WarnFormat(string format, object arg0, object arg1)
        {
            logger.WarnFormat(format, arg0, arg1);
        }
        protected void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.WarnFormat(format, arg0, arg1, arg2);
        }
        protected void WarnFormat(string format, params object[] args)
        {
            logger.WarnFormat(format, args);
        }
        #endregion

        #region Error Logging
        protected void Error(object message)
        {
            logger.Error(message);
        }
        protected void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }
        protected void ErrorFormat(string format, object arg0)
        {
            logger.ErrorFormat(format, arg0);
        }
        protected void ErrorFormat(string format, object arg0, object arg1)
        {
            logger.ErrorFormat(format, arg0, arg1);
        }
        protected void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            logger.ErrorFormat(format, arg0, arg1, arg2);
        }
        protected void ErrorFormat(string format, params object[] args)
        {
            logger.ErrorFormat(format, args);
        }
        #endregion

    }
}
