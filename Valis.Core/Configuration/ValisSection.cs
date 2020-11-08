using System.Configuration;

namespace Valis.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ValisSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Core", IsRequired = true)]
        public VsCoreConfiguration Core
        {
            get { return (VsCoreConfiguration)base["Core"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Manager", IsRequired = false)]
        public VsManagerConfiguration Manager
        {
            get { return (VsManagerConfiguration)base["Manager"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Daemon", IsRequired = false)]
        public VsDaemonConfiguration Daemon
        {
            get { return (VsDaemonConfiguration)base["Daemon"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Server", IsRequired = false)]
        public VsServerConfiguration Server
        {
            get { return (VsServerConfiguration)base["Server"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Reporter", IsRequired = false)]
        public VsReporterConfiguration Reporter
        {
            get { return (VsReporterConfiguration)base["Reporter"]; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.SectionInformation.Name;
        }
    }
}
