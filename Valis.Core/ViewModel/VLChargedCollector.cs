using System;
using System.Data.Common;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// H VLChargedCollector αποτελεί παραλλαγή τής VLCharge.
    /// <para>H διαφορά της είναι ότι αναφέρεται σε έναν VLCollector ο οποίος έχει ένα η περισσότερα CollectorPayments κατω απο αυτόν.</para>
    /// </summary>
    public class VLChargedCollector
    {
        /// <summary>
        /// Surveys Field
        /// </summary>
        public Int32 SurveyId;
        /// <summary>
        /// Surveys Field
        /// </summary>
        public String SurveyTitle;

        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public System.Int32 CollectorId;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public System.String CollectorTitle;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public CollectorType CollectorType;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public CollectorStatus Status;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public System.Int32 Responses;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public CreditType? CreditType;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public DateTime CreationDT;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public Int32 CreatedBy;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public DateTime? FirstChargeDt;
        /// <summary>
        /// SurveyCollectors Field
        /// </summary>
        public DateTime? LastChargeDt;


        public VLChargedCollector(DbDataReader reader)
        {
            this.SurveyId = reader.GetInt32(0);
            this.SurveyTitle = reader.GetString(1);

            this.CollectorId = reader.GetInt32(2);
            this.CollectorTitle = reader.GetString(3);
            this.CollectorType = (CollectorType)reader.GetByte(4);
            this.Status = (CollectorStatus)reader.GetByte(5);
            this.Responses = reader.GetInt32(6);
            if (!reader.IsDBNull(7)) this.CreditType = (CreditType)reader.GetByte(7);
            this.CreationDT = reader.GetDateTime(8);
            this.CreatedBy = reader.GetInt32(9);
            if (!reader.IsDBNull(10)) this.FirstChargeDt = reader.GetDateTime(10);
            if (!reader.IsDBNull(11)) this.LastChargeDt = reader.GetDateTime(11);
        }
    }
}
