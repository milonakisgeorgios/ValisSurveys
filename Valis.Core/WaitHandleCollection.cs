using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WaitHandleCollection : IEnumerable<WaitHandle>
    {
        List<WaitHandle> m_handles = new List<WaitHandle>();
        int m_waitHandleListVersion;
        AutoResetEvent m_msgEvent = new AutoResetEvent(false);

        /// <summary>
        /// 
        /// </summary>
        public WaitHandleCollection()
        {
            m_handles.Add(m_msgEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public int Add(WaitHandle handle)
        {
            if (m_handles.Count >= 63)
                throw new VLException("WaitHandleList has already 63 handles!");
            if (m_handles.Contains(handle))
                throw new VLException("WaitHandleList already contains the handle!");

            m_handles.Add(handle);
            m_waitHandleListVersion++;
            return m_handles.IndexOf(handle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool Remove(WaitHandle handle)
        {
            m_waitHandleListVersion++;
            return m_handles.Remove(handle);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return m_handles.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WaitHandle this[int index]
        {
            get
            {
                return m_handles[index];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Set()
        {
            m_msgEvent.Set();
        }

        WaitHandle[] m_arrayOfHandles;
        int m_arrayOfHandlesVersion = -1;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public WaitHandle[] GetArrayOfHandles()
        {
            if (m_arrayOfHandlesVersion != m_waitHandleListVersion)
            {
                m_arrayOfHandles = m_handles.ToArray();
                m_arrayOfHandlesVersion = m_waitHandleListVersion;
            }
            return m_arrayOfHandles;
        }




        #region IEnumerable<WaitHandle> Members

        IEnumerator<WaitHandle> IEnumerable<WaitHandle>.GetEnumerator()
        {
            return m_handles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_handles.GetEnumerator();
        }

        #endregion
    }
}
