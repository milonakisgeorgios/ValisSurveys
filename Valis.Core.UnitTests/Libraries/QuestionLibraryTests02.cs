using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Libraries
{
    [TestClass]
    public class QuestionLibraryTests02 : AdminBaseClass
    {

        [TestMethod, Description("Tests for AddLibraryQuestion to Survey #1")]
        public void QuestionLibraryTests02_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var libraryManager = VLLibraryManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε ένα Survey με μία ερώτηση μέσα:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);

                //δημιουργούμε ένα survey στην Default γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client.ClientId, "Questionnaire #1", "my ShowTitle", "my WelcomeText", textsLanguage: BuiltinLanguages.Greek);
                Assert.IsNotNull(survey1);


                //Τραβάμε την πρώτη σελίδα που δημιουργείται αυτόματα
                var page1 = surveyManager.GetFirstSurveyPage(survey1);

                //Δημιουργούμε μία ερώτηση:
                var question1 = surveyManager.CreateQuestion(page1, QuestionType.SingleLine, "my first question!");
                Assert.IsNotNull(question1);
                #endregion

                #region Δημιουργούμε μία library question:
                //Στο σύστημα υπάρχει μία κατηγορία ερωτήσεων:
                var category1 = libraryManager.GetLibraryQuestionCategories()[0];
                //για αυτή την κατηγορία, δεν έχουμε καθόλου ερωτήσεις:
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 0);

                //We create a new question:
                var libraryQuestion1 = libraryManager.CreateLibraryQuestion(category1.CategoryId, QuestionType.DropDown, "Choose your age:");
                Assert.IsNotNull(libraryQuestion1);

                libraryManager.CreateLibraryQuestionOption(libraryQuestion1, "8 years old");
                libraryManager.CreateLibraryQuestionOption(libraryQuestion1, "12 years old");
                libraryManager.CreateLibraryQuestionOption(libraryQuestion1, "14 years old");
                libraryManager.CreateLibraryQuestionOption(libraryQuestion1, "16 years old");
                libraryManager.CreateLibraryQuestionOption(libraryQuestion1, "18 years old");
                libraryManager.CreateLibraryQuestionOption(libraryQuestion1, "20 years old");

                //Εχουμε έξι (6) options:
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(libraryQuestion1).Count == 6);

                libraryQuestion1 = libraryManager.GetLibraryQuestionById(libraryQuestion1.QuestionId, libraryQuestion1.TextsLanguage);
                #endregion


                //Θέλω να προσθέσω την libraryQuestion1 σαν τελευταία ερώτηση στο survey μας:
                var question2 = surveyManager.AddLibraryQuestion(page1, libraryQuestion1);
                Assert.IsNotNull(question2);
            }
            finally
            {
                #region
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
                #endregion

                #region clean LibraryQuestions
                var categories = libraryManager.GetLibraryQuestionCategories();
                foreach (var item in categories)
                {
                    var questions = libraryManager.GetLibraryQuestions(item.CategoryId);
                    foreach (var q in questions)
                    {
                        libraryManager.DeleteLibraryQuestion(q.QuestionId);
                    }


                    if (item.IsBuiltIn)
                        continue;

                    libraryManager.DeleteLibraryQuestionCategory(item);
                }
                #endregion
            }
        }
    }
}
