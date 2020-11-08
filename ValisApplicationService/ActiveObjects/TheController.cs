using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using Valis.Core;

namespace ValisApplicationService
{
    /// <summary>
    /// Αυτός είναι ο κεντρικός controller του Valis Application Server
    /// </summary>
    internal sealed class TheController : UtilityActiveObject
    {
        internal Boolean UseRealHeartBeat = true;
        #region Heart Beat Timer
        System.Timers.Timer heartBeatTimer = new System.Timers.Timer();
        AutoResetEvent heartBeatEvent = new AutoResetEvent(false);
        Int32 heartBeatEventIndex;
        Int32 totalHeartBeats;
        #endregion
        Collection<TheMailler> maillers = new Collection<TheMailler>();
        static Int32 Number_Of_Maillers = 1;

        Collection<TheSystemMailer> system_maillers = new Collection<TheSystemMailer>();
        static Int32 Number_Of_System_Maillers = 1;



        #region Οι παρακάτω μέθοδοι καλούνται απο το νήμα του caller
        public static readonly TheController Instance = new TheController();
        
        private TheController()
            : base("TheController")
        {
        }


        public bool Start()
        {
            Debug("Entering Start()...");
            try
            {
                /*Προσπαθούμε να κάνουμε login στον */
                Globals.LogOnUser();


                if (!this.IsAlive)
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
                this.Error("TheController::Start()", ex);
            }
            finally
            {
                Debug("Leaving Start()...");
            }
            return false;
        }
        public bool Stop()
        {
            Debug("Entering Stop()...");
            try
            {
                //Λέμε στο Active Object να σταματήσει!
                if (this.IsAlive)
                {
                    PostStopMsg(true);
                    WaitResponse();
                }
                
                //Επιστρέφουμε στον δικό μας caller true!
                return true;
            }
            catch (Exception ex)
            {
                this.Error("TheController::Stop()",ex);
            }
            finally
            {
                Debug("Leaving Stop()...");
            }
            return false;
        }
        public bool Quit()
        {
            Debug("Entering Quit()...");
            try
            {
                //Προσοχή το νήμα σταματάει εντελώς!
                if (this.IsAlive)
                {
                    PostQuitMsg(false);
                    this.Join(2000);
                }


                Globals.LogOffUser();
                
                //Επιστρέφουμε στον δικό μας caller true!
                return true;
            }
            catch (Exception ex)
            {
                this.Error("TheController::Quit()", ex);
            }
            finally
            {
                Debug("Leaving Quit()...");
            }
            return false;
        }
        #endregion

        #region Οι παρακάτω μέθοδοι εκτελούνται απο το νήμα του Controller
        protected override void OnInitializeThread()
        {
            Debug("Entering OnInitializeThread()...");
            try
            {
                #region σετάρουμε τον timer και τον προσθέτουμε στην WaitHandleList!
                heartBeatTimer.AutoReset = true;
                heartBeatTimer.Elapsed += delegate { heartBeatEvent.Set(); };
                heartBeatEventIndex = this.WaitHandleList.Add(heartBeatEvent);
                #endregion

                //δημιουργούμε τους απαραίτητους maillers:
                for (int index = 1; index <= Number_Of_Maillers; index++)
                {
                    var threadName = "Mailler#" + index.ToString(CultureInfo.InvariantCulture);
                    this.InfoFormat("Creating thread {0}", threadName);
                    var mailler = new TheMailler(threadName);
                    this.maillers.Add(mailler);
                }

                //δημιουργούμε τους απαραίτητους system-maillers:
                for (int index = 1; index <= Number_Of_System_Maillers; index++)
                {
                    var threadName = "SystemMailler#" + index.ToString(CultureInfo.InvariantCulture);
                    this.InfoFormat("Creating thread {0}", threadName);
                    var mailler = new TheSystemMailer(threadName);
                    this.system_maillers.Add(mailler);
                }
            }
            catch (Exception ex)
            {
                this.Error("TheController::OnInitializeThread()",ex);
                throw;
            }
            finally
            {
                Debug("Leaving OnInitializeThread()...");
            }
            
        }
        protected override void HandleStartMsg()
        {
            Debug("Entering HandleStartMsg()...");
            try
            {
                //ξεκινάμε τους maillers:
                foreach (var _mailler in this.maillers)
                {
                    this.InfoFormat("Sending START message to {0}", _mailler.Name);
                    _mailler.Start();
                }

                //ξεκινάμε τους system_maillers:
                foreach (var _mailler in this.system_maillers)
                {
                    this.InfoFormat("Sending START message to {0}", _mailler.Name);
                    _mailler.Start();
                }


                #region Ξεκιναμε τον timer
                if (UseRealHeartBeat)
                {
                    totalHeartBeats = 0;
                    heartBeatTimer.Interval = Globals.Controller_HeartbeatInterval;
                    heartBeatTimer.Enabled = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Error("TheController::HandleStartMsg()", ex);
                throw;
            }
            finally
            {
                Debug("Leaving HandleStartMsg()...");
            }
        }
        protected override void HandleStopMsg()
        {
            Debug("Entering HandleStopMsg()...");
            try
            {
                #region σταματάμε τον timer
                if (heartBeatTimer.Enabled)
                {
                    heartBeatTimer.Enabled = false;
                    totalHeartBeats = 0;
                }
                #endregion

                //σταματάμε τους maillers:
                foreach (var _mailler in this.maillers)
                {
                    if (_mailler.IsAlive)
                    {
                        this.InfoFormat("Sending STOP message to {0}", _mailler.Name);
                        if (!_mailler.Stop(1000))
                        {
                            this.WarnFormat("Thread {0}, DID NOT STOPPED IN TIME!", _mailler.Name);
                        }
                    }
                }

                //σταματάμε τους system_maillers:
                foreach (var _mailler in this.system_maillers)
                {
                    if (_mailler.IsAlive)
                    {
                        this.InfoFormat("Sending STOP message to {0}", _mailler.Name);
                        if (!_mailler.Stop(1000))
                        {
                            this.WarnFormat("Thread {0}, DID NOT STOPPED IN TIME!", _mailler.Name);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Error("TheController::HandleStopMsg()", ex);
                throw;
            }
            finally
            {
                Debug("Leaving HandleStopMsg()...");
            }
            
        }
        protected override bool HandleQuitMsg()
        {
            Debug("Entering HandleQuitMsg()...");
            try
            {
                #region σταματάμε τον timer
                if (heartBeatTimer.Enabled)
                {
                    heartBeatTimer.Enabled = false;
                    totalHeartBeats = 0;
                }
                #endregion


                //κάνουμε quit τους maillers:
                foreach (var _mailler in this.maillers)
                {
                    if (_mailler.IsAlive)
                    {
                        this.InfoFormat("Sending QUIT message to {0}", _mailler.Name);
                        if (!_mailler.Quit(2000))
                        {
                            this.WarnFormat("Thread {0}, DID NOT QUITTED IN TIME!", _mailler.Name);
                            _mailler.Abort();
                        }
                    }
                }

                //κάνουμε quit τους system_maillers:
                foreach (var _mailler in this.system_maillers)
                {
                    if (_mailler.IsAlive)
                    {
                        this.InfoFormat("Sending QUIT message to {0}", _mailler.Name);
                        if (!_mailler.Quit(2000))
                        {
                            this.WarnFormat("Thread {0}, DID NOT QUITTED IN TIME!", _mailler.Name);
                            _mailler.Abort();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                this.Error("TheController::HandleQuitMsg()", ex);
                throw;
            }
            finally
            {
                Debug("Leaving HandleQuitMsg()...");
            }
            return true;
        }
        protected override void OnException(Exception ex)
        {
            this.Error("TheController::OnException()", ex);
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


        void HandleHeartBeat()
        {
            //Debug("Entering HandleHeartBeat()...");
            VLMessage message = null;
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
                 * Θέλουμε το επόμενο pending message για αποστολή:
                 */
                message = SurveyManager.GetNextPendingMessage(minuteOffset:2);
                if (message == null)
                {
                    return;
                }



                /*
                 * Σε αυτο το σημείο προετοιμάζουμε το μήνυμα για μαζική αποστολή
                 * Εδω έχουμε το περιθώριο να κάνουμε διαφορες ενέργειες προετοιμασίας, να ελέγξουμε την πληρωμή, κ.α.
                 */
                bool _isPaymentValid = SurveyManager.ValidatePaymentForMessage(message, true);
                if(_isPaymentValid == false)
                {
                    SurveyManager.PromoteMessageToPreparingErrorStatus(message, "Payment validation failed!");
                    message = null;
                    return;
                }





                /*
                 * Επειτα τo κάνουμε promote σε Status Prepared:
                 */
                this.Info(string.Format("Preparing message with id={0} and subject '{1}'", message.MessageId, message.Subject));
                SurveyManager.PromoteMessageToPreparedStatus(message);
                message = null;
            }
            catch (Exception ex)
            {
                this.Error("TheController::HandleHeartBeat()", ex);
                if(message != null)
                {
                    SurveyManager.PromoteMessageToPreparingErrorStatus(message, ex.Message);
                    message = null;
                }
            }
            finally
            {
                #region ξεκινάμε τον timer
                if (UseRealHeartBeat)
                {
                    heartBeatTimer.Enabled = true;
                }
                #endregion
                //Debug("Leaving HandleHeartBeat()...");
            }

        }


    }
}
