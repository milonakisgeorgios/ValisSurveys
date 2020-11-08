using log4net;
using System;
using ValisApplicationService.GuiElements;

namespace ValisApplicationService
{
    internal sealed class SrvLogger
    {
        ILog m_logger4net = null;
        IMonitor m_monitor = null;
        Type m_ownerType = null;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerType"></param>
        public SrvLogger(Type ownerType)
        {
            m_logger4net = m_logger4net = LogManager.GetLogger(ownerType);

            if (Globals.IsGuiPresent)
            {
                m_monitor = GuiMonitor.Instance;
            }

            m_ownerType = ownerType;
        }


        public void Debug(object message)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Verbose, m_ownerType.Name, message.ToString(), null);
                }
            }
            //else
            //{
                m_logger4net.Debug(message);
            //}
        }
        public void Debug(object message, Exception exception)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Verbose, m_ownerType.Name, message.ToString(), exception);
                }
            }
            //else
            //{
                m_logger4net.Debug(message, exception);
            //}
        }
        public void DebugFormat(string format, object arg0)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Verbose, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.DebugFormat(format, arg0);
            //}
        }
        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Verbose, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.DebugFormat(format, arg0, arg1);
            //}
        }
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1, arg2);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Verbose, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.DebugFormat(format, arg0, arg1, arg2);
            //}
        }
        public void DebugFormat(string format, params object[] args)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, args);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Verbose, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.DebugFormat(format, args);
            //}
        }



        public void Info(object message)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Info, m_ownerType.Name, message.ToString(), null);
                }
            }
            //else
            //{
                m_logger4net.Info(message);
            //}
        }
        public void Info(object message, Exception exception)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Info, m_ownerType.Name, message.ToString(), exception);
                }
            }
            //else
            //{
                m_logger4net.Info(message, exception);
            //}
        }
        public void InfoFormat(string format, object arg0)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Info, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.InfoFormat(format, arg0);
            //}
        }
        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Info, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.InfoFormat(format, arg0, arg1);
            //}
        }
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1, arg2);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Info, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.InfoFormat(format, arg0, arg1, arg2);
            //}
        }
        public void InfoFormat(string format, params object[] args)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, args);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Info, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.InfoFormat(format, args);
            //}
        }


        public void Warn(object message)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Warning, m_ownerType.Name, message.ToString(), null);
                }
            }
            //else
            //{
                m_logger4net.Warn(message);
            //}
        }
        public void Warn(object message, Exception exception)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Warning, m_ownerType.Name, message.ToString(), exception);
                }
            }
            //else
            //{
                m_logger4net.Warn(message, exception);
            //}
        }
        public void WarnFormat(string format, object arg0)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Warning, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.WarnFormat(format, arg0);
            //}
        }
        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Warning, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.WarnFormat(format, arg0, arg1);
            //}
        }
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1, arg2);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Warning, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.WarnFormat(format, arg0, arg1, arg2);
            //}
        }
        public void WarnFormat(string format, params object[] args)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, args);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Warning, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.WarnFormat(format, args);
            //}
        }


        public void Error(object message)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Error, m_ownerType.Name, message.ToString(), null);
                }
            }
            //else
            //{
                m_logger4net.Error(message);
            //}
        }
        public void Error(object message, Exception exception)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Error, m_ownerType.Name, message.ToString(), exception);
                }
            }
            //else
            //{
                m_logger4net.Error(message, exception);
            //}
        }
        public void ErrorFormat(string format, object arg0)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Error, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.ErrorFormat(format, arg0);
            //}
        }
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Error, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.ErrorFormat(format, arg0, arg1);
            //}
        }
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, arg0, arg1, arg2);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Error, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.ErrorFormat(format, arg0, arg1, arg2);
            //}
        }
        public void ErrorFormat(string format, params object[] args)
        {
            if (Globals.IsGuiPresent)
            {
                if (m_monitor != null)
                {
                    var message = string.Format(format, args);
                    m_monitor.ShowMessage(DateTime.Now, System.Threading.Thread.CurrentThread.ManagedThreadId, System.Diagnostics.TraceLevel.Error, m_ownerType.Name, message, null);
                }
            }
            //else
            //{
                m_logger4net.ErrorFormat(format, args);
            //}
        }

    }
}
