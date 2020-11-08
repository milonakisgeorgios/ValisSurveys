using System.Configuration;

namespace Valis.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class VsCoreConfiguration : ConfigurationElement
    {
        #region Attributes
        [ConfigurationProperty("SystemPublicHostName", IsRequired = false, DefaultValue="")]
        public string SystemPublicHostName
        {
            get 
            {
                var _tmp = base["SystemPublicHostName"] as string;
                return _tmp != null ? _tmp : string.Empty;
            }
        }

        [ConfigurationProperty("SystemPublicName", IsRequired = false, DefaultValue="Valis Surveys Prototype v1.0")]
        public string SystemPublicName
        {
            get 
            {
                var _tmp = base["SystemPublicName"] as string;
                return _tmp != null ? _tmp : "Valis Surveys Prototype v1.0";
            }
        }

        [ConfigurationProperty("BrandName", IsRequired = false, DefaultValue="Valis Surveys")]
        public string BrandName
        {
            get 
            {
                var _tmp = base["BrandName"] as string;
                return _tmp != null ? _tmp : "Valis Surveys";
            }
        }

        [ConfigurationProperty("IsProduction", IsRequired = false, DefaultValue=true)]
        public bool IsProduction
        {
            get 
            {
                return (bool)base["IsProduction"]; 
            }
        }

        /// <summary>
        /// Εάν θα εμφανίζεται η επιλογή του CreditType κατα την δημιουργία ενός collector που χρεώνεται
        /// </summary>
        [ConfigurationProperty("ShowCreditTypeSelector", IsRequired = false, DefaultValue = false)]
        public bool ShowCreditTypeSelector
        {
            get
            {
                return (bool)base["ShowCreditTypeSelector"];
            }
        }
        #endregion

        #region Elements
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Database", IsRequired = false)]
        public DatabaseConfigurationElement Database
        {
            get { return (DatabaseConfigurationElement)base["Database"]; }
        }

        /// <summary>
        /// Το path για το directory στο οποίο αποθηκεύνται τα αρχεία που συνοδεύουν ένα survey.
        /// <para>Επίσης σε αυτό το direcotry, δημιουργούνται και τα reports</para>
        /// </summary>
        [ConfigurationProperty("FileInventory", IsRequired = false)]
        public PathConfigurationElement FileInventory
        {
            get { return (PathConfigurationElement)base["FileInventory"]; }
        }

        /// <summary>
        /// Το url για την opted out σελίδα των receivers απο τα email-προσκλήσεις που στέλνει η υπηρεσία
        /// </summary>
        [ConfigurationProperty("RemoveUrl", IsRequired = false)]
        public URLConfigurationElement RemoveUrl
        {
            get { return (URLConfigurationElement)base["RemoveUrl"]; }
        }

        /// <summary>
        /// Το url για το validation του Sender email Address των Messages
        /// </summary>
        [ConfigurationProperty("VerifyUrl", IsRequired = false)]
        public URLConfigurationElement VerifyUrl
        {
            get { return (URLConfigurationElement)base["VerifyUrl"]; }
        }

        /// <summary>
        /// Ποιός είναι ο Host (Url) που εκτελεί τα Surveys?
        /// </summary>
        [ConfigurationProperty("RuntimeEngine", IsRequired = false)]
        public HostConfigurationElement RuntimeEngine
        {
            get { return (HostConfigurationElement)base["RuntimeEngine"]; }
        }


        /// <summary>
        /// Ποιός είναι ο Host (Url) που δημιουργεί το Report για το PhantomJs
        /// </summary>
        [ConfigurationProperty("ReportEngine", IsRequired = false)]
        public HostConfigurationElement ReportEngine
        {
            get { return (HostConfigurationElement)base["ReportEngine"]; }
        }


        /// <summary>
        /// Το path για το directory στο οποίο βρίσκεται το 'phantomjs.exe' και το 'rasterize.js'
        /// </summary>
        [ConfigurationProperty("PhantomJs", IsRequired = false)]
        public PathConfigurationElement PhantomJs
        {
            get { return (PathConfigurationElement)base["PhantomJs"]; }
        }
        #endregion
    }
}
