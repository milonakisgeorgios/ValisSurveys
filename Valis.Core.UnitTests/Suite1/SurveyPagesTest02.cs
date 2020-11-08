using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class SurveyPagesTest02 : AdminBaseClass
    {


        [TestMethod, Description("Delete operations for VLSurveyPage")]
        public void SurveyPagesTest02_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);


            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);

                //δημιουργούμε ένα survey στην Αγγλική γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client.ClientId, "K-12 Parent Survey", "K-12 Parent Survey", "In this survey, we are interested in learning more about your thoughts, feelings, and attitudes towards your child's school", textsLanguage: BuiltinLanguages.English);
                Assert.IsNotNull(survey1);


                //page1
                var page1 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsTrue(page1.DisplayOrder == 1);
                //Δημιουργούμε ερωτήσεις για το page1:
                var q1 = surveyManager.CreateQuestion(page1, QuestionType.SingleLine, "Question #1");
                Assert.IsTrue(q1.Page == page1.PageId);



                //page2
                var page2 = surveyManager.CreateSurveyPage(survey1, null, "Page #2");
                Assert.IsTrue(page2.DisplayOrder == 2);
                //Δημιουργούμε ερωτήσεις για το page2:
                var q2 = surveyManager.CreateQuestion(page2, QuestionType.SingleLine, "Question #2");
                Assert.IsTrue(q2.Page == page2.PageId);




                //page3
                var page3 = surveyManager.CreateSurveyPage(survey1, null, "Page #3");
                Assert.IsTrue(page3.DisplayOrder == 3);
                //Δημιουργούμε ερωτήσεις για το page3:
                var q3 = surveyManager.CreateQuestion(page3, QuestionType.SingleLine, "Question #3");
                Assert.IsTrue(q3.Page == page3.PageId);



                //page4
                var page4 = surveyManager.CreateSurveyPage(survey1, null, "Page #4");
                Assert.IsTrue(page4.DisplayOrder == 4);
                //Δημιουργούμε ερωτήσεις για το page4:
                var q4 = surveyManager.CreateQuestion(page4, QuestionType.SingleLine, "Question #4");
                Assert.IsTrue(q4.Page == page4.PageId);


                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 4);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(survey1).Count == 4);



                //Διαγράφουμε την page3 (και αυτόματα διαγράφει και την ερώτηση q3):
                surveyManager.DeleteSurveyPage(page3);

                Assert.IsNull(surveyManager.GetSurveyPageById(page3.Survey, page3.PageId));
                Assert.IsNull(surveyManager.GetQuestionById(q3.Survey, q3.QuestionId));
                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 3);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(survey1).Count == 3);


                //διαγράφουμε την page4 (αλλά όχι και την ερώτηση q4):
                surveyManager.DeleteSurveyPage(page4, DeleteQuestionsBehavior.MoveAbove);

                Assert.IsNull(surveyManager.GetSurveyPageById(page4.Survey, page4.PageId));
                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 2);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(survey1).Count == 3);

                q4 = surveyManager.GetQuestionById(q4.Survey, q4.QuestionId);
                Assert.IsNotNull(q4);
                Assert.IsTrue(q4.Page == page2.PageId); //MoveAbove


                _EXECUTEAndCATCH(delegate { surveyManager.DeleteSurveyPage(page2, DeleteQuestionsBehavior.MoveBellow); });
                _EXECUTEAndCATCH(delegate { surveyManager.DeleteSurveyPage(page1, DeleteQuestionsBehavior.MoveAbove); });


                //διαγράφουμε την page1 (αλλά όχι και την ερώτηση q1):
                surveyManager.DeleteSurveyPage(page1, DeleteQuestionsBehavior.MoveBellow);

                Assert.IsNull(surveyManager.GetSurveyPageById(page1.Survey, page1.PageId));
                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(survey1).Count == 3);


                Assert.IsTrue(surveyManager.GetQuestionsForPage(survey1.SurveyId, page2.PageId).Count == 3);
                q1 = surveyManager.GetQuestionById(q1.Survey, q1.QuestionId);
                Assert.IsTrue(q1.Page == page2.PageId);
                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId);
                Assert.IsTrue(q2.Page == page2.PageId);
                q4 = surveyManager.GetQuestionById(q4.Survey, q4.QuestionId);
                Assert.IsTrue(q4.Page == page2.PageId);
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
