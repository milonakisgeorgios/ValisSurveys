using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class CollectorTests02 : AdminBaseClass
    {

        [TestMethod, Description("")]
        public void CollectorTests02_01()
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
                ContactImportResult importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(list01.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                //Κανένα contact δεν είναι OptedOut ή Bounced:
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1");
                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);

                //Τώρα ο collector μας έχει 10 recipients:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 10);
                //Κανένα recipient δεν είναι OptedOut ή Bounced:
                var recipients = surveyManager.GetRecipients(collector01);
                foreach(var item in recipients)
                {
                    Assert.IsFalse(item.IsSentEmail);
                    Assert.IsFalse(item.IsOptedOut);
                    Assert.IsFalse(item.IsBouncedEmail);
                    Assert.IsFalse(item.HasResponded);
                }

                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);


                //Κάνουμε bounced τρία emails:
                var email01b = recipients[0].Email;
                var email02b = recipients[1].Email;
                var email03b = recipients[2].Email;

                Assert.IsTrue(surveyManager.BounceRecipient(collector01, email01b) == 1);
                Assert.IsTrue(surveyManager.BounceRecipient(collector01, email02b) == 1);
                Assert.IsTrue(surveyManager.BounceRecipient(collector01, email03b) == 1);


                //Τώρα έχουμε τρία (3) bounced emails στον Collector, και στην λίστα του πελάτη μας:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 3);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);



                //Κάνουμε optedOut τρία emails:
                var email01o = recipients[7].Email;
                var email02o = recipients[8].Email;
                var email03o = recipients[9].Email;

                Assert.IsTrue(surveyManager.OptOutRecipient(collector01, email01o) == 1);
                Assert.IsTrue(surveyManager.OptOutRecipient(collector01, email02o) == 1);
                Assert.IsTrue(surveyManager.OptOutRecipient(collector01, email03o) == 1);

                //Τώρα έχουμε τρία (3) OptedOut emails στον Collector, και στην λίστα του πελάτη μας:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 3);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 3);
                Assert.IsTrue(surveyManager.GetEmailedRecipients(collector01).Count == 0);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);


                #region Διαγραφές
                //Θα διαγράψουμε τα OptedOut recipients:
                Assert.IsTrue(surveyManager.RemoveAllOptedOutRecipients(collector01) == 3);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 7);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 3);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);


                //θα διαγράψουμε τα bounced recipients:
                Assert.IsTrue(surveyManager.RemoveAllBouncedRecipients(collector01) == 3);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 4);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);

                //Θα διαγράψουμε όλες τις επαφές απο την λίστα μας:
                systemManager.RemoveAllContactsFromList(list01.ListId);
                list01 = systemManager.GetClientListById(list01.ListId);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 4);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);
                //
                Assert.IsTrue(list01.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);


                //Θα διαγράχουμε όλα τα unsent recipients απο τον collector:
                Assert.IsTrue(surveyManager.RemoveAllUnsentRecipients(collector01) == 4);

                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);
                //
                Assert.IsTrue(list01.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);
                #endregion
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


        [TestMethod, Description("")]
        public void CollectorTests02_02()
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
                    systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(list01.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                //Κανένα contact δεν είναι OptedOut ή Bounced:
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);

                //We create a contact list:
                var list02 = systemManager.CreateClientList(client1.ClientId, "my-list-02");
                //Εισαγωγή 10 γραμμών
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list02.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                //Η λίστα μας έχει τωρα 10 contacts:
                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                //Κανένα contact δεν είναι OptedOut ή Bounced:
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
                #endregion

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                //We create a collector:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email1");
                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);
                //We create a collector:
                var collector02 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR02_email1");
                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector02, list01);

                //Τώρα ο collector01 έχει 10 recipients:
                collector01 = surveyManager.GetCollectorById(collector01.CollectorId);
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);
                //Τώρα ο collector02 έχει 10 recipients:
                collector02 = surveyManager.GetCollectorById(collector02.CollectorId);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 0);


                //Κάνουμε bounced τρία emails:
                Assert.IsTrue(surveyManager.BounceRecipient(collector01, "aellis@oyoloo.gov") == 2);
                Assert.IsTrue(surveyManager.BounceRecipient(collector01, "gcrawford@tambee.edu") == 2);
                Assert.IsTrue(surveyManager.BounceRecipient(collector01, "chart@wordtune.name") == 2);


                //Τώρα έχουμε τρία (3) bounced emails στον Collector, και στην λίστα του πελάτη μας:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 3);

                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 3);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);

                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 3);



                //Κάνουμε optedOut τρία emails:
                Assert.IsTrue(surveyManager.OptOutRecipient(collector01, "jcox@npath.mil") == 2);
                Assert.IsTrue(surveyManager.OptOutRecipient(collector01, "bmitchell@linktype.mil") == 2);
                Assert.IsTrue(surveyManager.OptOutRecipient(collector01, "nruiz@yadel.edu") == 2);

                //Τώρα έχουμε τρία (3) OptedOut emails στον Collector, και στην λίστα του πελάτη μας:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 3);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 3);

                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 3);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 3);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);

                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 3);


                #region Διαγραφές
                //Θα διαγράψουμε τα OptedOut recipients:
                Assert.IsTrue(surveyManager.RemoveAllOptedOutRecipients(collector01) == 3);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 7);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 3);

                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 3);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 3);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);

                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 3);


                //θα διαγράψουμε τα bounced recipients:
                Assert.IsTrue(surveyManager.RemoveAllBouncedRecipients(collector01) == 3);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 4);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);

                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 3);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 3);
                //
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);

                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 3);

                //Θα διαγράψουμε όλες τις επαφές απο την λίστα μας:
                systemManager.RemoveAllContactsFromList(list01.ListId);
                list01 = systemManager.GetClientListById(list01.ListId);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 4);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);

                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 10);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 3);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 3);
                //
                Assert.IsTrue(list01.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);

                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 3);


                //Θα διαγράχουμε όλα τα unsent recipients απο τον collector:
                Assert.IsTrue(surveyManager.RemoveAllUnsentRecipients(collector02) == 10);
                //Ελέγχουμε:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 4);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector01).Count == 0);

                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);
                Assert.IsTrue(surveyManager.GetOptedOutRecipients(collector02).Count == 0);
                Assert.IsTrue(surveyManager.GetBouncedRecipients(collector02).Count == 0);
                //
                Assert.IsTrue(list01.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);

                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 3);
                #endregion
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

    }
}
