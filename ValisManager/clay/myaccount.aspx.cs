using System;
using Valis.Core;

namespace ValisManager.clay
{
    public partial class myaccount : ManagerPage
    {
        VLClient m_client = null;
        VLClientProfile m_profile = null;
        VLCountry m_country = null;
        VLClientUser m_account = null;
        VLCredential m_credentials = null;

        protected VLClient SelectedClient
        {
            get
            {
                if (m_client == null)
                {
                    m_client = SystemManager.GetClientById(Globals.UserToken.ClientId.Value);
                }
                return m_client;
            }
        }

        protected VLClientProfile ClientProfile
        {
            get
            {
                if(m_profile == null)
                {
                    m_profile = SystemManager.GetClientProfileById(Globals.UserToken.Profile.Value);
                }
                return m_profile;
            }
        }

        protected VLClientUser SelectedUser
        {
            get
            {
                if(m_account == null)
                {
                    m_account = SystemManager.GetClientUserById(Globals.UserToken.Principal);
                }
                return m_account;
            }
        }

        protected VLCredential SelectedCredentials
        {
            get
            {
                if(m_credentials == null)
                {
                    m_credentials = SystemManager.GetCredentialForPrincipal(PrincipalType.ClientUser, Globals.UserToken.Principal);
                }
                return m_credentials;
            }
        }
        protected string CountryName
        {
            get
            {
                if(m_country == null)
                {
                    m_country = SystemManager.GetCountryByIf(this.SelectedClient.Country);
                }
                return m_country.Name;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(!this.IsPostBack)
            {
                SetFormFields(this.SelectedUser, this.SelectedCredentials);
            }
        }

        void SetFormFields(VLClientUser user, VLCredential credentials)
        {
            this.FirstName.Text = user.FirstName;
            this.LastName.Text = user.LastName;
            this.UserName.Text = credentials.LogOnToken;
            this.Email.Text = user.Email;
        }
        void GetFormFields(VLClientUser user, VLCredential credentials)
        {
            user.FirstName = this.FirstName.Text;
            user.LastName = this.LastName.Text;
            user.Email = this.Email.Text;

            credentials.LogOnToken = this.UserName.Text;
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                GetFormFields(this.SelectedUser, this.SelectedCredentials);

                bool successs = false;
                if(this.SelectedClient.IsDirty)
                {
                    m_account = SystemManager.UpdateClientUser(this.SelectedUser);
                    successs = true;
                }
                if(this.SelectedCredentials.IsDirty)
                {
                    m_credentials = SystemManager.UpdateCredential(this.SelectedCredentials);
                    successs = true;
                }

                if(successs == true)
                {
                    this.InfoMessage = "Update was successful";
                }
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }


    }
}