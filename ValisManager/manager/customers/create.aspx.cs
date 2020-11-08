using System;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.manager.customers
{
    public partial class create : ManagerPage
    {

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
                var listItem = new ListItem(item.DisplayName, item.Id);
                if (item.Id.Equals("GTB Standard Time"))
                    listItem.Selected = true;
                this.TimeZone.Items.Add(listItem);
            }

            this.cmProfile.Items.Clear();
            var profiles = SystemManager.GetClientProfiles();
            foreach(var item in profiles)
            {
                if (item.ProfileId == BuiltinProfiles.Default.ProfileId)
                {
                    var litem = new ListItem(item.Name, item.ProfileId.ToString(CultureInfo.InvariantCulture));
                    litem.Selected = true;
                    this.cmProfile.Items.Add(litem);
                }
                else
                {
                    this.cmProfile.Items.Add(new ListItem(item.Name, item.ProfileId.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }
        
        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new VLClient();
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

                client = SystemManager.CreateClient(client);

                this.Response.Redirect(_UrlSuffix("list.aspx"), false);
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