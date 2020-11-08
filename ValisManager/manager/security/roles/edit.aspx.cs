using System;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.security.roles
{
    public partial class edit : ManagerPage
    {
        protected VLRole SelectedRole
        {
            get
            {
                return this.ViewState["SelectedRole"] as VLRole;
            }
            set
            {
                this.ViewState["SelectedRole"] = value;
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
                if(!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα roleId
                    string _value = this.Request.Params["roleid"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var roleId = Int16.Parse(_value);
                        this.SelectedRole = SystemManager.GetRoleById(roleId);
                    }
                    if (this.SelectedRole == null)
                        throw new VLException("roleid is invalid!");

                    this.rolesform1.SetValues(this.SelectedRole);
                    if(this.SelectedRole.IsBuiltIn)
                    {
                        this.deleteBtn.Enabled = false;
                        this.saveBtn.Enabled = false;
                    }
                }
            }
            catch(Exception)
            {
                this.Response.Redirect("list.aspx", false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
        }



        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.rolesform1.GetValues(this.SelectedRole);

                SystemManager.UpdateRole(this.SelectedRole);

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

            this.Response.Redirect("list.aspx", false);
            this.Context.ApplicationInstance.CompleteRequest();
        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SystemManager.DeleteRole(this.SelectedRole);

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