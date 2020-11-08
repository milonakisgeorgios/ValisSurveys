using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class VLClientEx : VLClient
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string m_profileName;


        public System.String ProfileName
        {
            get { return this.m_profileName; }
        }


        internal VLClientEx(DbDataReader reader)
            : base(reader)
        {
            if (!reader.IsDBNull(18)) this.m_profileName = reader.GetString(18);
        }

    }
}
