using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Valis.Core
{
    public class VLRuntimeSession
    {
        [Flags]
        internal enum RuntimeSessionAttributes : short
        {
            None = 0,
            IsFinished = 8,            // 1 << 3
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_sessionId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        RuntimeRequestType m_requestType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_collector;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ResponseType m_responseType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_recipientKey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_recipientIP;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_userAgent;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_startDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_lastDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<string, object> m_container;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Stack<Int16> m_pagesStack;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_collectorPayment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isCharged;
        #endregion


        public object this[string name]
        {
            get
            {
                if (m_container.ContainsKey(name))
                {
                    return m_container[name];
                }
                return null;
            }
            set
            {
                if(m_container.ContainsKey(name))
                {
                    this.m_container.Remove(name);
                }
                m_container.Add(name, value);
            }
        }
        public void Remove(string key)
        {
            this.m_container.Remove(key);
        }
        public Dictionary<string, object>.KeyCollection Keys
        {
            get
            {
                return m_container.Keys;
            }
        }
        public Dictionary<string, object>.ValueCollection Values
        {
            get
            {
                return m_container.Values;
            }
        }
        public Dictionary<string, object> Container
        {
            get
            {
                return this.m_container;
            }
        }

        public Int16 PeekPage()
        {
            return m_pagesStack.Peek();
        }
        public Int16 PopPage()
        {
            return m_pagesStack.Pop();
        }
        public void PushPage(Int16 item)
        {
            m_pagesStack.Push(item);
        }
        public Boolean IsPagesStackEmpty
        {
            get
            {
                return m_pagesStack.Count <= 0;
            }
        }

        internal string GetStackDump()
        {
            var items = m_pagesStack.ToArray();
            StringBuilder sb = new StringBuilder();
            foreach(var item in items)
            {
                sb.AppendFormat("P_ID={0},", item);
            }
            return sb.ToString();
        }

        public Guid SessionId
        {
            get
            {
                return m_sessionId;
            }
        }
        public Int32 Survey
        {
            get
            {
                return m_survey;
            }
            internal set
            {
                m_survey = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RuntimeRequestType RequestType
        {
            get { return m_requestType; }
            internal set { m_requestType = value; }
        }
        public Int32? Collector
        {
            get
            {
                return m_collector;
            }
            internal set
            {
                m_collector = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ResponseType ResponseType
        {
            get { return m_responseType; }
            internal set { m_responseType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String RecipientKey
        {
            get { return m_recipientKey; }
            internal set { m_recipientKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String RecipientIP
        {
            get { return m_recipientIP; }
            internal set { m_recipientIP = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? UserAgent
        {
            get { return m_userAgent; }
            internal set { m_userAgent = value; }
        }
        public DateTime StartDt
        {
            get
            {
                return m_startDt;
            }
        }
        public DateTime LastDt
        {
            get
            {
                return m_lastDt;
            }
            internal set { m_lastDt = value; }
        }
        internal Int16 AttributeFlags
        {
            get
            {
                return m_attributeFlags;
            }
        }
        /// <summary>
        /// Μας λέει ότι το survey (που συνδέεται με αυτό το Session) έχει τερματίσει.
        /// </summary>
        public System.Boolean IsFinished
        {
            get { return (this.m_attributeFlags & ((short)RuntimeSessionAttributes.IsFinished)) == ((short)RuntimeSessionAttributes.IsFinished); }
            internal set
            {
                if (this.IsFinished == value)
                    return;

                if (value)
                    this.m_attributeFlags = (short)(this.m_attributeFlags | ((short)RuntimeSessionAttributes.IsFinished));
                else
                    this.m_attributeFlags = (short)(this.m_attributeFlags ^ ((short)RuntimeSessionAttributes.IsFinished));
            }
        }

        internal System.Byte[] SerializedData
        {
            get
            {
                return Utility.SerializeObject(m_container, true);
            }
        }

        internal System.Byte[] PagesStack
        {
            get
            {
                return Utility.SerializeObject(m_pagesStack, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32? CollectorPayment
        {
            get { return m_collectorPayment; }
            internal set { m_collectorPayment = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsCharged
        {
            get { return m_isCharged; }
            internal set { m_isCharged = value; }
        }
        /// <summary>
        /// Το session ανακτήθηκε όχι απο το URL segment.
        /// <para>Αυτό σημαίνι βασικά ότι ο χρήστης επιχειρεί να συνεχίσει το survey!!</para>
        /// </summary>
        internal Boolean IsRessurected { get; set; }

        internal VLRuntimeSession()
        {
            m_sessionId = Guid.NewGuid();
            m_container = new Dictionary<string, object>();
            m_pagesStack = new Stack<Int16>();
            m_startDt = m_lastDt = Utility.UtcNow();
        }
        internal VLRuntimeSession(DbDataReader reader)
        {
            System.Byte[] m_SerializedData;
            System.Byte[] m_PagesStackBytes;


            this.m_sessionId = reader.GetGuid(0);
            this.m_survey = reader.GetInt32(1);
            this.RequestType = (RuntimeRequestType)reader.GetByte(2);
            if (!reader.IsDBNull(3)) this.m_collector = reader.GetInt32(3);
            this.m_responseType = (ResponseType)reader.GetByte(4);
            if (!reader.IsDBNull(5)) this.m_recipientKey = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.m_recipientIP = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.m_userAgent = reader.GetInt32(7);

            this.m_startDt = reader.GetDateTime(8);
            this.m_lastDt = reader.GetDateTime(9);
            this.m_attributeFlags = reader.GetInt16(10);
            if (!reader.IsDBNull(11))
            {
                m_SerializedData = (byte[])reader.GetValue(11);
                m_container = (Dictionary<string, object>)Utility.DeserializeObject(m_SerializedData, true);
            }
            else
            {
                m_container = new Dictionary<string, object>();
            }
            if (!reader.IsDBNull(12))
            {
                m_PagesStackBytes = (byte[])reader.GetValue(12);
                m_pagesStack = (Stack<Int16>)Utility.DeserializeObject(m_PagesStackBytes, true);
            }
            else
            {
                m_pagesStack = new Stack<Int16>();
            }
            if (!reader.IsDBNull(13)) this.m_collectorPayment = reader.GetInt32(13);
            this.m_isCharged = reader.GetBoolean(14);
        }


    }
}
