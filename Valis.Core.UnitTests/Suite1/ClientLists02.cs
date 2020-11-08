using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Valis.Core.UnitTests.Suite1
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ClientLists02 : AdminBaseClass
    {

        [TestMethod, Description("OptedOut/Bounce and Remove operations #1, for VLClientList/Contacts")]
        public void ClientLists02_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                //We create a contact list:
                var list01 = systemManager.CreateClientList(client1.ClientId, "my-list-01");

                //Η λίστα μας είναι άδεια:
                Assert.IsTrue(list01.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01.ListId).Count == 0);

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
                var contacts = systemManager.GetContacts(list01.ListId);
                foreach(var item in contacts)
                {
                    Assert.IsFalse(item.IsBouncedEmail);
                    Assert.IsFalse(item.IsOptedOut);
                }

                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);

                //Κάνουμε bounced τρία emails:
                var email01b = contacts[0].Email;
                var email02b = contacts[1].Email;
                var email03b = contacts[2].Email;

                Assert.IsTrue(systemManager.BounceContacts(client1, email01b) == 1);
                Assert.IsTrue(systemManager.BounceContacts(client1, email02b) == 1);
                Assert.IsTrue(systemManager.BounceContacts(client1, email03b) == 1);

                //Τώρα έχουμε 3 Bounced Emails:
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);

                //Κάνουμε optedOut τρία emails:
                var email01o = contacts[7].Email;
                var email02o = contacts[8].Email;
                var email03o = contacts[9].Email;

                Assert.IsTrue(systemManager.OptOutContacts(client1, email01o) == 1);
                Assert.IsTrue(systemManager.OptOutContacts(client1, email02o) == 1);
                Assert.IsTrue(systemManager.OptOutContacts(client1, email03o) == 1);

                //Τώρα έχουμε 3 Bounced Emails & 3 OptedOut Emails:
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);


                //Θα διαγράψουμε τα OptedOut Emails:
                Assert.IsTrue(systemManager.RemoveAllOptedOutContactsFromList(list01.ListId) == 3);

                //Ελέγχουμε:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(list01.TotalContacts == 7);
                Assert.IsTrue(systemManager.GetContacts(list01).Count == 7);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 3);


                //Θα διαγράψουμε τα Bounced Emails:
                Assert.IsTrue(systemManager.RemoveAllBouncedContactsFromList(list01.ListId) == 3);

                //Ελέγχουμε:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(list01.TotalContacts == 4);
                Assert.IsTrue(systemManager.GetContacts(list01).Count == 4);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);


                //Θα διαγράψουμε όλα τα contacts απο αυτή την λίστα:
                Assert.IsTrue(systemManager.RemoveAllContactsFromList(list01.ListId) == 4);

                //Ελέγχουμε:
                list01 = systemManager.GetClientListById(list01.ListId);
                Assert.IsTrue(list01.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01).Count == 0);
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


        /// <summary>
        /// Εδώ θα ελέγξουμε ότι ότνα κάνουμε bounce ή OptedOut μία επαφή αυτή γίνεται σε όλες τις λίστες στις οποίες βρίσκεται:
        /// </summary>
        [TestMethod, Description("OptedOut/Bounce and Remove operations #1, for VLClientList/Contacts")]
        public void ClientLists02_02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργία δύο πελατών με λίστες που περιέχουν ίδιες επαφές:
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create a contact list:
                var list01a = systemManager.CreateClientList(client1.ClientId, "my-list-01a");
                //We create a contact list:
                var list01b = systemManager.CreateClientList(client1.ClientId, "my-list-01b");


                //Εισαγωγή 10 γραμμών στην list01a
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01a.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                systemManager.CreateContact(list01a.ListId, "milonakis@hotmail.com", "George", "milonakis");

                //Εισαγωγή 10 γραμμών στην list01b
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list01b.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }


                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);
                //We create a contact list:
                var list02 = systemManager.CreateClientList(client2.ClientId, "my-list-02");

                //Εισαγωγή 10 γραμμών στην list02
                using (var fs = new FileStream(Path.Combine(_ImportFilesPath, "MOCK_DATA_10a.csv"), FileMode.Open))
                {
                    systemManager.ImportContactsFromCSV(list02.ListId, fs, new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 });
                }
                #endregion


                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 11);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 11);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 0);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 0);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);

                
                //Κάνουμε bounce ένα μοναδικό email που βρίσκεται μόνο στην list01a:
                Assert.IsTrue(systemManager.BounceContacts(client1, "milonakis@hotMAIL.Com") == 1);

                #region ΕΛΕΓΧΟΥΜΕ:
                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 11);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 11);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 1);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 0);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
                #endregion

                //Κάνουμε bounce τρία (3) email που βρίσκεται στην list01a και στην list01b:
                Assert.IsTrue(systemManager.BounceContacts(client1, "aellis@oyoloo.gov") == 2);
                Assert.IsTrue(systemManager.BounceContacts(client1, "gcrawford@tambee.edu") == 2);
                Assert.IsTrue(systemManager.BounceContacts(client1, "chart@wordtune.name") == 2);

                #region ΕΛΕΓΧΟΥΜΕ:
                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 11);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 11);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 4);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 3);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
                #endregion


                //Κάνουμε optedout τρία (3) email που βρίσκonται στην list01a και στην list01b:
                Assert.IsTrue(systemManager.OptOutContacts(client1, "jcox@npath.mil") == 2);
                Assert.IsTrue(systemManager.OptOutContacts(client1, "bmitchell@linktype.mil") == 2);
                Assert.IsTrue(systemManager.OptOutContacts(client1, "nruiz@yadel.edu") == 2);

                #region ΕΛΕΓΧΟΥΜΕ:
                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 11);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 11);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 4);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 3);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
                #endregion



                //ΔΙΑΓΡΑΦΕΣ:
                Assert.IsTrue(systemManager.RemoveByDomainContactsFromList(list01a.ListId, "oyoloo.gov") == 1);
                Assert.IsTrue(systemManager.RemoveByDomainContactsFromList(list02.ListId, "oyoloo.gov") == 1);

                #region ΕΛΕΓΧΟΥΜΕ:
                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 3);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 3);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 9);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 9);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
                #endregion


                //ΔΙΑΓΡΑΦΕΣ:
                Assert.IsTrue(systemManager.RemoveByDomainContactsFromList(list01a.ListId, "hotmail.com") == 1);
                Assert.IsTrue(systemManager.RemoveByDomainContactsFromList(list01b.ListId, "hotmail.com") == 0);
                Assert.IsTrue(systemManager.RemoveByDomainContactsFromList(list02.ListId, "hotmail.com") == 0);

                #region ΕΛΕΓΧΟΥΜΕ:
                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 9);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 9);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 2);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 10);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 10);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 3);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 9);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 9);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
                #endregion


                //ΔΙΑΓΡΑΦΕΣ:
                Assert.IsTrue(systemManager.RemoveAllContactsFromList(list01b.ListId) == 10);

                #region ΕΛΕΓΧΟΥΜΕ:
                list01a = systemManager.GetClientListById(list01a.ListId);
                Assert.IsTrue(list01a.TotalContacts == 9);
                Assert.IsTrue(systemManager.GetContacts(list01a).Count == 9);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01a).Count == 3);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01a).Count == 2);

                list01b = systemManager.GetClientListById(list01b.ListId);
                Assert.IsTrue(list01b.TotalContacts == 0);
                Assert.IsTrue(systemManager.GetContacts(list01b).Count == 0);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list01b).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list01b).Count == 0);

                list02 = systemManager.GetClientListById(list02.ListId);
                Assert.IsTrue(list02.TotalContacts == 9);
                Assert.IsTrue(systemManager.GetContacts(list02).Count == 9);
                Assert.IsTrue(systemManager.GetOptedOutContacts(list02).Count == 0);
                Assert.IsTrue(systemManager.GetBouncedContacts(list02).Count == 0);
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

    }
}
