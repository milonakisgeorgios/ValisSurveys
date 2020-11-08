using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.manager.security.users
{
    public partial class usersform : ManagerUserControl
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Role.Items.Clear();
            var roles = SystemManager.GetRoles();
            foreach(var item in roles)
            {
                if (item.IsClientRole)
                    continue;
                this.Role.Items.Add(new ListItem(item.Name, item.RoleId.ToString(CultureInfo.InvariantCulture)));
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

        public void SetValues(VLSystemUser user, VLCredential credentials)
        {
            this.FirstName.Text = user.FirstName;
            this.LastName.Text = user.LastName;
            this.LogOnToken.Text = credentials.LogOnToken;
            this.Email.Text = user.Email;
            this.IsActive.Checked = user.IsActive;
            this.IsLockedOut.Checked = credentials.IsLockedOut;
            this.Role.SelectedValue = user.Role.ToString(CultureInfo.InvariantCulture);
            this.Notes.Text = user.Notes;

            if (user.IsBuiltIn)
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is WebControl)
                        ((WebControl)ctrl).Enabled = false;
                }
            }
        }


        public void GetValues(VLSystemUser user, ref string logOnToken, ref string pswdToken)
        {
            user.FirstName = this.FirstName.Text;
            user.LastName = this.LastName.Text;
            logOnToken = this.LogOnToken.Text;
            pswdToken = this.PswdToken.Text;
            user.Email = this.Email.Text;
            user.IsActive = this.IsActive.Checked;
            user.Role = Convert.ToInt16(this.Role.SelectedValue);
            user.Notes = this.Notes.Text;
        }
    }
}