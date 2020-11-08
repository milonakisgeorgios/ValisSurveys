using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Globalization;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class FileTests01 : AdminBaseClass
    {

        //[DeploymentItem("Images", "Images")]
        [TestMethod, Description("")]
        public void FileTests01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);


            try
            {
                //δημιουργούμε ένα client:
                var client1 = systemManager.CreateClient("Thomas Cookings S.A.", BuiltinCountries.Greece, "THMC", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //Δημιουργούμε ένα νέο survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #1", "Risk assessment");
                Assert.IsNotNull(survey1);

                //Στην αρχή δεν υπάρχει κανένα αρχείο για αυτό το survey:
                Assert.IsTrue(surveyManager.GetFiles(survey1).Count == 0);

                VLFile file1 = null;
                using (var fs = new FileStream(Path.Combine(_ImagesPath, "image1.jpg"), FileMode.Open))
                {
                    file1 = surveyManager.AssignFile(survey1, fs, "image1.jpg");
                }
                Assert.IsNotNull(file1);
                Assert.IsTrue(file1.Client == survey1.Client);
                Assert.IsTrue(file1.Survey == survey1.SurveyId);
                Assert.AreEqual<string>("image1.jpg", file1.OriginalFileName);
                Assert.AreEqual<string>("image1.jpg", file1.ManagedFileName);
                Assert.AreEqual<string>(".jpg", file1.Extension);
                Assert.IsTrue(file1.Size == 52077);
                Assert.IsTrue(file1.IsPhysicalFile);
                Assert.IsTrue(file1.InventoryPath == Path.Combine(survey1.Client.ToString(CultureInfo.InvariantCulture), survey1.SurveyId.ToString(CultureInfo.InvariantCulture)) + "\\");
                var svdFile1 = surveyManager.GetFileById(file1.FileId);
                Assert.AreEqual<VLFile>(file1, svdFile1);


                //τώρα έχουμε ένα αρχείο για αυτό το survey
                Assert.IsTrue(surveyManager.GetFiles(survey1).Count == 1);


                VLFile file2 = null;
                using (var fs = new FileStream(Path.Combine(_ImagesPath, "image2.jpg"), FileMode.Open))
                {
                    file2 = surveyManager.AssignFile(survey1, fs, "image2.jpg");
                }
                Assert.IsNotNull(file2);
                Assert.IsTrue(file2.Client == survey1.Client);
                Assert.IsTrue(file2.Survey == survey1.SurveyId);
                Assert.AreEqual<string>("image2.jpg", file2.OriginalFileName);
                Assert.AreEqual<string>("image2.jpg", file2.ManagedFileName);
                Assert.AreEqual<string>(".jpg", file2.Extension);
                Assert.IsTrue(file2.Size == 33013);
                Assert.IsTrue(file2.IsPhysicalFile);
                Assert.IsTrue(file2.InventoryPath == Path.Combine(survey1.Client.ToString(CultureInfo.InvariantCulture), survey1.SurveyId.ToString(CultureInfo.InvariantCulture)) + "\\");
                var svdfile2 = surveyManager.GetFileById(file2.FileId);
                Assert.AreEqual<VLFile>(file2, svdfile2);

                //τώρα έχουμε δύο αρχεία για αυτό το survey
                Assert.IsTrue(surveyManager.GetFiles(survey1).Count == 2);


                VLFile file3 = null;
                using (var fs = new FileStream(Path.Combine(_ImagesPath, "image2.jpg"), FileMode.Open))
                {
                    file3 = surveyManager.AssignFile(survey1, fs, "image2.jpg");
                }
                Assert.IsNotNull(file3);
                Assert.IsTrue(file3.Client == survey1.Client);
                Assert.IsTrue(file3.Survey == survey1.SurveyId);
                Assert.AreEqual<string>("image2.jpg", file3.OriginalFileName);
                Assert.AreEqual<string>("image2_1.jpg", file3.ManagedFileName);
                Assert.AreEqual<string>(".jpg", file3.Extension);
                Assert.IsTrue(file3.Size == 33013);
                Assert.IsTrue(file3.IsPhysicalFile);
                Assert.IsTrue(file3.InventoryPath == Path.Combine(survey1.Client.ToString(CultureInfo.InvariantCulture), survey1.SurveyId.ToString(CultureInfo.InvariantCulture)) + "\\");
                var svdfile3 = surveyManager.GetFileById(file3.FileId);
                Assert.AreEqual<VLFile>(file3, svdfile3);


                //τώρα έχουμε τρία αρχεία για αυτό το survey
                Assert.IsTrue(surveyManager.GetFiles(survey1).Count == 3);


                //διαγράφουμε το ένα
                surveyManager.Removefile(file3.FileId);
                Assert.IsNull(surveyManager.GetFileById(file3.FileId));
                //τώρα έχουμε δύο αρχεία για αυτό το survey
                Assert.IsTrue(surveyManager.GetFiles(survey1).Count == 2);


                //διαγράφουμε το ένα
                surveyManager.Removefile(file2.FileId);
                Assert.IsNull(surveyManager.GetFileById(file2.FileId));
                //τώρα έχουμε δύο αρχεία για αυτό το survey
                Assert.IsTrue(surveyManager.GetFiles(survey1).Count == 1);


            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    var surveys = surveyManager.GetSurveysForClient(client.ClientId);
                    foreach(var survey in surveys)
                    {
                        surveyManager.DeleteSurvey(survey.SurveyId);
                    }

                    systemManager.DeleteClient(client);
                }
            }
        }



    }
}
