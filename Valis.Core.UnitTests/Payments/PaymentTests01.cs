using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Valis.Core.UnitTests.Payments
{
    [TestClass]
    public class PaymentTests01 : AdminBaseClass
    {

        [TestMethod, Description("CRUD tests for VLPayment")]
        public void PaymentTests01_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsNotNull(client1);
                Assert.IsTrue(client1.Profile == BuiltinProfiles.UTESTPaid.ProfileId);
                #endregion

                //Στην αρχή ο πελάτης δεν έχει καμμία πληρωμή στο σύστημάς μας:
                Assert.IsTrue(systemManager.GetPayments(client1.ClientId).Count == 0);
                Assert.IsTrue(systemManager.GetPaymentsCount(client1.ClientId) == 0);

                //Δημιουργούμε ένα payment:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 2000);
                Assert.IsNotNull(payment1);
                Assert.IsTrue(payment1.Client == client1.ClientId);
                Assert.IsTrue(payment1.PaymentType == PaymentType.Default);
                Assert.IsNull(payment1.Comment);
                Assert.IsTrue(payment1.IsActive);
                Assert.IsTrue(payment1.CreditType == CreditType.EmailType);
                Assert.IsTrue(payment1.Quantity == 2000);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsFalse(payment1.IsTimeLimited);
                Assert.IsNull(payment1.CustomCode1);
                Assert.IsNull(payment1.CustomCode2);
                var svdPayment1 = systemManager.GetPaymentById(payment1.PaymentId);
                Assert.AreEqual<VLPayment>(payment1, svdPayment1);

                //τώρα ο πελάτης μας έχει μία (1) πληρωμή
                Assert.IsTrue(systemManager.GetPayments(client1.ClientId).Count == 1);
                Assert.IsTrue(systemManager.GetPaymentsCount(client1.ClientId) == 1);

                //Κάνουμε update
                payment1.CustomCode1 = "134eqwde3525244234";
                payment1.CustomCode2 = "!#$13#$%#$@#REFRQ";
                payment1.CreditType = CreditType.ClickType;
                payment1.Quantity = 456;
                payment1 = systemManager.UpdatePayment(payment1);
                Assert.IsNotNull(payment1);
                Assert.IsTrue(payment1.Client == client1.ClientId);
                Assert.IsTrue(payment1.PaymentType == PaymentType.Default);
                Assert.IsNull(payment1.Comment);
                Assert.IsTrue(payment1.IsActive);
                Assert.IsTrue(payment1.CreditType == CreditType.ClickType);
                Assert.IsTrue(payment1.Quantity == 456);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsFalse(payment1.IsTimeLimited);
                Assert.IsTrue(payment1.CustomCode1 == "134eqwde3525244234");
                Assert.IsTrue(payment1.CustomCode2 == "!#$13#$%#$@#REFRQ");


                //Δημιουργούμε ένα ακόμα payment:
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 2000);
                Assert.IsNotNull(payment2);


                //τώρα ο πελάτης μας έχει δύο (2) πληρωμές
                Assert.IsTrue(systemManager.GetPayments(client1.ClientId).Count == 2);
                Assert.IsTrue(systemManager.GetPaymentsCount(client1.ClientId) == 2);


                //Διαγράφουμε μία πληρωμή
                systemManager.DeletePayment(payment1);
                Assert.IsNull(systemManager.GetPaymentById(payment1.PaymentId));
                Assert.IsNotNull(systemManager.GetPaymentById(payment2.PaymentId));

                //τώρα ο πελάτης μας έχει μία (1) πληρωμή
                Assert.IsTrue(systemManager.GetPayments(client1.ClientId).Count == 1);
                Assert.IsTrue(systemManager.GetPaymentsCount(client1.ClientId) == 1);
            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }
        }



        [TestMethod, Description("CRUD tests for VLPayment")]
        public void PaymentTests01_02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia1 S.A.", BuiltinCountries.Greece, "man1", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsNotNull(client1);
                Assert.IsTrue(client1.Profile == BuiltinProfiles.UTESTPaid.ProfileId);

                var user1 = systemManager.CreateClientAccount(client1.ClientId, "f1", "l1", BuiltinRoles.PowerClient.RoleId, "f1l1@hotmail.com", "ltoken1", "passwd1");
                Assert.IsNotNull(user1);

                var account1 = valisSystem.LogOnUser("ltoken1", "passwd1");
                Assert.IsNotNull(account1);

                var systemManager1 = VLSystemManager.GetAnInstance(account1);
                #endregion

                #region Δημιουργούμε έναν πελάτη:
                var client2 = systemManager.CreateClient("MySoftavia2 S.A.", BuiltinCountries.Greece, "man2", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsNotNull(client2);
                Assert.IsTrue(client2.Profile == BuiltinProfiles.UTESTPaid.ProfileId);

                var user2 = systemManager.CreateClientAccount(client2.ClientId, "f2", "l2", BuiltinRoles.PowerClient.RoleId, "f2l2@hotmail.com", "ltoken2", "passwd2");
                Assert.IsNotNull(user2);

                var account2 = valisSystem.LogOnUser("ltoken2", "passwd2");
                Assert.IsNotNull(account2);

                var systemManager2 = VLSystemManager.GetAnInstance(account2);
                #endregion

                Assert.IsTrue(systemManager1.GetPayments(client1.ClientId).Count == 0);
                Assert.IsTrue(systemManager2.GetPayments(client2.ClientId).Count == 0);

                _EXECUTEAndCATCHType(delegate { systemManager1.GetPayments(client2.ClientId); }, typeof(VLAccessDeniedException));
                _EXECUTEAndCATCHType(delegate { systemManager2.GetPayments(client1.ClientId); }, typeof(VLAccessDeniedException));

                _EXECUTEAndCATCHType(delegate { systemManager1.GetPaymentsCount(client2.ClientId); }, typeof(VLAccessDeniedException));
                _EXECUTEAndCATCHType(delegate { systemManager2.GetPaymentsCount(client1.ClientId); }, typeof(VLAccessDeniedException));

                //Φτιάχνω απο μία πληρωμή για τον κάθε πελάτη:

                _EXECUTEAndCATCHType(delegate { systemManager1.AddPayment(client1.ClientId, CreditType.EmailType, 2000); }, typeof(VLAccessDeniedException));
                _EXECUTEAndCATCHType(delegate { systemManager2.AddPayment(client2.ClientId, CreditType.ClickType, 12000); }, typeof(VLAccessDeniedException));

                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 2000);
                Assert.IsTrue(payment1.Client == client1.ClientId);
                var payment2 = systemManager.AddPayment(client2.ClientId, CreditType.ClickType, 12000);
                Assert.IsTrue(payment2.Client == client2.ClientId);

                var svdPayment1 = systemManager1.GetPaymentById(payment1.PaymentId);
                Assert.AreEqual<VLPayment>(payment1, svdPayment1);
                var svdPayment2 = systemManager2.GetPaymentById(payment2.PaymentId);
                Assert.AreEqual<VLPayment>(payment2, svdPayment2);


                _EXECUTEAndCATCHType(delegate { systemManager1.GetPaymentById(payment2.PaymentId); }, typeof(VLAccessDeniedException));
                _EXECUTEAndCATCHType(delegate { systemManager2.GetPaymentById(payment1.PaymentId); }, typeof(VLAccessDeniedException));


                _EXECUTEAndCATCHType(delegate { systemManager1.DeletePayment(payment1.PaymentId); }, typeof(VLAccessDeniedException));
                _EXECUTEAndCATCHType(delegate { systemManager2.DeletePayment(payment2.PaymentId); }, typeof(VLAccessDeniedException));
            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }
        }



        [TestMethod, Description("CRUD tests for VLCollectorPayment (Charges)")]
        public void PaymentTests01_03()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia1 S.A.", BuiltinCountries.Greece, "man1", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsNotNull(client1);
                Assert.IsTrue(client1.Profile == BuiltinProfiles.UTESTPaid.ProfileId);

                var user1 = systemManager.CreateClientAccount(client1.ClientId, "f1", "l1", BuiltinRoles.PowerClient.RoleId, "f1l1@hotmail.com", "ltoken1", "passwd1");
                Assert.IsNotNull(user1);

                var account1 = valisSystem.LogOnUser("ltoken1", "passwd1");
                Assert.IsNotNull(account1);

                var systemManager1 = VLSystemManager.GetAnInstance(account1);
                var surveyManager1 = VLSurveyManager.GetAnInstance(account1);
                #endregion


                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.ClickType, 10);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.ResponseType, 20);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 12);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 19);
                Assert.IsNotNull(payment4);


                //We create a survey
                var survey1 = surveyManager1.CreateSurvey(client1.ClientId, "Employee Survey #1");
                Assert.IsNotNull(survey1);

                //Δημιουργούμε ένα collector (Αυτόματα παίρνουν τιμές τα πεδία που αφορούν την χρέωση του Collector):
                var collector01 = surveyManager1.CreateCollector(survey1.SurveyId, CollectorType.Email, "Collector01-Email", creditType: CreditType.EmailType);
                Assert.IsNotNull(collector01);
                Assert.IsTrue(collector01.UseCredits);
                Assert.IsTrue(collector01.CreditType == CreditType.EmailType);
                Assert.IsNull(collector01.FirstChargeDt);
                Assert.IsNull(collector01.LastChargeDt);
                Assert.IsTrue(collector01.Profile == client1.Profile);
                Assert.IsTrue(collector01.Status == CollectorStatus.New);

                //We create a collector (Αυτόματα παίρνουν τιμές τα πεδία που αφορούν την χρέωση του Collector):
                var collector02 = surveyManager1.CreateCollector(survey1.SurveyId, CollectorType.Email, "collector02-Email", creditType: CreditType.EmailType);
                Assert.IsNotNull(collector02);
                Assert.IsTrue(collector02.UseCredits);
                Assert.IsTrue(collector02.CreditType == CreditType.EmailType);
                Assert.IsNull(collector02.FirstChargeDt);
                Assert.IsNull(collector02.LastChargeDt);
                Assert.IsTrue(collector02.Profile == client1.Profile);
                Assert.IsTrue(collector02.Status == CollectorStatus.New);


                //Τώρα ο collector01 δεν έχει συνδεδεμένη καμμιά πληρωμή:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector01.CollectorId).Count == 0);
                //Τώρα ο collector02 δεν έχει συνδεδεμένη καμμιά πληρωμή:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector02.CollectorId).Count == 0);

                //Δεν μπορούμε να συνδέσουμε πληρωμή με άσχετο CreditType:
                _EXECUTEAndCATCHType(delegate { systemManager1.AddPaymentToCollector(collector01.CollectorId, payment1.PaymentId); }, typeof(VLException));
                //Δεν μπορούμε να συνδέσουμε πληρωμή με άσχετο CreditType:
                _EXECUTEAndCATCHType(delegate { systemManager1.AddPaymentToCollector(collector01.CollectorId, payment2.PaymentId); }, typeof(VLException));

                //Συνδέουμε μία πληρωμή με τον collector:
                var collectorPayment01 = systemManager1.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId);
                Assert.IsNotNull(collectorPayment01);
                Assert.IsTrue(collectorPayment01.Collector == collector01.CollectorId);
                Assert.IsTrue(collectorPayment01.Payment == payment3.PaymentId);
                Assert.IsTrue(collectorPayment01.UseOrder == 1);
                Assert.IsNull(collectorPayment01.QuantityLimit);
                Assert.IsTrue(collectorPayment01.QuantityUsed == 0);
                Assert.IsNull(collectorPayment01.FirstChargeDt);
                Assert.IsNull(collectorPayment01.LastChargeDt);
                Assert.IsTrue(collectorPayment01.IsActive);
                Assert.IsFalse(collectorPayment01.IsUsed);
                var svdCollectorPaymeny01 = systemManager1.GetCollectorPaymentById(collectorPayment01.CollectorPaymentId);
                Assert.AreEqual<VLCollectorPayment>(collectorPayment01, svdCollectorPaymeny01);

                //Τώρα υπάρχει ένα payment συνδεδέμένο με τον collector01:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector01.CollectorId).Count == 1);
                Assert.IsTrue(systemManager1.GetSurveyPayments(survey1.SurveyId).Count == 1);
                //Τώρα ο collector02 δεν έχει συνδεδεμένη καμμιά πληρωμή:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector02.CollectorId).Count == 0);


                //ΔΕΝ ΜΠΟΡΟΥΜΕ ΝΑ ΣΥΝΔΕΣΟΥΜΕ ΤΗΝ ΙΔΙΑ ΠΛΗΡΩΜΗ ΜΕ ΤΟΝ ΙΔΙΟ COLLECTOR για 2η φορά:
                _EXECUTEAndCATCHType(delegate { systemManager1.AddPaymentToCollector(collector01.CollectorId, payment3.PaymentId); }, typeof(VLException));

                //Συνδέουμε άλλη μία πληρωμή με τον collector:
                var collectorPayment02 = systemManager1.AddPaymentToCollector(collector01.CollectorId, payment4.PaymentId);
                Assert.IsNotNull(collectorPayment02);
                Assert.IsTrue(collectorPayment02.Collector == collector01.CollectorId);
                Assert.IsTrue(collectorPayment02.Payment == payment4.PaymentId);
                Assert.IsTrue(collectorPayment02.UseOrder == 2);
                Assert.IsNull(collectorPayment02.QuantityLimit);
                Assert.IsTrue(collectorPayment02.QuantityUsed == 0);
                Assert.IsNull(collectorPayment02.FirstChargeDt);
                Assert.IsNull(collectorPayment02.LastChargeDt);
                Assert.IsTrue(collectorPayment02.IsActive);
                Assert.IsFalse(collectorPayment02.IsUsed);
                var svdCollectorPaymeny02 = systemManager1.GetCollectorPaymentById(collectorPayment02.CollectorPaymentId);
                Assert.AreEqual<VLCollectorPayment>(collectorPayment02, svdCollectorPaymeny02);

                //Τώρα υπάρχει δύο payments συνδεδέμένο με τον collector01:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector01.CollectorId).Count == 2);
                Assert.IsTrue(systemManager1.GetSurveyPayments(survey1.SurveyId).Count == 2);
                //Τώρα ο collector02 δεν έχει συνδεδεμένη καμμιά πληρωμή:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector02.CollectorId).Count == 0);


                //Δεν μπορούμε να διαγράψουμε το payment3, διότι είναι συνδεδεμένο με τον collector01:
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment3.PaymentId); }, typeof(VLException));
                //Δεν μπορούμε να διαγράψουμε το payment4, διότι είναι συνδεδεμένο με τον collector01:
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment4.PaymentId); }, typeof(VLException));

                //Απενεργοποιούμε το collectorPayment01:
                collectorPayment01 = systemManager1.DeactivateCollectorPayment(collectorPayment01.CollectorPaymentId);
                Assert.IsFalse(collectorPayment01.IsActive);
                svdCollectorPaymeny01 = systemManager1.GetCollectorPaymentById(collectorPayment01.CollectorPaymentId);
                Assert.IsFalse(svdCollectorPaymeny01.IsActive);
                Assert.AreEqual<VLCollectorPayment>(collectorPayment01, svdCollectorPaymeny01);

                //Ενεργοποιούμε το collectorPayment01:
                collectorPayment01 = systemManager1.ActivateCollectorPayment(collectorPayment01.CollectorPaymentId);
                Assert.IsTrue(collectorPayment01.IsActive);
                svdCollectorPaymeny01 = systemManager1.GetCollectorPaymentById(collectorPayment01.CollectorPaymentId);
                Assert.IsTrue(svdCollectorPaymeny01.IsActive);
                Assert.AreEqual<VLCollectorPayment>(collectorPayment01, svdCollectorPaymeny01);


                //Οσο δεν έχει χρησιμοποιηθεί ένα payment μπορώ να το αφαιρέσω απο ένα collector:
                systemManager1.RemovePaymentFromCollector(collector01.CollectorId, payment3.PaymentId);
                Assert.IsNull(systemManager1.GetCollectorPaymentById(collectorPayment01.CollectorPaymentId));


                //Τώρα υπάρχει ένα payment συνδεδέμένο με τον collector01:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector01.CollectorId).Count == 1);
                Assert.IsTrue(systemManager1.GetSurveyPayments(survey1.SurveyId).Count == 1);
                //Τώρα ο collector02 δεν έχει συνδεδεμένη καμμιά πληρωμή:
                Assert.IsTrue(systemManager1.GetCollectorPayments(collector02.CollectorId).Count == 0);

                collectorPayment02 = systemManager1.GetCollectorPaymentById(collectorPayment02.CollectorPaymentId);
                Assert.IsTrue(collectorPayment02.UseOrder == 1);
                svdCollectorPaymeny02 = systemManager1.GetCollectorPaymentByCollectorAndPayment(collector01.CollectorId, payment4.PaymentId);
                Assert.AreEqual<VLCollectorPayment>(collectorPayment02, svdCollectorPaymeny02);


            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

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


        [TestMethod, Description("Payments & Messages Part1")]
        public void PaymentTests01_04()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργία πελάτη, και λίστας με 10 επαφές:
                var client1 = systemManager.CreateClient("MySoftavia1 S.A.", BuiltinCountries.Greece, "man1", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                var user1 = systemManager.CreateClientAccount(client1.ClientId, "f1", "l1", BuiltinRoles.PowerClient.RoleId, "f1l1@hotmail.com", "ltoken1", "passwd1");

                var account1 = valisSystem.LogOnUser("ltoken1", "passwd1");
                Assert.IsNotNull(account1);

                var systemManager1 = VLSystemManager.GetAnInstance(account1);
                var surveyManager1 = VLSurveyManager.GetAnInstance(account1);

                //We create a contact list:
                var list01 = systemManager1.CreateClientList(client1.ClientId, "my-list01");
                //εισαγωγή 10 γραμμών:
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.UnitTest);
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                #endregion

                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 6);
                Assert.IsNotNull(payment1);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 6);
                Assert.IsNotNull(payment2);
                var payment3 = systemManager.AddPayment(client1.ClientId, CreditType.ClickType, 60);
                Assert.IsNotNull(payment3);
                var payment4 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 60);
                Assert.IsNotNull(payment4);
                //We create a survey
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Employee Survey #1");
                //We create a collector:
                var collector1 = surveyManager.CreateCollector(survey1, CollectorType.Email, "Collector01-Email", CreditType.EmailType);
                //O collector έχει status New, δεν έχει recipients, δεν έχει messages, δεν έχει πληρωμές:
                Assert.IsTrue(collector1.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector1).Count == 0);
                Assert.IsTrue(surveyManager.GetMessages(collector1).Count == 0);
                Assert.IsTrue(systemManager.GetChargesForCollector(collector1.CollectorId).Count == 0);
                Assert.IsTrue(systemManager.GetCollectorPayments(collector1.CollectorId).Count == 0);

                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector1, list01);
                surveyManager.ImportRecipientsFromString(collector1.CollectorId, "milonakis@hotail.com, George, Milonakis", ContactImportOptions.Default);
                //Δημιουργούμε ένα Draft message στον collector:
                var message01 = surveyManager.CreateMessage(collector1, "gmilonakis@thetest12.gr", "Welcome to my Dream!", "Hello [RemoveLink]George[SurveyLink]!");
                message01.IsDeliveryMethodOK = true;
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);

                Assert.IsTrue(message01.Status == MessageStatus.Draft);
                //O collector έχει status new, έχει 11 recipients και 1 DRAFT message, και καμμία χρέωση:
                collector1 = surveyManager.GetCollectorById(collector1.CollectorId, collector1.TextsLanguage);
                Assert.IsTrue(collector1.Status == CollectorStatus.New);
                Assert.IsTrue(surveyManager.GetRecipients(collector1).Count == 11);
                Assert.IsTrue(surveyManager.GetMessages(collector1).Count == 1);
                Assert.IsTrue(systemManager.GetChargesForCollector(collector1.CollectorId).Count == 0);
                Assert.IsTrue(systemManager.GetCollectorPayments(collector1.CollectorId).Count == 0);



                //Δεν μπορούμε να χρονοπρογραμματίσουμε το message (διότι ο collector δεν έχει συνδεθεί με κάποιο payment):
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = surveyManager.ConvertTimeToUtc(_scheduleAt);
                _EXECUTEAndCATCHType(delegate { surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1); }, typeof(VLPaymentException));

                //Πρέπει να συνδέσουμε στον collector πληρωμή σωστού τύπου:
                _EXECUTEAndCATCHType(delegate { systemManager.AddPaymentToCollector(collector1.CollectorId, payment3.PaymentId); }, typeof(VLInvalidPaymentException));
                //Πρέπει το QuantityLimit να μην ξεπερνάει το σύνολο των διαθέσιμων credits:
                _EXECUTEAndCATCHType(delegate { systemManager.AddPaymentToCollector(collector1.CollectorId, payment1.PaymentId, quantityLimit: 7); }, typeof(VLInvalidPaymentException));



                //Συνδέουμε τον collector με μία πληρωμή:
                var collectorpayment01 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment1.PaymentId);
                Assert.IsTrue(systemManager.GetCollectorPayments(collector1.CollectorId).Count == 1);
                //Δεν μπορούμε να χρονοπρογραμματίσουμε το message (μας λείπει ένα email credit):
                _EXECUTEAndCATCHType(delegate { surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1); }, typeof(VLPaymentException));

                //Αφαιρούμε τις πληρωμές:
                systemManager.RemovePaymentFromCollector(collectorpayment01);
                Assert.IsTrue(systemManager.GetCollectorPayments(collector1.CollectorId).Count == 0);

                //Συνδέουμε τον collector με μία πληρωμή:
                collectorpayment01 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment4.PaymentId, quantityLimit: 10);
                Assert.IsTrue(systemManager.GetCollectorPayments(collector1.CollectorId).Count == 1);
                //Δεν μπορούμε να χρονοπρογραμματίσουμε το message (μας λείπει ένα email credit):
                _EXECUTEAndCATCHType(delegate { surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1); }, typeof(VLPaymentException));

                //Αφαιρούμε τις πληρωμές:
                systemManager.RemovePaymentFromCollector(collectorpayment01);
                Assert.IsTrue(systemManager.GetCollectorPayments(collector1.CollectorId).Count == 0);

                //Συνδέουμε τον collector με δύο πληρωμές:
                collectorpayment01 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment1.PaymentId);
                var collectorpayment02 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment2.PaymentId);
                //Τωρα μπορούμε να χρονοπρογραμματίσουμε το collector:
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1);
                collector1 = surveyManager.GetCollectorById(collector1.CollectorId, collector1.TextsLanguage);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);
                Assert.IsTrue(collector1.Status == CollectorStatus.Open);

                //τώρα ελέγχουμε τα ατοιμασμένα message recipients:
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                Assert.IsTrue(messageRecipients.Count == 11);
                Int32 P1 = 0; Int32 P2 = 0;
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsTrue(mr.IsCharged == false);
                    Assert.IsNull(mr.Error);
                    Assert.IsNotNull(mr.CollectorPayment);
                    if (mr.CollectorPayment == collectorpayment01.CollectorPaymentId)
                        P1++;
                    else if (mr.CollectorPayment == collectorpayment02.CollectorPaymentId)
                        P2++;
                }
                Assert.IsTrue(P1 == 6);
                Assert.IsTrue(P2 == 5);

                //Tα ποσά έχουν δεσμευτεί επάνω στους CollectorPayments:
                collectorpayment01 = systemManager.GetCollectorPaymentById(collectorpayment01.CollectorPaymentId);
                collectorpayment02 = systemManager.GetCollectorPaymentById(collectorpayment02.CollectorPaymentId);
                Assert.IsTrue(collectorpayment01.QuantityReserved == 6);
                Assert.IsTrue(collectorpayment01.QuantityUsed == 0);
                Assert.IsTrue(collectorpayment02.QuantityReserved == 5);
                Assert.IsTrue(collectorpayment02.QuantityUsed == 0);
                //αλλαγές δεν έχουν γίνει ακόμα επάνω στις εγγραφές των πληρωμών:
                payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                payment2 = systemManager.GetPaymentById(payment2.PaymentId);
                Assert.IsTrue(payment1.Quantity == 6);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsTrue(payment1.IsActive);
                Assert.IsTrue(payment2.Quantity == 6);
                Assert.IsTrue(payment2.QuantityUsed == 0);
                Assert.IsTrue(payment2.IsActive);

                //Δεν μπορούμε να αφαιρέσουμε τις πληρωμές απο τους collectors:
                _EXECUTEAndCATCHType(delegate { systemManager.RemovePaymentFromCollector(collectorpayment01.CollectorPaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.RemovePaymentFromCollector(collectorpayment02.CollectorPaymentId); }, typeof(VLException));

                //Δεν μπορούμε να διαγράψουμε ούτε τις εγγραφές των πληρωμών:
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment1.PaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment2.PaymentId); }, typeof(VLException));


            }
            finally
            {
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
            }
        }



        [TestMethod, Description("Payments & Messages Part2")]
        public void PaymentTests01_05()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργία πελάτη, και λίστας με 10 επαφές:
                var client1 = systemManager.CreateClient("MySoftavia1 S.A.", BuiltinCountries.Greece, "man1", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                var user1 = systemManager.CreateClientAccount(client1.ClientId, "f1", "l1", BuiltinRoles.PowerClient.RoleId, "f1l1@hotmail.com", "ltoken1", "passwd1");

                var account1 = valisSystem.LogOnUser("ltoken1", "passwd1");
                Assert.IsNotNull(account1);

                var systemManager1 = VLSystemManager.GetAnInstance(account1);
                var surveyManager1 = VLSurveyManager.GetAnInstance(account1);

                //We create a contact list:
                var list01 = systemManager1.CreateClientList(client1.ClientId, "my-list01");
                //εισαγωγή 10 γραμμών:
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.UnitTest);
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                #endregion

                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 6);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 6);
                //We create a survey
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Employee Survey #1");
                //We create a collector:
                var collector1 = surveyManager.CreateCollector(survey1, CollectorType.Email, "Collector01-Email", CreditType.EmailType);

                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector1, list01);
                surveyManager.ImportRecipientsFromString(collector1.CollectorId, "milonakis@hotail.com, George, Milonakis", ContactImportOptions.Default);
                //Δημιουργούμε ένα Draft message στον collector:
                var message01 = surveyManager.CreateMessage(collector1, "gmilonakis@thetest12.gr", "Welcome to my Dream!", "Hello [RemoveLink]George[SurveyLink]!");
                message01.IsDeliveryMethodOK = true;
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);


                //Δεν μπορούμε να χρονοπρογραμματίσουμε το message (διότι ο collector δεν έχει συνδεθεί με κάποιο payment):
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = surveyManager.ConvertTimeToUtc(_scheduleAt);

                //Συνδέουμε τον collector με δύο πληρωμές:
                var collectorpayment01 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment1.PaymentId);
                var collectorpayment02 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment2.PaymentId);
                //Τωρα μπορούμε να χρονοπρογραμματίσουμε το collector:
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1);
                collector1 = surveyManager.GetCollectorById(collector1.CollectorId, collector1.TextsLanguage);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);
                Assert.IsTrue(collector1.Status == CollectorStatus.Open);




                //Πρώτο βήμα αποστολής:
                message01 = surveyManager.GetNextPendingMessage(minuteOffset: 2);
                Assert.IsNotNull(message01);
                Assert.IsTrue(surveyManager.ValidatePaymentForMessage(message01, true));
                message01 = surveyManager.PromoteMessageToPreparedStatus(message01);
                Assert.IsNotNull(message01);



                //Δεύτερο βήμα αποστολής:
                #region ΑΠΟΣΤΟΛΗ EMAIL (όπως το κάνει αυτή την στιγμή ο TheMailler):
                message01 = surveyManager.GetNextPreparedMessage();
                Assert.IsNotNull(message01);
                Collection<VLRecipient> recipients = surveyManager.GetRecipientsForMessage(message01);
                foreach (var recipient in recipients)
                {
                    VLMessageRecipient messageRecipient = surveyManager.GetMessageRecipientById(message01.MessageId, recipient.RecipientId);
                    messageRecipient.SendDT = Utility.UtcNow();

                    /*Στέλνουμε μόνο όσα είναι σε status Pending:*/
                    Assert.IsTrue(messageRecipient.Status == MessageRecipientStatus.Pending);

                    Assert.IsTrue(systemManager.ChargePaymentForEmail(messageRecipient.CollectorPayment.Value, collector1.CollectorId, message01.MessageId, recipient.RecipientId));
                    messageRecipient.IsCharged = true;
                    message01.SentCounter++;
                    messageRecipient.ErrorCount = 0;
                    messageRecipient.Status = MessageRecipientStatus.Sent;
                    messageRecipient = surveyManager.UpdateMessageRecipient(messageRecipient);

                    recipient.IsSentEmail = true;
                    var updatedRecipient = surveyManager.UpdateRecipientIntl(recipient);
                }
                message01 = surveyManager.PromoteMessageToExecutedStatus(message01);
                collector1.HasSentEmails = true;
                collector1 = surveyManager.UpdateCollector(collector1);
                #endregion




                //τώρα ελέγχουμε τα ατοιμασμένα message recipients:
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                foreach (var mr in messageRecipients)
                {
                    Assert.IsTrue(mr.ErrorCount == 0);
                    Assert.IsTrue(mr.IsCharged);
                    Assert.IsNull(mr.Error);
                }

                //Tα ποσά έχουν δεσμευτεί επάνω στους CollectorPayments:
                collectorpayment01 = systemManager.GetCollectorPaymentById(collectorpayment01.CollectorPaymentId);
                collectorpayment02 = systemManager.GetCollectorPaymentById(collectorpayment02.CollectorPaymentId);
                Assert.IsTrue(collectorpayment01.QuantityReserved == 0);
                Assert.IsTrue(collectorpayment01.QuantityUsed == 6);
                Assert.IsTrue(collectorpayment02.QuantityReserved == 0);
                Assert.IsTrue(collectorpayment02.QuantityUsed == 5);
                //αλλαγές δεν έχουν γίνει ακόμα επάνω στις εγγραφές των πληρωμών:
                payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                payment2 = systemManager.GetPaymentById(payment2.PaymentId);
                Assert.IsTrue(payment1.Quantity == 6);
                Assert.IsTrue(payment1.QuantityUsed == 6);
                Assert.IsFalse(payment1.IsActive);
                Assert.IsTrue(payment2.Quantity == 6);
                Assert.IsTrue(payment2.QuantityUsed == 5);
                Assert.IsTrue(payment2.IsActive);


                //Δεν μπορούμε να αφαιρέσουμε τις πληρωμές απο τους collectors:
                _EXECUTEAndCATCHType(delegate { systemManager.RemovePaymentFromCollector(collectorpayment01.CollectorPaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.RemovePaymentFromCollector(collectorpayment02.CollectorPaymentId); }, typeof(VLException));

                //Δεν μπορούμε να διαγράψουμε ούτε τις εγγραφές των πληρωμών:
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment1.PaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment2.PaymentId); }, typeof(VLException));


            }
            finally
            {
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
            }
        }


        [TestMethod, Description("Payments & Messages Part3")]
        public void PaymentTests01_06()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργία πελάτη, και λίστας με 10 επαφές:
                var client1 = systemManager.CreateClient("MySoftavia1 S.A.", BuiltinCountries.Greece, "man1", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                var user1 = systemManager.CreateClientAccount(client1.ClientId, "f1", "l1", BuiltinRoles.PowerClient.RoleId, "f1l1@hotmail.com", "ltoken1", "passwd1");

                var account1 = valisSystem.LogOnUser("ltoken1", "passwd1");
                Assert.IsNotNull(account1);

                var systemManager1 = VLSystemManager.GetAnInstance(account1);
                var surveyManager1 = VLSurveyManager.GetAnInstance(account1);

                //We create a contact list:
                var list01 = systemManager1.CreateClientList(client1.ClientId, "my-list01");
                //εισαγωγή 10 γραμμών:
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, ContactImportOptions.UnitTest);
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                #endregion

                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 6);
                var payment2 = systemManager.AddPayment(client1.ClientId, CreditType.EmailType, 6);
                //We create a survey
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Employee Survey #1");
                //We create a collector:
                var collector1 = surveyManager.CreateCollector(survey1, CollectorType.Email, "Collector01-Email", CreditType.EmailType);

                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector1, list01);
                surveyManager.ImportRecipientsFromString(collector1.CollectorId, "milonakis@hotail.com, George, Milonakis", ContactImportOptions.Default);
                //Δημιουργούμε ένα Draft message στον collector:
                var message01 = surveyManager.CreateMessage(collector1, "gmilonakis@thetest12.gr", "Welcome to my Dream!", "Hello [RemoveLink]George[SurveyLink]!");
                message01.IsDeliveryMethodOK = true;
                message01.IsContentOK = true;
                message01 = surveyManager.UpdateMessage(message01);


                //Δεν μπορούμε να χρονοπρογραμματίσουμε το message (διότι ο collector δεν έχει συνδεθεί με κάποιο payment):
                DateTime _scheduleAt = DateTime.Now;
                DateTime _scheduleAt_UTC = surveyManager.ConvertTimeToUtc(_scheduleAt);

                //Συνδέουμε τον collector με δύο πληρωμές:
                var collectorpayment01 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment1.PaymentId);
                var collectorpayment02 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment2.PaymentId);
                //Τωρα μπορούμε να χρονοπρογραμματίσουμε το collector:
                message01 = surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1);
                collector1 = surveyManager.GetCollectorById(collector1.CollectorId, collector1.TextsLanguage);
                Assert.IsTrue(message01.Status == MessageStatus.Pending);
                Assert.IsTrue(collector1.Status == CollectorStatus.Open);




                //τώρα ελέγχουμε τα ατοιμασμένα message recipients:
                var messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                Assert.IsTrue(messageRecipients.Count == 11);

                //Tα ποσά έχουν δεσμευτεί επάνω στους CollectorPayments:
                collectorpayment01 = systemManager.GetCollectorPaymentById(collectorpayment01.CollectorPaymentId);
                collectorpayment02 = systemManager.GetCollectorPaymentById(collectorpayment02.CollectorPaymentId);
                Assert.IsTrue(collectorpayment01.QuantityReserved == 6);
                Assert.IsTrue(collectorpayment01.QuantityUsed == 0);
                Assert.IsFalse(collectorpayment01.IsUsed);
                Assert.IsTrue(collectorpayment02.QuantityReserved == 5);
                Assert.IsTrue(collectorpayment02.QuantityUsed == 0);
                Assert.IsFalse(collectorpayment02.IsUsed);
                //αλλαγές δεν έχουν γίνει ακόμα επάνω στις εγγραφές των πληρωμών:
                payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                payment2 = systemManager.GetPaymentById(payment2.PaymentId);
                Assert.IsTrue(payment1.Quantity == 6);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsTrue(payment1.IsActive);
                Assert.IsTrue(payment2.Quantity == 6);
                Assert.IsTrue(payment2.QuantityUsed == 0);
                Assert.IsTrue(payment2.IsActive);

                //Δεν μπορούμε να αφαιρέσουμε τις πληρωμές απο τους collectors:
                _EXECUTEAndCATCHType(delegate { systemManager.RemovePaymentFromCollector(collectorpayment01.CollectorPaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.RemovePaymentFromCollector(collectorpayment02.CollectorPaymentId); }, typeof(VLException));

                //Δεν μπορούμε να διαγράψουμε ούτε τις εγγραφές των πληρωμών:
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment1.PaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment2.PaymentId); }, typeof(VLException));




                //τώρα κάνουμε unschedule το message:
                message01 = surveyManager.UnScheduleMessage(message01.MessageId);
                Assert.IsTrue(message01.Status == MessageStatus.Draft);

                messageRecipients = surveyManager.GetMessageRecipients(message01.MessageId);
                Assert.IsTrue(messageRecipients.Count == 0);

                //Tα ποσά έχουν ελευθερωθεί επάνω στους CollectorPayments:
                collectorpayment01 = systemManager.GetCollectorPaymentById(collectorpayment01.CollectorPaymentId);
                collectorpayment02 = systemManager.GetCollectorPaymentById(collectorpayment02.CollectorPaymentId);
                Assert.IsTrue(collectorpayment01.QuantityReserved == 0);
                Assert.IsTrue(collectorpayment01.QuantityUsed == 0);
                Assert.IsFalse(collectorpayment01.IsUsed);
                Assert.IsTrue(collectorpayment02.QuantityReserved == 0);
                Assert.IsTrue(collectorpayment02.QuantityUsed == 0);
                Assert.IsFalse(collectorpayment02.IsUsed);
                //αλλαγές δεν έχουν γίνει ακόμα επάνω στις εγγραφές των πληρωμών:
                payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                payment2 = systemManager.GetPaymentById(payment2.PaymentId);
                Assert.IsTrue(payment1.Quantity == 6);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsTrue(payment1.IsActive);
                Assert.IsTrue(payment2.Quantity == 6);
                Assert.IsTrue(payment2.QuantityUsed == 0);
                Assert.IsTrue(payment2.IsActive);



                //Δεν μπορούμε να διαγράψουμε ούτε τις εγγραφές των πληρωμών:
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment1.PaymentId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeletePayment(payment2.PaymentId); }, typeof(VLException));

                //Μπορούμε να αφαιρέσουμε τις πληρωμές απο τους collectors:
                systemManager.RemovePaymentFromCollector(collectorpayment01.CollectorPaymentId);
                systemManager.RemovePaymentFromCollector(collectorpayment02.CollectorPaymentId);

                systemManager.DeletePayment(payment1.PaymentId);
                systemManager.DeletePayment(payment2.PaymentId);


                _EXECUTEAndCATCHType(delegate { surveyManager.ScheduleMessage(message01, _scheduleAt, scheduleDtOffset: -1); }, typeof(VLPaymentException));
            }
            finally
            {
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
            }
        }



        [TestMethod, Description("Payments & Clicks")]
        public void PaymentTests01_07()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργία πελάτη
                var client1 = systemManager.CreateClient("MySoftavia1 S.A.", BuiltinCountries.Greece, "man1", profile: BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                //Δημιουργούμε πληρωμές σε αυτόν τον πελάτη:
                var payment1 = systemManager.AddPayment(client1.ClientId, CreditType.ClickType, 2);
                Assert.IsTrue(payment1.CreditType == CreditType.ClickType);
                Assert.IsTrue(payment1.Quantity == 2);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsTrue(payment1.IsActive);
                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Employee Survey #2");
                //We create a collector:
                var collector1 = surveyManager.CreateCollector(survey1, CollectorType.WebLink, "Collecto01-webLink", CreditType.ClickType);
                Assert.IsTrue(collector1.Status == CollectorStatus.New);

                //εάν δεν προσθέσουμε Payment στον Collector, δεν μπορούμε να ανοίξουμε τον Collector:
                _EXECUTEAndCATCHType(delegate { surveyManager.OpenCollector(collector1); }, typeof(VLPaymentException));

                //Προσθέτουμε payment στον collector:
                var collectorPayment1 = systemManager.AddPaymentToCollector(collector1.CollectorId, payment1.PaymentId);
                Assert.IsTrue(collectorPayment1.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment1.QuantityUsed == 0);
                Assert.IsFalse(collectorPayment1.IsUsed);
                //και ανοίγουμε τον collector:
                collector1 = surveyManager.OpenCollector(collector1);
                Assert.IsTrue(collector1.Status == CollectorStatus.Open);

                //ελέγουμε την πληρωμή μας:
                payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                Assert.IsTrue(payment1.CreditType == CreditType.ClickType);
                Assert.IsTrue(payment1.Quantity == 2);
                Assert.IsTrue(payment1.QuantityUsed == 0);
                Assert.IsTrue(payment1.IsActive);
                //ελέγχουμε την χρέωσή μας:
                collectorPayment1 = systemManager.GetCollectorPaymentById(collectorPayment1.CollectorPaymentId);
                Assert.IsTrue(collectorPayment1.QuantityReserved == 0);
                Assert.IsTrue(collectorPayment1.QuantityUsed == 0);
                Assert.IsFalse(collectorPayment1.IsUsed);

                //2 Credits Available
                //Τώρα θέλουμε να εξομοιώσουμε το click απο το web (ValisServer.SurveyRuntime):
                {
                    /*Δημιουργούμε ένα εικονικό (ενεργοποιημένο) recipient:*/
                    var _vrecipient = surveyManager.CreateRecipientVirtual(collector1.CollectorId);
                    Assert.IsNotNull(_vrecipient);
                    /*Δημιουργούμε ένα RuntimeSession:*/
                    var session = surveyManager.CreateSession(survey1.SurveyId, RuntimeRequestType.Collector_WebLink, ResponseType.Default, "a useragent", collector1.CollectorId, recipientKey: _vrecipient.RecipientKey, recipientIP: "8.8.3.4");
                    Assert.IsNotNull(session);
                    Assert.IsTrue(session.Collector == collector1.CollectorId);
                    Assert.IsNull(session.CollectorPayment);
                    Assert.IsFalse(session.IsCharged);
                    /*Πραγματοποιούμε την χρέωση:*/
                    if (collector1.UseCredits && collector1.CreditType.HasValue && collector1.CreditType.Value == CreditType.ClickType)
                    {
                        session = surveyManager.ChargePaymentForClick(session.SessionId, collector1.CollectorId, collector1.Survey);
                        Assert.IsTrue(session.CollectorPayment == collectorPayment1.CollectorPaymentId);
                        Assert.IsTrue(session.IsCharged);

                        //ΔΕΝ ΕΠΙΤΡΕΠΕΤΑΙ ΝΑ ΧΡΕΩΣΟΥΜΕ ΔΙΠΛΗ ΦΟΡΑ:
                        _EXECUTEAndCATCHType(delegate { surveyManager.ChargePaymentForClick(session.SessionId, collector1.CollectorId, collector1.Survey); }, typeof(VLException));

                        //ελέγουμε την πληρωμή μας:
                        payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                        Assert.IsTrue(payment1.CreditType == CreditType.ClickType);
                        Assert.IsTrue(payment1.Quantity == 2);
                        Assert.IsTrue(payment1.QuantityUsed == 1);
                        Assert.IsTrue(payment1.IsActive);
                        //ελέγχουμε την χρέωσή μας:
                        collectorPayment1 = systemManager.GetCollectorPaymentById(collectorPayment1.CollectorPaymentId);
                        Assert.IsTrue(collectorPayment1.QuantityReserved == 0);
                        Assert.IsTrue(collectorPayment1.QuantityUsed == 1);
                        Assert.IsTrue(collectorPayment1.IsUsed);
                    }
                }

                //1 Credits Available
                //Τώρα θέλουμε να εξομοιώσουμε το click απο το web (ValisServer.SurveyRuntime):
                {
                    /*Δημιουργούμε ένα εικονικό (ενεργοποιημένο) recipient:*/
                    var _vrecipient = surveyManager.CreateRecipientVirtual(collector1.CollectorId);
                    Assert.IsNotNull(_vrecipient);
                    /*Δημιουργούμε ένα RuntimeSession:*/
                    var session = surveyManager.CreateSession(survey1.SurveyId, RuntimeRequestType.Collector_WebLink, ResponseType.Default, "a useragent", collector1.CollectorId, recipientKey: _vrecipient.RecipientKey, recipientIP: "8.8.3.4");
                    Assert.IsNotNull(session);
                    Assert.IsTrue(session.Collector == collector1.CollectorId);
                    Assert.IsNull(session.CollectorPayment);
                    Assert.IsFalse(session.IsCharged);
                    /*Πραγματοποιούμε την χρέωση:*/
                    if (collector1.UseCredits && collector1.CreditType.HasValue && collector1.CreditType.Value == CreditType.ClickType)
                    {
                        session = surveyManager.ChargePaymentForClick(session.SessionId, collector1.CollectorId, collector1.Survey);
                        Assert.IsTrue(session.CollectorPayment == collectorPayment1.CollectorPaymentId);
                        Assert.IsTrue(session.IsCharged);

                        //ελέγουμε την πληρωμή μας:
                        payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                        Assert.IsTrue(payment1.CreditType == CreditType.ClickType);
                        Assert.IsTrue(payment1.Quantity == 2);
                        Assert.IsTrue(payment1.QuantityUsed == 2);
                        Assert.IsFalse(payment1.IsActive);
                        //ελέγχουμε την χρέωσή μας:
                        collectorPayment1 = systemManager.GetCollectorPaymentById(collectorPayment1.CollectorPaymentId);
                        Assert.IsTrue(collectorPayment1.QuantityReserved == 0);
                        Assert.IsTrue(collectorPayment1.QuantityUsed == 2);
                        Assert.IsTrue(collectorPayment1.IsUsed);
                    }
                }

                //0 Credits Available
                //Τώρα θέλουμε να εξομοιώσουμε το click απο το web (ValisServer.SurveyRuntime):
                {
                    /*Δημιουργούμε ένα εικονικό (ενεργοποιημένο) recipient:*/
                    var _vrecipient = surveyManager.CreateRecipientVirtual(collector1.CollectorId);
                    Assert.IsNotNull(_vrecipient);
                    /*Δημιουργούμε ένα RuntimeSession:*/
                    var session = surveyManager.CreateSession(survey1.SurveyId, RuntimeRequestType.Collector_WebLink, ResponseType.Default, "a useragent", collector1.CollectorId, recipientKey: _vrecipient.RecipientKey, recipientIP: "8.8.3.4");
                    Assert.IsNotNull(session);
                    Assert.IsTrue(session.Collector == collector1.CollectorId);
                    Assert.IsNull(session.CollectorPayment);
                    Assert.IsFalse(session.IsCharged);
                    /*Πραγματοποιούμε την χρέωση:*/
                    if (collector1.UseCredits && collector1.CreditType.HasValue && collector1.CreditType.Value == CreditType.ClickType)
                    {
                        session = surveyManager.ChargePaymentForClick(session.SessionId, collector1.CollectorId, collector1.Survey);                //<!-- Δεν υπάρχουν διαθέσιμα credits
                        Assert.IsFalse(session.IsCharged);          //Δεν χρεώθηκε:
                        Assert.IsNull(session.CollectorPayment);

                        //ελέγουμε την πληρωμή μας:
                        payment1 = systemManager.GetPaymentById(payment1.PaymentId);
                        Assert.IsTrue(payment1.CreditType == CreditType.ClickType);
                        Assert.IsTrue(payment1.Quantity == 2);
                        Assert.IsTrue(payment1.QuantityUsed == 2);
                        Assert.IsFalse(payment1.IsActive);
                        //ελέγχουμε την χρέωσή μας:
                        collectorPayment1 = systemManager.GetCollectorPaymentById(collectorPayment1.CollectorPaymentId);
                        Assert.IsTrue(collectorPayment1.QuantityReserved == 0);
                        Assert.IsTrue(collectorPayment1.QuantityUsed == 2);
                        Assert.IsTrue(collectorPayment1.IsUsed);
                    }
                }



            }
            finally
            {
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


            }
        }



    }
}
