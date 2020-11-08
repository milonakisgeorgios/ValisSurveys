using SendGrid;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ValisApplicationService.Providers
{
    class SendGridProvider : IEmailProvider
    {
        NetworkCredential credentials = null;
        SendGrid.Web transportWeb = null;

        #region Dispose & Close
        // Flag: Has Dispose already been called? 
        bool disposed = false;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }


        ~SendGridProvider()
        {
            Dispose(false);
        }


        // Protected implementation of Dispose pattern. 
        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //

            }

            // Free any unmanaged objects here. 
            //


            disposed = true;
        }


        public void Close()
        {
            Dispose();
        }
        #endregion


        public SendGridProvider()
        {
            this.credentials = new NetworkCredential(Globals.Settings.Daemon.Mailer.SendGrid.Username, Globals.Settings.Daemon.Mailer.SendGrid.Password);
            this.transportWeb = new SendGrid.Web(credentials);
        }




        public bool SendEmail(MailAddress from, MailAddress to, MailAddress replyTo, string subject, Encoding subjectEncoding, string body, Encoding bodyEncoding, bool isBodyHtml)
        {
            if (this.disposed)
            {
                throw new ApplicationException("Object is disposed!");
            }


            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage(from, new MailAddress[] { to }, subject, null, body);
            if(replyTo != null)
            {
                myMessage.ReplyTo = new MailAddress[] { replyTo };
            }

            // Send the email.
            if (transportWeb != null)
            {
                transportWeb.Deliver(myMessage);
                return true;
            }

            return false;            
        }
    }
}
