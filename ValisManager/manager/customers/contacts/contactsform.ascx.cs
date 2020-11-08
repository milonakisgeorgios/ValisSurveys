using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.manager.customers.contacts
{
    public partial class contactsform : ManagerUserControl
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Role.Items.Clear();
            var roles = SystemManager.GetRoles();
            foreach (var item in roles)
            {
                if (!item.IsClientRole)
                    continue;

                this.Role.Items.Add(new ListItem(item.Name, item.RoleId.ToString(CultureInfo.InvariantCulture)));
            }

            this.Country.Items.Clear();
            var countries = SystemManager.GetCountries();
            foreach (var item in countries)
            {
                this.Country.Items.Add(new ListItem(item.Name, item.CountryId.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public bool IsEditMode
        {
            get
            {
                var _obj = this.ViewState["IsEditMode"];
                if (_obj == null)
                    return false;

                return (Boolean)_obj;
            }
            set
            {
                this.ViewState["IsEditMode"] = value;
            }
        }

        public void SetValues(VLClientUser user, VLCredential credentials)
        {
            this.FirstName.Text = user.FirstName;
            this.LastName.Text = user.LastName;
            this.LogOnToken.Text = credentials.LogOnToken;
            this.Email.Text = user.Email;

            this.Title.Text = user.Title;
            this.Department.Text = user.Department;
            if(user.Country.HasValue)
            {
                this.Country.SelectedValue = user.Country.Value.ToString(CultureInfo.InvariantCulture);
            }
            this.Prefecture.Text = user.Prefecture;
            this.Town.Text = user.Town;
            this.Address.Text = user.Address;
            this.Zip.Text = user.Zip;
            this.Telephone1.Text = user.Telephone1;
            this.Telephone2.Text = user.Telephone2;

            this.IsActive.Checked = user.IsActive;
            this.IsLockedOut.Checked = credentials.IsLockedOut;
            this.Role.SelectedValue = user.Role.ToString(CultureInfo.InvariantCulture);
            this.Comment.Text = user.Comment;


            if (user.IsBuiltIn)
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is WebControl)
                        ((WebControl)ctrl).Enabled = false;
                }
            }
        }

        public void GetValues(VLClientUser user, ref string logOnToken, ref string pswdToken)
        {
            user.FirstName = this.FirstName.Text;
            user.LastName = this.LastName.Text;
            logOnToken = this.LogOnToken.Text;
            pswdToken = this.PswdToken.Text;
            user.Email = this.Email.Text;
            user.Title = this.Title.Text;
            user.Department = this.Department.Text;
            if(!string.IsNullOrWhiteSpace(this.Country.SelectedValue))
            {
                user.Country = Int32.Parse(this.Country.SelectedValue);
            }
            else
            {
                user.Country = null;
            }
            user.Prefecture = this.Prefecture.Text;
            user.Town = this.Town.Text;
            user.Address = this.Address.Text;
            user.Zip = this.Zip.Text;
            user.Telephone1 = this.Telephone1.Text;
            user.Telephone2 = this.Telephone2.Text;

            user.IsActive = this.IsActive.Checked;
            user.Role = Int16.Parse(this.Role.SelectedValue);
            user.Comment = this.Comment.Text;
        }
    }
}