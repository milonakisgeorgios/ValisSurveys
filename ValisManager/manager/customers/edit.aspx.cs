using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.manager.customers
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

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            this.Country.Items.Clear();
            var countries = SystemManager.GetCountries();
            foreach (var item in countries)
            {
                this.Country.Items.Add(new ListItem(item.Name, item.CountryId.ToString(CultureInfo.InvariantCulture)));
            }

            this.TimeZone.Items.Clear();
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            foreach (var item in timezones)
            {
                this.TimeZone.Items.Add(new ListItem(item.DisplayName, item.Id));
            }


            this.cmProfile.Items.Clear();
            var profiles = SystemManager.GetClientProfiles();
            foreach (var item in profiles)
            {
                this.cmProfile.Items.Add(new ListItem(item.Name, item.ProfileId.ToString(CultureInfo.InvariantCulture)));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if(!this.IsPostBack)
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

                    SetValues(this.SelectedClient);
                }
                this.balanceStatistics1.SurveyClientId = this.SelectedClient.ClientId;
            }
            catch(Exception ex)
            {
                this.Response.Redirect("list.aspx");
            }
        }


        void SetValues(VLClient client)
        {
            this.Code.Text = client.Code;
            this.Name.Text = client.Name;
            this.Profession.Text = client.Profession;
            this.Country.SelectedValue = client.Country.ToString(CultureInfo.InvariantCulture);
            this.TimeZone.SelectedValue = client.TimeZoneId;
            this.Prefecture.Text = client.Prefecture;
            this.Town.Text = client.Town;
            this.Address.Text = client.Address;
            this.Zip.Text = client.Zip;
            this.Telephone1.Text = client.Telephone1;
            this.Telephone2.Text = client.Telephone2;
            this.WebSite.Text = client.WebSite;
            this.Comment.Text = client.Comment;
            this.cmProfile.SelectedValue = client.Profile.ToString(CultureInfo.InvariantCulture);
        }
        void GetValues(VLClient client)
        {
            client.Code = this.Code.Text;
            client.Name = this.Name.Text;
            client.Profession = this.Profession.Text;
            client.Country = Int32.Parse(this.Country.SelectedValue);
            client.TimeZoneId = this.TimeZone.SelectedValue;
            client.Prefecture = this.Prefecture.Text;
            client.Town = this.Town.Text;
            client.Address = this.Address.Text;
            client.Zip = this.Zip.Text;
            client.Telephone1 = this.Telephone1.Text;
            client.Telephone2 = this.Telephone2.Text;
            client.WebSite = this.WebSite.Text;
            client.Comment = this.Comment.Text;
            client.Profile = Int32.Parse(this.cmProfile.SelectedValue);

        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SystemManager.DeleteClient(this.SelectedClient);

                this.Response.Redirect(_UrlSuffix("list.aspx"), false);
                this.Context.ApplicationInstance.CompleteRequest();
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
                GetValues(this.SelectedClient);

                SystemManager.UpdateClient(this.SelectedClient);

                this.Response.Redirect(_UrlSuffix("list.aspx"), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

        }
    }
}