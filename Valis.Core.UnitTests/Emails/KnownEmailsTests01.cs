using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Valis.Core.UnitTests.Emails
{
    [TestClass]
    public class KnownEmailsTests01 : AdminBaseClass
    {

        [TestMethod, Description("CRUD tests for VLKnownEmail")]
        public void KnownEmailsTests01_01()
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

                //Δεν υπάρχει κανένα KnownEmail:
                Assert.IsTrue(systemManager.GetKnownEmails().Count == 0);
                Assert.IsTrue(systemManager.GetKnownEmails(client1.ClientId).Count == 0);

                //Δημιουργούμε ένα KnownEmail:
                var email1 = systemManager.RegisterKnownEmail(client1.ClientId, "milonakis@hotmail.com", validateDomain: true);
                Assert.IsNotNull(email1);
                Assert.IsTrue(email1.Client == client1.ClientId);
                Assert.IsTrue(email1.DomainPart == "hotmail.com");
                Assert.IsTrue(email1.LocalPart == "milonakis");
                Assert.IsTrue(email1.EmailAddress == "milonakis@hotmail.com");
                Assert.IsNotNull(email1.RegisterDt);
                Assert.IsFalse(email1.IsBounced);
                Assert.IsTrue(email1.IsDomainOK);
                Assert.IsFalse(email1.IsOptedOut);
                Assert.IsFalse(email1.IsVerified);
                Assert.IsNull(email1.OptedOutDt);
                Assert.IsNull(email1.VerifiedDt);
                var svdEmail1 = systemManager.GetKnownEmailById(email1.EmailId);
                Assert.AreEqual<VLKnownEmail>(email1, svdEmail1);
                svdEmail1 = systemManager.GetKnownEmailByAddress(client1.ClientId, email1.EmailAddress);
                Assert.AreEqual<VLKnownEmail>(email1, svdEmail1);

                //Δεν μπορούμε να κάνουμε register email που δεν έχει valid format:
                _EXECUTEAndCATCHType(delegate { systemManager.RegisterKnownEmail(client1.ClientId, "", validateDomain: false); }, typeof(ArgumentNullException));
                _EXECUTEAndCATCHType(delegate { systemManager.RegisterKnownEmail(client1.ClientId, "assdasdasd", validateDomain: false); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.RegisterKnownEmail(client1.ClientId, "assd@asd@asd", validateDomain: false); }, typeof(VLException));

                //χωρίς validateDomain μπρούμε να εισαγουμε κάτι σκάρτο:
                var email2 = systemManager.RegisterKnownEmail(client1.ClientId, "test1222@gmilonakis.gr", validateDomain: false);
                Assert.IsNotNull(email2);

                //Μετράμε:
                Assert.IsTrue(systemManager.GetKnownEmails(client1.ClientId).Count == 2);
                //Διαγράφουμε το email2:
                systemManager.DeleteKnownEmail(email2.EmailId);
                //Μετράμε:
                Assert.IsTrue(systemManager.GetKnownEmails(client1.ClientId).Count == 1);

                //Με το validateDomain δεν μπορούμε να εισαγουμε κάτι σκάρτο:
                _EXECUTEAndCATCHType(delegate { systemManager.RegisterKnownEmail(client1.ClientId, "test1222@gmilonakis.gr", validateDomain: true); }, typeof(VLException));

                //Κάνουμε update:
                email1.IsOptedOut = true;
                email1.OptedOutDt = DateTime.UtcNow;
                email1.IsVerified = true;
                email1 = systemManager.UpdateKnownEmail(email1);
                Assert.IsTrue(email1.Client == client1.ClientId);
                Assert.IsTrue(email1.DomainPart == "hotmail.com");
                Assert.IsTrue(email1.LocalPart == "milonakis");
                Assert.IsTrue(email1.EmailAddress == "milonakis@hotmail.com");
                Assert.IsNotNull(email1.RegisterDt);
                Assert.IsFalse(email1.IsBounced);
                Assert.IsTrue(email1.IsDomainOK);
                Assert.IsTrue(email1.IsOptedOut);
                Assert.IsTrue(email1.IsVerified);
                Assert.IsNotNull(email1.OptedOutDt);
                Assert.IsNull(email1.VerifiedDt);
                svdEmail1 = systemManager.GetKnownEmailById(email1.EmailId);
                Assert.AreEqual<VLKnownEmail>(email1, svdEmail1);
                svdEmail1 = systemManager.GetKnownEmailByAddress(client1.ClientId, email1.EmailAddress);
                Assert.AreEqual<VLKnownEmail>(email1, svdEmail1);


                //Μετράμε:
                Assert.IsTrue(systemManager.GetKnownEmails(client1.ClientId).Count == 1);

                //Δεν μπορούμε να εισάουμε (για τον ίδιο πελάτη) εγγραφή με ίδιο EmailAddress:
                _EXECUTEAndCATCHType(delegate { systemManager.RegisterKnownEmail(client1.ClientId, "milonakis@hotmail.com", validateDomain: true); }, typeof(VLException));


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
