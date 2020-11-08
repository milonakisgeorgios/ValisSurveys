using System;
using System.Text;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.clay
{
    public partial class verify : ManagerPage
    {

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            try
            {
                //ελέγχουμε εάν υπάρχει το VerificationCode:
                if (string.IsNullOrEmpty(Request.Params["code"]))
                {
                    throw new VLException("Invalid call!");
                }

                //Πραγματοποιούμε το verification:
                var senderVerificationCode = Request.Params["code"];
                var message = SurveyManager.VerifySenderEmail(senderVerificationCode);
                if (message == null)
                {
                    throw new VLException("There is no such VerificationCode in your account!");
                }

                //Τώρα θέλουμε να κάνουμε redirection στην σελίδα που ΄βλέπουμε αυτό το message:
                var collector = SurveyManager.GetCollectorById(message.Collector);
                if (collector == null)
                {
                    throw new VLException("Invalid collector!");
                }

                //http://localhost:39952/clay/mysurveys/collectors/messages.aspx?surveyid=1&collectorId=1&textslanguage=0
                Response.Redirect(string.Format("~/clay/mysurveys/collectors/messages.aspx?surveyid={0}&collectorId={1}&textslanguage={2}&senderverified=1", collector.Survey, collector.CollectorId, collector.TextsLanguage));

            }
            catch (Exception ex)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<div style=\"margin: auto; width: 400px;font-size: 18px; border: 1px solid red; padding: 12px;\">");
                html.Append(ex.Message);
                html.Append("</div>");

                var lit = new LiteralControl(html.ToString());
                this.PlaceHolder1.Controls.Add(lit);
            }
        }
    }
}