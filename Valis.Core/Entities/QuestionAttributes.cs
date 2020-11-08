using System;

namespace Valis.Core
{
    /// <summary>
    /// χρησιμοποιούνται απο τις κλάσεις: VLLibraryQuestion και VLSurveyQuestion
    /// </summary>
    [Flags]
    internal enum QuestionAttributes : int
    {
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        OptionalInputBox = 1,            // 1 << 0
        /// <summary>
        /// 
        /// </summary>
        RandomizeOptionsSequence = 2,            // 1 << 1
        /// <summary>
        /// 
        /// </summary>
        DoNotRandomizeLastOption = 4,            // 1 << 2
        /// <summary>
        /// 
        /// </summary>
        RandomizeColumnSequence = 8,            // 1 << 3
        /// <summary>
        /// 
        /// </summary>
        OneResponsePerColumn = 16,           //1 << 4
        /// <summary>
        /// 
        /// </summary>
        AddNoneOfTheAbove = 32,           //1 << 5
        /// <summary>
        /// 
        /// </summary>
        UseDateTimeControls = 64,           //1 << 6
        /// <summary>
        /// 
        /// </summary>
        AddResetLink = 128,          //1 << 7
        /// <summary>
        /// 
        /// </summary>
        HorizontalLayout = 256,          //1 << 8
        /// <summary>
        /// 
        /// </summary>
        HasSkipLogic = 512           //1 << 9
    }
}
