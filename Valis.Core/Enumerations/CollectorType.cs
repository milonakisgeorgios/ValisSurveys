
namespace Valis.Core
{
    public enum CollectorType : byte
    {
        /// <summary>
        /// Create a Web Link to send via email or post to your web site
        /// </summary>
        WebLink = 0,
        /// <summary>
        /// Create custom email invitations and track who responds in your list
        /// </summary>
        Email = 1,
        /// <summary>
        /// Embed your survey on your website or display your survey in a popup window. 
        /// </summary>
        Website = 2,
        /// <summary>
        /// Post your survey to your Facebook Wall or Friends, or embed on your Page
        /// </summary>
        ShareOnFacebook = 3,
    }
}
