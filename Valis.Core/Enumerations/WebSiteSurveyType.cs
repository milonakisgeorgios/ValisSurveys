namespace Valis.Core
{
    /// <summary>
    /// the method you would like to use for integrating the survey into your website
    /// </summary>
    public enum WebSiteSurveyType : byte
    {
        /// <summary>
        /// Put your survey into a page on your website. 
        /// </summary>
        Embedded = 0,
        /// <summary>
        /// Put your survey into a page on your website. 
        /// </summary>
        InvitationPopup = 1,
        /// <summary>
        /// Popup a window containing your survey when someone visits a specific page on your website. 
        /// </summary>
        surveyPopup = 2
    }
}
