using System;
using System.Diagnostics;

namespace ValisApplicationService
{
    internal interface IMonitor
    {
        void ShowMessage(DateTime date, Int32 thread, TraceLevel level, string logger, string message, Exception exception);

    }
}
