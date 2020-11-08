
namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum ComparisonOperator : byte
    {
        None            = 0,
        /// <summary>
        /// ResponseDetail.UserInput Equals UserInput1
        /// </summary>
        Equals          = 1,
        /// <summary>
        /// ResponseDetail.UserInput is Greater than UserInput1
        /// </summary>
        Greater         = 2,
        /// <summary>
        /// ResponseDetail.UserInput is Less than UserInput1
        /// </summary>
        Less            = 3,
        /// <summary>
        /// ResponseDetail.UserInput is GreaterOrEqual than UserInput1
        /// </summary>
        GreaterOrEqual  = 4,
        /// <summary>
        /// ResponseDetail.UserInput is LessOrEqual than UserInput1
        /// </summary>
        LessOrEqual     = 5,
        /// <summary>
        /// ResponseDetail.UserInput is NotEqual UserInput1
        /// </summary>
        NotEqual        = 6,
        /// <summary>
        /// ResponseDetail.UserInput is between UserInput1 and UserInput2
        /// </summary>
        Between         = 7,

        //In            = 8,
        //NotIn         = 9,

        /// <summary>
        /// ResponseDetail.UserInput like
        /// </summary>
        Like            = 10,
        /// <summary>
        ///  ResponseDetail.UserInput StartsWith
        /// </summary>
        StartsWith      = 11,
        /// <summary>
        ///  ResponseDetail.UserInput EndsWith
        /// </summary>
        EndsWith        = 12,
        /// <summary>
        /// 
        /// </summary>
        //AllWords        = 13,
        /// <summary>
        /// 
        /// </summary>
        //AnyWord         = 14,
        /// <summary>
        /// 
        /// </summary>
        //ExactPhrase     = 15,


        /// <summary>
        /// SelectedOption/SelectedColumn
        /// </summary>
        IsChecked       = 16,
        /// <summary>
        /// SelectedOption/SelectedColumn
        /// </summary>
        IsNotChecked    = 17
    }
}
