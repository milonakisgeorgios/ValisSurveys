using System;
using System.Configuration;

namespace Valis.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        public DatabaseConfigurationElement() { }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("ConnString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this["ConnString"]; }
            set { this["ConnString"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Provider", IsRequired = true)]
        public string ProviderFactory
        {
            get { return (string)this["Provider"]; }
            set { this["Provider"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Assembly", IsRequired = true)]
        public string AssemblyName
        {
            get { return (string)this["Assembly"]; }
            set { this["Assembly"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ConnectionString;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PathConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return (string)base["Path"]; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HostConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Host", IsRequired = true)]
        public string Host
        {
            get { return (string)base["Host"]; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class URLConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url
        {
            get { return (string)base["Url"]; }
        }
    }

    public class TimerConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Interval", IsRequired = true)]
        public Int32 Interval
        {
            get { return (Int32)base["Interval"]; }
        }
    }

    public class MailerConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Provider", IsRequired = true)]
        public string Provider
        {
            get { return (string)base["Provider"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("NumberOfThreads", IsRequired = true)]
        public Int32 NumberOfThreads
        {
            get { return (Int32)base["NumberOfThreads"]; }
        }

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
        [ConfigurationProperty("SendGridProvider", IsRequired = false)]
        public EmailProviderConfigurationElement SendGrid
        {
            get { return (EmailProviderConfigurationElement)base["SendGridProvider"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("SmtpProvider", IsRequired = false)]
        public EmailProviderConfigurationElement Smpt
        {
            get { return (EmailProviderConfigurationElement)base["SmtpProvider"]; }
        }


        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("LocalFileProvider", IsRequired = true)]
        public PathConfigurationElement LocalFile
        {
            get { return (PathConfigurationElement)base["LocalFileProvider"]; }
        }
    }

    public class EmailProviderConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Server", IsRequired = false)]
        public string Server
        {
            get { return (string)base["Server"]; }
        }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = false)]
        public Int32 Port
        {
            get { return (Int32)base["Port"]; }
        }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Username", IsRequired = false)]
        public string Username
        {
            get { return (string)base["Username"]; }
        }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = false)]
        public string Password
        {
            get { return (string)base["Password"]; }
        }
    }
}
