using System;
using System.Data.Common;

namespace Valis.Core.ViewModel
{
    public class VLBalance
    {
        public CreditType CreditType;
        public Int32 Balance;
        public Int32 Reserved;

        public VLBalance(DbDataReader reader)
        {
            this.CreditType = (CreditType)reader.GetByte(0);
            this.Balance = reader.GetInt32(1);
            this.Reserved = reader.GetInt32(2);
        }
    }
}
