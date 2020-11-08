using System;

namespace Valis.Core
{
    public class VLQuestionOptionHelper
    {
        public Int32 Survey { get; set; }
        public Int16 Question { get; set; }
        public Byte OptionId { get; set; }
        public Int16? skipToPage { get; set; }
        public Int16? SkipToQuestion { get; set; }
        public String SkipToWebUrl { get; set; }

        internal VLQuestionOption OptionPtr { get; set; }
    }
}
