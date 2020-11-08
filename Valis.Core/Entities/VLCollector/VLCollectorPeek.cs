using System;
using System.Data.Common;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLCollectorPeek
    {
        public Int32 SurveyId { get; internal set; }
        public Int32 CollectorId { get; internal set; }
        public string CollectorName { get; internal set; }
        public string SurveyName { get; internal set; }
        public Int32 TotalRecipients { get; internal set; }



        internal VLCollectorPeek(DbDataReader reader)
        {
            this.SurveyId = reader.GetInt32(0);
            this.CollectorId = reader.GetInt32(1);
            this.CollectorName = reader.GetString(2);
            this.SurveyName = reader.GetString(3);
            this.TotalRecipients = reader.GetInt32(4);
        }
    }
}
