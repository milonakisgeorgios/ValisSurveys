using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class MessageTests01 : AdminBaseClass
    {

        /// <summary>
        /// Χωρίς πληρωμές
        /// </summary>
        [TestMethod, Description("")]
        public void MessageTests01_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργία πελάτη, και λίστας με 10 επαφές:
                //Δημιουργούμε ένα πελάτη
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                //Εισαγωγή 10 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.UnitTest);
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");

                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1");
                //O collector έχει status New, δεν έχει recipients και δεν έχει messages:
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 0);

                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                surveyManager.ImportRecipientsFromString(collector01.CollectorId, "milonakis@hotail.com, George, Milonakis", ContactImportOptions.Default);

                //O collector έχει status New, έχει recipients και δεν έχει messages:
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 0);


                //Δημιουργούμε ένα draft message στον collector
                var message01 = surveyManager.CreateMessage(collector01.CollectorId);
                Assert.IsTrue(message01.Collector == collector01.CollectorId);
                Assert.IsTrue(message01.Status == MessageStatus.Draft);


                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);

                //to message που δημιουργήσαμε default έχει για αποστολή σε όλους τους recipients:
                Assert.IsTrue(message01.DeliveryMethod == DeliveryMethod.All);
                Assert.IsTrue(surveyManager.CompileRecipientsForMessage(message01).Count == 11);
                
                #region
                message01.DeliveryMethod = DeliveryMethod.AllResponded;
                message01 = surveyManager.UpdateMessage(message01);
                Assert.IsTrue(message01.DeliveryMethod == DeliveryMethod.AllResponded);
                Assert.IsTrue(surveyManager.CompileRecipientsForMessage(message01).Count == 0);
                Assert.IsFalse(message01.IsDeliveryMethodOK);

                message01.DeliveryMethod = DeliveryMethod.NewAndUnsent;
                message01 = surveyManager.UpdateMessage(message01);
                Assert.IsTrue(message01.DeliveryMethod == DeliveryMethod.NewAndUnsent);
                Assert.IsTrue(surveyManager.CompileRecipientsForMessage(message01).Count == 11);

                message01.DeliveryMethod = DeliveryMethod.NotResponded;
                message01 = surveyManager.UpdateMessage(message01);
                Assert.IsTrue(message01.DeliveryMethod == DeliveryMethod.NotResponded);
                Assert.IsTrue(surveyManager.CompileRecipientsForMessage(message01).Count == 0);

                message01.DeliveryMethod = DeliveryMethod.All;
                message01.IsDeliveryMethodOK = true;
                message01 = surveyManager.UpdateMessage(message01);
                Assert.IsTrue(message01.DeliveryMethod == DeliveryMethod.All);
                Assert.IsTrue(surveyManager.CompileRecipientsForMessage(message01).Count == 11);
                Assert.IsTrue(message01.IsDeliveryMethodOK);
                #endregion

                //Μπορούμε να αλλάξουμε το sender:
                message01.Sender = "ffffffff";
                _EXECUTEAndCATCHType(delegate { surveyManager.UpdateMessage(message01); }, typeof(VLException));
                message01.Sender = "administrator@globo.com";
                message01 = surveyManager.UpdateMessage(message01);
                Assert.AreEqual<string>(message01.Sender, "administrator@globo.com");

                //Μπορούμε να αλλάξουμε το body:
                message01.Body = "Hello George!";
                _EXECUTEAndCATCHType(delegate { surveyManager.UpdateMessage(message01); }, typeof(VLException));
                message01.Body = "Hello [SurveyLink]George!";
                _EXECUTEAndCATCHType(delegate { surveyManager.UpdateMessage(message01); }, typeof(VLException));
                message01.Body = "Hello [RemoveLink]George!";
                _EXECUTEAndCATCHType(delegate { surveyManager.UpdateMessage(message01); }, typeof(VLException));
                message01.Body = "Hello [RemoveLink]George[SurveyLink]!";
                message01 = surveyManager.UpdateMessage(message01);
                Assert.AreEqual<string>(message01.Body, "Hello [RemoveLink]George[SurveyLink]!");

                //Μπορούμε να αλλάξουμε το subject:
                message01.Subject = "Πάρτε μέρος στην έρευνα!";
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);
                Assert.AreEqual<string>(message01.Subject, "Πάρτε μέρος στην έρευνα!");


                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);
                //To Draft Message θα σταλεί σε 11 recipients:
                Assert.IsTrue(surveyManager.CompileRecipientsForMessage(message01).Count == 11);
                //To Draft Message έχει sender, subject και body:
                Assert.IsNotNull(message01.Sender);
                Assert.IsNotNull(message01.Subject);
                Assert.IsNotNull(message01.Body);
                //To Draft Message δεν έχει χρονοπρογραμματιστεί:
                Assert.IsNull(message01.ScheduledAt);


                //Χρονοπρογραμματίζουμε το message:
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = surveyManager.ConvertTimeToUtc(_scheduleAt);

                surveyManager.ScheduleMessage(message01, _scheduleAt, promoteMessageToPendingStatus:true, validatePayment: true, scheduleDtOffset:-1);

                message01 = surveyManager.GetMessageById(message01.MessageId);

                //τώρα το message βρίσκεται σε status Pending:
                Assert.IsTrue(message01.Status == MessageStatus.Pending);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsTrue(message01.ScheduledAt.Value.Year == _scheduleAt_UTC.Year);
                Assert.IsTrue(message01.ScheduledAt.Value.Month == _scheduleAt_UTC.Month);
                Assert.IsTrue(message01.ScheduledAt.Value.Day == _scheduleAt_UTC.Day);
                Assert.IsTrue(message01.ScheduledAt.Value.Hour == _scheduleAt_UTC.Hour);
                Assert.IsTrue(message01.ScheduledAt.Value.Minute == _scheduleAt_UTC.Minute);
                //τώρα το message διαθέτει 11 MessageRecipients:
                Assert.IsTrue(surveyManager.GetMessageRecipientsCount(message01.MessageId) == 11);
                Assert.IsTrue(surveyManager.GetMessageRecipients(message01.MessageId).Count == 11);
                Assert.IsTrue(surveyManager.GetRecipientsForMessage(message01).Count == 11);


            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

                    var collectors = surveyManager.GetCollectors(item.SurveyId);
                    foreach (var cl in collectors)
                    {
                        if(cl.Status == CollectorStatus.Open)
                        {
                            var totalScheduledMessages = surveyManager.GetMessages(cl.CollectorId);
                            foreach(var msg in totalScheduledMessages)
                            {
                                if(msg.Status == MessageStatus.Pending)
                                {
                                    surveyManager.UnScheduleMessage(msg.MessageId);
                                }
                            }
                            cl.Status = CollectorStatus.Close;
                            surveyManager.CloseCollector(cl.CollectorId);
                        }

                        surveyManager.DeleteCollector(cl.CollectorId);
                    }

                    surveyManager.DeleteSurvey(item);
                }

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }
        }

        /// <summary>
        /// Πληρωμές με emailType
        /// ScheduleMessage, UnScheduleMessage
        /// </summary>
        [TestMethod, Description("")]
        public void MessageTests01_02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργία PAID πελάτη, λίστας με 10 επαφές και πληρωμών
                //Δημιουργούμε ένα πελάτη
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                //Εισαγωγή 10 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.UnitTest);
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);


                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.ClickType, 10);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 20);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 8);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 24);
                Assert.IsNotNull(payment4);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");

                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1", CreditType.EmailType);

                //O collector έχει status New, δεν έχει recipients και δεν έχει messages:
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 0);

                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                surveyManager.ImportRecipientsFromString(collector01.CollectorId, "milonakis@hotail.com, George, Milonakis", ContactImportOptions.Default);

                //O collector έχει status New, έχει recipients και δεν έχει messages:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 0);

                //Δημιουργούμε ένα draft message στον collector
                var message01 = surveyManager.CreateMessage(collector01.CollectorId);
                Assert.IsNotNull(message01);

                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);

                //δίνουμε recipients στο μήνυμα!
                message01.DeliveryMethod = DeliveryMethod.All;
                message01.IsDeliveryMethodOK = true;
                message01 = surveyManager.UpdateMessage(message01);


                //Μπορούμε να αλλάξουμε το subject:
                message01.Subject = "Πάρτε μέρος στην έρευνα!";
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);


                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);


                //Χρονοπρογραμματίζουμε το message:
                DateTime _scheduleAt = DateTime.Now.AddHours(1);
                DateTime _scheduleAt_UTC = surveyManager.ConvertTimeToUtc(_scheduleAt);

                //ΔΕν μπορούμε διότι δεν υπάρχουν πληρωμές:
                //throw new VLException("You cannot schedule this message! There are no payments associated with the collector!");
                _EXECUTEAndCATCHType(delegate{surveyManager.ScheduleMessage(message01, _scheduleAt, true);}, typeof(VLException));


                //Πρέπει να συνδεσω το collector με πληρωμές, για το ίδιο CreditType:
                var collectorPayment3 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId);
                var collectorPayment4 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment4.PaymentId);
 



                //τώρα μπορώ να χρονοπρογραματίσουμε το μήνυμα
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, true);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNull(message01.PreparedAt);
                Assert.IsNull(message01.PreparingAt);
                Assert.IsNull(message01.ExecutingAt);

                //Εχουμε συνολικά 11 MessageRecipients
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                Assert.IsTrue(messageRecipients.Count == 11);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.New);
                    Assert.IsFalse(mr.IsCharged);
                    Assert.IsNotNull(mr.CollectorPayment);
                }
                //απο αυτά, τα  8 χρεώθηκαν με τον collectorPayment3, και τα υπόλοιπα 3 με τον collectorPayment4
                Assert.IsTrue(messageRecipients.Count(x => x.CollectorPayment == collectorPayment3.CollectorPaymentId) == 8);
                Assert.IsTrue(messageRecipients.Count(x => x.CollectorPayment == collectorPayment4.CollectorPaymentId) == 3);

                //διαβάζουμε τώρα τους collectorPayments
                collectorPayment3 = systemManager.GetCollectorPaymentById(collectorPayment3.CollectorPaymentId);
                Assert.IsTrue(collectorPayment3.Payment == payment3.PaymentId);
                Assert.IsTrue(collectorPayment3.UseOrder == 1);
                Assert.IsNull(collectorPayment3.QuantityLimit);
                Assert.IsTrue(collectorPayment3.QuantityReserved == 8);
                Assert.IsTrue(collectorPayment3.QuantityUsed == 0);
                Assert.IsNull(collectorPayment3.FirstChargeDt);
                Assert.IsNull(collectorPayment3.LastChargeDt);
                Assert.IsTrue(collectorPayment3.IsActive);
                collectorPayment4 = systemManager.GetCollectorPaymentById(collectorPayment4.CollectorPaymentId);
                Assert.IsTrue(collectorPayment4.Payment == payment4.PaymentId);
                Assert.IsTrue(collectorPayment4.UseOrder == 2);
                Assert.IsNull(collectorPayment4.QuantityLimit);
                Assert.IsTrue(collectorPayment4.QuantityReserved == 3);
                Assert.IsTrue(collectorPayment4.QuantityUsed == 0);
                Assert.IsNull(collectorPayment4.FirstChargeDt);
                Assert.IsNull(collectorPayment4.LastChargeDt);
                Assert.IsTrue(collectorPayment4.IsActive);




                //Ακυρώνουμε την αποστολή του μηνύματος
                message01 = surveyManager.UnScheduleMessage(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Draft);
                Assert.IsFalse(message01.IsScheduleOK);
                Assert.IsNull(message01.ScheduledAt);
                Assert.IsNull(message01.PendingAt);
                Assert.IsNull(message01.PreparedAt);
                Assert.IsNull(message01.PreparingAt);
                Assert.IsNull(message01.ExecutingAt);

                //ΔΕΝ εχουμε κανέα MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                Assert.IsTrue(messageRecipients.Count == 0);


                //διαβάζουμε τώρα τους collectorPayments
                collectorPayment3 = systemManager.GetCollectorPaymentById(collectorPayment3.CollectorPaymentId);
                Assert.IsTrue(collectorPayment3.Payment == payment3.PaymentId);
                Assert.IsTrue(collectorPayment3.UseOrder == 1);
                Assert.IsNull(collectorPayment3.QuantityLimit);
                Assert.IsTrue(collectorPayment3.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment3.QuantityUsed == 0);
                Assert.IsNull(collectorPayment3.FirstChargeDt);
                Assert.IsNull(collectorPayment3.LastChargeDt);
                Assert.IsTrue(collectorPayment3.IsActive);
                collectorPayment4 = systemManager.GetCollectorPaymentById(collectorPayment4.CollectorPaymentId);
                Assert.IsTrue(collectorPayment4.Payment == payment4.PaymentId);
                Assert.IsTrue(collectorPayment4.UseOrder == 2);
                Assert.IsNull(collectorPayment4.QuantityLimit);
                Assert.IsTrue(collectorPayment4.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment4.QuantityUsed == 0);
                Assert.IsNull(collectorPayment4.FirstChargeDt);
                Assert.IsNull(collectorPayment4.LastChargeDt);
                Assert.IsTrue(collectorPayment4.IsActive);
            }
            finally
            {
                #region clean up
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

                    surveyManager.UnitTesting_DestroySurvey(item.SurveyId);
                }

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
                #endregion
            }
        }



        /// <summary>
        /// Πληρωμές με emailType
        /// </summary>
        [TestMethod, Description("")]
        public void MessageTests01_03()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργία PAID πελάτη, λίστας με 10 επαφές και πληρωμών
                //Δημιουργούμε ένα πελάτη
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                //Εισαγωγή 10 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.UnitTest);
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);


                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.ClickType, 10);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 20);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 8);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 24);
                Assert.IsNotNull(payment4);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1", CreditType.EmailType);
                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                surveyManager.ImportRecipientsFromString(collector01.CollectorId, "milonakis@hotail.com, George, Milonakis", ContactImportOptions.Default);
                //Δημιουργούμε ένα draft message στον collector
                var message01 = surveyManager.CreateMessage(collector01.CollectorId);
                //δίνουμε recipients στο μήνυμα & αλλάζουμε το content:
                message01.DeliveryMethod = DeliveryMethod.All;
                message01.IsDeliveryMethodOK = true;
                message01.Subject = "Πάρτε μέρος στην έρευνα!";
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);


                //Πρέπει να συνδεσω το collector με πληρωμές, για το ίδιο CreditType:
                var collectorPayment3 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId);
                var collectorPayment4 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment4.PaymentId);

                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);

                //Χρονοπρογραμματίζουμε το message:
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = systemManager.ConvertTimeToUtc(_scheduleAt);



                //ΧΡΟΝΟΠΡΟΓΡΑΜΜΑΤΙΖΟΥΜΕ το μήνυμα
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, true, scheduleDtOffset: -1);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);

                //Εχουμε συνολικά 11 MessageRecipients
                Assert.IsTrue(surveyManager.GetMessageRecipients(message01.MessageId).Count == 11);

                //διαβάζουμε τώρα τους collectorPayments
                collectorPayment3 = systemManager.GetCollectorPaymentById(collectorPayment3.CollectorPaymentId);
                collectorPayment4 = systemManager.GetCollectorPaymentById(collectorPayment4.CollectorPaymentId);
                #region
                Assert.IsTrue(collectorPayment3.Payment == payment3.PaymentId);
                Assert.IsTrue(collectorPayment3.UseOrder == 1);
                Assert.IsNull(collectorPayment3.QuantityLimit);
                Assert.IsTrue(collectorPayment3.QuantityReserved == 8);
                Assert.IsTrue(collectorPayment3.QuantityUsed == 0);
                Assert.IsNull(collectorPayment3.FirstChargeDt);
                Assert.IsNull(collectorPayment3.LastChargeDt);
                Assert.IsTrue(collectorPayment3.IsActive);
                Assert.IsTrue(collectorPayment4.Payment == payment4.PaymentId);
                Assert.IsTrue(collectorPayment4.UseOrder == 2);
                Assert.IsNull(collectorPayment4.QuantityLimit);
                Assert.IsTrue(collectorPayment4.QuantityReserved == 3);
                Assert.IsTrue(collectorPayment4.QuantityUsed == 0);
                Assert.IsNull(collectorPayment4.FirstChargeDt);
                Assert.IsNull(collectorPayment4.LastChargeDt);
                Assert.IsTrue(collectorPayment4.IsActive);
                #endregion




                //BHMA 1a
                message01 = surveyManager.GetNextPendingMessage(minuteOffset: 10);
                Assert.IsTrue(message01.Status == MessageStatus.Preparing);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNull(message01.PreparedAt);
                Assert.IsNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);
                //MessageRecipients
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(mr.IsCharged);
                    Assert.IsNotNull(mr.CollectorPayment);
                }


                //BHMA 1b
                bool _canContinue = surveyManager.ValidatePaymentForMessage(message01, true);
                Assert.IsTrue(_canContinue);


                //BHMA 1c
                message01 = surveyManager.PromoteMessageToPreparedStatus(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Prepared);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);



                //BHMA 2a
                message01 = surveyManager.GetNextPreparedMessage();
                Assert.IsTrue(message01.Status == MessageStatus.Executing);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNotNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);
                //MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(mr.IsCharged);
                    Assert.IsNotNull(mr.CollectorPayment);
                }

                //BHMA 2b (SendMails, )
                int totalRows = 0, pageIndex = 1, pageSize = 20;
                var recipients = surveyManager.GetRecipientsForMessage(message01, pageIndex++, pageSize, ref totalRows);
                Assert.IsTrue(recipients.Count == 11);
                Assert.IsTrue(totalRows == 11);
                foreach (var recipient in recipients)
                {
                    //loop each recipient and send the email:
                    var messageRecipient = surveyManager.GetMessageRecipientById(message01.MessageId, recipient.RecipientId);
                    #region
                    Assert.IsNotNull(messageRecipient);
                    Assert.IsTrue(messageRecipient.ErrorCount == 0);
                    Assert.IsNull(messageRecipient.SendDT);
                    Assert.IsTrue(messageRecipient.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(messageRecipient.IsCharged);
                    Assert.IsNotNull(messageRecipient.CollectorPayment);
                    #endregion
                    messageRecipient.SendDT = Utility.UtcNow();

                    
                    bool charged = false;
                    if(messageRecipient.CollectorPayment.HasValue)
                    {                                                            
                        charged = systemManager.ChargePaymentForEmail(messageRecipient.CollectorPayment.Value, collector01.CollectorId, message01.MessageId, recipient.RecipientId);
                    }
                    else
                    {
                        charged = true;
                    }
                    Assert.IsTrue(charged);


                    //ΘΕΩΡΟΥΜΕ ΟΤΙ ΣΤΕΙΛΑΜΕ ΜΕ ΕΠΙΤΥΧΙΑ ΤΟ ΕΜΑΙΛ:
                    messageRecipient.Status = MessageRecipientStatus.Sent;
                    messageRecipient.IsCharged = true;
                    message01.SentCounter++;

                    //
                    messageRecipient = surveyManager.UpdateMessageRecipient(messageRecipient);

                    /*το email, στάλθηκε, πρέπει να ενημερώσουμε και το Recipient:*/
                    if (recipient.IsSentEmail == false)
                    {
                        recipient.IsSentEmail = true;
                        var updatedRecipient = surveyManager.UpdateRecipient(recipient);
                    }
                }



                //BHMA 2c
                message01 = surveyManager.PromoteMessageToExecutedStatus(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Executed);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNotNull(message01.ExecutingAt);
                Assert.IsNotNull(message01.TerminatedAt);
                //MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNotNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.Sent);
                    Assert.IsTrue(mr.IsCharged);
                    Assert.IsNotNull(mr.CollectorPayment);
                }


            }
            finally
            {
                #region clean up
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;


                    surveyManager.UnitTesting_DestroySurvey(item.SurveyId);
                }

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
                #endregion
            }
        }


        /// <summary>
        /// Πληρωμές με emailType
        /// Το παρακάτω test το χρησιμοποιώ για να κάνω Debug το ValisApplicationService.
        /// Το σταματάω στο σημέιο message01 = surveyManager.ScheduleMessage(...), και μετά εκτελώ το sending με το ValisApplicationService
        /// </summary>
        [TestMethod, Description("")]
        public void MessageTests01_04()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργία PAID πελάτη, λίστας με 9995 επαφές και πληρωμών
                //Δημιουργούμε ένα πελάτη
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                //Εισαγωγή 10000 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA-10000.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.Default);
                }
                //Η λίστα μας έχει τωρα 10000 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 9995);


                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 617);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 32);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 569);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 5000);
                Assert.IsNotNull(payment4);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1", CreditType.EmailType);
                //Δημιουργούμε στον collector 9995 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                //Δημιουργούμε ένα draft message στον collector
                var message01 = surveyManager.CreateMessage(collector01.CollectorId);
                //δίνουμε recipients στο μήνυμα & αλλάζουμε το content, και ειδικά για αυτό το test δεν θέλουμε payment validation
                message01.DeliveryMethod = DeliveryMethod.All;
                message01.IsDeliveryMethodOK = true;
                message01.Subject = "Πάρτε μέρος στην έρευνα!";
                message01.IsContentOK = true;
                message01.SkipPaymentValidations = true;
                message01 = surveyManager.UpdateMessageIntl(message01);


                //Πρέπει να συνδεσω το collector με πληρωμές, για το ίδιο CreditType:
                var collectorPayment1 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment1.PaymentId);
                var collectorPayment2 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment2.PaymentId);
                var collectorPayment3 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId);
                var collectorPayment4 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment4.PaymentId);

                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 9995);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);

                //Χρονοπρογραμματίζουμε το message:
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = systemManager.ConvertTimeToUtc(_scheduleAt);



                //ΧΡΟΝΟΠΡΟΓΡΑΜΜΑΤΙΖΟΥΜΕ το μήνυμα
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, true, validatePayment: true, scheduleDtOffset: -1);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);

                //Εχουμε συνολικά 11 MessageRecipients
                Assert.IsTrue(surveyManager.GetMessageRecipients(message01.MessageId).Count == 9995);

                //διαβάζουμε τώρα τους collectorPayments
                collectorPayment1 = systemManager.GetCollectorPaymentById(collectorPayment1.CollectorPaymentId);
                collectorPayment2 = systemManager.GetCollectorPaymentById(collectorPayment2.CollectorPaymentId);
                collectorPayment3 = systemManager.GetCollectorPaymentById(collectorPayment3.CollectorPaymentId);
                collectorPayment4 = systemManager.GetCollectorPaymentById(collectorPayment4.CollectorPaymentId);
                #region
                Assert.IsTrue(collectorPayment1.Payment == payment1.PaymentId);
                Assert.IsTrue(collectorPayment1.UseOrder == 1);
                Assert.IsNull(collectorPayment1.QuantityLimit);
                Assert.IsTrue(collectorPayment1.QuantityReserved == 617);
                Assert.IsTrue(collectorPayment1.QuantityUsed == 0);
                Assert.IsNull(collectorPayment1.FirstChargeDt);
                Assert.IsNull(collectorPayment1.LastChargeDt);
                Assert.IsTrue(collectorPayment1.IsActive);
                Assert.IsTrue(collectorPayment2.Payment == payment2.PaymentId);
                Assert.IsTrue(collectorPayment2.UseOrder == 2);
                Assert.IsNull(collectorPayment2.QuantityLimit);
                Assert.IsTrue(collectorPayment2.QuantityReserved == 32);
                Assert.IsTrue(collectorPayment2.QuantityUsed == 0);
                Assert.IsNull(collectorPayment2.FirstChargeDt);
                Assert.IsNull(collectorPayment2.LastChargeDt);
                Assert.IsTrue(collectorPayment2.IsActive);
                Assert.IsTrue(collectorPayment3.Payment == payment3.PaymentId);
                Assert.IsTrue(collectorPayment3.UseOrder == 3);
                Assert.IsNull(collectorPayment3.QuantityLimit);
                Assert.IsTrue(collectorPayment3.QuantityReserved == 569);
                Assert.IsTrue(collectorPayment3.QuantityUsed == 0);
                Assert.IsNull(collectorPayment3.FirstChargeDt);
                Assert.IsNull(collectorPayment3.LastChargeDt);
                Assert.IsTrue(collectorPayment3.IsActive);
                Assert.IsTrue(collectorPayment4.Payment == payment4.PaymentId);
                Assert.IsTrue(collectorPayment4.UseOrder == 4);
                Assert.IsNull(collectorPayment4.QuantityLimit);
                Assert.IsTrue(collectorPayment4.QuantityReserved == 5000);
                Assert.IsTrue(collectorPayment4.QuantityUsed == 0);
                Assert.IsNull(collectorPayment4.FirstChargeDt);
                Assert.IsNull(collectorPayment4.LastChargeDt);
                Assert.IsTrue(collectorPayment4.IsActive);
                #endregion




                //BHMA 1a
                message01 = surveyManager.GetNextPendingMessage(minuteOffset: 10);
                Assert.IsTrue(message01.Status == MessageStatus.Preparing);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNull(message01.PreparedAt);
                Assert.IsNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);
                //MessageRecipients
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    if (mr.CollectorPayment.HasValue)
                    {
                        Assert.IsTrue(mr.ErrorCount == 0);
                        Assert.IsNull(mr.SendDT);
                        Assert.IsTrue(mr.Status == MessageRecipientStatus.Pending);
                        Assert.IsFalse(mr.IsCharged);
                        Assert.IsNotNull(mr.CollectorPayment);
                    }
                    else
                    {
                        Assert.IsTrue(mr.ErrorCount == 0);
                        Assert.IsNull(mr.SendDT);
                        Assert.IsTrue(mr.Status == MessageRecipientStatus.NoCredit);
                        Assert.IsFalse(mr.IsCharged);
                        Assert.IsNull(mr.CollectorPayment);
                    }
                }


                //BHMA 1b
                bool _canContinue = surveyManager.ValidatePaymentForMessage(message01, true);
                Assert.IsTrue(_canContinue);


                //BHMA 1c
                message01 = surveyManager.PromoteMessageToPreparedStatus(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Prepared);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);



                //BHMA 2a
                message01 = surveyManager.GetNextPreparedMessage();
                Assert.IsTrue(message01.Status == MessageStatus.Executing);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNotNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);
                //MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    if (mr.CollectorPayment.HasValue)
                    {
                        Assert.IsTrue(mr.ErrorCount == 0);
                        Assert.IsNull(mr.SendDT);
                        Assert.IsTrue(mr.Status == MessageRecipientStatus.Pending);
                        Assert.IsFalse(mr.IsCharged);
                        Assert.IsNotNull(mr.CollectorPayment);
                    }
                    else
                    {
                        Assert.IsTrue(mr.ErrorCount == 0);
                        Assert.IsNull(mr.SendDT);
                        Assert.IsTrue(mr.Status == MessageRecipientStatus.NoCredit);
                        Assert.IsFalse(mr.IsCharged);
                        Assert.IsNull(mr.CollectorPayment);
                    }
                }

                //BHMA 2b (SendMails, )
                int totalRows = 0, pageIndex = 1, pageSize = 20;
                var recipients = surveyManager.GetRecipientsForMessage(message01);
                Assert.IsTrue(recipients.Count == 9995);

                foreach (var recipient in recipients)
                {
                    //loop each recipient and send the email:
                    var messageRecipient = surveyManager.GetMessageRecipientById(message01.MessageId, recipient.RecipientId);
                    Assert.IsNotNull(messageRecipient);
                    /*Στέλνουμε μόνο όσα είναι σε status Pending:*/
                    if (messageRecipient.Status != MessageRecipientStatus.Pending)
                    {
                        message01.SkipCounter++;
                        continue;
                    }
                    #region
                    Assert.IsTrue(messageRecipient.ErrorCount == 0);
                    Assert.IsNull(messageRecipient.SendDT);
                    Assert.IsTrue(messageRecipient.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(messageRecipient.IsCharged);
                    Assert.IsNotNull(messageRecipient.CollectorPayment);
                    #endregion
                    messageRecipient.SendDT = Utility.UtcNow();

                    bool charged = false;
                    if (messageRecipient.CollectorPayment.HasValue)
                    {
                        charged = systemManager.ChargePaymentForEmail(messageRecipient.CollectorPayment.Value, collector01.CollectorId, message01.MessageId, recipient.RecipientId);
                    }
                    else
                    {
                        charged = true;
                    }
                    Assert.IsTrue(charged);


                    //ΘΕΩΡΟΥΜΕ ΟΤΙ ΣΤΕΙΛΑΜΕ ΜΕ ΕΠΙΤΥΧΙΑ ΤΟ ΕΜΑΙΛ:
                    messageRecipient.Status = MessageRecipientStatus.Sent;
                    messageRecipient.IsCharged = true;
                    message01.SentCounter++;

                    //
                    messageRecipient = surveyManager.UpdateMessageRecipient(messageRecipient);

                    /*το email, στάλθηκε, πρέπει να ενημερώσουμε και το Recipient:*/
                    if (recipient.IsSentEmail == false)
                    {
                        recipient.IsSentEmail = true;
                        var updatedRecipient = surveyManager.UpdateRecipient(recipient);
                    }
                }



                //BHMA 2c
                message01 = surveyManager.PromoteMessageToExecutedStatus(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Executed);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNotNull(message01.ExecutingAt);
                Assert.IsNotNull(message01.TerminatedAt);
                Assert.IsTrue(message01.SentCounter == 6218);
                Assert.IsTrue(message01.SkipCounter == 3777);
                //MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    if (mr.IsCharged)
                    {
                        Assert.IsTrue(mr.ErrorCount == 0);
                        Assert.IsNotNull(mr.SendDT);
                        Assert.IsTrue(mr.Status == MessageRecipientStatus.Sent);
                        Assert.IsTrue(mr.IsCharged);
                        Assert.IsNotNull(mr.CollectorPayment);
                    }
                    else
                    {
                        Assert.IsTrue(mr.ErrorCount == 0);
                        Assert.IsNull(mr.SendDT);
                        Assert.IsTrue(mr.Status == MessageRecipientStatus.NoCredit);
                        Assert.IsFalse(mr.IsCharged);
                        Assert.IsNull(mr.CollectorPayment);
                    }
                }


            }
            finally
            {
                #region clean up
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;


                    surveyManager.UnitTesting_DestroySurvey(item.SurveyId);
                }

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
                #endregion
            }
        }


        /// <summary>
        /// Πληρωμές με Responses
        /// Το παρακάτω test το χρησιμοποιώ για να κάνω Debug το ValisApplicationService.
        /// Το σταματάω στο σημέιο message01 = surveyManager.ScheduleMessage(...), και μετά εκτελώ το sending με το ValisApplicationService
        /// </summary>
        [TestMethod, Description("")]
        public void MessageTests01_05()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργία PAID πελάτη, λίστας με 500 επαφές και πληρωμών
                //Δημιουργούμε ένα πελάτη
                var client1 = systemManager.CreateClient("Globo S.A.", BuiltinCountries.Greece, "GLB", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                //Εισαγωγή 500 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA-500.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.Default);
                }
                //Η λίστα μας έχει τωρα 500 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 500);


                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 126);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 96);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 39);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 239);
                Assert.IsNotNull(payment4);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1", CreditType.ResponseType);
                //Δημιουργούμε στον collector 500 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                //Δημιουργούμε ένα draft message στον collector
                var message01 = surveyManager.CreateMessage(collector01.CollectorId);
                //δίνουμε recipients στο μήνυμα & αλλάζουμε το content, και ειδικά για αυτό το test δεν θέλουμε payment validation
                message01.DeliveryMethod = DeliveryMethod.All;
                message01.IsDeliveryMethodOK = true;
                message01.Subject = "Πάρτε μέρος στην έρευνα!";
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);

                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 500);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);

                //Χρονοπρογραμματίζουμε το message:
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = systemManager.ConvertTimeToUtc(_scheduleAt);



                //ΔΕΝ ΜΠΟΡΟΥΜΕ ΝΑ ΧΡΟΝΟΠΡΟΓΡΑΜΜΑΤΙΣΟΥΜΕ ΤΟ MESSAGE, ΕΑΝ ΔΕΜ ΦΕΡΕΙ Ο COLLECTOR ΠΛΗΡΩΜΕΣ:
                _EXECUTEAndCATCHType(delegate { surveyManager.ScheduleMessage(message01, _scheduleAt, true, validatePayment: true, scheduleDtOffset: -1); }, typeof(VLException));


                //Πρέπει να συνδεσω το collector με πληρωμές, για το ίδιο CreditType:
                var collectorPayment1 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment1.PaymentId);
                var collectorPayment2 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment2.PaymentId);
                var collectorPayment3 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId);


                //ΧΡΟΝΟΠΡΟΓΡΑΜΜΑΤΙΖΟΥΜΕ το μήνυμα
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, true, validatePayment: true, scheduleDtOffset: -1);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);

                //Εχουμε συνολικά 11 MessageRecipients
                Assert.IsTrue(surveyManager.GetMessageRecipients(message01.MessageId).Count == 500);

                //διαβάζουμε τώρα τους collectorPayments
                collectorPayment1 = systemManager.GetCollectorPaymentById(collectorPayment1.CollectorPaymentId);
                collectorPayment2 = systemManager.GetCollectorPaymentById(collectorPayment2.CollectorPaymentId);
                collectorPayment3 = systemManager.GetCollectorPaymentById(collectorPayment3.CollectorPaymentId);
                #region
                Assert.IsTrue(collectorPayment1.Payment == payment1.PaymentId);
                Assert.IsTrue(collectorPayment1.UseOrder == 1);
                Assert.IsNull(collectorPayment1.QuantityLimit);
                Assert.IsTrue(collectorPayment1.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment1.QuantityUsed == 0);
                Assert.IsNull(collectorPayment1.FirstChargeDt);
                Assert.IsNull(collectorPayment1.LastChargeDt);
                Assert.IsTrue(collectorPayment1.IsActive);
                Assert.IsTrue(collectorPayment2.Payment == payment2.PaymentId);
                Assert.IsTrue(collectorPayment2.UseOrder == 2);
                Assert.IsNull(collectorPayment2.QuantityLimit);
                Assert.IsTrue(collectorPayment2.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment2.QuantityUsed == 0);
                Assert.IsNull(collectorPayment2.FirstChargeDt);
                Assert.IsNull(collectorPayment2.LastChargeDt);
                Assert.IsTrue(collectorPayment2.IsActive);
                Assert.IsTrue(collectorPayment3.Payment == payment3.PaymentId);
                Assert.IsTrue(collectorPayment3.UseOrder == 3);
                Assert.IsNull(collectorPayment3.QuantityLimit);
                Assert.IsTrue(collectorPayment3.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment3.QuantityUsed == 0);
                Assert.IsNull(collectorPayment3.FirstChargeDt);
                Assert.IsNull(collectorPayment3.LastChargeDt);
                Assert.IsTrue(collectorPayment3.IsActive);
                #endregion




                //BHMA 1a
                message01 = surveyManager.GetNextPendingMessage(minuteOffset: 10);
                Assert.IsTrue(message01.Status == MessageStatus.Preparing);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNull(message01.PreparedAt);
                Assert.IsNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);
                //MessageRecipients
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(mr.IsCharged);
                    Assert.IsNull(mr.CollectorPayment);
                    Assert.IsNull(mr.Error);
                }


                //BHMA 1b
                bool _canContinue = surveyManager.ValidatePaymentForMessage(message01, true);
                Assert.IsTrue(_canContinue);


                //BHMA 1c
                message01 = surveyManager.PromoteMessageToPreparedStatus(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Prepared);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);



                //BHMA 2a
                message01 = surveyManager.GetNextPreparedMessage();
                Assert.IsTrue(message01.Status == MessageStatus.Executing);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNotNull(message01.ExecutingAt);
                Assert.IsNull(message01.TerminatedAt);
                //MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(mr.IsCharged);
                    Assert.IsNull(mr.CollectorPayment);
                    Assert.IsNull(mr.Error);
                }

                //BHMA 2b (SendMails, )
                var recipients = surveyManager.GetRecipientsForMessage(message01);
                Assert.IsTrue(recipients.Count == 500);

                foreach (var recipient in recipients)
                {
                    //loop each recipient and send the email:
                    var messageRecipient = surveyManager.GetMessageRecipientById(message01.MessageId, recipient.RecipientId);
                    Assert.IsNotNull(messageRecipient);
                    /*Στέλνουμε μόνο όσα είναι σε status Pending:*/
                    if (messageRecipient.Status != MessageRecipientStatus.Pending)
                    {
                        message01.SkipCounter++;
                        continue;
                    }
                    #region
                    Assert.IsTrue(messageRecipient.ErrorCount == 0);
                    Assert.IsNull(messageRecipient.SendDT);
                    Assert.IsTrue(messageRecipient.Status == MessageRecipientStatus.Pending);
                    Assert.IsFalse(messageRecipient.IsCharged);
                    Assert.IsNull(messageRecipient.CollectorPayment);
                    #endregion
                    messageRecipient.SendDT = Utility.UtcNow();

                    bool charged = false;
                    if (messageRecipient.CollectorPayment.HasValue)
                    {
                        charged = systemManager.ChargePaymentForEmail(messageRecipient.CollectorPayment.Value, collector01.CollectorId, message01.MessageId, recipient.RecipientId);
                    }
                    else
                    {
                        charged = true;
                    }
                    Assert.IsTrue(charged);


                    //ΘΕΩΡΟΥΜΕ ΟΤΙ ΣΤΕΙΛΑΜΕ ΜΕ ΕΠΙΤΥΧΙΑ ΤΟ ΕΜΑΙΛ:
                    messageRecipient.Status = MessageRecipientStatus.Sent;
                    messageRecipient.IsCharged = true;
                    message01.SentCounter++;

                    //
                    messageRecipient = surveyManager.UpdateMessageRecipient(messageRecipient);

                    /*το email, στάλθηκε, πρέπει να ενημερώσουμε και το Recipient:*/
                    if (recipient.IsSentEmail == false)
                    {
                        recipient.IsSentEmail = true;
                        var updatedRecipient = surveyManager.UpdateRecipient(recipient);
                    }
                }



                //BHMA 2c
                message01 = surveyManager.PromoteMessageToExecutedStatus(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Executed);
                Assert.IsTrue(message01.IsScheduleOK);
                Assert.IsNotNull(message01.ScheduledAt);
                Assert.IsNotNull(message01.PendingAt);
                Assert.IsNotNull(message01.PreparingAt);
                Assert.IsNotNull(message01.PreparedAt);
                Assert.IsNotNull(message01.ExecutingAt);
                Assert.IsNotNull(message01.TerminatedAt);
                Assert.IsTrue(message01.SentCounter == 500);
                Assert.IsTrue(message01.SkipCounter == 0);
                //MessageRecipients
                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsNotNull(mr.SendDT);
                    Assert.IsTrue(mr.Status == MessageRecipientStatus.Sent);
                    Assert.IsTrue(mr.IsCharged);
                    Assert.IsNull(mr.CollectorPayment);
                    Assert.IsNull(mr.Error);
                }


            }
            finally
            {
                #region clean up
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;


                    surveyManager.UnitTesting_DestroySurvey(item.SurveyId);
                }

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
                #endregion
            }
        }



        /// <summary>
        /// Πληρωμές με Responses
        /// Το παρακάτω test το χρησιμοποιώ για να κάνω Debug το ValisApplicationService.
        /// Το σταματάω στο σημέιο message01 = surveyManager.ScheduleMessage(...), και μετά εκτελώ το sending με το ValisApplicationService
        /// </summary>
        [TestMethod, Description("")]
        public void MessageTests01_06()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργία PAID πελάτη, λίστας με 500 επαφές και πληρωμών
                //Δημιουργούμε ένα πελάτη
                var client1 = systemManager.CreateClient("Matterware S.A.", BuiltinCountries.Greece, "MaBs", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                //Εισαγωγή 500 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA-500.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.Default);
                }
                //Η λίστα μας έχει τωρα 500 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 500);


                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 2);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 1);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 2);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 3);
                Assert.IsNotNull(payment4);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1", CreditType.ResponseType);
                //Δημιουργούμε στον collector 500 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                //Δημιουργούμε ένα draft message στον collector
                var message01 = surveyManager.CreateMessage(collector01.CollectorId);
                //δίνουμε recipients στο μήνυμα & αλλάζουμε το content, και ειδικά για αυτό το test δεν θέλουμε payment validation
                message01.DeliveryMethod = DeliveryMethod.All;
                message01.IsDeliveryMethodOK = true;
                message01.Subject = "Πάρτε μέρος στην έρευνα!";
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);

                //O collector έχει status New, έχει recipients και έχει 1 DRAFT message:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId, collector01.TextsLanguage);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 500);
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);

                //Χρονοπρογραμματίζουμε το message:
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = systemManager.ConvertTimeToUtc(_scheduleAt);



                //ΔΕΝ ΜΠΟΡΟΥΜΕ ΝΑ ΧΡΟΝΟΠΡΟΓΡΑΜΜΑΤΙΣΟΥΜΕ ΤΟ MESSAGE, ΕΑΝ ΔΕΜ ΦΕΡΕΙ Ο COLLECTOR ΠΛΗΡΩΜΕΣ:
                _EXECUTEAndCATCHType(delegate { surveyManager.ScheduleMessage(message01, _scheduleAt, true, validatePayment: true, scheduleDtOffset: -1); }, typeof(VLException));


                //Πρέπει να συνδεσω το collector με πληρωμές, για το ίδιο CreditType:
                var collectorPayment1 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment1.PaymentId);
                var collectorPayment2 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment2.PaymentId);
                var collectorPayment3 = systemManager.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId);


                //ΧΡΟΝΟΠΡΟΓΡΑΜΜΑΤΙΖΟΥΜΕ το μήνυμα
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, true, validatePayment: true, scheduleDtOffset: -1);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);



            }
            finally
            {
                #region clean up
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;


                    surveyManager.UnitTesting_DestroySurvey(item.SurveyId);
                }

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
                #endregion
            }
        }

    }
}
