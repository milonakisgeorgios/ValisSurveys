using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Valis.Core.UnitTests.Suite1
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ClientLists01 : AdminBaseClass
    {

        [TestMethod, Description("CRUD operations for VLClientList")]
        public void ClientLists01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 0);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 0);


                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                Assert.IsNotNull(list01);
                Assert.IsTrue(list01.Client == client1.ClientId);
                Assert.AreEqual<string>("my-list-01", list01.Name);
                var svdList = systemManager.GetClientListById(list01.ListId);
                Assert.AreEqual<VLClientList>(list01, svdList);


                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 1);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 0);

                //Δεν μπορούμε να δημιουργήσουμε λίστα που υπάρχει ήδη:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateClientList(client1.ClientId, "my-list-01"); }, typeof(VLException));


                //We create a contact list:
                var list02 = systemManager.CreateClientList(client1.ClientId, "my-list-02");
                Assert.IsNotNull(list02);
                Assert.IsTrue(list02.Client == client1.ClientId);
                Assert.AreEqual<string>("my-list-02", list02.Name);
                svdList = systemManager.GetClientListById(list02.ListId);
                Assert.AreEqual<VLClientList>(list02, svdList);


                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 2);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 0);


                //We create a contact list:
                var list03 = systemManager.CreateClientList(client2.ClientId, "my-list-01");
                Assert.IsNotNull(list03);
                Assert.IsTrue(list03.Client == client2.ClientId);
                Assert.AreEqual<string>("my-list-01", list03.Name);
                svdList = systemManager.GetClientListById(list03.ListId);
                Assert.AreEqual<VLClientList>(list03, svdList);


                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 2);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 1);


                //Update
                list02.Name = "my-list-01";
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateClientList(list02); }, typeof(VLException));

                //Update
                list03.Name = "Η πρώτη λίστα-πείραμα";
                list03 = systemManager.UpdateClientList(list03);
                Assert.IsNotNull(list03);
                Assert.IsTrue(list03.Client == client2.ClientId);
                Assert.AreEqual<string>("Η πρώτη λίστα-πείραμα", list03.Name);
                svdList = systemManager.GetClientListById(list03.ListId);
                Assert.AreEqual<VLClientList>(list03, svdList);



                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 2);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 1);


                //Delete
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 1);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 1);


                //Delete
                systemManager.DeleteClientList(list03);
                Assert.IsNull(systemManager.GetClientListById(list03.ListId));
                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 1);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 0);

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


        [TestMethod, Description("CRUD operations for VLClientList/VLContacts")]
        public void ClientLists01_02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);


                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                Assert.IsNotNull(list01);
                Assert.IsTrue(list01.Client == client1.ClientId);
                //We create a contact list:
                var list02 = systemManager.CreateClientList(client1.ClientId, "my-list-02");
                Assert.IsNotNull(list02);
                Assert.IsTrue(list02.Client == client1.ClientId);
                //We create a contact list:
                var list03 = systemManager.CreateClientList(client2.ClientId, "my-list-02");
                Assert.IsNotNull(list03);
                Assert.IsTrue(list03.Client == client2.ClientId);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetClientLists(client1).Count == 2);
                Assert.IsTrue(systemManager.GetClientLists(client2).Count == 1);


                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 0);



                #region list01
                //Δημιουργούμε μία νέα επαφή στην λίστα01
                var contact01 = systemManager.CreateContact(list01.ListId, "11297@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(contact01);
                Assert.IsTrue(contact01.ClientId == client1.ClientId);
                Assert.IsTrue(contact01.ListId == list01.ListId);
                Assert.IsNull(contact01.Title);
                Assert.IsNull(contact01.Organization);
                Assert.IsNull(contact01.Department);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", contact01.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", contact01.LastName);
                Assert.AreEqual<string>("11297@nbg.gr", contact01.Email);
                Assert.IsNull(contact01.Comment);
                var svdContact = systemManager.GetContactById(contact01.ContactId);
                Assert.AreEqual<VLContact>(contact01, svdContact);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 1);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 0);

                //το email μέσα σε κάθε λίστα είναι μοναδικό:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateContact(list01.ListId, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));


                //Δημιουργούμε μία νέα επαφή στην λίστα01
                var contact02 = systemManager.CreateContact(list01.ListId, "12072@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(contact02);
                Assert.IsTrue(contact02.ClientId == client1.ClientId);
                Assert.IsTrue(contact02.ListId == list01.ListId);
                Assert.IsNull(contact02.Title);
                Assert.IsNull(contact02.Organization);
                Assert.IsNull(contact02.Department);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", contact02.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", contact02.LastName);
                Assert.AreEqual<string>("12072@nbg.gr", contact02.Email);
                Assert.IsNull(contact02.Comment);
                svdContact = systemManager.GetContactById(contact02.ContactId);
                Assert.AreEqual<VLContact>(contact02, svdContact);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 0);
                #endregion

                #region list02
                //Δημιουργούμε μία νέα επαφή στην λίστα02
                var contact03 = systemManager.CreateContact(list02.ListId, "11297@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(contact03);
                Assert.IsTrue(contact03.ClientId == client1.ClientId);
                Assert.IsTrue(contact03.ListId == list02.ListId);
                Assert.IsNull(contact03.Title);
                Assert.IsNull(contact03.Organization);
                Assert.IsNull(contact03.Department);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", contact03.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", contact03.LastName);
                Assert.AreEqual<string>("11297@nbg.gr", contact03.Email);
                Assert.IsNull(contact03.Comment);
                svdContact = systemManager.GetContactById(contact03.ContactId);
                Assert.AreEqual<VLContact>(contact03, svdContact);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 1);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 0);


                //το email μέσα σε κάθε λίστα είναι μοναδικό:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateContact(list02.ListId, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateContact(list01.ListId, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));


                //Δημιουργούμε μία νέα επαφή στην λίστα01
                var contact04 = systemManager.CreateContact(list02.ListId, "12072@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(contact04);
                Assert.IsTrue(contact04.ClientId == client1.ClientId);
                Assert.IsTrue(contact04.ListId == list02.ListId);
                Assert.IsNull(contact04.Title);
                Assert.IsNull(contact04.Organization);
                Assert.IsNull(contact04.Department);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", contact04.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", contact04.LastName);
                Assert.AreEqual<string>("12072@nbg.gr", contact04.Email);
                Assert.IsNull(contact04.Comment);
                svdContact = systemManager.GetContactById(contact04.ContactId);
                Assert.AreEqual<VLContact>(contact04, svdContact);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 0);
                #endregion


                #region list03
                //Δημιουργούμε μία νέα επαφή στην λίστα03
                var contact05 = systemManager.CreateContact(list03.ListId, "11297@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(contact05);
                Assert.IsTrue(contact05.ClientId == client2.ClientId);
                Assert.IsTrue(contact05.ListId == list03.ListId);
                Assert.IsNull(contact05.Title);
                Assert.IsNull(contact05.Organization);
                Assert.IsNull(contact05.Department);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", contact05.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", contact05.LastName);
                Assert.AreEqual<string>("11297@nbg.gr", contact05.Email);
                Assert.IsNull(contact05.Comment);
                svdContact = systemManager.GetContactById(contact05.ContactId);
                Assert.AreEqual<VLContact>(contact05, svdContact);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 1);


                //το email μέσα σε κάθε λίστα είναι μοναδικό:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateContact(list03.ListId, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateContact(list02.ListId, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateContact(list01.ListId, "11297@nbg.gr", "ΔΗΜΗΤΡΙΟΣ", "ΓΕΩΡΓΙΤΣΙΩΤΗΣ"); }, typeof(VLException));


                //Δημιουργούμε μία νέα επαφή στην λίστα01
                var contact06 = systemManager.CreateContact(list03.ListId, "12072@nbg.gr", "ΑΙΚΑΤΕΡΙΝΗ", "ΚΟΤΣΙΦΑΚΗ");
                Assert.IsNotNull(contact06);
                Assert.IsTrue(contact06.ClientId == client2.ClientId);
                Assert.IsTrue(contact06.ListId == list03.ListId);
                Assert.IsNull(contact06.Title);
                Assert.IsNull(contact06.Organization);
                Assert.IsNull(contact06.Department);
                Assert.AreEqual<string>("ΑΙΚΑΤΕΡΙΝΗ", contact06.FirstName);
                Assert.AreEqual<string>("ΚΟΤΣΙΦΑΚΗ", contact06.LastName);
                Assert.AreEqual<string>("12072@nbg.gr", contact06.Email);
                Assert.IsNull(contact06.Comment);
                svdContact = systemManager.GetContactById(contact06.ContactId);
                Assert.AreEqual<VLContact>(contact06, svdContact);

                //Καταμέτρηση
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 2);
                Assert.IsTrue(systemManager.GetContacts(list03.ListId).Count == 2);
                #endregion
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


        [TestMethod, Description("Import operations #1 for VLClientList/VLContacts")]
        public void clientList01_03()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);


                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                Assert.IsNotNull(list01);
                Assert.IsTrue(list01.Client == client1.ClientId);
                //We create a contact list:
                var list02 = systemManager.CreateClientList(client2.ClientId, "my-list-02");
                Assert.IsNotNull(list02);
                Assert.IsTrue(list02.Client == client2.ClientId);



                //Εισαγωγή 1000 γραμμών
                ContactImportResult importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                }
                Assert.IsTrue(importResult.SuccesfullImports == 1000);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 1000);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);




                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.SuccesfullImports == 10);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);



                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10aa.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 10);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 10);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);





                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10b.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 8);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 8);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);




                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10c.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 6);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 2);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 6);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);



                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10ca.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 6);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 2);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 6);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);






                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10cb.csv"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 6);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 2);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 6);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);



                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImagesPath, "airplane1.jpg"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 206);
                Assert.IsTrue(importResult.SuccesfullImports == 0);
                Assert.IsTrue(importResult.InvalidEmails == 118);
                Assert.IsTrue(importResult.SameEmails == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);



                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                importResult = null;
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10d.txt"), FileMode.Open))
                {
                    importResult = systemManager.ImportContactsFromCSV(list01.ListId, fs, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, DelimiterCharacter="\t", EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 6);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 2);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 6);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);


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


        [TestMethod, Description("Import operations #2 for VLClientList/VLContacts")]
        public void clientList01_04()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);


                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                Assert.IsNotNull(list01);
                Assert.IsTrue(list01.Client == client1.ClientId);
                //We create a contact list:
                var list02 = systemManager.CreateClientList(client2.ClientId, "my-list-02");
                Assert.IsNotNull(list02);
                Assert.IsTrue(list02.Client == client2.ClientId);

                var s1 = "1,Rebecca,Johnston,rjohnston@twitterlist.org,Syria,115.25.131.108\n2,Donald,Henderson,dhenderson@wikizz.info,Niue,85.186.113.81\n3,Eugene,Wagner,ewagner@tanoodle.com,Cote d'Ivoire,225.48.163.209\n4,Philip,Rivera,privera@meezzy.edu,Marshall Islands,162.95.237.119\n5,Gary,Frazier,gfrazier@jayo.biz,Qatar,42.45.26.151\n6,Pamela,Campbell,pcampbell@devcast.net,Cameroon,242.206.197.185\n7,Louise,Hunt,lhunt@roodel.info,Bermuda,228.174.174.112\n8,Rebecca,Sullivan,rsullivan@mydo.gov,Morocco,10.5.33.157\n9,Scott,Cole,scole@roomm.net,Austria,182.94.80.70\n10,Brian,Bishop,bbishop@skibox.name,Laos,60.120.120.198\n";
                //Εισαγωγή 10 γραμμών
                ContactImportResult importResult = null;
                importResult = systemManager.ImportContactsFromString(list01.ListId, s1, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 10);
                Assert.IsTrue(importResult.InvalidEmails == 0);
                Assert.IsTrue(importResult.SameEmails == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 10);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);



                //Δημιουργία ξανά της list01:
                list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");
                var s2 = "1,Rebecca,Johnston,,Syria,115.25.131.108\n2,Donald,Henderson,,Niue,85.186.113.81\n3,Eugene,Wagner,ewagner@tanoodle.com,Cote d'Ivoire,225.48.163.209\n4,Philip,Rivera,privera@meezzy.edu,Marshall Islands,162.95.237.119\n5,Gary,Frazier,gfrazier@jayo.biz,Qatar,42.45.26.151\n6,Pamela,Campbell,pcampbell@devcast.net,Cameroon,242.206.197.185\n7,Louise,Hunt,lhunt@roodel.info,Bermuda,228.174.174.112\n8,Rebecca,Sullivan,rsullivan@mydo.gov,Morocco,10.5.33.157\n9,Scott,Cole,scole@roomm.net,Austria,182.94.80.70\n10,Brian,Bishop,bbishop@skibox.name,Laos,60.120.120.198\n";
                //Εισαγωγή 10 γραμμών
                importResult = null;
                importResult = systemManager.ImportContactsFromString(list01.ListId, s2, new ContactImportOptions { HasHeaderRecord = false, ContinueOnError = true, EmailOrdinal = 4, FirstNameOrdinal = 2, LastNameOrdinal = 3 });
                Assert.IsTrue(importResult.FailedImports == 0);
                Assert.IsTrue(importResult.SuccesfullImports == 8);
                Assert.IsTrue(importResult.InvalidEmails == 2);
                Assert.IsTrue(importResult.SameEmails == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 8);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);
                //Διαγραφή της list01:
                systemManager.DeleteClientList(list01);
                Assert.IsNull(systemManager.GetClientListById(list01.ListId));
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);
                Assert.IsTrue(systemManager.GetContacts(list02.ListId).Count == 0);

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
    }
}
