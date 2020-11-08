using System;
using System.Globalization;

namespace ValisManager.manager.clientprofiles
{
    public partial class list : ManagerPage
    {
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
                    return "Name";
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
                return "10";
            }
        }


    }
}