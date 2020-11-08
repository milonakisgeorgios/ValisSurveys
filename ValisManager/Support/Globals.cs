using log4net;
using System.Web;
using Valis.Core;

namespace ValisManager
{
    static public class Globals
    {
        public static ILog Logger = LogManager.GetLogger(typeof(Globals));

        static public string HomePage = "~/clay/Home.aspx";
        static public string MySurveysPage = "~/clay/mysurveys/mysurveys.aspx";
        static public string LogOffPage = "~/logoff.aspx";
        static public string LoginPage = "~/clay/login/login.aspx";
        static public string SystemDefaultPage = "~/manager/home.aspx";

        /// <summary>
        /// Represents the current logged elore user.
        /// <para>The UserToken is always stored in user's session.</para>
        /// </summary>
        static public VLAccessToken UserToken
        {
            get
            {
                var _userToken = HttpContext.Current.Session["UserToken"] as VLAccessToken;
                if (_userToken == null)
                {
                    HttpContext.Current.Response.Redirect(LogOffPage, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                return _userToken;
            }
            set
            {
                HttpContext.Current.Session["UserToken"] = value;
            }
        }

        /// <summary>
        /// Μας λέει εάν ο τρέχων χρήστης ανήκει σε έναν Πελάτη ο οποίος χρεώνεται για τα διάφορα
        /// resources που χρησιμοποιεί
        /// </summary>
        static public bool UseCredits
        {
            get
            {
                if (Globals.UserToken.UseCredits.HasValue)
                    return Globals.UserToken.UseCredits.Value;

                return false;
            }
        }

        /// <summary>
        /// Η ονομασία του Πελάτη στον οποίο ανήκει ο τρέχων χρήστης (ClientUser)
        /// </summary>
        static public string ClientName
        {
            get
            {
                return Globals.UserToken.ClientName;
            }
        }

        /// <summary>
        /// Μας λέει ότι ο ValisManager τρέχει σε production mode!
        /// </summary>
        static public bool IsProductionPlatform
        {
            get
            {
                return ValisSystem.Core.IsProduction;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static public bool ShowCreditTypeSelector
        {
            get
            {
                return ValisSystem.Core.ShowCreditTypeSelector;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        static public string SystemPublicName
        {
            get
            {
                return ValisSystem.Core.SystemPublicName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        static public string BrandName
        {
            get
            {
                return ValisSystem.Core.BrandName;
            }
        }
    }
}