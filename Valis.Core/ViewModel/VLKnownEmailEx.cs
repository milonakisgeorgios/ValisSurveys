using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class VLKnownEmailEx : VLKnownEmail
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string m_clientName;
        
        public System.String ClientName
        {
            get { return this.m_clientName; }
        }


        internal VLKnownEmailEx(DbDataReader reader)
            : base(reader)
        {
            if (!reader.IsDBNull(12)) this.m_clientName = reader.GetString(12);
        }
    }
}
