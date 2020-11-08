using System;
using System.Globalization;
using System.Text;
using System.Web;
using Valis.Core;

namespace ValisManager.clay.mysurveys
{
    public partial class mysurveys : ManagerPage
    {
        #region JqGrid support
        /// <summary>
        /// jqGrid's current page
        /// </summary>
        protected string PageNumber
        {
            get
            {
                string pageno = this.Request.Params["pageno"];
                if (string.IsNullOrEmpty(pageno))
                {
                    return "1";
                }
                try
                {
                    return Int32.Parse(pageno, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                }
                catch
                {
                    return "1";
                }
            }
        }
        /// <summary>
        /// jqGrid's current sortname
        /// </summary>
        protected string SortName
        {
            get
            {
                string sortname = this.Request.Params["sortname"];
                if (string.IsNullOrEmpty(sortname))
                {
                    return "Title";
                }
                return sortname;
            }
        }
        /// <summary>
        /// jqGrid's current sortorder
        /// </summary>
        protected string SortOrder
        {
            get
            {
                string sortorder = this.Request.Params["sortorder"];
                if (string.IsNullOrEmpty(sortorder))
                {
                    return "asc";
                }
                return sortorder;
            }
        }
        /// <summary>
        /// jqGrid's current rownum
        /// </summary>
        protected string RowNum
        {
            get
            {
                string rowNum = this.Request.Params["rowNum"] as string;
                if (string.IsNullOrEmpty(rowNum))
                {
                    return "12";
                }
                try
                {
                    return Int32.Parse(rowNum, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                }
                catch
                {
                    return "12";
                }
            }
        }
        #endregion


        protected string TargetLanguages
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach(var language in BuiltinLanguages.Languages)
                {
                    if (language.LanguageId == BuiltinLanguages.Invariant)
                        continue;
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", language.LanguageId, language.EnglishName);
                }
                return sb.ToString();
            }
        }

        protected string LanguagesArray
        {
            get
            {
                bool addcoma = false;

                StringBuilder sb = new StringBuilder();
                sb.Append("var languages = [");
                
                foreach (var language in BuiltinLanguages.Languages)
                {
                    if (language.LanguageId == BuiltinLanguages.Invariant)
                        continue;
                    if (addcoma)
                        sb.Append(", ");
                    sb.AppendFormat("{{id:{0}, name:'{1}'}}", language.LanguageId, HttpUtility.HtmlEncode(language.EnglishName));
                    addcoma = true;
                }

                sb.Append("];");
                return sb.ToString();
            }
        }
    }
}