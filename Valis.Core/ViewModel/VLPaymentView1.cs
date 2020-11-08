using System;
using System.Data.Common;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class VLPaymentView1
    {
        public Int32 PaymentId;
        public Int32 Client;
        public string CustomCode1;
        public string CustomCode2;
        public DateTime PaymentDate;
        public Int32 Quantity;
        public Int32 QuantityUsed;
        public Int32 QuantityReserved;
        public Int32 Responses;
        public Boolean IsActive;
        public CreditType CreditType;

        public string TotalCredits;
        public string UsedCredits;
        public string RestCredits;

        public VLPaymentView1(DbDataReader reader)
        {
            this.PaymentId = reader.GetInt32(0);
            this.Client = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.CustomCode1 = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.CustomCode2 = reader.GetString(3);
            this.PaymentDate = reader.GetDateTime(4);
            this.Quantity = reader.GetInt32(5);
            this.QuantityUsed = reader.GetInt32(6);
            this.Responses = reader.GetInt32(7);
            this.QuantityReserved = reader.GetInt32(8);
            this.IsActive = reader.GetBoolean(9);
            this.CreditType = (CreditType)reader.GetByte(10);
        }
    }
}
