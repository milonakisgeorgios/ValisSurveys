namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum ValidationMode : byte
    {
        /// <summary>
        /// don't validate comment text 
        /// </summary>
        DoNotValidate = 0,
        /// <summary>
        /// must be a specific length 
        /// </summary>
        TextOfSpecificLength = 1,
        /// <summary>
        /// must be a whole number 
        /// </summary>
        WholeNumber = 2,
        /// <summary>
        /// must be a decimal number
        /// </summary>
        DecimalNumber = 3,
        /// <summary>
        /// must be a date (MM/DD/YYYY) 
        /// </summary>
        Date1 =4,
        /// <summary>
        /// must be a date (DD/MM/YYYY) 
        /// </summary>
        Date2 = 5,
        /// <summary>
        /// must be an email address   
        /// </summary>
        Email = 6,
        /// <summary>
        /// 
        /// </summary>
        RegularExpression = 7
    }
}
