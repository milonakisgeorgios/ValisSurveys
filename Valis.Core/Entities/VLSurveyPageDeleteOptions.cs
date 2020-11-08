using System;

namespace Valis.Core
{
    /// <summary>
    ///
    /// </summary>
    public sealed class VLSurveyPageDeleteOptions
    {
        public Int32 SurveyId { get; internal set; }
        public Int16 PageId { get; internal set; }
        public string ShowTitle { get; internal set; }
        public Boolean CanBeDeleted { get; internal set; }
        public Boolean HasUserDeletePermission { get; internal set; }
        public Boolean IsBuiltin { get; internal set; }
        public Boolean SurveyHasResponses { get; internal set; }
        public Boolean HasQuestions { get; internal set; }
        public Boolean HasNextPage { get; internal set; }
        public Int16 NextPageId { get; internal set; }
        public Boolean HasPreviousPage { get; internal set; }
        public Int16 PreviousPageId { get; internal set; }
    }
}
