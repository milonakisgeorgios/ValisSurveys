
namespace Valis.Core
{
    /// <summary>
    /// Allow Responses to be Edited?
    /// </summary>
    public enum EditResponseModes : byte
    {
        /// <summary>
        /// No, once a page in the survey is submitted, respondents cannot go back and change existing responses.
        /// </summary>
        NotAllowed = 0,
        /// <summary>
        /// Yes, respondents can go back to previous pages in the survey and update existing responses until the survey is finished or until they have exited the survey. After the survey is finished, the respondent will not be able to re-enter the survey.
        /// </summary>
        AllowedBetween = 1,
        /// <summary>
        /// Yes, respondents can re-enter the survey at any time to update their responses.
        /// </summary>
        AllowedAlways = 2
    }
}
