using System;
using Valis.Core;

namespace ValisManager.clay.addressbook
{
    public partial class export : ManagerPage
    {
        VLClientList m_selectedList;

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