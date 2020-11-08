using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.clay.addressbook
{
    public partial class add : ManagerPage
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
            if(this.IsPostBack == false)
            {
                if (string.IsNullOrEmpty(Request.Params["listId"]))
                    throw new ArgumentNullException("listId");

                this.ClientListId = Int32.Parse(Request.Params["listId"]);
            }
        }

        protected void importFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(csvFile.HasFile)
                {
                    HttpPostedFile oFile = csvFile.PostedFile;
                    if (oFile == null)
                        throw new VLException("Κανένα αρχείο δεν έγινε upload!");

                    int fileLength = oFile.ContentLength;
                    if (fileLength <= 0)
                        throw new VLException("To αρχείο είχε μηδενικό μέγεθος!");

                    var importResult = SystemManager.ImportContactsFromCSV(this.ClientListId, oFile.InputStream, ContactImportOptions.Default);


                    ShowImportResultsPanel(importResult, this.cvsImportResults);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected void addContactsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(this.manualText.Text))
                {
                    var importResult = SystemManager.ImportContactsFromString(this.ClientListId, this.manualText.Text, ContactImportOptions.Default);

                    ShowImportResultsPanel(importResult, this.stringImportResults);

                    this.manualText.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        void ShowImportResultsPanel(ContactImportResult importResult, PlaceHolder pholder)
        {
            StringBuilder sb = new StringBuilder();


            if (importResult.SuccesfullImports > 0)
            {
                sb.Append("<div class=\"succesfullResult\">");
                sb.Append("<span>");

                if (importResult.SuccesfullImports == 1)
                    sb.Append("Successfully added one contact. ");
                else
                    sb.AppendFormat("Successfully added {0} contacts. ", importResult.SuccesfullImports);
                sb.Append("</span>");
                sb.Append("<br/>");
                sb.Append("<span>");
            }
            else
            {
                sb.Append("<div class=\"errorResult\">");
                sb.Append("<span>");
            }


            if (importResult.InvalidEmails == 1)
            {
                sb.Append("There is one (1) invalid email. ");
                sb.Append("</span><br/><span>");
            }
            else if (importResult.InvalidEmails > 1)
            {
                sb.AppendFormat("There are {0} invalid emails. ", importResult.InvalidEmails);
                sb.Append("</span><br/><span>");
            }

            if (importResult.SameEmails == 1)
            {
                sb.Append("There is one (1) same email. ");
                sb.Append("</span><br/><span>");
            }
            else if (importResult.SameEmails > 1)
            {
                sb.AppendFormat("There are {0} same emails. ", importResult.SameEmails);
                sb.Append("</span><br/><span>");
            }

            if (importResult.FailedImports == 1)
            {
                sb.Append("Failed to import one (1) contact. ");
                sb.Append("</span><br/><span>");
            }
            else if (importResult.FailedImports > 1)
            {
                sb.AppendFormat("Failed to import {0} contacts. ", importResult.FailedImports);
                sb.Append("</span><br/><span>");
            }

            if(importResult.OptedOutEmails == 1)
            {
                sb.Append("There is one (1) OptedOut email. ");
                sb.Append("</span><br/><span>");
            }
            else if (importResult.OptedOutEmails > 1)
            {
                sb.AppendFormat("There are {0} OptedOut emails. ", importResult.OptedOutEmails);
                sb.Append("</span><br/><span>");
            }

            if (importResult.BouncedEmails == 1)
            {
                sb.Append("There is one (1) Bounced email. ");
                sb.Append("</span><br/><span>");
            }
            else if (importResult.BouncedEmails > 1)
            {
                sb.AppendFormat("There are {0} Bounced emails. ", importResult.BouncedEmails);
                sb.Append("</span><br/><span>");
            }

            sb.Append("</span>");
            sb.Append("</div>");




            pholder.Controls.Add(new LiteralControl(sb.ToString()));
        }


    }
}