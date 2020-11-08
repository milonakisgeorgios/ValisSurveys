
namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum DeleteQuestionsBehavior : byte
    {
        /// <summary>
        ///Delete the page and all the questions it contains.
        /// </summary>
        DeleteAll = 0,
        /// <summary>
        /// Move the questions one page above, and delete the page
        /// </summary>
        MoveAbove = 1,
        /// <summary>
        /// Move the questions one page bellow and delete the page
        /// </summary>
        MoveBellow = 2
    }
}
