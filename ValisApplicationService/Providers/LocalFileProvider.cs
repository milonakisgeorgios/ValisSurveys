using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Valis.Core;

namespace ValisApplicationService.Providers
{
    /// <summary>
    /// 
    /// </summary>
    class LocalFileProvider : IEmailProvider
    {
        // Flag: Has Dispose already been called? 
        bool disposed = false;
        NetworkCredential credentials = null;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);    
        }


        public LocalFileProvider()
        {

        }

        ~LocalFileProvider()
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



        //public bool SendEmail(Valis.Core.VLSurvey survey, Valis.Core.VLCollector collector, Valis.Core.VLMessage message, Valis.Core.VLRecipient recipient)
        //{
        //    if (survey == null) throw new ArgumentNullException("survey");
        //    if (collector == null) throw new ArgumentNullException("collector");
        //    if (message == null) throw new ArgumentNullException("message");
        //    if (recipient == null) throw new ArgumentNullException("recipient");

        //    if (collector.CollectorType != CollectorType.Email)
        //    {
        //        throw new VLException("Invalid collector.");
        //    }


        //    //mail from -> to:
        //    MailAddress from = new MailAddress(message.Sender, string.Format("{0} via {1}", message.Sender, ValisSystem.Core.SystemPublicName), Encoding.UTF8);
        //    MailAddress to = new MailAddress(recipient.Email);
        //    using (MailMessage mail = new MailMessage(from, to))
        //    {
        //        //SUBJECT:
        //        mail.Subject = message.Subject;
        //        mail.SubjectEncoding = Encoding.UTF8;
        //        //BODY:
        //        var body = message.Body;
        //        body = body.Replace("[SurveyLink]", Utility.GetSurveyRuntimeURL(survey, collector, recipient));
        //        body = body.Replace("[RemoveLink]", Utility.GetRemoveRecipientURL(survey, collector, message, recipient));
        //        mail.BodyEncoding = Encoding.UTF8;
        //        mail.IsBodyHtml = false;
        //        mail.Body = body;

        //        using (FileStream fs = new FileStream(ValisSystem.Daemon.Mailer.LocalFile.Path, FileMode.Append))
        //        {
        //            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
        //            {
        //                w.WriteLine("--------------------------------------------------------------------------");
        //                w.WriteLine(string.Format("From: {0}", mail.From.Address));
        //                w.WriteLine(string.Format("To: {0}", mail.To[0].Address));
        //                w.WriteLine(string.Format("subject: {0}", mail.Subject));
        //                w.WriteLine();
        //                w.WriteLine(mail.Body);
        //                w.WriteLine();
        //                w.WriteLine();
        //            }
        //        }
        //    }


        //    return true;
        //}

        public bool SendEmail(MailAddress from, MailAddress to, MailAddress replyTo, string subject, Encoding subjectEncoding, string body, Encoding bodyEncoding, bool isBodyHtml)
        {
            if (this.disposed)
            {
                throw new ApplicationException("Object is disposed!");
            }

            using (MailMessage mail = new MailMessage(from, to))
            {
                //SUBJECT:
                mail.Subject = subject;
                mail.SubjectEncoding = subjectEncoding;
                //BODY:
                mail.BodyEncoding = bodyEncoding;
                mail.IsBodyHtml = isBodyHtml;
                mail.Body = body;


                using (FileStream fs = new FileStream(ValisSystem.Daemon.Mailer.LocalFile.Path, FileMode.Append))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("--------------------------------------------------------------------------");
                        w.WriteLine(string.Format("From: {0}", mail.From.Address));
                        w.WriteLine(string.Format("To: {0}", mail.To[0].Address));
                        if (replyTo != null)
                        {
                            w.WriteLine(string.Format("Replyto: {0}", replyTo.Address));
                        }
                        w.WriteLine(string.Format("subject: {0}", mail.Subject));
                        w.WriteLine();
                        w.WriteLine(mail.Body);
                        w.WriteLine();
                        w.WriteLine();
                    }
                }
                return true;
            }
        }
    }
}
