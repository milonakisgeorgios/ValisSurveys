using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace ValisServer.Runtime
{
    /// <summary>
    /// Οταν κληθεί η CannotExecuteSurvey, έχουμε στα σίγουρα βρεί το Survey + surveyTheme!
    /// </summary>
    public class CannotExecuteSurvey : SurveyHttpHandler
    {
        public Int32 ErrorCode { get; set; }
        public string ErrorMessage { get; set; }


        #region ProcessRequestImplementation
        protected override void ProcessRequestImplementation()
        {
            ShowCannotExecutePage(this.ErrorCode, this.ErrorMessage);
        }

        #endregion

    }
}