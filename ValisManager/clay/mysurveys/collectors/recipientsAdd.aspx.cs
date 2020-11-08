using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class recipientsAdd : CollectorsPage
    {
        Collection<VLClientList> m_lists = null;

        Collection<VLClientList> Lists
        {
            get
            {
                if (m_lists == null)
                {
                    m_lists = SystemManager.GetClientLists(Globals.UserToken.ClientId.Value);
                }
                return m_lists;
            }
        }

        protected bool ShowAddressBookList
        {
            get
            {
                if (Lists.Count <= 0)
                    return false;

                return true;
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            if(!this.IsPostBack)
            {
                //Γεμίζουμε το AddressBookList με λίστες που περιέχουν τουλάχιστον μία εγγραφή
                this.AddressBookList.Items.Clear();

                foreach (var item in Lists)
                {
                    if(item.TotalContacts>0)
                    {
                        var text = string.Format("{0} ({1} {2})", item.Name, item.TotalContacts, item.TotalContacts == 1 ? "contact":"contacts");
                        var listItem = new ListItem(Server.HtmlEncode(string.Format(text)), item.ListId.ToString());
                        this.AddressBookList.Items.Add(listItem);
                    }
                }

                //Γεμίζουμε το CollectorsList με collectors που υπάρχουν ήδη σε surveys
                this.CollectorsList.Items.Clear();
                var collectors = SurveyManager.GetCollectorPeeks(CollectorType.Email).Where(x => x.TotalRecipients > 0);
                foreach(var item in collectors)
                {
                    var listItem = new ListItem(Server.HtmlEncode(string.Format("{0} - {1}", item.SurveyName, item.CollectorName)), item.CollectorId.ToString());
                    this.CollectorsList.Items.Add(listItem);
                }


                if (Lists.Count > 0)
                    this.rdbtnFromList.Checked = true;
                else
                    this.rdbtnManual.Checked = true;
            }
        }


        protected void OnAddManualrecipients(object sender, EventArgs e)
        {
            this.rdbtnManual.Checked = true;
            try
            {
                var importResult = SurveyManager.ImportRecipientsFromString(this.CollectorId, this.manualText.Text, ContactImportOptions.Default);
                
                importSummary(importResult);

                this.manualText.Text = string.Empty;
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected void OnAddAddressListRecipients(object sender, EventArgs e)
        {
            this.rdbtnFromList.Checked = true;
            try
            {
                Int32 selectedListId = Int32.Parse(this.AddressBookList.SelectedValue);

                var importResult = SurveyManager.ImportRecipientsFromList(this.CollectorId, selectedListId);

                importSummary(importResult);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected void OnAddCollListRecipients(object sender, EventArgs e)
        {
            this.rdbtnFromSurvey.Checked = true;
            try
            {
                Int32 sourceCollectorId = Int32.Parse(this.CollectorsList.SelectedValue);

                var importResult = SurveyManager.ImportRecipientsFromCollector(this.CollectorId, sourceCollectorId);

                importSummary(importResult);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }


        protected void importSummary(ContactImportResult importResult)
        {
            string message = null;
            
            var totalRecipients = SurveyManager.GetRecipientsCount(this.CollectorId);
            if (importResult.SuccesfullImports > 0)
            {
                if (importResult.SuccesfullImports == 1)
                {
                    if (totalRecipients == 1)
                    {
                        message = "Successfully added 1 recipient. The Collector now has 1 recipient.";
                    }
                    else
                    {
                        message = string.Format("Successfully added 1 recipient. The Collector now has {0} recipients.", totalRecipients);
                    }
                }
                else
                {
                    message = string.Format("Successfully added {0} recipients. The Collector now has {1} recipients.", importResult.SuccesfullImports, totalRecipients);
                }
            }
            else
            {
                if (totalRecipients == 1)
                {
                    message = "No recipient was added. The Collector has 1 recipient.";
                }
                else
                {
                    message = string.Format("No recipient was added. The Collector has {0} recipients.", totalRecipients);
                }
            }

            this.InfoMessage = message;
        }
    }
}