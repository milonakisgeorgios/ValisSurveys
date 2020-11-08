using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class Translations01 : AdminBaseClass
    {

        [TestMethod, Description("διαδικασία μετάφρασης!")]
        public void Translations01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            try
            {
                //Δημιουργούμε ένα πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                Assert.IsTrue(surveyManager.GetSurveysForClient(client1.ClientId).Count == 0);

                //We create a survey στα ελληνικά (και αυτόματα δημιουργείται και ένα view και μία σελίδα για αυτό:)
                var surveyGR1 = surveyManager.CreateSurvey(client1, "Η Ευτυχια", textsLanguage: BuiltinLanguages.Greek);
                Assert.IsTrue(surveyGR1.PrimaryLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(surveyGR1.TextsLanguage == BuiltinLanguages.Greek);

                surveyGR1.ShowTitle = surveyGR1.Title;
                surveyGR1 = surveyManager.UpdateSurvey(surveyGR1);

                //Τραβάμε τηνσελίδα
                var pageGR1 = surveyManager.GetFirstSurveyPage(surveyGR1);
                pageGR1.ShowTitle = "Μιλήστε μας για εσάς!";
                pageGR1 = surveyManager.UpdateSurveyPage(pageGR1);
                //Δημιουργούμε μία ερώτηση:
                var questionGR1a = surveyManager.CreateQuestion(pageGR1, QuestionType.DropDown, "Πως ονομάζεσαι;");
                var optionGR1a_1 = surveyManager.CreateQuestionOption(questionGR1a, "Μανωλης");
                var optionGR1a_2 = surveyManager.CreateQuestionOption(questionGR1a, "Γιώργος");
                var optionGR1a_3 = surveyManager.CreateQuestionOption(questionGR1a, "Μενέλαος");
                //Δημιουργούμε μία ερώτηση:
                var questionGR1b = surveyManager.CreateQuestion(pageGR1, QuestionType.MatrixOnePerRow, "Παρακαλώ απαντήστε;");
                var columnGR1b_1 = surveyManager.CreateQuestionColumn(questionGR1b, "κολώνα-1");
                var columnGR1b_2 = surveyManager.CreateQuestionColumn(questionGR1b, "κολώνα-2");
                var columnGR1b_3 = surveyManager.CreateQuestionColumn(questionGR1b, "κολώνα-3");
                var columnGR1b_4 = surveyManager.CreateQuestionColumn(questionGR1b, "κολώνα-4");

                surveyManager.CreateQuestionOption(questionGR1b, "Μαθημα-1");
                surveyManager.CreateQuestionOption(questionGR1b, "Μαθημα-2");
                surveyManager.CreateQuestionOption(questionGR1b, "Μαθημα-4");


                /*Δημιουργούμε ένα variance του survey:*/
                var surveyEN1 = surveyManager.AddSurveyLanguage(surveyGR1.SurveyId, BuiltinLanguages.Greek, BuiltinLanguages.English);
                Assert.IsTrue(surveyEN1.PrimaryLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(surveyEN1.TextsLanguage == BuiltinLanguages.English);


                /*Survey Translation:*/
                surveyEN1.ShowTitle = "Hapiness";
                surveyManager.UpdateSurvey(surveyEN1);
                surveyEN1 = surveyManager.GetSurveyById(surveyEN1.SurveyId, surveyEN1.TextsLanguage);
                Assert.IsTrue(surveyEN1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(surveyEN1.ShowTitle, "Hapiness");

                surveyGR1 = surveyManager.GetSurveyById(surveyGR1.SurveyId, surveyGR1.TextsLanguage);
                Assert.IsTrue(surveyGR1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(surveyGR1.ShowTitle, "Η Ευτυχια");

                /*Page translation:*/
                var pageEN1 = surveyManager.GetFirstSurveyPage(surveyEN1);
                Assert.IsTrue(pageEN1.TextsLanguage == BuiltinLanguages.English);
                pageEN1.ShowTitle = "Speak for yourself!";
                surveyManager.UpdateSurveyPage(pageEN1);

                pageEN1 = surveyManager.GetSurveyPageById(surveyEN1.SurveyId, pageGR1.PageId, BuiltinLanguages.English);
                Assert.IsTrue(pageEN1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(pageEN1.ShowTitle, "Speak for yourself!");
                pageGR1 = surveyManager.GetSurveyPageById(surveyGR1.SurveyId, pageEN1.PageId, BuiltinLanguages.Greek);
                Assert.IsTrue(pageGR1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(pageGR1.ShowTitle, "Μιλήστε μας για εσάς!");

                /*Question Translation:*/
                var questionEN1a = surveyManager.GetQuestionById(questionGR1a.Survey, questionGR1a.QuestionId, BuiltinLanguages.English);
                Assert.IsTrue(questionEN1a.TextsLanguage == BuiltinLanguages.English);
                questionEN1a.QuestionText = "What is your name?";
                surveyManager.UpdateQuestion(questionEN1a);

                questionEN1a = surveyManager.GetQuestionById(surveyGR1.SurveyId, questionGR1a.QuestionId, BuiltinLanguages.English);
                Assert.IsTrue(questionEN1a.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(questionEN1a.QuestionText, "What is your name?");
                questionGR1a = surveyManager.GetQuestionById(surveyEN1.SurveyId, questionEN1a.QuestionId, BuiltinLanguages.Greek);
                Assert.IsTrue(questionGR1a.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(questionGR1a.QuestionText, "Πως ονομάζεσαι;");

                /*Options Translation:*/
                var optionEN1a_1 = surveyManager.GetQuestionOptionById(optionGR1a_1.Survey, optionGR1a_1.Question, optionGR1a_1.OptionId, BuiltinLanguages.English);
                Assert.IsTrue(optionEN1a_1.TextsLanguage == BuiltinLanguages.English);
                optionEN1a_1.OptionText = "Manolis";
                surveyManager.UpdateQuestionOption(optionEN1a_1);

                optionEN1a_1 = surveyManager.GetQuestionOptionById(optionGR1a_1.Survey, optionGR1a_1.Question, optionGR1a_1.OptionId, BuiltinLanguages.English);
                Assert.IsTrue(optionEN1a_1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(optionEN1a_1.OptionText, "Manolis");
                optionGR1a_1 = surveyManager.GetQuestionOptionById(optionGR1a_1.Survey, optionGR1a_1.Question, optionGR1a_1.OptionId, BuiltinLanguages.Greek);
                Assert.IsTrue(optionGR1a_1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(optionGR1a_1.OptionText, "Μανωλης");


                /*Columns Translation:*/
                var columnEN1b_1 = surveyManager.GetQuestionColumnById(columnGR1b_1.Survey, columnGR1b_1.Question, columnGR1b_1.ColumnId, BuiltinLanguages.English);
                Assert.IsTrue(columnEN1b_1.TextsLanguage == BuiltinLanguages.English);
                columnEN1b_1.ColumnText = "column-1";
                surveyManager.UpdateQuestionColumn(columnEN1b_1);

                columnEN1b_1 = surveyManager.GetQuestionColumnById(columnGR1b_1.Survey, columnGR1b_1.Question, columnGR1b_1.ColumnId, BuiltinLanguages.English);
                Assert.IsTrue(columnEN1b_1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(columnEN1b_1.ColumnText, "column-1");
                columnGR1b_1 = surveyManager.GetQuestionColumnById(columnGR1b_1.Survey, columnGR1b_1.Question, columnGR1b_1.ColumnId, BuiltinLanguages.Greek);
                Assert.IsTrue(columnGR1b_1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(columnGR1b_1.ColumnText, "κολώνα-1");
            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    var _surveys = surveyManager.GetSurveysForClient(client.ClientId);
                    foreach (var srv in _surveys)
                    {
                        surveyManager.DeleteSurvey(srv);
                    }

                    systemManager.DeleteClient(client);
                }
            }


        }

    }
}
