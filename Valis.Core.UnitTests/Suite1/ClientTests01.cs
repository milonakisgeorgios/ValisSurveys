using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ClientTests01 : AdminBaseClass
    {

        

        [TestMethod, Description("CRUD operations for VLClient")]
        public void ClientTests01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(systemManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(systemManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //δεν εχουμε κανένα client:
                Assert.IsTrue(systemManager.GetClients().Count == 0);

                //δημιουργούμε ένα client:
                var client1 = systemManager.CreateClient("Thomas Cookings S.A.", BuiltinCountries.Greece, "THMC", profile: BuiltinProfiles.UTESTFree.ProfileId);
                #region
                Assert.IsNotNull(client1);
                Assert.AreEqual<string>(client1.Code, "THMC");
                Assert.AreEqual<string>(client1.Name, "Thomas Cookings S.A.");
                Assert.IsNull(client1.Profession);
                Assert.IsTrue(client1.Country == BuiltinCountries.Greece);
                Assert.IsNull(client1.Town);
                Assert.IsNull(client1.Address);
                Assert.IsNull(client1.Zip);
                Assert.IsNull(client1.Telephone1);
                Assert.IsNull(client1.Telephone2);
                Assert.IsNull(client1.WebSite);
                Assert.IsFalse(client1.IsBuiltIn);
                Assert.IsNull(client1.Comment);
                Assert.IsTrue(client1.FolderSequence == 0);
                var svdClient1 = systemManager.GetClientByName(client1.Name);
                Assert.AreEqual<VLClient>(client1, svdClient1);
                svdClient1 = systemManager.GetClientById(client1.ClientId);
                Assert.AreEqual<VLClient>(client1, svdClient1);
                #endregion


                Assert.IsTrue(systemManager.GetClients().Count == 1);

                //Δεν μπορούμε να δημιουργήσουμε πελάτη με όνομα που υπάρχει:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateClient("Thomas Cookings S.A.", BuiltinCountries.Germany, "FGVVV1", profile: BuiltinProfiles.UTESTFree.ProfileId); }, typeof(VLException));


                //δημιουργούμε ένα client:
                var client2 = systemManager.CreateClient("Παπαδοπουλος Μπισκοτα", BuiltinCountries.Greece, "ΠΑΠΑ-ΜΠΙΣΚ", profile: BuiltinProfiles.UTESTFree.ProfileId);
                #region
                Assert.IsNotNull(client2);
                Assert.AreEqual<string>(client2.Code, "ΠΑΠΑ-ΜΠΙΣΚ");
                Assert.AreEqual<string>(client2.Name, "Παπαδοπουλος Μπισκοτα");
                Assert.IsNull(client2.Profession);
                Assert.IsTrue(client2.Country == BuiltinCountries.Greece);
                Assert.IsNull(client2.Town);
                Assert.IsNull(client2.Address);
                Assert.IsNull(client2.Zip);
                Assert.IsNull(client2.Telephone1);
                Assert.IsNull(client2.Telephone2);
                Assert.IsNull(client2.WebSite);
                Assert.IsFalse(client2.IsBuiltIn);
                Assert.IsNull(client2.Comment);
                Assert.IsTrue(client2.FolderSequence == 0);
                var svdclient2 = systemManager.GetClientByName(client2.Name);
                Assert.AreEqual<VLClient>(client2, svdclient2);
                svdclient2 = systemManager.GetClientById(client2.ClientId);
                Assert.AreEqual<VLClient>(client2, svdclient2);
                #endregion


                Assert.IsTrue(systemManager.GetClients().Count == 2);


                //Κάνουμε update:
                client1.Prefecture = "Νομός Ηλείας";
                client1.Town = "Λεχαινά";
                client1.Address = "Κεντρική οδός ρετούνης, μετά τον στάβλο του κυρ- γιάννη";
                client1.Profession = "Γεωργικά Μηχανήματα";
                client1.Code = "COOKINGS";
                client1 = systemManager.UpdateClient(client1);
                #region
                Assert.IsNotNull(client1);
                Assert.AreEqual<string>(client1.Code, "COOKINGS");
                Assert.AreEqual<string>(client1.Name, "Thomas Cookings S.A.");
                Assert.AreEqual<string>(client1.Profession, "Γεωργικά Μηχανήματα");
                Assert.IsTrue(client1.Country == BuiltinCountries.Greece);
                Assert.AreEqual<string>(client1.Prefecture, "Νομός Ηλείας");
                Assert.AreEqual<string>(client1.Town, "Λεχαινά");
                Assert.AreEqual<string>(client1.Address, "Κεντρική οδός ρετούνης, μετά τον στάβλο του κυρ- γιάννη");
                Assert.IsNull(client1.Zip);
                Assert.IsNull(client1.Telephone1);
                Assert.IsNull(client1.Telephone2);
                Assert.IsNull(client1.WebSite);
                Assert.IsFalse(client1.IsBuiltIn);
                Assert.IsNull(client1.Comment);
                Assert.IsTrue(client1.FolderSequence == 0);
                svdClient1 = systemManager.GetClientByName(client1.Name);
                Assert.AreEqual<VLClient>(client1, svdClient1);
                svdClient1 = systemManager.GetClientById(client1.ClientId);
                Assert.AreEqual<VLClient>(client1, svdClient1);
                #endregion


                Assert.IsTrue(systemManager.GetClients().Count == 2);
                //διαγραφή
                systemManager.DeleteClient(client2);
                Assert.IsNull(systemManager.GetClientById(client2.ClientId));

                Assert.IsTrue(systemManager.GetClients().Count == 1);

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


        [TestMethod, Description("VLClient and VLClientProfiles!")]
        public void ClientTests01_02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);


            try
            {
                //Δημιουργούμε ένα client. Το UseCredits παίρνει την τιμή που φέρει το profile του:
                Assert.IsFalse(BuiltinProfiles.UTESTFree.UseCredits);
                var client = systemManager.CreateClient("Thomas Cooking S.A.", BuiltinCountries.Greece, "THMC", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);
                Assert.IsTrue(client.Profile == BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsFalse(client.UseCredits);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client, "s1");
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.Client == client.ClientId);
                //Δημιουργούμε ένα collector:
                var collector1 = surveyManager.CreateCollector(survey1, CollectorType.WebLink, "c1");
                Assert.IsNotNull(collector1);
                Assert.IsTrue(collector1.Profile == BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsFalse(collector1.UseCredits);
                Assert.IsNull(collector1.CreditType);


                //Αλλάζουμε το profile του πελάτη σε profile που χρεώνεται:
                client.Profile = BuiltinProfiles.UTESTPaid.ProfileId;
                client = systemManager.UpdateClient(client);
                Assert.IsNotNull(client);
                Assert.IsTrue(client.Profile == BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsTrue(client.UseCredits);

                //Αν και άλλαξε το UseCredits, για τον πελάτη μας, ότι έχει δημιουργήσει κρατάει το προηγούμενο UseCredits & Profile:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsNotNull(survey1);
                collector1 = surveyManager.GetCollectorById(collector1.CollectorId);
                Assert.IsNotNull(collector1);
                Assert.IsTrue(collector1.Profile == BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsFalse(collector1.UseCredits);
                Assert.IsNull(collector1.CreditType);

                //Οτι καινούργιο δημιουργήσει ο client, θα φέρει το νέο Profile & UseCredits:
                var survey2 = surveyManager.CreateSurvey(client, "s2");
                Assert.IsNotNull(survey2);
                Assert.IsTrue(survey2.Client == client.ClientId);
                //Δημιουργούμε ένα collector:
                var collector2 = surveyManager.CreateCollector(survey2, CollectorType.WebLink, "c2", CreditType.ClickType);
                Assert.IsNotNull(collector2);
                Assert.IsTrue(collector2.Profile == BuiltinProfiles.UTESTPaid.ProfileId);
                Assert.IsTrue(collector2.UseCredits);
                Assert.IsTrue(collector2.CreditType == CreditType.ClickType);



                
            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    var surveys = surveyManager.GetSurveys();
                    foreach(var s in surveys)
                    {
                        surveyManager.UnitTesting_DestroySurvey(s.SurveyId);
                    }
                    systemManager.DeleteClient(client);
                }
            }
        }
    }
}
