using System;
using System.Globalization;
using System.Text;
using Valis.Core;

namespace ValisManager
{
    public class ManagerPage : System.Web.UI.Page
    {
        /// <summary>
        /// Μας λέει εάν ο τρέχων Πελάτης, χρεώνεται
        /// </summary>
        protected Boolean UseCredits
        {
            get
            {
                return Globals.UseCredits;
            }
        }

        /// <summary>
        /// Ενας διακόπτης εάν θα εμφανίζεται η επιλογή του CreditType κατα την δημιουργία ενός νέου Collector!
        /// </summary>
        protected Boolean ShowCreditTypeSelector
        {
            get
            {
                return Globals.ShowCreditTypeSelector;
            }
        }


        /// <summary>
        /// Αυτό είναι το clientId του πελάτη στον οποίο ανήκει ο τρέχων loged-in χρήστης.
        /// <para></para>
        /// </summary>
        protected Int32 ClientId
        {
            get
            {
                if (Globals.UserToken.ClientId.HasValue == false)
                {
                    throw new VLException("The current user does not belong to a Client!");
                }
                return Globals.UserToken.ClientId.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected VLSystemManager SystemManager
        {
            get
            {
                if (this.Context.Items["VLSystemManager"] == null)
                {
                    this.Context.Items["VLSystemManager"] = VLSystemManager.GetAnInstance(Globals.UserToken);
                }
                return (VLSystemManager)this.Context.Items["VLSystemManager"];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected VLSurveyManager SurveyManager
        {
            get
            {
                if (this.Context.Items["VLSurveyManager"] == null)
                {
                    this.Context.Items["VLSurveyManager"] = VLSurveyManager.GetAnInstance(Globals.UserToken);
                }
                return (VLSurveyManager)this.Context.Items["VLSurveyManager"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected VLLibraryManager LibraryManager
        {
            get
            {
                if (this.Context.Items["LibraryManager"] == null)
                {
                    this.Context.Items["LibraryManager"] = VLLibraryManager.GetAnInstance(Globals.UserToken);
                }
                return (VLLibraryManager)this.Context.Items["LibraryManager"];
            }
        }



        protected string GetTranslatableIcon()
        {
            return "<img class=\"translatable-image\" title=\"This field is translatable\" alt=\"translatable field\" src=\"/content/images/Translatable.png\" />";
            //return "<span class=\"translatable-image\" title=\"This field is translatable\" />";
        }

        protected string GetRequiredIcon()
        {
            return "<img class=\"required-image\" title=\"This field is required\" alt=\"required field\" src=\"/content/images/requiredIcon1.gif\">";
        }


        /// <summary>
        /// Επιστρέφει τα validation Options για SingleLine τύπο ερώτησης.
        /// Δεν επιστρέφουμε τα WholeNumber & DecimalNumber καθώς υπάρχουν ειδικά question types
        /// </summary>
        /// <returns></returns>
        protected string GetSingleLineValidationOptions()
        {
            StringBuilder sb = new StringBuilder();

            //Τα  διαβάζουμε απο το enumration Valis.Core.ValidationMode:

            //DoNotValidate
            sb.Append("<option value=\"0\">don't validate comment text </option>");
            //TextOfSpecificLength
            sb.Append("<option value=\"1\">must be a specific length</option>");
            //WholeNumber
            //sb.Append("<option value=\"2\">must be a whole number</option>");
            //DecimalNumber
            //sb.Append("<option value=\"3\">must be a decimal number</option>");
            //Date1
            //sb.Append("<option value=\"4\">must be a date (MM/DD/YYYY) </option>");
            //Date2
            //sb.Append("<option value=\"5\">must be a date (DD/MM/YYYY)</option>");
            //Email
            sb.Append("<option value=\"6\">must be an email address</option>");
            //RegularExpression
            //sb.Append("<option value=\"7\">use a Regular Expression</option>");

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetValidationOptions()
        {
            StringBuilder sb = new StringBuilder();

            //Τα  διαβάζουμε απο το enumration Valis.Core.ValidationMode:

            //DoNotValidate
            sb.Append("<option value=\"0\">don't validate comment text </option>");
            //TextOfSpecificLength
            sb.Append("<option value=\"1\">must be a specific length</option>");
            //WholeNumber
            sb.Append("<option value=\"2\">must be a whole number</option>");
            //DecimalNumber
            sb.Append("<option value=\"3\">must be a decimal number</option>");
            //Date1
            sb.Append("<option value=\"4\">must be a date (MM/DD/YYYY) </option>");
            //Date2
            sb.Append("<option value=\"5\">must be a date (DD/MM/YYYY)</option>");
            //Email
            sb.Append("<option value=\"6\">must be an email address</option>");
            //RegularExpression
            //sb.Append("<option value=\"7\">use a Regular Expression</option>");

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetOtherFieldTypes()
        {
            var values = Enum.GetValues(typeof(OtherFieldType));
            
            StringBuilder sb = new StringBuilder();
            foreach (OtherFieldType item in values)
            {
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", ((Byte)item).ToString(CultureInfo.InvariantCulture), item.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// This property is checked during the OnPreRenderComplete event handler,
        /// and if it is not empty, its text it will be rendered by the showExceptionBand method.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// This property is checked during the OnPreRenderComplete event handler,
        /// and if it is not empty, its text it will be rendered by the showInfoBand method.
        /// </summary>
        public string InfoMessage { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "clientGlobalVariables1", string.Format("var theManagerPath = '{0}';var theAccessToken = {1};var isSysadmin = {2};", this.ResolveUrl("~/"), Globals.UserToken.AccessTokenId, Globals.UserToken.IsSysAdmin ? "true" : "false"), true);

            base.OnPreRenderComplete(e);

            if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showExceptionBand", string.Format("$(document).ready(function(){{showErrorBand('{0}');}});", Server.HtmlEncode(this.ErrorMessage).Replace(Environment.NewLine, "<br/>").Replace(@"'", @"\'")), true);
            }
            if (!string.IsNullOrWhiteSpace(this.InfoMessage))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showInfoBand", string.Format("$(document).ready(function(){{showInfoBand('{0}');}});", Server.HtmlEncode(this.InfoMessage).Replace(Environment.NewLine, "<br/>").Replace(@"'", @"\'")), true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(System.Web.HttpContext context)
        {
            if (Globals.UserToken == null)
            {
                context.Response.Redirect(Globals.LogOffPage, true);
            }
            base.ProcessRequest(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreLoad(EventArgs e)
        {
            if(!this.IsPostBack)
            {
                SaveUrlSuffix();
            }
            base.OnPreLoad(e);
        }
        /// <summary>
        /// Εδώ αποθηκεύεται το τμήμα του URL που αφορά τις παραμέτρους του JqGrid
        /// </summary>
        protected string UrlSuffix
        {
            get
            {
                return this.ViewState["UrlSuffix"] as string;
            }
            set
            {
                this.ViewState["UrlSuffix"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void SaveUrlSuffix()
        {
            var pos = this.Request.RawUrl.IndexOf("pageno=");
            if (pos != -1)
            {
                UrlSuffix = this.Request.RawUrl.Substring(pos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string _UrlSuffix(string url)
        {
            if (url.Contains("?"))
            {
                url = url + "&" + this.UrlSuffix;
            }
            else
            {
                url = url + "?" + this.UrlSuffix;
            }
            return url;
        }

    }
}