using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class CollectorTests01 : AdminBaseClass
    {

        [TestMethod, Description("CRUD operations for Collectors #1")]
        public void CollectorTest01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                Assert.IsTrue(survey1.Client == client1.ClientId);


                //τώρα έχουμε 0 collector:
                Assert.IsTrue(surveyManager.GetCollectors(survey1).Count == 0);


                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.WebLink, "COLLECTOR01_webLink1");
                Assert.IsNotNull(collector01);
                Assert.IsTrue(collector01.Survey == survey1.SurveyId);
                Assert.IsTrue(collector01.CollectorType == CollectorType.WebLink);
                Assert.AreEqual<string>("COLLECTOR01_webLink1", collector01.Name);
                Assert.IsTrue(collector01.Status == CollectorStatus.Open);

                var svdCollector01 = surveyManager.GetCollectorById(collector01.CollectorId);
                Assert.AreEqual<VLCollector>(collector01, svdCollector01);
                svdCollector01 = surveyManager.GetCollectorByWebLink(collector01.WebLink);
                Assert.AreEqual<VLCollector>(collector01, svdCollector01);


                //τώρα έχουμε 1 collector:
                Assert.IsTrue(surveyManager.GetCollectors(survey1).Count == 1);




                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector02 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.WebLink, "COLLECTOR01_webLink2");
                Assert.IsNotNull(collector02);
                Assert.IsTrue(collector02.Survey == survey1.SurveyId);
                Assert.IsTrue(collector02.CollectorType == CollectorType.WebLink);
                Assert.AreEqual<string>("COLLECTOR01_webLink2", collector02.Name);
                Assert.IsTrue(collector02.Status == CollectorStatus.Open);

                var svdCollector02 = surveyManager.GetCollectorById(collector02.CollectorId);
                Assert.AreEqual<VLCollector>(collector02, svdCollector02);



                //τώρα έχουμε 2 collectors:
                Assert.IsTrue(surveyManager.GetCollectors(survey1).Count == 2);




                //Κλείνουμε τον collector01:
                collector01 = surveyManager.CloseCollector(collector01);
                Assert.IsTrue(collector01.Status == CollectorStatus.Close);
                //Διαγράφουμε ένα collector:
                surveyManager.DeleteCollector(collector01);
                Assert.IsNull(surveyManager.GetCollectorById(collector01.CollectorId));
                Assert.IsTrue(surveyManager.GetCollectors(survey1).Count == 1);


                //Κλείνουμε τον collector01:
                collector02 = surveyManager.CloseCollector(collector02);
                Assert.IsTrue(collector02.Status == CollectorStatus.Close);
                //Διαγράφουμε ένα collector:
                surveyManager.DeleteCollector(collector02);
                Assert.IsNull(surveyManager.GetCollectorById(collector02.CollectorId));
                Assert.IsTrue(surveyManager.GetCollectors(survey1).Count == 0);
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


        [TestMethod, Description("CRUD operations for CollectorMessages #1")]
        public void CollectorTest02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            
            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                Assert.IsTrue(survey1.Client == client1.ClientId);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email");
                Assert.IsNotNull(collector01);


                //δεν έχουμε κανένα message:
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 0);

                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ MESSAGE:
                var message01 = surveyManager.CreateMessage(collector01, "milonakis@hotail.com", "please answer the following questions", "We are conducting a survey, and your response would be appreciated. Here is a link to the survey: [SurveyLink]. In Order to be removed: [RemoveLink]");
                Assert.IsNotNull(message01);
                Assert.IsTrue(message01.Collector == collector01.CollectorId);

                var svdMessage01 = surveyManager.GetMessageById(message01.MessageId);
                Assert.AreEqual<VLMessage>(message01, svdMessage01);


                //έχουμε ένα message:
                Assert.IsTrue(surveyManager.GetMessages(collector01).Count == 1);



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



        [TestMethod, Description("CRUD operations for Recipients")]
        public void CollectorTest03()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                Assert.IsTrue(survey1.Client == client1.ClientId);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email");
                Assert.IsNotNull(collector01);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector02 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR02_email");
                Assert.IsNotNull(collector02);


                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);

                #region collector01
                //Δημιοργούμε μία νέα επαφή στον collector01:
                var recipient01 = surveyManager.CreateRecipient(collector01, "11297@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(recipient01);
                Assert.IsTrue(recipient01.Collector == collector01.CollectorId);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", recipient01.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", recipient01.LastName);
                Assert.AreEqual<string>("11297@nbg.gr", recipient01.Email);
                Assert.IsNull(recipient01.Title);
                Assert.IsNull(recipient01.CustomValue);
                var svdRecipient01 = surveyManager.GetRecipientById(recipient01.RecipientId);
                Assert.AreEqual<VLRecipient>(recipient01, svdRecipient01);
                svdRecipient01 = surveyManager.GetRecipientByEmail(recipient01.Collector, recipient01.Email);
                Assert.AreEqual<VLRecipient>(recipient01, svdRecipient01);
                svdRecipient01 = surveyManager.GetRecipientByKey(recipient01.Collector, recipient01.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient01, svdRecipient01);
                svdRecipient01 = surveyManager.GetRecipientByKey(recipient01.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient01, svdRecipient01);


                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);

                //το email μέσα σε κάθε collector είναι μοναδικό:
                _EXECUTEAndCATCHType(delegate { surveyManager.CreateRecipient(collector01, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));


                //Δημιοργούμε μία νέα επαφή στον collector02:
                var recipient02 = surveyManager.CreateRecipient(collector01, "12072@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ");
                Assert.IsNotNull(recipient02);
                Assert.IsTrue(recipient02.Collector == collector01.CollectorId);
                Assert.AreEqual<string>("ΔΗΜΗΤΡΙΟΣ", recipient02.FirstName);
                Assert.AreEqual<string>("ΓΕΩΡΓΙΤΣΙΩΤΗΣ", recipient02.LastName);
                Assert.AreEqual<string>("12072@nbg.gr", recipient02.Email);
                Assert.IsNull(recipient02.Title);
                Assert.IsNull(recipient02.CustomValue);
                var svdRecipient02 = surveyManager.GetRecipientById(recipient02.RecipientId);
                Assert.AreEqual<VLRecipient>(recipient02, svdRecipient02);
                svdRecipient02 = surveyManager.GetRecipientByEmail(recipient02.Collector, recipient02.Email);
                Assert.AreEqual<VLRecipient>(recipient02, svdRecipient02);
                svdRecipient02 = surveyManager.GetRecipientByKey(recipient02.Collector, recipient02.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient02, svdRecipient02);
                svdRecipient02 = surveyManager.GetRecipientByKey(recipient02.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient02, svdRecipient02);
                #endregion

                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 2);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);


                #region collector02
                //Δημιοργούμε μία νέα επαφή στον collector02:
                var recipient03 = surveyManager.CreateRecipient(collector02, "11297@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(recipient03);
                Assert.IsTrue(recipient03.Collector == collector02.CollectorId);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", recipient03.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", recipient03.LastName);
                Assert.AreEqual<string>("11297@nbg.gr", recipient03.Email);
                Assert.IsNull(recipient03.Title);
                Assert.IsNull(recipient03.CustomValue);
                var svdRecipient03 = surveyManager.GetRecipientById(recipient03.RecipientId);
                Assert.AreEqual<VLRecipient>(recipient03, svdRecipient03);
                svdRecipient03 = surveyManager.GetRecipientByEmail(recipient03.Collector, recipient03.Email);
                Assert.AreEqual<VLRecipient>(recipient03, svdRecipient03);
                svdRecipient03 = surveyManager.GetRecipientByKey(recipient03.Collector, recipient03.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient03, svdRecipient03);
                svdRecipient03 = surveyManager.GetRecipientByKey(recipient03.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient03, svdRecipient03);


                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 2);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1);


                //Δημιοργούμε μία νέα επαφή στον collector02:
                var recipient04 = surveyManager.CreateRecipient(collector02, "12072@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ");
                Assert.IsNotNull(recipient04);
                Assert.IsTrue(recipient04.Collector == collector02.CollectorId);
                Assert.AreEqual<string>("ΔΗΜΗΤΡΙΟΣ", recipient04.FirstName);
                Assert.AreEqual<string>("ΓΕΩΡΓΙΤΣΙΩΤΗΣ", recipient04.LastName);
                Assert.AreEqual<string>("12072@nbg.gr", recipient04.Email);
                Assert.IsNull(recipient04.Title);
                Assert.IsNull(recipient04.CustomValue);
                var svdRecipient04 = surveyManager.GetRecipientById(recipient04.RecipientId);
                Assert.AreEqual<VLRecipient>(recipient04, svdRecipient04);
                svdRecipient04 = surveyManager.GetRecipientByEmail(recipient04.Collector, recipient04.Email);
                Assert.AreEqual<VLRecipient>(recipient04, svdRecipient04);
                svdRecipient04 = surveyManager.GetRecipientByKey(recipient04.Collector, recipient04.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient04, svdRecipient04);
                svdRecipient04 = surveyManager.GetRecipientByKey(recipient04.RecipientKey);
                Assert.AreEqual<VLRecipient>(recipient04, svdRecipient04);
                #endregion


                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 2);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 2);


                //UPDATE
                recipient04.Title = "Mr";
                recipient04.RecipientKey = "qwerty";
                recipient04 = surveyManager.UpdateRecipient(recipient04);
                Assert.IsNotNull(recipient04);
                Assert.IsTrue(recipient04.Collector == collector02.CollectorId);
                Assert.AreEqual<string>("ΔΗΜΗΤΡΙΟΣ", recipient04.FirstName);
                Assert.AreEqual<string>("ΓΕΩΡΓΙΤΣΙΩΤΗΣ", recipient04.LastName);
                Assert.AreEqual<string>("12072@nbg.gr", recipient04.Email);
                Assert.AreEqual<string>("Mr", recipient04.Title);
                Assert.IsNull(recipient04.CustomValue);
                Assert.AreEqual<string>("qwerty", recipient04.RecipientKey);

                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 2);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 2);

                //DELETE
                surveyManager.DeleteRecipient(recipient01);
                Assert.IsNull(surveyManager.GetRecipientById(recipient01.RecipientId));
                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 2);

                //DELETE
                surveyManager.DeleteRecipient(recipient04);
                Assert.IsNull(surveyManager.GetRecipientById(recipient04.RecipientId));
                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1);

                //DELETE
                surveyManager.DeleteRecipient(recipient03);
                Assert.IsNull(surveyManager.GetRecipientById(recipient03.RecipientId));
                //Καταμέτρηση
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);

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


        [TestMethod, Description("IMPORT operations for Recipients")]
        public void CollectorTest04()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-contacts-1");
                Assert.IsNotNull(list01);
                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                Assert.IsTrue(survey1.Client == client1.ClientId);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector01 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR01_email");
                Assert.IsNotNull(collector01);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector02 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR02_email");
                Assert.IsNotNull(collector02);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector03 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR03_email");
                Assert.IsNotNull(collector03);
                //ΔΗΜΙΟΥΡΓΟΥΜΕ ΕΝΑ COLLECTOR:
                var collector04 = surveyManager.CreateCollector(survey1.SurveyId, CollectorType.Email, "COLLECTOR04_email");
                Assert.IsNotNull(collector04);


                #region βάζουμε στην λίστα μας 1000 επαφές:
                ContactImportResult importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                }
                Assert.IsTrue(importResult.SuccesfullImports == 1000);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 1000);
                #endregion

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 0);


                
                //Παίρνουμε τις επαφές απο την λίστα και τις βαζουμε στον collector01:
                importResult = surveyManager.ImportRecipientsFromList(collector01.CollectorId, list01.ListId);
                Assert.IsTrue(importResult.SuccesfullImports == 1000);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 0);



                //Παίρνουμε τις επαφές απο τον collector01 και τις βάζουμε στον collector02:
                importResult = surveyManager.ImportRecipientsFromCollector(collector02.CollectorId, collector01.CollectorId);
                Assert.IsTrue(importResult.SuccesfullImports == 1000);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 0);




                //Βάζουμε στον Collector03 1000 επαφές:
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA.csv"), FileMode.Open))
                {
                    importResult = surveyManager.ImportRecipientsFromCSV(collector03.CollectorId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                }
                Assert.IsTrue(importResult.SuccesfullImports == 1000);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 0);







                //Παίρνουμε τις επαφές απο τον collector01 και τις βάζουμε στον collector02:
                importResult = surveyManager.ImportRecipientsFromCollector(collector02.CollectorId, collector01.CollectorId);
                Assert.IsTrue(importResult.SuccesfullImports == 0);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 1000);

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 0);



                //Βάζουμε στον Collector03 1000 επαφές:
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA.csv"), FileMode.Open))
                {
                    importResult = surveyManager.ImportRecipientsFromCSV(collector03.CollectorId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                }
                Assert.IsTrue(importResult.SuccesfullImports == 0);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 1000);

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 0);



                //Βάζουμε στον Collector04 10 επαφές:
                var s2 = "1,Rebecca,Johnston,,Syria,115.25.131.108\n2,Donald,Henderson,,Niue,85.186.113.81\n3,Eugene,Wagner,ewagner@tanoodle.com,Cote d'Ivoire,225.48.163.209\n4,Philip,Rivera,privera@meezzy.edu,Marshall Islands,162.95.237.119\n5,Gary,Frazier,gfrazier@jayo.biz,Qatar,42.45.26.151\n6,Pamela,Campbell,pcampbell@devcast.net,Cameroon,242.206.197.185\n7,Louise,Hunt,lhunt@roodel.info,Bermuda,228.174.174.112\n8,Rebecca,Sullivan,rsullivan@mydo.gov,Morocco,10.5.33.157\n9,Scott,Cole,scole@roomm.net,Austria,182.94.80.70\n10,Brian,Bishop,bbishop@skibox.name,Laos,60.120.120.198\n";
                importResult = surveyManager.ImportRecipientsFromString(collector04.CollectorId, s2, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 8);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 0);

                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 8);


                //ΔΙΑΓΡΑΦΗ
                surveyManager.RemoveAllUnsentRecipients(collector01);
                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 8);

                //ΔΙΑΓΡΑΦΗ
                surveyManager.RemoveAllUnsentRecipients(collector03);
                //ΚΑΤΑΜΕΤΡΗΣΗ Recipients:
                Assert.IsTrue(surveyManager.GetRecipients(collector01).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector02).Count == 1000);
                Assert.IsTrue(surveyManager.GetRecipients(collector03).Count == 0);
                Assert.IsTrue(surveyManager.GetRecipients(collector04).Count == 8);
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
