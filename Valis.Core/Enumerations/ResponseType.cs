namespace Valis.Core
{
    public enum ResponseType : byte
    {
        /// <summary>
        /// To survey το τρέχει ο recipient remotelly
        /// </summary>
        Default = 0,
        /// <summary>
        /// Το survey το τρέχει ο χειριστής manually
        /// </summary>
        Manual = 1
    }
}
