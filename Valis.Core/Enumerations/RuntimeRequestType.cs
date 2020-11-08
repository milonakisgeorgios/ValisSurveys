
namespace Valis.Core
{
    /// <summary>
    /// To παρακάτω enumeration όμαδοποιεί τα διαφορετικών ειδών Requests που εξυπηρετεί το Runtime των surveys
    /// </summary>
    public enum RuntimeRequestType : byte
    {
        /// <summary>
        /// Αγνωστο Request
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Αυτού του είδους τα requests αρχίζουν: /s/......
        /// </summary>
        Preview = 1,
        /// <summary>
        /// Αυτού του είδους τα requests αρχίζουν: /w/......
        /// </summary>
        Collector_WebLink = 2,
        /// <summary>
        /// Αυτού του είδους τα requests αρχίζουν: /em/......
        /// </summary>
        Collector_Email = 3,
        /// <summary>
        /// Αυτού του είδους τα requests αρχίζουν: /wm/.......
        /// </summary>
        Manual_webLink = 4,
        /// <summary>
        /// Αυτού του είδους τα requests αρχίζουν: /emm/......
        /// </summary>
        Manual_Email = 5
    }
}
