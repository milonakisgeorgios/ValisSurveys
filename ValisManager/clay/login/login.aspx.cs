using System;
using System.Threading;
using System.Web.Security;
using Valis.Core;

namespace ValisManager.clay.login
{
    public partial class login : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Globals.IsProductionPlatform)
            {
                this.AutoLoginPlaceHolder.Visible = false;
            }
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.username.Text))
                    throw new ApplicationException("invalid username!");
                if (string.IsNullOrWhiteSpace(this.password.Text))
                    throw new ApplicationException("invalid password!");

                ValisSystem system = new ValisSystem();
                var userToken = system.LogOnUser(this.username.Text, this.password.Text);

                if (userToken == null)
                {
                    throw new VLException("Invalid Credentials!");
                }

                Globals.UserToken = userToken;
                FormsAuthentication.SetAuthCookie(this.username.Text, false /* createPersistentCookie */);

                if(userToken.PrincipalType == PrincipalType.ClientUser)
                {
                    //a normal user logged in:
                    if (string.IsNullOrWhiteSpace(Request.QueryString["ReturnUrl"]))
                    {
                        Response.Redirect(Globals.HomePage, false);
                    }
                    else
                    {
                        var returnUrl = Request.QueryString["ReturnUrl"];
                        if (returnUrl.Contains(ValisSystem.Settings.Core.VerifyUrl.Url))
                        {
                            Response.Redirect(Request.QueryString["ReturnUrl"], false);
                        }
                        else
                        {
                            Response.Redirect(Globals.HomePage, false);
                        }
                    }
                }
                else
                {
                    //a system user logged in:
                    //if (string.IsNullOrWhiteSpace(Request.QueryString["ReturnUrl"]))
                    //{
                        Response.Redirect(Globals.SystemDefaultPage, false);
                    //}
                    //else
                    //{
                    //    Response.Redirect(Request.QueryString["ReturnUrl"], false);
                    //}
                }
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.password.Text = string.Empty;
                this.username.Text = string.Empty;

                ClientScript.RegisterStartupScript(this.GetType(), "error", string.Format("alert('{0}');", ex.Message), true);
            }
        }

        protected void AutoLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Globals.IsProductionPlatform)
            {
                switch (Int32.Parse(AutoLogin.SelectedValue))
                {
                    case 1://SystemAdmin
                        {
                            this.username.Text = "sysadmin";
                            this.password.Text = "tolk!3n";
                            loginButton_Click(this.loginButton, EventArgs.Empty);
                        }
                        break;
                    case 2://Developer
                        {
                            this.username.Text = "developer";
                            this.password.Text = "tolk!3n";
                            loginButton_Click(this.loginButton, EventArgs.Empty);
                        }
                        break;
                    case 3://Admin
                        {
                            this.username.Text = "admin";
                            this.password.Text = "tolk!3n";
                            loginButton_Click(this.loginButton, EventArgs.Empty);
                        }
                        break;
                    case 4://DemoClient 1
                        {
                            this.username.Text = "erikweber";
                            this.password.Text = "erikweber@123";
                            loginButton_Click(this.loginButton, EventArgs.Empty);
                        }
                        break;
                    case 5://DemoClient 2
                        {
                            this.username.Text = "RebeccaBrink";
                            this.password.Text = "RebeccaBrink@fl12";
                            loginButton_Click(this.loginButton, EventArgs.Empty);
                        }
                        break;
                }
            }
        }

    }
}