using System.Configuration;

namespace Valis.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VsDaemonConfiguration : ConfigurationElement
    {
        #region Attributes
        
        [ConfigurationProperty("ServiceName", IsRequired = true)]
        public string ServiceName
        {
            get { return (string)base["ServiceName"]; }
        }

        [ConfigurationProperty("LogOnToken", IsRequired = true)]
        public string LogOnToken
        {
            get { return (string)base["LogOnToken"]; }
        }

        [ConfigurationProperty("PswdToken", IsRequired = true)]
        public string PswdToken
        {
            get { return (string)base["PswdToken"]; }
        }

        #endregion


        #region Elements

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("HeartbeatTimer", IsRequired = true)]
        public TimerConfigurationElement HeartbeatTimer
        {
            get { return (TimerConfigurationElement)base["HeartbeatTimer"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Mailer", IsRequired = true)]
        public MailerConfigurationElement Mailer
        {
            get { return (MailerConfigurationElement)base["Mailer"]; }
        }


        #endregion
    }
}
