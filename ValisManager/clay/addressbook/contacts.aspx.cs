using System;
using System.Globalization;
using Valis.Core;

namespace ValisManager.clay.addressbook
{
    public partial class contacts : ManagerPage
    {
        VLClientList m_selectedList;


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
                    return "Email";
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
                return "18";
            }
        }


        public Int32 ClientListId
        {
            get
            {
                Object _obj = this.ViewState["ClientListId"];
                if (_obj == null) return -1;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["ClientListId"] = value;
            }
        }

        public VLClientList SelectedClientList
        {
            get
            {
                if (m_selectedList == null)
                {
                    m_selectedList = SystemManager.GetClientListById(this.ClientListId);
                }
                return m_selectedList;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                if (string.IsNullOrEmpty(Request.Params["listId"]))
                    throw new ArgumentNullException("listId");

                this.ClientListId = Int32.Parse(Request.Params["listId"]);
            }
        }
    }
}