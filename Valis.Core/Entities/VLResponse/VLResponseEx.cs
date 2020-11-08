using System;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Valis.Core
{
    /// <summary>
    /// Προσθέτει στην VLResponseEx ένα public collections για τα ResponseDetails
    /// </summary>
    public sealed class VLResponseEx : VLResponse
    {
        public Collection<VLResponseDetail> Details { get; set; }

        public VLResponseDetail GetResponseDetail(Int16 questionId)
        {
            foreach (var d in this.Details)
            {
                if (d.Question == questionId)
                    return d;
            }
            return null;
        }

        public VLResponseDetail GetResponseDetail(Int16 questionId, byte optionId)
        {
            foreach (var d in this.Details)
            {
                if (d.SelectedOption.HasValue)
                {
                    if (d.Question == questionId && d.SelectedOption.Value == optionId)
                        return d;
                }
            }
            return null;
        }
        
        #region class constructors
        internal VLResponseEx() : base()
        {
            this.Details = new Collection<VLResponseDetail>();
        }
        internal VLResponseEx(DbDataReader reader)
            : base(reader)
        {
            this.Details = new Collection<VLResponseDetail>();
        }
        #endregion

    }
}
