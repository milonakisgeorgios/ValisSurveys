using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveObject
    {
        TraceSwitch traceSwitch = new TraceSwitch("ActiveObjectSwitch", "Κάνει tracing τα ActiveObjects");
        Thread m_thread;
        bool m_setResponse;
        AutoResetEvent m_notifyEvent = new AutoResetEvent(false);
        WaitHandleCollection waitHandleList = new WaitHandleCollection();
        enum ActiveObjectSignals
        {
            GK_QUIT = 0x0001,
            GK_PULSE1 = 0x0002,
            GK_PULSE2 = 0x0003,
            GK_PULSE3 = 0x0004,
            GK_PULSE4 = 0x0005,
            GK_PULSE5 = 0x0006,
            GK_PAUSE = 0x0007,
            GK_CONTINUE = 0x0008,
            GK_KILL = 0x0009,
            GK_START = 0x0010,
            GK_STOP = 0x0011,
            GK_EXCEPTION = 0x0012
        }
        class ActiveObjectMessage
        {
            ActiveObjectSignals m_signal;
            object m_value;

            public ActiveObjectMessage(ActiveObjectSignals Signal)
            {
                m_signal = Signal;
            }

            public ActiveObjectMessage(ActiveObjectSignals Signal, object Value)
            {
                m_signal = Signal;
                m_value = Value;
            }

            public ActiveObjectSignals ActiveObjectSignal
            {
                get
                {
                    return m_signal;
                }
            }
            public object Value
            {
                get
                {
                    return m_value;
                }
            }
        }
        Queue<ActiveObjectMessage> m_messageQueue = new Queue<ActiveObjectMessage>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadName"></param>
        public ActiveObject(string threadName)
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO constructor, threadName = {0}", threadName));
            m_thread = new Thread(new ThreadStart(this.ActiveLoop));
            m_thread.Name = threadName;
            m_thread.IsBackground = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return m_thread.Name;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ManagedThreadId
        {
            get
            {
                return m_thread.ManagedThreadId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public WaitHandleCollection WaitHandleList
        {
            get
            {
                return waitHandleList;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean WaitResponse()
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.WaitResponse(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name));
            return m_notifyEvent.WaitOne();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public Boolean WaitResponse(Int32 milliseconds)
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.WaitResponse({1}), call Thread = '{2}'", m_thread.Name, milliseconds, Thread.CurrentThread.Name));
            return m_notifyEvent.WaitOne(milliseconds, false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Join()
        {
            m_thread.Join();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool Join(int milliseconds)
        {
            return m_thread.Join(milliseconds);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Abort()
        {
            m_thread.Abort();
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return m_thread.IsAlive;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        public void StartActiveObject(bool setResponse)
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.StartActiveObject({1}), call Thread = '{2}'", m_thread.Name, setResponse.ToString(), Thread.CurrentThread.Name));
            m_setResponse = setResponse;
            m_thread.Start();
        }


        #region PostFunctions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostQuitMsg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostQuitMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostQuitMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_QUIT)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostPulse1Msg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostPulse1Msg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostPulse1Msg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_PULSE1)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostPulse2Msg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostPulse2Msg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostPulse2Msg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_PULSE2)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostPulse3Msg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostPulse3Msg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostPulse3Msg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_PULSE3)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostPulse4Msg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostPulse4Msg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostPulse4Msg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_PULSE4)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostPulse5Msg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostPulse5Msg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostPulse5Msg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_PULSE5)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostPauseMsg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostPauseMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostPauseMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_PAUSE)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostContinueMsg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostContinueMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostContinueMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_CONTINUE)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostKillMsg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostKillMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostKillMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_KILL)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostStartMsg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostStartMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostStartMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_START)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        protected virtual void PostStopMsg(bool setResponse) { Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostStopMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostStopMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_STOP)); } waitHandleList.Set(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setResponse"></param>
        /// <param name="ex"></param>
        protected virtual void PostExceptionMsg(bool setResponse, Exception ex) { Trace.WriteLineIf(traceSwitch.TraceWarning, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.PostExceptionMsg(), call Thread = '{1}'", m_thread.Name, Thread.CurrentThread.Name)); if (m_thread.IsAlive == false) throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Active Object '{0}', Is Not Alive!\nOperation PostExceptionMsg, is not allowed!", m_thread.Name)); m_setResponse = setResponse; lock (m_messageQueue) { m_messageQueue.Enqueue(new ActiveObjectMessage(ActiveObjectSignals.GK_EXCEPTION, ex)); } waitHandleList.Set(); }
        #endregion

        void ActiveLoop()
        {
            Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop() -->ENTER!", m_thread.Name));

            if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }

            try
            {
                OnInitializeThread();


                System.Boolean quit = false;
                Int32 index = 0;

                while (!quit)
                {
                    index = WaitHandle.WaitAny(waitHandleList.GetArrayOfHandles());

                    if (index == 0)
                    {
                        #region Our Messages
                        while (m_messageQueue.Count > 0)
                        {
                            ActiveObjectMessage message = null;
                            lock (m_messageQueue)
                            {
                                message = m_messageQueue.Dequeue();
                            }

                            switch (message.ActiveObjectSignal)
                            {
                                case ActiveObjectSignals.GK_QUIT:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_QUIT", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    quit = HandleQuitMsg();
                                    break;
                                case ActiveObjectSignals.GK_PULSE1:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_PULSE1", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandlePulse1Msg();
                                    break;
                                case ActiveObjectSignals.GK_PULSE2:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_PULSE2", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandlePulse2Msg();
                                    break;
                                case ActiveObjectSignals.GK_PULSE3:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_PULSE3", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandlePulse3Msg();
                                    break;
                                case ActiveObjectSignals.GK_PULSE4:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_PULSE4", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandlePulse4Msg();
                                    break;
                                case ActiveObjectSignals.GK_PULSE5:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_PULSE5", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandlePulse5Msg();
                                    break;
                                case ActiveObjectSignals.GK_PAUSE:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_PAUSE", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandlePauseMsg();
                                    break;
                                case ActiveObjectSignals.GK_CONTINUE:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_CONTINUE", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandleContinueMsg();
                                    break;
                                case ActiveObjectSignals.GK_KILL:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_KILL", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    quit = true;
                                    break;
                                case ActiveObjectSignals.GK_START:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_START", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandleStartMsg();
                                    break;
                                case ActiveObjectSignals.GK_STOP:
                                    Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_STOP", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    HandleStopMsg();
                                    break;
                                case ActiveObjectSignals.GK_EXCEPTION:
                                    Trace.WriteLineIf(traceSwitch.TraceWarning, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GK_EXCEPTION", m_thread.Name));
                                    if (m_setResponse == true) { m_notifyEvent.Set(); m_setResponse = false; }
                                    quit = HandleExceptionMsg((Exception)message.Value);
                                    break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Client Code WaitHandleEvents
                        Trace.WriteLineIf(traceSwitch.TraceVerbose, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), GeneralWaitableHandler({1})", m_thread.Name, index));
                        quit = GeneralWaitableHandler(index);
                        #endregion
                    }
                }

                OnExitingThread();
            }
            catch (ThreadAbortException)
            {
                Trace.WriteLineIf(traceSwitch.TraceWarning, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), ThreadAbortException!", m_thread.Name));
                try
                {
                    OnThreadAbortException();
                }
                catch (Exception innerEx)
                {
                    Trace.WriteLineIf(traceSwitch.TraceError, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), Exception={1}!", m_thread.Name, innerEx.Message));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(traceSwitch.TraceError, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), Exception={1}!", m_thread.Name, ex.Message));
                try
                {
                    OnException(ex);
                }
                catch (Exception innerEx)
                {
                    Trace.WriteLineIf(traceSwitch.TraceError, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop(), Exception={1}!", m_thread.Name, innerEx.Message));
                }
            }
            finally
            {

            }


            Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format(CultureInfo.InvariantCulture, "AO<{0}>.ActiveLoop() -->LEAVE!", m_thread.Name));
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnInitializeThread() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnExitingThread() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnThreadAbortException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnException(Exception ex) { }

        #region Overridable Handler Functions
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool HandleQuitMsg() { return true; }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandlePulse1Msg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandlePulse2Msg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandlePulse3Msg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandlePulse4Msg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandlePulse5Msg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandlePauseMsg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandleContinueMsg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandleStartMsg() { }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandleStopMsg() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual bool HandleExceptionMsg(Exception ex) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual bool GeneralWaitableHandler(int index) { return false; }
        #endregion

    }
}
