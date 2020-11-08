using System;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Valis.Core
{
    /// <summary>
    /// Προσθέτει στην VLSurveyQuestion δύο public collections για τα Options και τα Columns αντίστοιχα
    /// </summary>
    public class VLSurveyQuestionEx : VLSurveyQuestion
    {
        public Collection<VLQuestionOption> Options { get; set; }

        public Collection<VLQuestionColumn> Columns { get; set; }

        public VLQuestionOption GetQuestionOption(Byte optionId)
        {
            foreach(var op in Options)
            {
                if (op.OptionId == optionId)
                    return op;
            }
            return null;
        }
        public VLQuestionColumn GetQuestionColumn(Byte columnId)
        {
            foreach (var col in Columns)
            {
                if (col.ColumnId == columnId)
                    return col;
            }
            return null;
        }

        #region class constructors
        internal VLSurveyQuestionEx() : base()
        {
            this.Options = new Collection<VLQuestionOption>();
            this.Columns = new Collection<VLQuestionColumn>();
        }
        internal VLSurveyQuestionEx(DbDataReader reader)
            : base(reader)
        {
            this.Options = new Collection<VLQuestionOption>();
            this.Columns = new Collection<VLQuestionColumn>();
        }
        #endregion

    }
}
