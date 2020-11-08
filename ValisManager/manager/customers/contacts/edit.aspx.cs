using System;
using System.Threading;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.customers.contacts
{
    public partial class edit : ManagerPage
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
        protected VLClientUser SelectedUser
        {
            get
            {
                return this.ViewState["SelectedUser"] as VLClientUser;
            }
            set
            {
                this.ViewState["SelectedUser"] = value;
            }
        }
        protected VLCredential SelectedCredentials
        {
            get
            {
                return this.ViewState["SelectedCredentials"] as VLCredential;
            }
            set
            {
                this.ViewState["SelectedCredentials"] = value;
            }
        }

        protected string SelectedUserName
        {
            get
            {
                if (this.SelectedUser != null)
                {
                    return string.Format("{0} {1}", this.SelectedUser.LastName, this.SelectedUser.FirstName);
                }
                return string.Empty;
            }
        }

        protected string GetDeleteButtonHandler
        {
            get
            {
                PostBackOptions myPostBackOptions = new PostBackOptions(this.deleteBtn);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.RequiresJavaScriptProtocol = false;
                myPostBackOptions.PerformValidation = false;
                myPostBackOptions.ClientSubmit = true;

                return Page.ClientScript.GetPostBackEventReference(myPostBackOptions);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα ClientId
                    //string _value1 = this.Request.Params["ClientId"];
                    //if (!string.IsNullOrEmpty(_value1))
                    //{
                    //    var clientId = Int32.Parse(_value1);
                    //    this.SelectedClient = SystemManager.GetClientById(clientId);
                    //}
                    //if (this.SelectedClient == null)
                    //    throw new VLException("ClientId is invalid!");


                    //Πρέπει στο url να υπάρχει ένα UserId
                    string _value2 = this.Request.Params["UserId"];
                    if (!string.IsNullOrEmpty(_value2))
                    {
                        var userId = Int32.Parse(_value2);
                        this.SelectedUser = SystemManager.GetClientUserById(userId);
                        this.SelectedCredentials = SystemManager.GetCredentialForPrincipal(this.SelectedUser);
                    }
                    if (this.SelectedUser == null || this.SelectedCredentials == null)
                        throw new VLException("UserId is invalid!");

                    this.contactsform1.IsEditMode = true;
                    this.contactsform1.SetValues(SelectedUser, SelectedCredentials);
                    if (this.SelectedUser.IsBuiltIn)
                    {
                        this.deleteBtn.Enabled = false;
                        this.saveBtn.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Response.Redirect(_UrlSuffix("../list.aspx"));
            }
        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SystemManager.DeleteClientUser(this.SelectedUser);

                this.Response.Redirect(_UrlSuffix(string.Format("../edit.aspx?ClientId={0}", SelectedUser.Client)), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string logOnToken = null;
                string pswdToken = null;
                this.contactsform1.GetValues(this.SelectedUser, ref logOnToken, ref pswdToken);

                this.SelectedUser = SystemManager.UpdateClientUser(this.SelectedUser);
                if (!string.Equals(logOnToken, this.SelectedCredentials.LogOnToken, StringComparison.InvariantCulture))
                {
                    this.SelectedCredentials.LogOnToken = logOnToken;
                    this.SelectedCredentials = SystemManager.UpdateCredential(this.SelectedCredentials);
                }

                this.Response.Redirect(_UrlSuffix(string.Format("../edit.aspx?ClientId={0}", SelectedUser.Client)), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}