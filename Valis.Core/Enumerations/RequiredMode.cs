namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum RequiredMode : byte
    {
        /// <summary>
        /// Respondent must answer  all rows
        /// </summary>
        All = 0,
        /// <summary>
        /// Respondent must answer at least X rows
        /// </summary>
        AtLeast = 1,
        /// <summary>
        /// Respondent must answer at most X rows
        /// </summary>
        AtMost = 2,
        /// <summary>
        /// Respondent must answer exactly X rows
        /// </summary>
        Exactly = 3,
        /// <summary>
        /// Respondent must answer a range from X to Y rows
        /// </summary>
        Range = 4
    }
}
