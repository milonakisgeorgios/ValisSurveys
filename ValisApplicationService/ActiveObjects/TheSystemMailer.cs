using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Valis.Core;
using ValisApplicationService.Providers;

namespace ValisApplicationService
{
    /// <summary>
    /// Αυτή η class, στέλνει όλα τα SystemEmail που παράγει το σύστημα μας!
    /// </summary>
    internal sealed class TheSystemMailer : UtilityActiveObject
    {
        internal Boolean UseRealHeartBeat = true;
        #region Heart Beat Timer
        System.Timers.Timer heartBeatTimer = new System.Timers.Timer();
        AutoResetEvent heartBeatEvent = new AutoResetEvent(false);
        Int32 heartBeatEventIndex;
        Int32 totalHeartBeats;
        #endregion
        
        #region Οι παρακάτω μέθοδοι καλούνται απο το νήμα του caller
        public TheSystemMailer(string name)
            : base(name)
        {
            DebugFormat("TheSystemMailer({0})::ctor called", name);
        }
        public bool Start()
        {
            DebugFormat("TheSystemMailer({0})::Start(), entering...", this.Name);
            try
            {

                //Ξεκινάμε το νήμα του ActiveObject:
                if (this.IsAlive == false)
                {
                    this.StartActiveObject(true);
                    this.WaitResponse();
                }

                //Του στέλνουμε ένα start message, και δεν περιμένουμε response να δούμε εάν ξεκίνησε!
                PostStartMsg(false);


                //Επιστρέφουμε true για να δείξουμε ότι όλα είναι καλά!
                return true;
            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::Start()", this.Name), ex);
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::Start() ,leaving...", this.Name);
            }
            return false;
        }
        public bool Stop(int waittime)
        {
            DebugFormat("TheSystemMailer({0})::Stop(), entering...", this.Name);
            try
            {

                //Λέμε στο Active Object να σταματήσει!
                if (this.IsAlive)
                {
                    PostStopMsg(true);
                    if (WaitResponse(waittime) == false)
                    {
                        DebugFormat("TheSystemMailer({0})::Stop(), timeouted...", this.Name);
                        return false;
                    }
                }

                //Επιστρέφουμε στον δικό μας caller true!
                return true;
            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::Stop()", this.Name), ex);
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::Stop(), leaving...", this.Name);
            }
            return false;
        }
        public bool Quit(int waittime)
        {
            DebugFormat("TheSystemMailer({0})::Quit(), entering...", this.Name);
            try
            {

                //Λέμε στο Active Object να σταματήσει!
                if (this.IsAlive)
                {
                    PostQuitMsg(true);
                    if (WaitResponse(waittime) == false)
                    {
                        DebugFormat("TheSystemMailer({0})::Quit(), timeouted...", this.Name);
                        return false;
                    }
                }

                //Επιστρέφουμε στον δικό μας caller true!
                return true;
            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::Quit()", this.Name), ex);
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::Quit(), leaving...", this.Name);
            }
            return false;
        }
        #endregion

        #region Οι παρακάτω μέθοδοι εκτελούνται απο το νήμα του Controller
        protected override void OnInitializeThread()
        {
            DebugFormat("TheSystemMailer({0})::OnInitializeThread(), entering...", this.Name);
            try
            {
                #region σετάρουμε τον timer και τον προσθέτουμε στην WaitHandleList!
                heartBeatTimer.AutoReset = true;
                heartBeatTimer.Elapsed += delegate { heartBeatEvent.Set(); };
                heartBeatEventIndex = this.WaitHandleList.Add(heartBeatEvent);
                #endregion
            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::OnInitializeThread()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::OnInitializeThread(), leaving...", this.Name);
            }
        }
        protected override void HandleStartMsg()
        {
            DebugFormat("TheSystemMailer({0})::HandleStartMsg(), entering...", this.Name);
            try
            {
                #region Ξεκιναμε τον timer
                if (UseRealHeartBeat)
                {
                    totalHeartBeats = 0;
                    heartBeatTimer.Interval = Globals.SystemMailler_HeartbeatInterval;
                    heartBeatTimer.Enabled = true;
                }
                #endregion


            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::HandleStartMsg()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::HandleStartMsg(), leaving...", this.Name);
            }
        }
        protected override void HandleStopMsg()
        {
            DebugFormat("TheSystemMailer({0})::HandleStopMsg(), entering...", this.Name);
            try
            {
                #region σταματάμε τον timer
                if (heartBeatTimer.Enabled)
                {
                    heartBeatTimer.Enabled = false;
                    totalHeartBeats = 0;
                }
                #endregion


            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::HandleStopMsg()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::HandleStopMsg(), leaving...", this.Name);
            }
        }
        protected override bool HandleQuitMsg()
        {
            DebugFormat("TheSystemMailer({0})::HandleQuitMsg(), entering...", this.Name);
            try
            {
                #region σταματάμε τον timer
                if (heartBeatTimer.Enabled)
                {
                    heartBeatTimer.Enabled = false;
                    totalHeartBeats = 0;
                }
                #endregion



            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheSystemMailer({0})::HandleQuitMsg()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheSystemMailer({0})::HandleQuitMsg(), leaving...", this.Name);
            }
            return true;
        }
        protected override void OnException(Exception ex)
        {
            this.Error("TheSystemMailer::OnException()", ex);
        }
        protected override bool GeneralWaitableHandler(int index)
        {
            if (index == heartBeatEventIndex)
            {
                HandleHeartBeat();
            }

            return false;
        }

        public void EmulateHeartBeat()
        {
            HandleHeartBeat();
        }
        #endregion


        IEmailProvider GetProvider()
        {
            switch (Globals.Settings.Daemon.Mailer.Provider)
            {
                case "SmtpProvider":
                    return new SmtpProvider();
                case "SendGridProvider":
                    return new SendGridProvider();
                case "LocalFileProvider":
                    return new LocalFileProvider();
                default:
                    throw new VLException(string.Format("'{0}' is an unknown mailer provider!", Globals.Settings.Daemon.Mailer.Provider));
            }
        }


        void HandleHeartBeat()
        {
            //DebugFormat("({0})  Entering HandleHeartBeat()...", this.Name);

            try
            {
                #region Σταματάμε τον timer για να μην έχουμε περιττό pressing
                if (heartBeatTimer.Enabled)
                {
                    heartBeatTimer.Enabled = false;
                }
                #endregion

                /*
                 * TODO: send some kind of heartbeat to show we are alive
                 */


                /*
                 * Ζητάμε τα επόμενα δώδεκα (12) System emails, που είναι έτοιμα για αποστολή
                 */
                var pendingEmails = SystemManager.GetPendingSystemEmails(maxRows: 12);


                foreach(var pendingEmail in pendingEmails)
                {
                    try
                    {
                        SendSystemEmail(pendingEmail);
                        SystemManager.UpdateSystemEmail(pendingEmail);
                    }
                    catch (Exception ex)
                    {
                        this.Error("TheSystemMailer::Sending emails loop ->", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error("TheSystemMailer::HandleHeartBeat()", ex);
            }
            finally
            {
                #region ξεκινάμε τον timer
                if (UseRealHeartBeat)
                {
                    heartBeatTimer.Enabled = true;
                }
                #endregion
                //DebugFormat("({0})  Leaving HandleHeartBeat()...", this.Name);
            }
        }

        void SendSystemEmail(VLSystemEmail pendingEmail)
        {
            IEmailProvider provider = GetProvider();

            InfoFormat("TheSystemMailer({0})::SendSystemEmail(), called for EmailId={1} & Subject={2} & ToAddress={3}", this.Name, pendingEmail.EmailId, pendingEmail.Subject, pendingEmail.ToAddress);

            var subject = pendingEmail.Subject;
            var body = pendingEmail.Body;
            MailAddress from = new MailAddress(pendingEmail.FromAddress, pendingEmail.FromDisplayName, Encoding.UTF8);
            MailAddress to = new MailAddress(pendingEmail.ToAddress);
            MailAddress replyTo = new MailAddress(pendingEmail.FromAddress);

            bool emailed = provider.SendEmail(from, to, replyTo, subject, Encoding.UTF8, body, Encoding.UTF8, false);
            if (emailed == false)
            {
                DebugFormat("Sending email to {0} (EmailId = {1}, subject = {2})-> FAILED", pendingEmail.ToAddress, pendingEmail.EmailId, pendingEmail.Subject);
                pendingEmail.Status = EmailStatus.Failed;
                pendingEmail.Error = "provider.SendEmail() returned false!";
                pendingEmail.SendDT = Utility.UtcNow();
                return;
            }

            DebugFormat("Sending email to {0} (EmailId = {1}, subject = {2})-> SUCCESS", pendingEmail.ToAddress, pendingEmail.EmailId, pendingEmail.Subject);
             pendingEmail.Status = EmailStatus.Sent;
             pendingEmail.SendDT = Utility.UtcNow();
        }
    }
}
