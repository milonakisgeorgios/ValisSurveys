using System;
using System.Net.Mail;
using System.Text;
using Valis.Core;

namespace ValisApplicationService.Providers
{
    /// <summary>
    /// SendGridEmailDelivery.Simplified.SMTPWebAPI
    /// </summary>
    internal interface IEmailProvider : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="subjectEncoding"></param>
        /// <param name="body"></param>
        /// <param name="bodyEncoding"></param>
        /// <param name="isBodyHtml"></param>
        /// <returns></returns>
        bool SendEmail(MailAddress from, MailAddress to, MailAddress replyTo, string subject, Encoding subjectEncoding, string body, Encoding bodyEncoding, bool isBodyHtml);
    }
}
