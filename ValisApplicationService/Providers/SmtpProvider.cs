using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Valis.Core;

namespace ValisApplicationService.Providers
{
    class SmtpProvider : IEmailProvider
    {
        // Flag: Has Dispose already been called? 
        bool disposed = false;
        NetworkCredential credentials = null;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);    
        }


        public SmtpProvider()
        {
            this.credentials = new NetworkCredential(Globals.Settings.Daemon.Mailer.Smpt.Username, Globals.Settings.Daemon.Mailer.Smpt.Password);
        }

        ~SmtpProvider()
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



        public bool SendEmail(MailAddress from, MailAddress to, MailAddress replyTo, string subject, Encoding subjectEncoding, string body, Encoding bodyEncoding, bool isBodyHtml)
        {
            if (this.disposed)
            {
                throw new ApplicationException("Object is disposed!");
            }

            using (MailMessage mail = new MailMessage(from, to))
            {
                if (replyTo != null)
                {
                    mail.ReplyToList.Add(replyTo);
                }
                //SUBJECT:
                mail.Subject = subject;
                mail.SubjectEncoding = subjectEncoding;
                //BODY:
                mail.BodyEncoding = bodyEncoding;
                mail.IsBodyHtml = isBodyHtml;
                mail.Body = body;

                using (SmtpClient client = new SmtpClient(Globals.Settings.Daemon.Mailer.Smpt.Server, Globals.Settings.Daemon.Mailer.Smpt.Port))
                {
                    client.Credentials = this.credentials;
                    //client.UseDefaultCredentials = true;
                    client.EnableSsl = true;

                    client.Send(mail);
                }
            }

            return true;
        }

    }
}
