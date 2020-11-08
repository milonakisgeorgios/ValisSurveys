using System.Configuration;

namespace Valis.Core.Configuration
{
    public sealed class VsServerConfiguration : ConfigurationElement
    {
        #region Attributes

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
    }
}
