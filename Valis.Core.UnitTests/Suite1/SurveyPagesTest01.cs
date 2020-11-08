using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class SurveyPagesTest01 : AdminBaseClass
    {

        [TestMethod, Description("CRUD operations for VLSurveyPage")]
        public void SurveyPagesTest01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);

                //δημιουργούμε ένα survey στην Αγγλική γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client.ClientId, "K-12 Parent Survey", "K-12 Parent Survey", "In this survey, we are interested in learning more about your thoughts, feelings, and attitudes towards your child's school", textsLanguage: BuiltinLanguages.English);
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.English);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.English);
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);

                //εχει δημιουργηθεί αυτόματα μία σελίδα:
                var page0 = surveyManager.GetSurveyPages(survey1)[0];
                Assert.IsNotNull(page0);
                Assert.IsTrue(page0.DisplayOrder == 1);

                //page1
                var page1 = surveyManager.CreateSurveyPage(survey1, null, "In this first section, we'd like to learn more about some of your roles, beliefs, and attitudes as well as some of the activities that you do as the parent of a school-aged child.");
                Assert.IsNotNull(page1);
                Assert.IsTrue(page1.Survey == survey1.SurveyId);
                Assert.IsTrue(page1.TextsLanguage == survey1.TextsLanguage);
                Assert.IsNull(page1.ShowTitle);
                Assert.AreEqual<string>(page1.Description, "In this first section, we'd like to learn more about some of your roles, beliefs, and attitudes as well as some of the activities that you do as the parent of a school-aged child.");
                Assert.IsTrue(page1.DisplayOrder == 2);

                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);

                //page2
                var page2 = surveyManager.CreateSurveyPage(survey1, null, "In this section, we'd like to learn more about your perceptions of your child and your child's interactions with his/her school.");
                Assert.IsNotNull(page2);
                Assert.IsTrue(page2.Survey == survey1.SurveyId);
                Assert.IsTrue(page2.TextsLanguage == survey1.TextsLanguage);
                Assert.IsNull(page2.ShowTitle);
                Assert.AreEqual<string>(page2.Description, "In this section, we'd like to learn more about your perceptions of your child and your child's interactions with his/her school.");
                Assert.IsTrue(page2.DisplayOrder == 3);

                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 3);

                //page3
                var page3 = surveyManager.CreateSurveyPage(survey1, null, "In this section, we'd like to learn more about your perceptions of the overall climate at your child's school.");
                Assert.IsNotNull(page3);
                Assert.IsTrue(page3.Survey == survey1.SurveyId);
                Assert.IsTrue(page3.TextsLanguage == survey1.TextsLanguage);
                Assert.IsNull(page3.ShowTitle);
                Assert.IsTrue(page3.DisplayOrder == 4);

                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 4);

                //page4
                var page4 = surveyManager.CreateSurveyPage(survey1, null, "Your demographic information:");
                Assert.IsNotNull(page4);
                Assert.IsTrue(page4.Survey == survey1.SurveyId);
                Assert.IsTrue(page4.TextsLanguage == survey1.TextsLanguage);
                Assert.IsNull(page4.ShowTitle);
                Assert.IsTrue(page4.DisplayOrder == 5);

                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 5);


                //Διαβάζουμε όλες τις σελίδες του survey μας:
                var pages = surveyManager.GetSurveyPages(survey1);
                Assert.IsTrue(pages.Count == 5);
                foreach (var item in pages)
                {
                    var svdPage = surveyManager.GetSurveyPageById(item.Survey, item.PageId, item.TextsLanguage);
                    Assert.AreEqual<VLSurveyPage>(item, svdPage);
                }


                //Update page1:
                page1.ShowTitle = "page1 - showtitle";
                page1.Description = "page1 - description";
                page1 = surveyManager.UpdateSurveyPage(page1);
                Assert.IsNotNull(page1);
                Assert.IsTrue(page1.Survey == survey1.SurveyId);
                Assert.IsTrue(page1.TextsLanguage == survey1.TextsLanguage);
                Assert.AreEqual<string>(page1.ShowTitle, "page1 - showtitle");
                Assert.AreEqual<string>(page1.Description, "page1 - description");
                Assert.IsTrue(page1.DisplayOrder == 2);



                //Διαγράφουμε την page 2
                surveyManager.DeleteSurveyPage(page2);
                Assert.IsNull(surveyManager.GetSurveyPageById(page2.Survey, page2.PageId, page2.TextsLanguage));


                //Διαβάζουμε όλες τις σελίδες του survey μας:
                pages = surveyManager.GetSurveyPages(survey1);
                Assert.IsTrue(pages.Count == 4);

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




        [TestMethod, Description("Insert after/before operations for VLSurveyPage")]
        public void SurveyPagesTest01_02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);


            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);


                //δημιουργούμε ένα survey στην Αγγλική γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client.ClientId, "K-12 Parent Survey", "K-12 Parent Survey", "In this survey, we are interested in learning more about your thoughts, feelings, and attitudes towards your child's school", textsLanguage: BuiltinLanguages.English);
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.English);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.English);
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                Assert.IsTrue(survey1.TicketSequence == 0);


                var page1 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsNotNull(page1);
                Assert.IsTrue(page1.DisplayOrder == 1);
                var page2 = surveyManager.CreateSurveyPage(survey1, "This page's #2 title!");
                Assert.IsNotNull(page2);
                Assert.IsTrue(page2.DisplayOrder == 2);
                var page3 = surveyManager.CreateSurveyPage(survey1, "This page's #3 title!");
                Assert.IsNotNull(page3);
                Assert.IsTrue(page3.DisplayOrder == 3);


                var _nextPage = surveyManager.GetFirstSurveyPage(survey1);
                Assert.AreEqual<VLSurveyPage>(page1, _nextPage);
                _nextPage = surveyManager.GetNextSurveyPage(_nextPage);
                Assert.AreEqual<VLSurveyPage>(page2, _nextPage);
                _nextPage = surveyManager.GetNextSurveyPage(_nextPage);
                Assert.AreEqual<VLSurveyPage>(page3, _nextPage);



                /***************CreateSurveyPageBefore*/
                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
                #endregion

                var _testPage = surveyManager.CreateSurveyPageBefore(page1, "This page's #0 title!");
                Assert.IsTrue(_testPage.DisplayOrder == 1);
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 2);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 3);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 4);

                surveyManager.DeleteSurveyPage(_testPage);
                Assert.IsNull(surveyManager.GetSurveyPageById(_testPage.Survey, _testPage.PageId));



                /***************CreateSurveyPageBefore*/
                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
                #endregion

                _testPage = surveyManager.CreateSurveyPageBefore(page2, "This page's #0 title!");
                Assert.IsTrue(_testPage.DisplayOrder == 2);
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 3);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 4);

                surveyManager.DeleteSurveyPage(_testPage);
                Assert.IsNull(surveyManager.GetSurveyPageById(_testPage.Survey, _testPage.PageId));



                /***************CreateSurveyPageBefore*/
                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
                #endregion

                _testPage = surveyManager.CreateSurveyPageBefore(page3, "This page's #0 title!");
                Assert.IsTrue(_testPage.DisplayOrder == 3);
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 4);

                surveyManager.DeleteSurveyPage(_testPage);
                Assert.IsNull(surveyManager.GetSurveyPageById(_testPage.Survey, _testPage.PageId));



                /***************CreateSurveyPageAfter*/
                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
                #endregion

                _testPage = surveyManager.CreateSurveyPageAfter(page1, "This page's #0 title!");
                Assert.IsNotNull(_testPage);
                Assert.IsTrue(_testPage.DisplayOrder == 2);
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 3);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 4);

                surveyManager.DeleteSurveyPage(_testPage);
                Assert.IsNull(surveyManager.GetSurveyPageById(_testPage.Survey, _testPage.PageId));



                /***************CreateSurveyPageAfter*/
                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
                #endregion

                _testPage = surveyManager.CreateSurveyPageAfter(page2, "This page's #0 title!");
                Assert.IsNotNull(_testPage);
                Assert.IsTrue(_testPage.DisplayOrder == 3);
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 4);

                surveyManager.DeleteSurveyPage(_testPage);
                Assert.IsNull(surveyManager.GetSurveyPageById(_testPage.Survey, _testPage.PageId));



                /***************CreateSurveyPageAfter*/
                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
                #endregion

                _testPage = surveyManager.CreateSurveyPageAfter(page3, "This page's #0 title!");
                Assert.IsNotNull(_testPage);
                Assert.IsTrue(_testPage.DisplayOrder == 4);
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);

                surveyManager.DeleteSurveyPage(_testPage);
                Assert.IsNull(surveyManager.GetSurveyPageById(_testPage.Survey, _testPage.PageId));


                #region Test DisplayOrders:
                page1 = surveyManager.GetSurveyPageById(page1.Survey, page1.PageId);
                Assert.IsTrue(page1.DisplayOrder == 1);
                page2 = surveyManager.GetSurveyPageById(page2.Survey, page2.PageId);
                Assert.IsTrue(page2.DisplayOrder == 2);
                page3 = surveyManager.GetSurveyPageById(page3.Survey, page3.PageId);
                Assert.IsTrue(page3.DisplayOrder == 3);
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
