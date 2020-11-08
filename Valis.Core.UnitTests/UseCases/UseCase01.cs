using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Valis.Core.UnitTests.UseCases
{
    [TestClass]
    public class UseCase01 : SurveyFacilityBaseClass
    {

        [TestMethod, Description("")]
        public void UseCase01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {

                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MatterWare S.A.", BuiltinCountries.Greece, "MABS", profile : BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                #region Δημιουργούμε ενα account για τον πελάτη μας
                var contact01 = systemManager.CreateClientAccount(client1.ClientId, "John","Pitaris", BuiltinRoles.PowerClient.RoleId, "pitarakis@hotmail.com","gmil","tolk!3n",isApproved: true);
                #endregion

                //Κάνει login το account του πελάτη μας:
                var ac = valisSystem.LogOnUser("gmil", "tolk!3n");
                var surveyManager = VLSurveyManager.GetAnInstance(ac);

                //Δημιουργούμε δύο surveys
                var survey1 = CreateSurvey1(surveyManager, client1, "Demo Survey #1");


                var survey2 = CreateSurvey2(surveyManager, client1, "Demo Survey #2", "competency assessment (demo-survey-2)");
                CreateResponsesForSurvey2(surveyManager, survey2, 300, 220, 800);
            
            }
            finally
            {
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(admin); ;
                if (surveyManager!= null)
                {
                    var surveys = surveyManager.GetSurveys();
                    foreach (var item in surveys)
                    {
                        if (item.IsBuiltIn)
                            continue;

                        var collectors = surveyManager.GetCollectors(item.SurveyId);
                        foreach(var c in collectors)
                        {
                            if(c.Status == CollectorStatus.Open)
                                surveyManager.CloseCollector(c);
                        }

                        surveyManager.DeleteSurvey(item);
                    }
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
