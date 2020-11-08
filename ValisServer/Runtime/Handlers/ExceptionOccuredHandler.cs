using System;
using System.Text;
using System.Web;

namespace ValisServer.Runtime
{
    /// <summary>
    /// Οταν κληθεί η ExceptionOccuredHandler, δεν μπορούμε να στηριχτούμε ότι έχουμε κάποιο στοιχεία διαθέσιμο.
    /// <para>Για αυτό το λόγο εμφανίζουμε ένα μήνυμα λάθους γενικό</para>
    /// </summary>
    public class ExceptionOccuredHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public Int32 ErrorCode { get; set; }
        public string ErrorMessage { get; set; }


        public void ProcessRequest(HttpContext context)
        {
            var fileContents = System.IO.File.ReadAllText(context.Server.MapPath(@"~/exceptionoccured.html"));
            var responseHtml = new StringBuilder(fileContents);

            responseHtml.Replace("#@ERRORMESSAGE", this.ErrorMessage);
            responseHtml.Replace("#@ERRORCODE", this.ErrorCode.ToString());

            context.Response.ContentType = "text/html";
            context.Response.Write(responseHtml.ToString());
        }
    }
}