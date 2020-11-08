using System;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.security.users
{
    public partial class edit : ManagerPage
    {
        protected VLSystemUser SelectedUser
        {
            get
            {
                return this.ViewState["SelectedUser"] as VLSystemUser;
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
                if(this.SelectedUser != null)
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
                    //Πρέπει στο url να υπάρχει ένα UserId
                    string _value = this.Request.Params["UserId"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var userId = Int32.Parse(_value);
                        this.SelectedUser = SystemManager.GetSystemUserById(userId);
                        this.SelectedCredentials = SystemManager.GetCredentialForPrincipal(this.SelectedUser);
                    }
                    if (this.SelectedUser == null || this.SelectedCredentials == null)
                        throw new VLException("UserId is invalid!");

                    this.usersform1.IsEditMode = true;
                    this.usersform1.SetValues(SelectedUser, SelectedCredentials);
                    if(this.SelectedUser.IsBuiltIn)
                    {
                        this.deleteBtn.Enabled = false;
                        this.saveBtn.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Response.Redirect("list.aspx", false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SystemManager.DeleteSystemUser(this.SelectedUser);

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

            this.Response.Redirect("list.aspx", false);
            this.Context.ApplicationInstance.CompleteRequest();
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string logOnToken = null;
                string pswdToken = null;
                this.usersform1.GetValues(this.SelectedUser, ref logOnToken, ref pswdToken);

                this.SelectedUser = SystemManager.UpdateSystemUser(this.SelectedUser);
                if(!string.Equals(logOnToken, this.SelectedCredentials.LogOnToken, StringComparison.InvariantCulture))
                {
                    this.SelectedCredentials.LogOnToken = logOnToken;
                    this.SelectedCredentials = SystemManager.UpdateCredential(this.SelectedCredentials);
                }

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

            this.Response.Redirect("list.aspx", false);
            this.Context.ApplicationInstance.CompleteRequest();
        }

    }
}