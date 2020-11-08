using System;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLSurveyQuestionDeleteOptions
    {
        public Int32 SurveyId { get; internal set; }
        public Int16 PageId { get; internal set; }
        public Int16 QuestionId { get; internal set; }
        public System.Int16? MasterQuestion { get; internal set; }

        public System.String QuestionText { get; internal set; }

        public QuestionType QuestionType { get; internal set; }

        public Boolean CanBeDeleted { get; internal set; }
        public Boolean HasUserDeletePermission { get; internal set; }
        public Boolean IsBuiltin { get; internal set; }
        public Boolean SurveyHasResponses { get; internal set; }
    }
}
