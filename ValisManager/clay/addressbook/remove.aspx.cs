using System;
using Valis.Core;

namespace ValisManager.clay.addressbook
{
    public partial class remove : ManagerPage
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

        protected void btnRemoveContacts_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rdbtnAll.Checked)
                {
                    //Remove All Contacts
                    var total = SystemManager.RemoveAllContactsFromList(this.SelectedClientList.ListId);
                    this.InfoMessage = string.Format("Removed {0} total contacts", total);
                }
                else if (this.rdbtnOptOut.Checked)
                {
                    //Remove All Opted-Out Contacts
                    var total = SystemManager.RemoveAllOptedOutContactsFromList(this.SelectedClientList.ListId);
                    this.InfoMessage = string.Format("Removed {0} total contacts", total);

                }
                else if (this.rdbtnBounced.Checked)
                {
                    //Remove All Bounced Email Recipients
                    var total = SystemManager.RemoveAllBouncedContactsFromList(this.SelectedClientList.ListId);
                    this.InfoMessage = string.Format("Removed {0} total contacts", total);
                }
                else if (this.rdbtnDomain.Checked)
                {
                    //Remove All Contacts by Domain Name
                    var total = SystemManager.RemoveByDomainContactsFromList(this.SelectedClientList.ListId, this.txtDomainName.Text);
                    this.InfoMessage = string.Format("Removed {0} total contacts", total);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.lblbtnAll.Text = string.Format("Remove All Contacts ({0} total)", SystemManager.GetContactsCount(this.SelectedClientList.ListId));
            this.lblbtnOptOut.Text = string.Format("Remove All Opted-Out Contacts ({0} total)", SystemManager.GetOptedOutContactsCount(this.SelectedClientList.ListId));
            this.lblbtnBounced.Text = string.Format("Remove All Bounced Email Recipients ({0} total)", SystemManager.GetBouncedContactsCount(this.SelectedClientList.ListId));
        }


    }
}