namespace Valis.Core
{
    public enum RandomizationMode : byte
    {
        /// <summary>
        /// Randomize answer choices for each respondent
        /// </summary>
        Randomize = 0,
        /// <summary>
        /// Flip answer choices for each respondent
        /// </summary>
        Flip = 1,
        /// <summary>
        /// Sort answer choices alphabetically
        /// </summary>
        Sort = 2
    }
}
