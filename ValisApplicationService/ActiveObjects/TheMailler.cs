using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Valis.Core;
using ValisApplicationService.Providers;

namespace ValisApplicationService
{
    /// <summary>
    /// Αυτή η class, έχει την ευθύνη της αποστολής των emails που παράγονται απο Collectors τύπου CollectorType.Email!
    /// </summary>
    internal sealed class TheMailler : UtilityActiveObject
    {
        internal Boolean UseRealHeartBeat = true;
        #region Heart Beat Timer
        System.Timers.Timer heartBeatTimer = new System.Timers.Timer();
        AutoResetEvent heartBeatEvent = new AutoResetEvent(false);
        Int32 heartBeatEventIndex;
        Int32 totalHeartBeats;
        #endregion

        #region Οι παρακάτω μέθοδοι καλούνται απο το νήμα του caller
        public TheMailler(string name)
            : base(name)
        {
            DebugFormat("TheMailler({0})::ctor called", name);
        }
        public bool Start()
        {
            DebugFormat("TheMailler({0})::Start(), entering...", this.Name);
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
                this.Error(string.Format("TheMailler({0})::Start()", this.Name), ex);
            }
            finally
            {
                DebugFormat("TheMailler({0})::Start() ,leaving...", this.Name);
            }
            return false;
        }
        public bool Stop(int waittime)
        {
            DebugFormat("TheMailler({0})::Stop(), entering...", this.Name);
            try
            {

                //Λέμε στο Active Object να σταματήσει!
                if (this.IsAlive)
                {
                    PostStopMsg(true);
                    if (WaitResponse(waittime) == false)
                    {
                        DebugFormat("TheMailler({0})::Stop(), timeouted...", this.Name);
                        return false;
                    }
                }

                //Επιστρέφουμε στον δικό μας caller true!
                return true;
            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheMailler({0})::Stop()", this.Name), ex);
            }
            finally
            {
                DebugFormat("TheMailler({0})::Stop(), leaving...", this.Name);
            }
            return false;
        }
        public bool Quit(int waittime)
        {
            DebugFormat("TheMailler({0})::Quit(), entering...", this.Name);
            try
            {

                //Λέμε στο Active Object να σταματήσει!
                if (this.IsAlive)
                {
                    PostQuitMsg(true);
                    if (WaitResponse(waittime) == false)
                    {
                        DebugFormat("TheMailler({0})::Quit(), timeouted...", this.Name);
                        return false;
                    }
                }

                //Επιστρέφουμε στον δικό μας caller true!
                return true;
            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheMailler({0})::Quit()", this.Name), ex);
            }
            finally
            {
                DebugFormat("TheMailler({0})::Quit(), leaving...", this.Name);
            }
            return false;
        }
        #endregion

        #region Οι παρακάτω μέθοδοι εκτελούνται απο το νήμα του Controller
        protected override void OnInitializeThread()
        {
            DebugFormat("TheMailler({0})::OnInitializeThread(), entering...", this.Name);
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
                this.Error(string.Format("TheMailler({0})::OnInitializeThread()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheMailler({0})::OnInitializeThread(), leaving...", this.Name);
            }
        }
        protected override void HandleStartMsg()
        {
            DebugFormat("TheMailler({0})::HandleStartMsg(), entering...", this.Name);
            try
            {
                #region Ξεκιναμε τον timer
                if (UseRealHeartBeat)
                {
                    totalHeartBeats = 0;
                    heartBeatTimer.Interval = Globals.Mailler_HeartbeatInterval;
                    heartBeatTimer.Enabled = true;
                }
                #endregion


            }
            catch (Exception ex)
            {
                this.Error(string.Format("TheMailler({0})::HandleStartMsg()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheMailler({0})::HandleStartMsg(), leaving...", this.Name);
            }
        }
        protected override void HandleStopMsg()
        {
            DebugFormat("TheMailler({0})::HandleStopMsg(), entering...", this.Name);
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
                this.Error(string.Format("TheMailler({0})::HandleStopMsg()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheMailler({0})::HandleStopMsg(), leaving...", this.Name);
            }
        }
        protected override bool HandleQuitMsg()
        {
            DebugFormat("TheMailler({0})::HandleQuitMsg(), entering...", this.Name);
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
                this.Error(string.Format("TheMailler({0})::HandleQuitMsg()", this.Name), ex);
                throw;
            }
            finally
            {
                DebugFormat("TheMailler({0})::HandleQuitMsg(), leaving...", this.Name);
            }
            return true;
        }
        protected override void OnException(Exception ex)
        {
            this.Error("TheMailler::OnException()", ex);
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
                 * Θέλουμε το επόμενο prepared message για  αποστολή
                 */
                message = SurveyManager.GetNextPreparedMessage();
                if (message == null)
                {
                    return;
                }

                #region fetch collector & survey:
                var collector = SurveyManager.GetCollectorById(message.Collector);
                if (collector == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id,"Collector", message.Collector));
                }
                if (collector.CollectorType != CollectorType.Email)
                {
                    throw new VLException(string.Format("Collector '{0}', has wrong CollectorType.",collector.Name));
                }
                var survey = SurveyManager.GetSurveyById(collector.Survey);
                if (survey == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", collector.Survey));
                }
                #endregion


                SendMails(survey, collector, message);


                /*Η αποστολή τελείωσε*/
                if(message.FailedCounter > 0)
                    message = SurveyManager.PromoteMessageToExecutedWithErrorsStatus(message);
                else
                    message = SurveyManager.PromoteMessageToExecutedStatus(message);

                if (collector.HasSentEmails == false && message.SentCounter > 0)
                {
                    collector.HasSentEmails = true;
                    collector = SurveyManager.UpdateCollector(collector);
                }
            }
            catch (Exception ex)
            {
                try
                {                    
                    if (message != null)
                    {
                        if(message.Status != MessageStatus.Executed)
                        {
                            //Το exception έγινε πρίν τελειώσει η εκτέλεση της αποστολής των emails:
                            this.Error(string.Format("Exception while sending emails for messageid={0}", message.MessageId), ex);
                            SurveyManager.PromoteMessageToExecutingErrorStatus(message, ex);
                            message = null;
                        }
                        else
                        {
                            //To exception έγινε μετά την αποστολή των emails και την κλ΄σησ της μεθόδου PromoteMessageToExecutedStatus:
                            this.Error(string.Format("Exception after sending emails for messageid={0}. The delivery of the emails is completed!", message.MessageId), ex);
                            //SurveyManager.PromoteMessageToExecutingErrorStatus(message, ex);
                            message = null;
                        }
                    }
                    else
                    {
                        this.Error("TheMailler::HandleHeartBeat()", ex);
                    }
                }
                catch(Exception innerex)
                {
                    this.Error("TheMailler::HandleHeartBeat()", innerex);
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
                //DebugFormat("({0})  Leaving HandleHeartBeat()...", this.Name);
            }
        }

        void SendMails(VLSurvey survey, VLCollector collector, VLMessage message)
        {
            IEmailProvider provider = GetProvider();
            int totalRows = 0, pageIndex = 1, pageSize = 20;

            var fromDisplayName = SystemManager.GetSystemParameterByKey("@FromDisplayName");
            if (fromDisplayName == null) throw new VLException("SystemParameters: @FromDisplayName undefined");

            InfoFormat("TheMailler({0})::SendMails(), called for messageid={1} & subject={2}", this.Name, message.MessageId, message.Subject);
            

            /*διαβάζουμε σελίδα - σελίδα όλα τα messageRecipients για αυτό το message:*/
            var recipients = SurveyManager.GetRecipientsForMessage(message, pageIndex++, pageSize, ref totalRows);
            while (recipients.Count > 0)
            {                
                foreach (var recipient in recipients)
                {
                    #region loop each recipient and send the email:
                    
                    //Εχουμε έναν Recipient:
                    VLMessageRecipient messageRecipient = null;
                    try
                    {
                        messageRecipient = SurveyManager.GetMessageRecipientById(message.MessageId, recipient.RecipientId);
                        if (messageRecipient == null)
                            throw new VLException(string.Format("There is no MessageRecipient for messageId={0} and recipient!d={1}", message.MessageId, recipient.RecipientId));
                        messageRecipient.SendDT = Utility.UtcNow();

                        /*Οσα Recipients έχουν γίνει OptedOut δεν τα στέλνουμε:*/
                        if(recipient.IsOptedOut)
                        {
                            DebugFormat("Sending email to {0} (messageId = {1}, recipient = {2})-> FAILED (IsOptedOut)", recipient.Email, message.MessageId, recipient.RecipientId);
                            message.SkipCounter++;
                            messageRecipient.ErrorCount++;
                            messageRecipient.Status = MessageRecipientStatus.OptedOut;
                            continue;
                        }
                        /*Οσα Recipients έχουν γίνει Bounced, δεν τα στέλνουμε:*/
                        if(recipient.IsBouncedEmail)
                        {
                            DebugFormat("Sending email to {0} (messageId = {1}, recipient = {2})-> FAILED (IsBouncedEmail)", recipient.Email, message.MessageId, recipient.RecipientId);
                            message.SkipCounter++;
                            messageRecipient.ErrorCount++;
                            messageRecipient.Status = MessageRecipientStatus.Bounced;
                            continue;
                        }
                        /*Στέλνουμε μόνο όσα είναι σε status Pending:*/
                        if (messageRecipient.Status != MessageRecipientStatus.Pending)
                        {
                            message.SkipCounter++;
                            continue;
                        }


                        //TODO:
                        //Ελεγχος για τα QUOTAS του Πελάτη



                        if (collector.UseCredits && collector.CreditType.HasValue && collector.CreditType.Value == CreditType.EmailType)
                        {
                            #region Πραγματοποιούμε την ΧΡΕΩΣΗ για αυτό το email που πρόκειται να στείλουμε:
                            bool charged = false;
                            if (messageRecipient.CollectorPayment.HasValue)
                            {
                                charged = SystemManager.ChargePaymentForEmail(messageRecipient.CollectorPayment.Value, collector.CollectorId, message.MessageId, recipient.RecipientId);
                            }
                            else
                            {
                                charged = true;
                            }
                            if(charged == false)
                            {
                                DebugFormat("Sending email to {0} (messageId = {1}, recipient = {2})-> NoCredit", recipient.Email, message.MessageId, recipient.RecipientId);
                                message.FailedCounter++;
                                messageRecipient.ErrorCount++;
                                messageRecipient.Status = MessageRecipientStatus.NoCredit;
                                continue;
                            }
                            if (messageRecipient.CollectorPayment.HasValue)
                            {
                                messageRecipient.IsCharged = true;
                            }
                            #endregion
                        }


                        /*
                         * ΑΠΟ ΕΔΩ ΚΑΙ ΚΑΤΩ ΕΧΟΥΜΕ ΧΡΕΩΣEI ΓΙΑ ΤΗΝ ΑΠΟΣΤΟΛΗ ΤΟΥ EMAIL.
                         * ΓΙΑ ΑΥΤΟ ΕΑΝ ΣΥΜΒΕΙ ΚΑΤΙ ΦΡΟΝΤΙΖΟΥΜΕ ΝΑ ΞΕΧΡΕΩΣΟΥΜΕ, ΠΡΩΤΑ:
                         */
                        try
                        {
                            /*Προετοιμάζουμε το body του μηνύματος, αντικαθιστώντας τυχόν placeholders:*/
                            var subject = message.Subject;
                            var body = message.Body;
                            body = body.Replace("[SurveyLink]", Utility.GetSurveyRuntimeURL(survey, collector, recipient));
                            body = body.Replace("[RemoveLink]", Utility.GetRemoveRecipientURL(survey, collector, message, recipient));
                            var displayName = string.Format("{0} via {1}", message.Sender, fromDisplayName.ParameterValue);
                            MailAddress from = new MailAddress(message.Sender, displayName, Encoding.UTF8);
                            MailAddress to = new MailAddress(recipient.Email);
                            MailAddress replyTo = new MailAddress(message.Sender);

                            bool emailed = provider.SendEmail(from, to, replyTo, subject, Encoding.UTF8, body, Encoding.UTF8, false);
                            if (emailed == false)
                            {
                                DebugFormat("Sending email to {0} (messageId = {1}, recipient = {2})-> FAILED", recipient.Email, message.MessageId, recipient.RecipientId);
                                message.FailedCounter++;
                                messageRecipient.ErrorCount++;
                                messageRecipient.Status = MessageRecipientStatus.Failed;
                                messageRecipient.Error = "provider.SendEmail() returned false!";
                                continue;
                            }


                            DebugFormat("Sending email to {0} (messageId = {1}, recipient = {2})-> SUCCESS", recipient.Email, message.MessageId, recipient.RecipientId);
                            message.SentCounter++;
                            messageRecipient.ErrorCount = 0;
                            messageRecipient.Status = MessageRecipientStatus.Sent;
                        }
                        catch
                        {
                            if (collector.CreditType.HasValue && collector.CreditType.Value == CreditType.EmailType)
                            {
                                #region Ξεχρεώνουμε
                                if (messageRecipient.CollectorPayment.HasValue && messageRecipient.IsCharged)
                                {
                                    bool uncharged = SystemManager.UnchargePaymentForEmail(messageRecipient.CollectorPayment.Value, collector.CollectorId, message.MessageId, recipient.RecipientId);
                                    if (uncharged)
                                    {
                                        messageRecipient.IsCharged = false;
                                    }
                                }
                                #endregion
                            }
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Error(string.Format("Sending email to {0} (messageId = {1}, recipient = {2})-> Exception", recipient.Email, message.MessageId, recipient.RecipientId), ex);                        
                        message.FailedCounter++;
                        if (messageRecipient != null)
                        {
                            messageRecipient.ErrorCount++;
                            messageRecipient.Status = MessageRecipientStatus.Failed;
                            messageRecipient.Error = Utility.UnWindExceptionContent(ex);
                        }
                    }
                    finally
                    {
                        if (messageRecipient != null)
                        {
                            try
                            {
                                messageRecipient = SurveyManager.UpdateMessageRecipient(messageRecipient);

                                if (messageRecipient.Status == MessageRecipientStatus.Sent)
                                {
                                    /*το email, στάλθηκε, πρέπει να ενημερώσουμε και το Recipient:*/
                                    if (recipient.IsSentEmail == false)
                                    {
                                        recipient.IsSentEmail = true;
                                        var updatedRecipient = SurveyManager.UpdateRecipientIntl(recipient);
                                    }
                                }
                            }
                            catch(Exception innerEx)
                            {
                                this.Error(string.Format("TheMailler::SendMails():finally"), innerEx);
                            }
                        }
                    }
                    #endregion
                }

                recipients = SurveyManager.GetRecipientsForMessage(message, pageIndex++, pageSize, ref totalRows);
            }

            provider.Dispose();
        }




    }
}
