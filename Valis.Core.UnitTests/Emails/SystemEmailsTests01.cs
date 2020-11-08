using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Valis.Core.UnitTests.Emails
{
    [TestClass]
    public class SystemEmailsTests01 : AdminBaseClass
    {
        [TestMethod, Description("CRUD tests for VLSystemEmail")]
        public void SystemEmailsTests01_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //Δεν υπάρχει κανένα SystemEmail:
                Assert.IsTrue(systemManager.GetSystemEmails().Count == 0);

                //Δημιουργούμε ένα systemEmail:
                var email1 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "milonakis@hotmail.com", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email1);
                Assert.IsTrue(email1.ModuleName == "unittest");
                Assert.IsTrue(email1.FromAddress == "noreply@artiaservices.com");
                Assert.IsTrue(email1.FromDisplayName == "My Surveys System");
                Assert.IsTrue(email1.ToAddress == "milonakis@hotmail.com");
                Assert.IsTrue(email1.Subject == "Παρακαλώ προχωρήστε σε validation του email σας!");
                Assert.IsTrue(email1.Body == "Πατήστε το link!");
                Assert.IsTrue(email1.Status == EmailStatus.Pending);
                Assert.IsNull(email1.Error);
                var svdEmail1 = systemManager.GetSystemEmailById(email1.EmailId);
                Assert.AreEqual<VLSystemEmail>(email1, svdEmail1);

                //τώρα έχουμε ένα systemEmail:
                Assert.IsTrue(systemManager.GetSystemEmails().Count == 1);

                //Δεν μπορούμε να δημιουργήσουμε SystemEmail με invalid FromAddress και ToAddress
                _EXECUTEAndCATCHType(delegate { systemManager.CreateSystemEmail("unittest", "noreply@artiaservices", "My Surveys System", "milonakis@hotmail.com", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!"); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "milonakis@hotma", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!"); }, typeof(VLException));

                //Μπορούμε να δημιουργήσουμε ένα ολοίδιο SystemEmail με αυτό που δημιουργήσαμε πιο πρίν:
                var email2 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "milonakis@hotmail.com", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email2);

                //τώρα έχουμε δύο (2) systemEmail:
                Assert.IsTrue(systemManager.GetSystemEmails().Count == 2);



                //Μπορούμε να κάνουμε update συγκεκριμένα πράγματα:
                email1.SendDT = Utility.UtcNow();
                email1.Status = EmailStatus.Sent;
                email1 = systemManager.UpdateSystemEmail(email1);
                Assert.IsTrue(email1.ModuleName == "unittest");
                Assert.IsTrue(email1.FromAddress == "noreply@artiaservices.com");
                Assert.IsTrue(email1.FromDisplayName == "My Surveys System");
                Assert.IsTrue(email1.ToAddress == "milonakis@hotmail.com");
                Assert.IsTrue(email1.Subject == "Παρακαλώ προχωρήστε σε validation του email σας!");
                Assert.IsTrue(email1.Body == "Πατήστε το link!");
                Assert.IsTrue(email1.Status == EmailStatus.Sent);
                Assert.IsNotNull(email1.SendDT);
                Assert.IsNull(email1.Error);

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

                var systemEmails = systemManager.GetSystemEmails();
                foreach(var item in systemEmails)
                {
                    systemManager.DeleteSystemEmail(item.EmailId);
                }
            }
        }


        [TestMethod, Description("CRUD tests for VLSystemEmail")]
        public void SystemEmailsTests01_02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //Δεν υπάρχει κανένα SystemEmail:
                Assert.IsTrue(systemManager.GetSystemEmails().Count == 0);

                //Δημιουργούμε μερικά systemEmail:
                var email1 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "milonakis@hotmail.com", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email1);

                var email2 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "rallen@quire.org", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email2);

                var email3 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "afranklin@aibox.edu", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email3);

                var email4 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "nthomas@vinte.name", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email4);

                var email5 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "trobertson@oyonder.gov", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email5);

                //Εχουμε δημιουργήσει πέντε (5) SystemEmails:
                Assert.IsTrue(systemManager.GetSystemEmails().Count == 5);

                //τώρα τραβάμε για αποστολή 4 pending Systememails:
                var pendingEmails01 = systemManager.GetPendingSystemEmails(maxRows: 4);
                Assert.IsTrue(pendingEmails01.Count == 4);



                var email6 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "sflores@wikizz.net", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email6);

                var email7 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "sgomez@oyoba.com", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email7);

                var email8 = systemManager.CreateSystemEmail("unittest", "noreply@artiaservices.com", "My Surveys System", "bmarshall@topiclounge.mil", "Παρακαλώ προχωρήστε σε validation του email σας!", "Πατήστε το link!");
                Assert.IsNotNull(email8);


                //Εχουμε δημιουργήσει οχτώ (8) SystemEmails:
                Assert.IsTrue(systemManager.GetSystemEmails().Count == 8);



                var systemEmails = systemManager.GetSystemEmails();
                Assert.IsTrue(systemEmails.Count == 8);
                Int32 _pending = 0;
                Int32 _executing = 0;
                foreach(var item in systemEmails)
                {
                    if (item.Status == EmailStatus.Pending)
                        _pending++;
                    else if (item.Status == EmailStatus.Executing)
                        _executing++;
                }

                Assert.IsTrue(_pending == 4);
                Assert.IsTrue(_executing == 4);


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

                var systemEmails = systemManager.GetSystemEmails();
                foreach (var item in systemEmails)
                {
                    systemManager.DeleteSystemEmail(item.EmailId);
                }
            }
        }



        [TestMethod, Description("Verify Reply-to address!")]
        public void SystemEmailsTests01_03()
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
                //Δημιουργούμε στον collector 10 recipients:
                surveyManager.ImportRecipientsFromList(collector01, list01);


                //Δημιουργούμε ένα μήνυμα στον collector:
                var message01 = surveyManager.CreateMessage(collector01, "milonakis@thetest12.gr");
                Assert.IsTrue(message01.Status == MessageStatus.Draft);
                Assert.IsFalse(message01.IsSenderOK);
                Assert.IsTrue(message01.Sender == "milonakis@thetest12.gr");


                //τώρα θα δημιουργήσουμε ένα VerifySender email:
                var email = surveyManager.SendVerifySenderEmail(message01);
                Assert.IsNotNull(email);

                //ελέγχουμε το message01:
                message01 = surveyManager.GetMessageById(message01.MessageId);
                Assert.IsFalse(message01.IsSenderOK);
                Assert.IsTrue(message01.Sender == "milonakis@thetest12.gr");
                //ελέγχουμε εάν υπάρχει το Message.Sender στα Knownemails:
                Assert.IsNull(systemManager.GetKnownEmailByAddress(survey1.Client, message01.Sender));


                //Τώρα θα μιμηθούμε ότι κάποιος πάτησε το link:
                message01 = surveyManager.VerifySenderEmail(message01.SenderVerificationCode);
                Assert.IsTrue(message01.IsSenderOK);
                Assert.IsTrue(message01.Sender == "milonakis@thetest12.gr");
                //τώρα υπάρχει ένα KnownEmail:
                var knownEmail1 = systemManager.GetKnownEmailByAddress(survey1.Client, message01.Sender);
                Assert.IsNotNull(knownEmail1);
                Assert.IsTrue(knownEmail1.IsDomainOK);
                Assert.IsTrue(knownEmail1.IsVerified);



                //τώρα θέλουμε να κάνουμε update Message.Sender. Η διαδικασία του Sender Verification ξεκιν΄απο την αρχή:
                message01.Sender = "manolis@tralala.com";
                message01 = surveyManager.UpdateMessage(message01);
                Assert.IsTrue(message01.Status == MessageStatus.Draft);
                Assert.IsFalse(message01.IsSenderOK);
                Assert.IsTrue(message01.Sender == "manolis@tralala.com");


                //εάν όμως φτιάξουμε ένα νέο message που θα φέρει σαν sender την αρχική email address, τότε αυτό θα είναι αυτόμτα veirified:
                var message02 = surveyManager.CreateMessage(collector01, "milonakis@thetest12.gr");
                Assert.IsTrue(message02.Status == MessageStatus.Draft);
                Assert.IsTrue(message02.IsSenderOK);                            //VERIFIED!!!!
                Assert.IsTrue(message02.Sender == "milonakis@thetest12.gr");





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

                var systemEmails = systemManager.GetSystemEmails();
                foreach (var item in systemEmails)
                {
                    systemManager.DeleteSystemEmail(item.EmailId);
                }
            }
        }
    }
}
