using System;
using System.Data.Common;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// Η VlCharge αποτελεί ένα συγκεντρωτικό view για ένα VLCollectorPayment, που περιλαμβάνει στοιχεία και απο τα αντίστοιχα VLCollector, VLPayment και VLSurvey.
    /// <para>Μπορούμε να πούμε ότι η VLCharge αναπαριστά την χρεώση που προκύπτει για την λειτουργία ενός Collector.</para>
    /// </summary>
    public class VLCharge
    {
        [Flags]
        internal enum CollectorPaymentAttributes : int
        {
            None = 0,
            IsActive = 1,               // 1 << 0
            IsUsed = 2,             // 1 << 1
            AttributeYyy = 4,            // 1 << 2
        }


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
        /// CollectorPayments Field
        /// </summary>
        public Int32 CollectorPaymentId;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        public Int32 PaymentId;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        public Int16 UseOrder;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        public Int32 QuantityReserved;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        public Int32 QuantityUsed;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        public DateTime? FirstChargeDt;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        public DateTime? LastChargeDt;
        /// <summary>
        /// CollectorPayments Field
        /// </summary>
        internal Int32 AttributeFlags;

        /// <summary>
        /// Μας λέει εάν αυτό το CollectorPayment, μπορεί να χρησιμοιηθεί για την λειτουργία του Collector
        /// </summary>
        public System.Boolean IsActive
        {
            get { return (this.AttributeFlags & ((int)CollectorPaymentAttributes.IsActive)) == ((int)CollectorPaymentAttributes.IsActive); }
        }

        /// <summary>
        /// Μας λέει εάν αυτό το CollectorPayment, χρησιμοποιήθηκε για την λειτοργία του Collector
        /// </summary>
        public System.Boolean IsUsed
        {
            get { return (this.AttributeFlags & ((int)CollectorPaymentAttributes.IsUsed)) == ((int)CollectorPaymentAttributes.IsUsed); }
        }

        /// <summary>
        /// Payments Field
        /// </summary>
        public DateTime PaymentDate;
        /// <summary>
        /// Payments Field
        /// </summary>
        public Boolean PaymentIsActive;
        /// <summary>
        /// Payments Field
        /// </summary>
        public Int32 PaymentQuantity;
        /// <summary>
        /// Payments Field
        /// </summary>
        public Int32 PaymentQuantityUsed;


        public VLCharge(DbDataReader reader)
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

            this.CollectorPaymentId = reader.GetInt32(10);

            this.PaymentId = reader.GetInt32(11);

            this.UseOrder = reader.GetInt16(12);
            this.QuantityReserved = reader.GetInt32(13);
            this.QuantityUsed = reader.GetInt32(14);
            if (!reader.IsDBNull(15)) this.FirstChargeDt = reader.GetDateTime(15);
            if (!reader.IsDBNull(16)) this.LastChargeDt = reader.GetDateTime(16);
            this.AttributeFlags = reader.GetInt32(17);

            this.PaymentDate = reader.GetDateTime(18);
            this.PaymentIsActive = reader.GetBoolean(19);
            this.PaymentQuantity = reader.GetInt32(20);
            this.PaymentQuantityUsed = reader.GetInt32(21);
        }

    }
}
