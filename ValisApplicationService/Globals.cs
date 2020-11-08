using System;
using Valis.Core;
using Valis.Core.Configuration;

namespace ValisApplicationService
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Globals
    {
        static ValisSystem m_valisSystem = new ValisSystem();
        internal static ValisSection Settings = ValisSystem.Settings;


        /// <summary>
        /// Το ονομα του service.
        /// <para>Το όνομα του service ορίζεται στο configuration αρχείο της εγκατάστασης!</para>
        /// </summary>
        internal static string ServiceName
        {
            get
            {
                return ValisSystem.Daemon.ServiceName;
            }
        }
        /// <summary>
        /// To LogOnToken που θα χρησιμοποιήσει το service για να συνδεθεί στο valis
        /// </summary>
        internal static string LogOnToken
        {
            get
            {
                return ValisSystem.Daemon.LogOnToken;
            }
        }
        /// <summary>
        /// To Password που θα χρησιμοποιήσει το service για να συνδεθεί στο valis
        /// </summary>
        internal static string PswdToken
        {
            get
            {
                return ValisSystem.Daemon.PswdToken;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal static Int32 Controller_HeartbeatInterval
        {
            get
            {
                return ValisSystem.Daemon.HeartbeatTimer.Interval;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal static VLAccessToken AccessToken { get; set; }

        internal static void LogOnUser()
        {
            if (Globals.AccessToken == null)
            {
                Globals.AccessToken = m_valisSystem.LogOnUser(Globals.LogOnToken, Globals.PswdToken);
                if (Globals.AccessToken == null)
                {
                    throw new VLException(string.Format("ValisDaemon failed to LogOn to the Valis system! LogOnToken={0}, PswdToken={1}", Globals.LogOnToken, Globals.PswdToken));
                }
            }
        }

        internal static void LogOffUser()
        {
            if (Globals.AccessToken != null)
            {
                m_valisSystem.LogOffUser(Globals.AccessToken);
                Globals.AccessToken = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal static Int32 Mailler_HeartbeatInterval
        {
            get
            {
                return ValisSystem.Daemon.Mailer.HeartbeatTimer.Interval;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal static Int32 SystemMailler_HeartbeatInterval
        {
            get
            {
                return ValisSystem.Daemon.Mailer.HeartbeatTimer.Interval;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static bool IsGuiPresent { get; set; }
    }
}
