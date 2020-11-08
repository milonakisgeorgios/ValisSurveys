using System;
using System.Threading;
using Valis.Core;

namespace ValisManager.manager.customers.contacts
{
    public partial class create : ManagerPage
    {
        protected VLClient SelectedClient
        {
            get
            {
                return this.ViewState["SelectedClient"] as VLClient;
            }
            set
            {
                this.ViewState["SelectedClient"] = value;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα ClientId
                    string _value = this.Request.Params["ClientId"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var clientId = Int32.Parse(_value);
                        this.SelectedClient = SystemManager.GetClientById(clientId);
                    }
                    if (this.SelectedClient == null)
                        throw new VLException("ClientId is invalid!");


                }
            }
            catch(Exception)
            {
                this.Response.Redirect("../list.aspx");
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string logOnToken = null;
                string pswdToken = null;
                var temporalUser = new VLClientUser();
                this.contactsform1.GetValues(temporalUser, ref logOnToken, ref pswdToken);

                var account = SystemManager.CreateClientAccount(this.SelectedClient.ClientId, temporalUser.FirstName, temporalUser.LastName, temporalUser.Role, temporalUser.Email, logOnToken, pswdToken);
                account.Title = temporalUser.Title;
                account.Department = temporalUser.Department;
                account.Country = temporalUser.Country;
                account.Prefecture = temporalUser.Prefecture;
                account.Town = temporalUser.Town;
                account.Address = temporalUser.Address;
                account.Zip = temporalUser.Zip;
                account.Telephone1 = temporalUser.Telephone1;
                account.Telephone2 = temporalUser.Telephone2;
                account.IsActive = temporalUser.IsActive;
                account.Comment = temporalUser.Comment;

                if(account.IsDirty)
                {
                    account = SystemManager.UpdateClientUser(account);
                }


                this.Response.Redirect(_UrlSuffix(string.Format("../edit.aspx?ClientId={0}", account.Client)), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

    }
}