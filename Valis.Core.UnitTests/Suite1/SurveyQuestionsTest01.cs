using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    /// <summary>
    /// Summary description for SurveysTests02
    /// </summary>
    [TestClass]
    public class SurveyQuestionsTest01 : SurveyFacilityBaseClass
    {
        public SurveyQuestionsTest01()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion



        [TestMethod, Description("READ operations for VLSurveyQuestion")]
        public void SurveyQuestionsTest01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //Δημιουργούμε ένα νέο survey:
                var survey = CreateSurvey1(surveyManager, client1);
                Assert.IsNotNull(survey);
                Assert.IsTrue(survey.TextsLanguage == BuiltinLanguages.Greek);

                //σε αυτό το survey έχουμε 8 pages:
                var pages = surveyManager.GetSurveyPages(survey);
                Assert.IsTrue(pages.Count == 8);

                //στην πρώτη σελίδα έχουμε δύο questions:
                var questions = surveyManager.GetQuestionsForPage(pages[0]);
                Assert.IsTrue(questions.Count == 2);
                foreach (var item in questions)
                {
                    var svdQuestion = surveyManager.GetQuestionById(item.Survey, item.QuestionId, item.TextsLanguage);
                    Assert.AreEqual<VLSurveyQuestion>(item, svdQuestion);
                }

                //στην δεύτερη σελίδα έχουμε μία ερώτηση:
                questions = surveyManager.GetQuestionsForPage(pages[1]);
                Assert.IsTrue(questions.Count == 1);
                foreach (var item in questions)
                {
                    var svdQuestion = surveyManager.GetQuestionById(item.Survey, item.QuestionId, item.TextsLanguage);
                    Assert.AreEqual<VLSurveyQuestion>(item, svdQuestion);
                }

                //στην τρίτη σελίδα έχουμε μία ερώτηση:
                questions = surveyManager.GetQuestionsForPage(pages[2]);
                Assert.IsTrue(questions.Count == 1);
                foreach (var item in questions)
                {
                    var svdQuestion = surveyManager.GetQuestionById(item.Survey, item.QuestionId, item.TextsLanguage);
                    Assert.AreEqual<VLSurveyQuestion>(item, svdQuestion);
                }

                //στην τέταρτη σελίδα έχουμε μία ερώτηση:
                questions = surveyManager.GetQuestionsForPage(pages[3]);
                Assert.IsTrue(questions.Count == 1);
                foreach (var item in questions)
                {
                    var svdQuestion = surveyManager.GetQuestionById(item.Survey, item.QuestionId, item.TextsLanguage);
                    Assert.AreEqual<VLSurveyQuestion>(item, svdQuestion);
                }

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


        [TestMethod, Description("CRUD operations for VLSurveyQuestion")]
        public void SurveyQuestionsTest01_02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo1", "ερωτηματολόγιο - demo1");
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                var survey2 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo2", "ερωτηματολόγιο - demo2");
                Assert.IsNotNull(survey2);
                Assert.IsTrue(survey2.PageSequence == 1);
                Assert.IsTrue(survey2.QuestionSequence == 0);

                //Δημιουργούμε μερικές σελίδες:
                var page11 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsNotNull(page11);
                var page12 = surveyManager.CreateSurveyPage(survey1, "page12");
                Assert.IsNotNull(page12);

                var page21 = surveyManager.GetFirstSurveyPage(survey2);
                Assert.IsNotNull(page21);
                var page22 = surveyManager.CreateSurveyPage(survey2, "page22");
                Assert.IsNotNull(page22);
                var page23 = surveyManager.CreateSurveyPage(survey2, "page23");
                Assert.IsNotNull(page23);


                //Ελέγχουμε τα PageSequence & QuestionSequence πεδία επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.IsTrue(survey2.PageSequence == 3);
                Assert.IsTrue(survey2.QuestionSequence == 0);
                


                //Δημιουργούμε ερωτήσεις για το page12:
                var q1 = surveyManager.CreateQuestion(page12, QuestionType.SingleLine, "Πως σας λένε");
                Assert.IsNotNull(q1);
                Assert.IsTrue(q1.Survey == page12.Survey);
                Assert.IsTrue(q1.Page == page12.PageId);
                Assert.IsTrue(q1.QuestionId == 1);
                Assert.IsNull(q1.MasterQuestion);
                Assert.IsTrue(q1.DisplayOrder == 1);
                Assert.IsTrue(q1.QuestionType == QuestionType.SingleLine);
                Assert.IsNull(q1.CustomType);
                Assert.IsFalse(q1.IsRequired);
                Assert.IsTrue(q1.ValidationBehavior == ValidationMode.DoNotValidate);
                Assert.IsNull(q1.RegularExpression);
                Assert.IsTrue(q1.OptionsSequence == 0);
                Assert.IsTrue(q1.ColumnsSequence == 0);
                Assert.IsTrue(q1.TextsLanguage == page12.TextsLanguage);
                Assert.AreEqual<string>(q1.QuestionText, "Πως σας λένε");
                Assert.IsNull(q1.Description);
                Assert.IsNull(q1.HelpText);
                Assert.IsNull(q1.FrontLabelText);
                Assert.IsNull(q1.AfterLabelText);
                Assert.IsNull(q1.InsideText);
                Assert.IsNull(q1.RequiredMessage);
                Assert.IsNull(q1.ValidationMessage);

                var svdQ1 = surveyManager.GetQuestionById(q1.Survey, q1.QuestionId);
                Assert.AreEqual<VLSurveyQuestion>(q1, svdQ1);

                //Ελέγχουμε το σύνολο των ερωτήσεων ανα σελίδα:
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page11).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page12).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page21).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page22).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page23).Count == 0);

                //Ελέγχουμε τα PageSequence & QuestionSequence πεδία επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 1);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.IsTrue(survey2.PageSequence == 3);
                Assert.IsTrue(survey2.QuestionSequence == 0);





                //Δημιουργούμε ερωτήσεις για το page12:
                var q2 = surveyManager.CreateQuestion(page12, QuestionType.SingleLine, "Πως σας λένε2");
                Assert.IsNotNull(q2);
                Assert.IsTrue(q2.Survey == page12.Survey);
                Assert.IsTrue(q2.Page == page12.PageId);
                Assert.IsTrue(q2.QuestionId == 2);
                Assert.IsTrue(q2.DisplayOrder == 2);
                Assert.IsTrue(q2.QuestionType == QuestionType.SingleLine);
                Assert.AreEqual<string>(q2.QuestionText, "Πως σας λένε2");
                var svdQ2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId);
                Assert.AreEqual<VLSurveyQuestion>(q2, svdQ2);

                //Δημιουργούμε ερωτήσεις για το page11:
                var q3 = surveyManager.CreateQuestion(page11, QuestionType.MultipleLine, "Πως σας λένε3");
                Assert.IsNotNull(q3);
                Assert.IsTrue(q3.Survey == page11.Survey);
                Assert.IsTrue(q3.Page == page11.PageId);
                Assert.IsTrue(q3.QuestionId == 3);
                Assert.IsTrue(q3.DisplayOrder == 1);
                Assert.IsTrue(q3.QuestionType == QuestionType.MultipleLine);
                Assert.AreEqual<string>(q3.QuestionText, "Πως σας λένε3");
                var svdQ3 = surveyManager.GetQuestionById(q3.Survey, q3.QuestionId);
                Assert.AreEqual<VLSurveyQuestion>(q3, svdQ3);

                //Ελέγχουμε το σύνολο των ερωτήσεων ανα σελίδα:
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page11).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page12).Count == 2);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page21).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page22).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page23).Count == 0);

                //Ελέγχουμε τα PageSequence & QuestionSequence πεδία επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 3);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.IsTrue(survey2.PageSequence == 3);
                Assert.IsTrue(survey2.QuestionSequence == 0);



                var q11 = surveyManager.CreateQuestion(page23, QuestionType.SingleLine, "question1");
                Assert.IsTrue(q11.QuestionId == 1);
                Assert.IsTrue(q11.DisplayOrder == 1);
                var q12 = surveyManager.CreateQuestion(page23, QuestionType.SingleLine, "question2");
                Assert.IsTrue(q12.QuestionId == 2);
                Assert.IsTrue(q12.DisplayOrder == 2);
                var q13 = surveyManager.CreateQuestion(page23, QuestionType.SingleLine, "question3");
                Assert.IsTrue(q13.QuestionId == 3);
                Assert.IsTrue(q13.DisplayOrder == 3);
                var q14 = surveyManager.CreateQuestion(page23, QuestionType.SingleLine, "question4");
                Assert.IsTrue(q14.QuestionId == 4);
                Assert.IsTrue(q14.DisplayOrder == 4);
                var q15 = surveyManager.CreateQuestion(page23, QuestionType.SingleLine, "question5");
                Assert.IsTrue(q15.QuestionId == 5);
                Assert.IsTrue(q15.DisplayOrder == 5);

                //Ελέγχουμε το σύνολο των ερωτήσεων ανα σελίδα:
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page11).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page12).Count == 2);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page21).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page22).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page23).Count == 5);

                //Ελέγχουμε τα PageSequence & QuestionSequence πεδία επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 3);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.IsTrue(survey2.PageSequence == 3);
                Assert.IsTrue(survey2.QuestionSequence == 5);





                //ΔΙΑΓΡΑΦΕΣ
                surveyManager.DeleteQuestion(q14);

                //Ελέγχουμε το σύνολο των ερωτήσεων ανα σελίδα:
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page11).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page12).Count == 2);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page21).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page22).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page23).Count == 4);

                //Ελέγχουμε τα PageSequence & QuestionSequence πεδία επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 3);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.IsTrue(survey2.PageSequence == 3);
                Assert.IsTrue(survey2.QuestionSequence == 5);


                q11 = surveyManager.GetQuestionById(q11.Survey, q11.QuestionId);
                Assert.IsTrue(q11.QuestionId == 1);
                Assert.IsTrue(q11.DisplayOrder == 1);
                q12 = surveyManager.GetQuestionById(q12.Survey, q12.QuestionId);
                Assert.IsTrue(q12.QuestionId == 2);
                Assert.IsTrue(q12.DisplayOrder == 2);
                q13 = surveyManager.GetQuestionById(q13.Survey, q13.QuestionId);
                Assert.IsTrue(q13.QuestionId == 3);
                Assert.IsTrue(q13.DisplayOrder == 3);
                q14 = surveyManager.GetQuestionById(q14.Survey, q14.QuestionId);
                Assert.IsNull(q14);
                q15 = surveyManager.GetQuestionById(q15.Survey, q15.QuestionId);
                Assert.IsTrue(q15.QuestionId == 5);
                Assert.IsTrue(q15.DisplayOrder == 4);



                //ΔΙΑΓΡΑΦΕΣ
                surveyManager.DeleteQuestion(q11);

                //Ελέγχουμε το σύνολο των ερωτήσεων ανα σελίδα:
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page11).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page12).Count == 2);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page21).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page22).Count == 0);
                Assert.IsTrue(surveyManager.GetQuestionsForPage(page23).Count == 3);

                //Ελέγχουμε τα PageSequence & QuestionSequence πεδία επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 3);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.IsTrue(survey2.PageSequence == 3);
                Assert.IsTrue(survey2.QuestionSequence == 5);


                q11 = surveyManager.GetQuestionById(q11.Survey, q11.QuestionId);
                Assert.IsNull(q11);
                q12 = surveyManager.GetQuestionById(q12.Survey, q12.QuestionId);
                Assert.IsTrue(q12.QuestionId == 2);
                Assert.IsTrue(q12.DisplayOrder == 1);
                q13 = surveyManager.GetQuestionById(q13.Survey, q13.QuestionId);
                Assert.IsTrue(q13.QuestionId == 3);
                Assert.IsTrue(q13.DisplayOrder == 2);
                //q14 = surveyManager.GetQuestionById(q14.Survey, q14.QuestionId);
                //Assert.IsNull(q14);
                q15 = surveyManager.GetQuestionById(q15.Survey, q15.QuestionId);
                Assert.IsTrue(q15.QuestionId == 5);
                Assert.IsTrue(q15.DisplayOrder == 3);
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



        [TestMethod, Description("CRUD operations for VLSurveyQuestion")]
        public void SurveyQuestionsTest01_03()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo1", "ερωτηματολόγιο - demo1");
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                var survey2 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo2", "ερωτηματολόγιο - demo2");
                Assert.IsNotNull(survey2);
                Assert.IsTrue(survey2.PageSequence == 1);
                Assert.IsTrue(survey2.QuestionSequence == 0);

                //Δημιουργούμε μερικές σελίδες:
                var page11 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsNotNull(page11);
                var page12 = surveyManager.CreateSurveyPage(survey1, "page12");
                Assert.IsNotNull(page12);

                var page21 = surveyManager.GetFirstSurveyPage(survey2);
                Assert.IsNotNull(page21);
                var page22 = surveyManager.CreateSurveyPage(survey2, "page22");
                Assert.IsNotNull(page22);
                var page23 = surveyManager.CreateSurveyPage(survey2, "page23");
                Assert.IsNotNull(page23);

                //Δημιουργούμε μία ερώτηση στο page12:
                var q1 = surveyManager.CreateQuestion(page12, QuestionType.SingleLine, "Πως σας λένε");
                Assert.IsTrue(q1.Survey == page12.Survey);
                Assert.IsTrue(q1.Page == page12.PageId);
                Assert.IsTrue(q1.QuestionId == 1);
                Assert.IsTrue(q1.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>(q1.QuestionText, "Πως σας λένε");

                var svdQ1 = surveyManager.GetQuestionById(q1.Survey, q1.QuestionId);
                Assert.AreEqual<VLSurveyQuestion>(q1, svdQ1);


                svdQ1 = surveyManager.GetQuestionById(q1.Survey, q1.QuestionId, BuiltinLanguages.English);
                Assert.IsNull(svdQ1);

                //Προσθέτουμε στο survey1 την αγγλική γλώσσα:
                surveyManager.AddSurveyLanguage(survey1.SurveyId, survey1.TextsLanguage, BuiltinLanguages.English);


                svdQ1 = surveyManager.GetQuestionById(q1.Survey, q1.QuestionId, BuiltinLanguages.English);
                Assert.IsNotNull(svdQ1);
                Assert.IsTrue(svdQ1.Survey == q1.Survey);
                Assert.IsTrue(svdQ1.Page == q1.Page);
                Assert.IsTrue(svdQ1.QuestionId == q1.QuestionId);
                Assert.IsTrue(svdQ1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(svdQ1.QuestionText, "Πως σας λένε");


                //τώρα κάνουμε updates
                q1.QuestionText = "QuestionText12341234QWEQW";
                q1.Description = "Description@#$#!@erqerwer2";
                q1.AfterLabelText = "adf3w4y46462ybv35";
                q1.FrontLabelText = "sdfwrt3%$@#R@$%^@$twr";
                q1.HelpText = "adfadfw2423423423";
                q1.ValidationBehavior = ValidationMode.Date1;
                q1.ValidationMessage = "We need a Date1!";
                q1 = surveyManager.UpdateQuestion(q1);
                Assert.IsTrue(q1.Survey == page12.Survey);
                Assert.IsTrue(q1.Page == page12.PageId);
                Assert.IsTrue(q1.QuestionId == 1);
                Assert.IsNull(q1.MasterQuestion);
                Assert.IsTrue(q1.DisplayOrder == 1);
                Assert.IsTrue(q1.QuestionType == QuestionType.SingleLine);
                Assert.IsNull(q1.CustomType);
                Assert.IsFalse(q1.IsRequired);
                Assert.IsTrue(q1.ValidationBehavior == ValidationMode.Date1);
                Assert.IsNull(q1.RegularExpression);
                Assert.IsTrue(q1.OptionsSequence == 0);
                Assert.IsTrue(q1.ColumnsSequence == 0);
                Assert.IsTrue(q1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(q1.QuestionText, "QuestionText12341234QWEQW");
                Assert.AreEqual<string>(q1.Description, "Description@#$#!@erqerwer2");
                Assert.AreEqual<string>(q1.HelpText, "adfadfw2423423423");
                Assert.AreEqual<string>(q1.FrontLabelText, "sdfwrt3%$@#R@$%^@$twr");
                Assert.AreEqual<string>(q1.AfterLabelText, "adf3w4y46462ybv35");
                Assert.IsNull(q1.InsideText);
                Assert.IsNull(q1.RequiredMessage);
                Assert.AreEqual<string>(q1.ValidationMessage, "We need a Date1!");


                svdQ1 = surveyManager.GetQuestionById(q1.Survey, q1.QuestionId, BuiltinLanguages.English);
                Assert.IsNotNull(svdQ1);
                Assert.IsTrue(svdQ1.Survey == page12.Survey);
                Assert.IsTrue(svdQ1.Page == page12.PageId);
                Assert.IsTrue(svdQ1.QuestionId == 1);
                Assert.IsNull(svdQ1.MasterQuestion);
                Assert.IsTrue(svdQ1.DisplayOrder == 1);
                Assert.IsTrue(svdQ1.QuestionType == QuestionType.SingleLine);
                Assert.IsNull(svdQ1.CustomType);
                Assert.IsFalse(svdQ1.IsRequired);
                Assert.IsTrue(svdQ1.ValidationBehavior == ValidationMode.Date1);
                Assert.IsNull(svdQ1.RegularExpression);
                Assert.IsTrue(svdQ1.OptionsSequence == 0);
                Assert.IsTrue(svdQ1.ColumnsSequence == 0);
                Assert.IsTrue(svdQ1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(svdQ1.QuestionText, "Πως σας λένε");
                Assert.IsNull(svdQ1.Description);
                Assert.IsNull(svdQ1.HelpText);
                Assert.IsNull(svdQ1.FrontLabelText);
                Assert.IsNull(svdQ1.AfterLabelText);
                Assert.IsNull(svdQ1.InsideText);
                Assert.IsNull(svdQ1.RequiredMessage);
                Assert.IsNull(svdQ1.ValidationMessage);

                svdQ1.QuestionText = "QuestionText-English";
                svdQ1.Description = "Description-English";
                svdQ1.HelpText = "HelpText-English";
                svdQ1.FrontLabelText = "FrontLabelText-English";
                svdQ1.AfterLabelText = "AfterLabelText-English";
                svdQ1.InsideText = "InsideText-English";
                svdQ1.RequiredMessage = "RequiredMessage-English";
                svdQ1.ValidationMessage = "ValidationMessage-English";
                svdQ1 = surveyManager.UpdateQuestion(svdQ1);
                Assert.IsTrue(svdQ1.Survey == page12.Survey);
                Assert.IsTrue(svdQ1.Page == page12.PageId);
                Assert.IsTrue(svdQ1.QuestionId == 1);
                Assert.IsNull(svdQ1.MasterQuestion);
                Assert.IsTrue(svdQ1.DisplayOrder == 1);
                Assert.IsTrue(svdQ1.QuestionType == QuestionType.SingleLine);
                Assert.IsNull(svdQ1.CustomType);
                Assert.IsFalse(svdQ1.IsRequired);
                Assert.IsTrue(svdQ1.ValidationBehavior == ValidationMode.Date1);
                Assert.IsNull(svdQ1.RegularExpression);
                Assert.IsTrue(svdQ1.OptionsSequence == 0);
                Assert.IsTrue(svdQ1.ColumnsSequence == 0);
                Assert.IsTrue(svdQ1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(svdQ1.QuestionText, "QuestionText-English");
                Assert.AreEqual<string>(svdQ1.Description, "Description-English");
                Assert.AreEqual<string>(svdQ1.HelpText, "HelpText-English");
                Assert.AreEqual<string>(svdQ1.FrontLabelText, "FrontLabelText-English");
                Assert.AreEqual<string>(svdQ1.AfterLabelText, "AfterLabelText-English");
                Assert.AreEqual<string>(svdQ1.InsideText, "InsideText-English");
                Assert.IsNull(svdQ1.RequiredMessage);
                Assert.AreEqual<string>(svdQ1.ValidationMessage, "ValidationMessage-English");




                q1 = surveyManager.UpdateQuestion(q1);
                Assert.IsTrue(q1.Survey == page12.Survey);
                Assert.IsTrue(q1.Page == page12.PageId);
                Assert.IsTrue(q1.QuestionId == 1);
                Assert.IsNull(q1.MasterQuestion);
                Assert.IsTrue(q1.DisplayOrder == 1);
                Assert.IsTrue(q1.QuestionType == QuestionType.SingleLine);
                Assert.IsNull(q1.CustomType);
                Assert.IsFalse(q1.IsRequired);
                Assert.IsTrue(q1.ValidationBehavior == ValidationMode.Date1);
                Assert.IsNull(q1.RegularExpression);
                Assert.IsTrue(q1.OptionsSequence == 0);
                Assert.IsTrue(q1.ColumnsSequence == 0);
                Assert.IsTrue(q1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(q1.QuestionText, "QuestionText12341234QWEQW");
                Assert.AreEqual<string>(q1.Description, "Description@#$#!@erqerwer2");
                Assert.AreEqual<string>(q1.HelpText, "adfadfw2423423423");
                Assert.AreEqual<string>(q1.FrontLabelText, "sdfwrt3%$@#R@$%^@$twr");
                Assert.AreEqual<string>(q1.AfterLabelText, "adf3w4y46462ybv35");
                Assert.IsNull(q1.InsideText);
                Assert.IsNull(q1.RequiredMessage);
                Assert.AreEqual<string>(q1.ValidationMessage, "We need a Date1!");



                //ΔΙΑΓΡΑΦΗ
                surveyManager.DeleteQuestion(q1);
                Assert.IsNull(surveyManager.GetQuestionById(q1.Survey, q1.QuestionId, BuiltinLanguages.Greek));
                Assert.IsNull(surveyManager.GetQuestionById(q1.Survey, q1.QuestionId, BuiltinLanguages.English));
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



        [TestMethod, Description("VLSurveyQuestion Create/createBefore/createAfter")]
        public void SurveyQuestionsTest01_04a()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo1", "ερωτηματολόγιο - demo1");
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                Assert.IsTrue(survey1.TicketSequence == 0);

                //Δημιουργούμε μερικές σελίδες:
                var page01 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsNotNull(page01);
                var page02 = surveyManager.CreateSurveyPage(survey1, "page02");
                Assert.IsNotNull(page02);
                var page03 = surveyManager.CreateSurveyPage(survey1, "page03");
                Assert.IsNotNull(page03);


                //Δημιουργούμε ερωτήσεις στο page1:
                var q01_1 = surveyManager.CreateQuestion(page01, QuestionType.SingleLine, "page01 -> question 01:");
                Assert.IsTrue(q01_1.QuestionId == 1);
                Assert.IsTrue(q01_1.DisplayOrder == 1);
                var q01_2 = surveyManager.CreateQuestion(page01, QuestionType.SingleLine, "page01 -> question 02:");
                Assert.IsTrue(q01_2.QuestionId == 2);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                //Δημιουργούμε ερωτήσεις στο page1:
                var q02_1 = surveyManager.CreateQuestion(page02, QuestionType.SingleLine, "page02 -> question 01:");
                Assert.IsTrue(q02_1.QuestionId == 3);
                Assert.IsTrue(q02_1.DisplayOrder == 1);
                var q02_2 = surveyManager.CreateQuestion(page02, QuestionType.SingleLine, "page02 -> question 02:");
                Assert.IsTrue(q02_2.QuestionId == 4);
                Assert.IsTrue(q02_2.DisplayOrder == 2);
                var q02_3 = surveyManager.CreateQuestion(page02, QuestionType.SingleLine, "page02 -> question 03:");
                Assert.IsTrue(q02_3.QuestionId == 5);
                Assert.IsTrue(q02_3.DisplayOrder == 3);
                var q02_4 = surveyManager.CreateQuestion(page02, QuestionType.SingleLine, "page02 -> question 04:");
                Assert.IsTrue(q02_4.QuestionId == 6);
                Assert.IsTrue(q02_4.DisplayOrder == 4);

                //Δημιουργούμε ερωτήσεις στο page3:
                var q03_1 = surveyManager.CreateQuestion(page03, QuestionType.SingleLine, "page03 -> question 01:");
                Assert.IsTrue(q03_1.QuestionId == 7);
                Assert.IsTrue(q03_1.DisplayOrder == 1);
                var q03_2 = surveyManager.CreateQuestion(page03, QuestionType.SingleLine, "page03 -> question 02:");
                Assert.IsTrue(q03_2.QuestionId == 8);
                Assert.IsTrue(q03_2.DisplayOrder == 2);


                //ΕΛΕΓΧΟΥΜΕ τα sequences επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.PageSequence == 3);
                Assert.IsTrue(survey1.QuestionSequence == 8);
                Assert.IsTrue(survey1.TicketSequence == 0);


                Action ReadAllStuff = delegate()
                {
                    page01 = surveyManager.GetSurveyPageById(page01.Survey, page01.PageId);
                    q01_1 = surveyManager.GetQuestionById(q01_1.Survey, q01_1.QuestionId);
                    q01_2 = surveyManager.GetQuestionById(q01_2.Survey, q01_2.QuestionId);

                    page02 = surveyManager.GetSurveyPageById(page02.Survey, page02.PageId);
                    q02_1 = surveyManager.GetQuestionById(q02_1.Survey, q02_1.QuestionId);
                    q02_2 = surveyManager.GetQuestionById(q02_2.Survey, q02_2.QuestionId);
                    q02_3 = surveyManager.GetQuestionById(q02_3.Survey, q02_3.QuestionId);
                    q02_4 = surveyManager.GetQuestionById(q02_4.Survey, q02_4.QuestionId);

                    page03 = surveyManager.GetSurveyPageById(page03.Survey, page03.PageId);
                    q03_1 = surveyManager.GetQuestionById(q03_1.Survey, q03_1.QuestionId);
                    q03_2 = surveyManager.GetQuestionById(q03_2.Survey, q03_2.QuestionId);
                };
                Action EnsureBaseCase = delegate()
                {
                    ReadAllStuff();
                    Assert.IsTrue(q01_1.DisplayOrder == 1);
                    Assert.IsTrue(q01_2.DisplayOrder == 2);

                    Assert.IsTrue(q02_1.DisplayOrder == 1);
                    Assert.IsTrue(q02_2.DisplayOrder == 2);
                    Assert.IsTrue(q02_3.DisplayOrder == 3);
                    Assert.IsTrue(q02_4.DisplayOrder == 4);

                    Assert.IsTrue(q03_1.DisplayOrder == 1);
                    Assert.IsTrue(q03_2.DisplayOrder == 2);
                };

                #region διαβάζουμε όλες τις σελίδες και ερωτήσεις απο το σύστημα και ελέγχουμε:
                EnsureBaseCase();
                #endregion


                #region CreateQuestionBefore q02_1
                var _beacon = surveyManager.CreateQuestionBefore(q02_1, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_1.Survey);
                Assert.IsNotNull(_beacon.Page == q02_1.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(_beacon.DisplayOrder == 1);
                Assert.IsTrue(q02_1.DisplayOrder == 2);
                Assert.IsTrue(q02_2.DisplayOrder == 3);
                Assert.IsTrue(q02_3.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion

                #region CreateQuestionBefore q02_2
                _beacon = surveyManager.CreateQuestionBefore(q02_2, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_2.Survey);
                Assert.IsNotNull(_beacon.Page == q02_2.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(_beacon.DisplayOrder == 2);
                Assert.IsTrue(q02_2.DisplayOrder == 3);
                Assert.IsTrue(q02_3.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion

                #region CreateQuestionBefore q02_3
                _beacon = surveyManager.CreateQuestionBefore(q02_3, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_3.Survey);
                Assert.IsNotNull(_beacon.Page == q02_3.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(q02_2.DisplayOrder == 2);
                Assert.IsTrue(_beacon.DisplayOrder == 3);
                Assert.IsTrue(q02_3.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion

                #region CreateQuestionBefore q02_4
                _beacon = surveyManager.CreateQuestionBefore(q02_4, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_4.Survey);
                Assert.IsNotNull(_beacon.Page == q02_4.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(q02_2.DisplayOrder == 2);
                Assert.IsTrue(q02_3.DisplayOrder == 3);
                Assert.IsTrue(_beacon.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion



                #region CreateQuestionAfter q02_1
                _beacon = surveyManager.CreateQuestionAfter(q02_1, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_1.Survey);
                Assert.IsNotNull(_beacon.Page == q02_1.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(_beacon.DisplayOrder == 2);
                Assert.IsTrue(q02_2.DisplayOrder == 3);
                Assert.IsTrue(q02_3.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion

                #region CreateQuestionAfter q02_2
                _beacon = surveyManager.CreateQuestionAfter(q02_2, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_2.Survey);
                Assert.IsNotNull(_beacon.Page == q02_2.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(q02_2.DisplayOrder == 2);
                Assert.IsTrue(_beacon.DisplayOrder == 3);
                Assert.IsTrue(q02_3.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion

                #region CreateQuestionAfter q02_3
                _beacon = surveyManager.CreateQuestionAfter(q02_3, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_3.Survey);
                Assert.IsNotNull(_beacon.Page == q02_3.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(q02_2.DisplayOrder == 2);
                Assert.IsTrue(q02_3.DisplayOrder == 3);
                Assert.IsTrue(_beacon.DisplayOrder == 4);
                Assert.IsTrue(q02_4.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
                #endregion


                #region CreateQuestionAfter q02_4
                _beacon = surveyManager.CreateQuestionAfter(q02_4, QuestionType.SingleLine, "The beacon question!!!");
                Assert.IsNotNull(_beacon.Survey == q02_4.Survey);
                Assert.IsNotNull(_beacon.Page == q02_4.Page);

                ReadAllStuff();

                Assert.IsTrue(q01_1.DisplayOrder == 1);
                Assert.IsTrue(q01_2.DisplayOrder == 2);

                Assert.IsTrue(q02_1.DisplayOrder == 1);
                Assert.IsTrue(q02_2.DisplayOrder == 2);
                Assert.IsTrue(q02_3.DisplayOrder == 3);
                Assert.IsTrue(q02_4.DisplayOrder == 4);
                Assert.IsTrue(_beacon.DisplayOrder == 5);

                Assert.IsTrue(q03_1.DisplayOrder == 1);
                Assert.IsTrue(q03_2.DisplayOrder == 2);

                surveyManager.DeleteQuestion(_beacon);
                Assert.IsNull(surveyManager.GetQuestionById(_beacon.Survey, _beacon.QuestionId));

                EnsureBaseCase();
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

        [TestMethod, Description("READ operations for VLQuestionOption/VLQuestionColumn")]
        public void SurveyQuestionsTest01_05()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //Διαβάζουμε ένα survey απο την βάση μας:
                var survey = CreateSurvey1(surveyManager, client1);
                Assert.IsNotNull(survey);
                Assert.IsTrue(survey.TextsLanguage == BuiltinLanguages.Greek);

                //σε αυτό το survey έχουμε 8 pages:
                var pages = surveyManager.GetSurveyPages(survey);
                Assert.IsTrue(pages.Count == 8);

                //Η πρώτη σελίδα έχει δύο (2) ερωτήσεις:
                var questions = surveyManager.GetQuestionsForPage(pages[0]);
                Assert.IsTrue(questions.Count == 2);
                Assert.IsTrue(questions[0].TextsLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(questions[1].TextsLanguage == BuiltinLanguages.Greek);

                //Η πρώτη ερώτηση έχει πέντε (5) options:
                Assert.IsTrue(questions[0].QuestionId == 1);
                var options = surveyManager.GetQuestionOptions(questions[0]);
                Assert.IsTrue(options.Count == 5);
                foreach(var item in options)
                {
                    Assert.IsTrue(item.TextsLanguage == BuiltinLanguages.Greek);
                    Assert.IsTrue(item.Question == questions[0].QuestionId);

                    var svdOption = surveyManager.GetQuestionOptionById(item.Survey, item.Question, item.OptionId, item.TextsLanguage);
                    Assert.AreEqual<VLQuestionOption>(item, svdOption);
                }

                //Η πρωτη ερώτηση έχει πέντε (5) κολώνες:
                var columns = surveyManager.GetQuestionColumns(questions[0]);
                Assert.IsTrue(columns.Count == 5);
                foreach (var item in columns)
                {
                    Assert.IsTrue(item.TextsLanguage == BuiltinLanguages.Greek);
                    Assert.IsTrue(item.Question == questions[0].QuestionId);

                    var svdColumn = surveyManager.GetQuestionColumnById(item.Survey, item.Question, item.ColumnId, item.TextsLanguage);
                    Assert.AreEqual<VLQuestionColumn>(item, svdColumn);
                }





                //Διαβάζουμε το survey στην αγγλική γλώσσα
                survey = surveyManager.GetSurveyById(survey.SurveyId, BuiltinLanguages.English);
                Assert.IsNotNull(survey);
                Assert.IsTrue(survey.TextsLanguage == BuiltinLanguages.English);

                pages = surveyManager.GetSurveyPages(survey);
                Assert.IsTrue(pages.Count == 8);

                questions = surveyManager.GetQuestionsForPage(pages[0]);
                Assert.IsTrue(questions.Count == 2);
                Assert.IsTrue(questions[0].TextsLanguage == BuiltinLanguages.English);
                Assert.IsTrue(questions[1].TextsLanguage == BuiltinLanguages.English);

                options = surveyManager.GetQuestionOptions(questions[0]);
                Assert.IsTrue(options.Count == 5);
                foreach (var item in options)
                {
                    Assert.IsTrue(item.TextsLanguage == BuiltinLanguages.English);
                    Assert.IsTrue(item.Question == questions[0].QuestionId);

                    var svdOption = surveyManager.GetQuestionOptionById(item.Survey, item.Question, item.OptionId, item.TextsLanguage);
                    Assert.AreEqual<VLQuestionOption>(item, svdOption);
                }

                columns = surveyManager.GetQuestionColumns(questions[0]);
                Assert.IsTrue(columns.Count == 5);
                foreach (var item in columns)
                {
                    Assert.IsTrue(item.TextsLanguage == BuiltinLanguages.English);
                    Assert.IsTrue(item.Question == questions[0].QuestionId);

                    var svdColumn = surveyManager.GetQuestionColumnById(item.Survey, item.Question, item.ColumnId, item.TextsLanguage);
                    Assert.AreEqual<VLQuestionColumn>(item, svdColumn);
                }
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


        [TestMethod, Description("CRUD operations for VLQuestionOption")]
        public void SurveyQuestionsTest01_06()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo1", "ερωτηματολόγιο - demo1");
                Assert.IsNotNull(survey1);
                //Δημιουργούμε δύο σελίδες:
                var page11 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsNotNull(page11);
                var page12 = surveyManager.CreateSurveyPage(survey1, "page12");
                Assert.IsNotNull(page12);

                //Δημιουργούμε ερωτήσεις:
                var q1 = surveyManager.CreateQuestion(page12, QuestionType.SingleLine, "Πως σας λένε");
                Assert.IsNotNull(q1);
                Assert.IsTrue(q1.ColumnsSequence == 0);
                Assert.IsTrue(q1.OptionsSequence == 0);
                var q2 = surveyManager.CreateQuestion(page12, QuestionType.OneFromMany, "Πως σας λένε2");
                Assert.IsNotNull(q2);
                Assert.IsTrue(q2.ColumnsSequence == 0);
                Assert.IsTrue(q2.OptionsSequence == 0);

                //Ελέγχουμε τα sequences επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 2);


                //We create an option:
                var option11 = surveyManager.CreateQuestionOption(q2.Survey, q2.QuestionId, "The time is sufficient for performing my tasks#2", QuestionOptionType.Default);
                #region
                Assert.IsNotNull(option11);
                Assert.IsTrue(option11.Survey == q2.Survey);
                Assert.IsTrue(option11.Question == q2.QuestionId);
                Assert.IsTrue(option11.OptionType == QuestionOptionType.Default);
                Assert.IsTrue(option11.DisplayOrder == 1);
                Assert.IsTrue(option11.AttributeFlags == 0);
                Assert.IsTrue(option11.TextsLanguage == q1.TextsLanguage);
                Assert.AreEqual<string>(option11.OptionText, "The time is sufficient for performing my tasks#2");
                var svdOption11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, option11.TextsLanguage);
                Assert.AreEqual<VLQuestionOption>(option11, svdOption11);
                Assert.IsNull(surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.English));
                #endregion

                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId, q2.TextsLanguage);
                Assert.IsTrue(q2.ColumnsSequence == 0);
                Assert.IsTrue(q2.OptionsSequence == 1);
                Assert.IsTrue(surveyManager.GetQuestionOptions(q2).Count == 1);

                //We create an option:
                var option12 = surveyManager.CreateQuestionOption(q2, "We help each other in my working environment#2");
                #region
                Assert.IsNotNull(option12);
                Assert.IsTrue(option12.Survey == q2.Survey);
                Assert.IsTrue(option12.Question == q2.QuestionId);
                Assert.IsTrue(option12.OptionType == QuestionOptionType.Default);
                Assert.IsTrue(option12.DisplayOrder == 2);
                Assert.IsTrue(option12.AttributeFlags == 0);
                Assert.IsTrue(option12.TextsLanguage == q1.TextsLanguage);
                Assert.AreEqual<string>(option12.OptionText, "We help each other in my working environment#2");
                var svdoption12 = surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, option12.TextsLanguage);
                Assert.AreEqual<VLQuestionOption>(option12, svdoption12);
                Assert.IsNull(surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, BuiltinLanguages.English));
                #endregion

                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId, q2.TextsLanguage);
                Assert.IsTrue(q2.ColumnsSequence == 0);
                Assert.IsTrue(q2.OptionsSequence == 2);
                Assert.IsTrue(surveyManager.GetQuestionOptions(q2).Count == 2);


                //Κάνουμε update το option12:
                option12.OptionText = "adfaerqvrqev qcvqecvqeαδφαδ αδφ αεφ α";
                option12 = surveyManager.UpdateQuestionOption(option12);
                #region
                Assert.IsNotNull(option12);
                Assert.IsTrue(option12.Survey == q2.Survey);
                Assert.IsTrue(option12.Question == q2.QuestionId);
                Assert.IsTrue(option12.OptionType == QuestionOptionType.Default);
                Assert.IsTrue(option12.DisplayOrder == 2);
                Assert.IsTrue(option12.AttributeFlags == 0);
                Assert.IsTrue(option12.TextsLanguage == q1.TextsLanguage);
                Assert.AreEqual<string>(option12.OptionText, "adfaerqvrqev qcvqecvqeαδφαδ αδφ αεφ α");
                svdoption12 = surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, option12.TextsLanguage);
                Assert.AreEqual<VLQuestionOption>(option12, svdoption12);
                Assert.IsNull(surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, BuiltinLanguages.English));
                #endregion



                #region ADDLANGUAGE
                survey1 = surveyManager.AddSurveyLanguage(survey1.SurveyId, survey1.TextsLanguage, BuiltinLanguages.English);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.English);

                //read option11 in English:
                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.English);
                Assert.IsNotNull(option11);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(option11.OptionText, "The time is sufficient for performing my tasks#2");
                //read option11 in Greek:
                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.Greek);
                Assert.IsNotNull(option11);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(option11.OptionText, "The time is sufficient for performing my tasks#2");


                //Update
                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.Greek);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.Greek);
                option11.OptionText = "Ειμαι αρκετά μεγάλος για το show?";
                surveyManager.UpdateQuestionOption(option11);


                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.English);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.English);
                option11.OptionText = "Am I old enough for the show?";
                option11 = surveyManager.UpdateQuestionOption(option11);

                //Checking:
                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.Greek);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(option11.OptionText, "Ειμαι αρκετά μεγάλος για το show?");

                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.English);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(option11.OptionText, "Am I old enough for the show?");

                option12 = surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, BuiltinLanguages.Greek);
                Assert.IsTrue(option12.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(option12.OptionText, "adfaerqvrqev qcvqecvqeαδφαδ αδφ αεφ α");

                option12 = surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, BuiltinLanguages.English);
                Assert.IsTrue(option12.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(option12.OptionText, "adfaerqvrqev qcvqecvqeαδφαδ αδφ αεφ α");
                #endregion


                #region REMOVELANGUAGE:
                surveyManager.RemoveSurveyLanguage(survey1.SurveyId, BuiltinLanguages.English);
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, BuiltinLanguages.Greek);
                Assert.IsNotNull(survey1);
                Assert.IsNull(surveyManager.GetSurveyById(survey1.SurveyId, BuiltinLanguages.English));

                option11 = surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.Greek);
                Assert.IsTrue(option11.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(option11.OptionText, "Ειμαι αρκετά μεγάλος για το show?");

                Assert.IsNull(surveyManager.GetQuestionOptionById(option11.Survey, option11.Question, option11.OptionId, BuiltinLanguages.English));

                option12 = surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, BuiltinLanguages.Greek);
                Assert.IsTrue(option12.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(option12.OptionText, "adfaerqvrqev qcvqecvqeαδφαδ αδφ αεφ α");

                Assert.IsNull(surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, BuiltinLanguages.English));
                #endregion




                //Delete
                surveyManager.DeleteQuestionOption(option12);
                Assert.IsNull(surveyManager.GetQuestionOptionById(option12.Survey, option12.Question, option12.OptionId, option12.TextsLanguage));

                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId, q2.TextsLanguage);
                Assert.IsTrue(q2.ColumnsSequence == 0);
                Assert.IsTrue(q2.OptionsSequence == 2);
                Assert.IsTrue(surveyManager.GetQuestionOptions(q2).Count == 1);

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

        [TestMethod, Description("CRUD operations for VLQuestionColumn")]
        public void SurveyQuestionsTest01_07()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            //H Default γλώσσα της συνεδρίας είναι τα Ελληνικά:
            Assert.IsTrue(surveyManager.DefaultLanguage == admin.DefaultLanguage);
            Assert.IsTrue(surveyManager.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "ερωτηματολόγιο - demo1", "ερωτηματολόγιο - demo1");
                Assert.IsNotNull(survey1);
                //Δημιουργούμε δύο σελίδες:
                var page11 = surveyManager.GetFirstSurveyPage(survey1);
                Assert.IsNotNull(page11);
                var page12 = surveyManager.CreateSurveyPage(survey1, "page12");
                Assert.IsNotNull(page12);

                //Δημιουργούμε ερωτήσεις:
                var q1 = surveyManager.CreateQuestion(page12, QuestionType.SingleLine, "Πως σας λένε");
                Assert.IsNotNull(q1);
                Assert.IsTrue(q1.ColumnsSequence == 0);
                Assert.IsTrue(q1.OptionsSequence == 0);
                var q2 = surveyManager.CreateQuestion(page12, QuestionType.MatrixManyPerRow, "Πως σας λένε2");
                Assert.IsNotNull(q2);
                Assert.IsTrue(q2.ColumnsSequence == 0);
                Assert.IsTrue(q2.OptionsSequence == 0);

                //Ελέγχουμε τα sequences επάνω στο survey:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.IsTrue(survey1.PageSequence == 2);
                Assert.IsTrue(survey1.QuestionSequence == 2);

                //Δημιουργούμε ένα column:
                var column1 = surveyManager.CreateQuestionColumn(q2.Survey, q2.QuestionId, "absolutely not true#2");
                #region
                Assert.IsNotNull(column1);
                Assert.IsTrue(column1.Survey == q2.Survey);
                Assert.IsTrue(column1.Question == q2.QuestionId);
                Assert.IsTrue(column1.DisplayOrder == 1);
                Assert.IsTrue(column1.AttributeFlags == 0);
                Assert.IsTrue(column1.TextsLanguage == q1.TextsLanguage);
                Assert.AreEqual<string>(column1.ColumnText, "absolutely not true#2");
                var svdColumn1 = surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, column1.TextsLanguage);
                Assert.AreEqual<VLQuestionColumn>(column1, svdColumn1);
                Assert.IsNull(surveyManager.GetQuestionOptionById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.English));
                #endregion

                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId, q2.TextsLanguage);
                Assert.IsTrue(q2.ColumnsSequence == 1);
                Assert.IsTrue(q2.OptionsSequence == 0);
                Assert.IsTrue(surveyManager.GetQuestionColumns(q2).Count == 1);

                //Δημιουργούμε ένα column:
                var column2 = surveyManager.CreateQuestionColumn(q2.Survey, q2.QuestionId, "not true#2");
                #region
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.Survey == q2.Survey);
                Assert.IsTrue(column2.Question == q2.QuestionId);
                Assert.IsTrue(column2.DisplayOrder == 2);
                Assert.IsTrue(column2.AttributeFlags == 0);
                Assert.IsTrue(column2.TextsLanguage == q1.TextsLanguage);
                Assert.AreEqual<string>(column2.ColumnText, "not true#2");
                var svdColumn2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, column2.TextsLanguage);
                Assert.AreEqual<VLQuestionColumn>(column2, svdColumn2);
                Assert.IsNull(surveyManager.GetQuestionOptionById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.English));
                #endregion


                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId, q2.TextsLanguage);
                Assert.IsTrue(q2.ColumnsSequence == 2);
                Assert.IsTrue(q2.OptionsSequence == 0);
                Assert.IsTrue(surveyManager.GetQuestionColumns(q2).Count == 2);

                //Κανουμε update to column2
                column2.ColumnText = "asfaerqeqeαεφαδ;3412;ε2;";
                column2 = surveyManager.UpdateQuestionColumn(column2);
                #region
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.Survey == q2.Survey);
                Assert.IsTrue(column2.Question == q2.QuestionId);
                Assert.IsTrue(column2.DisplayOrder == 2);
                Assert.IsTrue(column2.AttributeFlags == 0);
                Assert.IsTrue(column2.TextsLanguage == q1.TextsLanguage);
                Assert.AreEqual<string>(column2.ColumnText, "asfaerqeqeαεφαδ;3412;ε2;");
                svdColumn2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, column2.TextsLanguage);
                Assert.AreEqual<VLQuestionColumn>(column2, svdColumn2);
                Assert.IsNull(surveyManager.GetQuestionOptionById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.English));
                #endregion


                #region ADDLANGUAGE
                survey1 = surveyManager.AddSurveyLanguage(survey1.SurveyId, survey1.TextsLanguage, BuiltinLanguages.English);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.English);

                //read column2 in english:
                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.English);
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(column2.ColumnText, "asfaerqeqeαεφαδ;3412;ε2;");
                //read column2 in greek:
                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.Greek);
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(column2.ColumnText, "asfaerqeqeαεφαδ;3412;ε2;");

                //Update
                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.Greek);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.Greek);
                column2.ColumnText = "τελείως μάπα";
                surveyManager.UpdateQuestionColumn(column2);

                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.English);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.English);
                column2.ColumnText = "total scrap!";
                surveyManager.UpdateQuestionColumn(column2);


                //Checking:

                column1 = surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.English);
                Assert.IsNotNull(column1);
                Assert.IsTrue(column1.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(column1.ColumnText, "absolutely not true#2");

                column1 = surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.Greek);
                Assert.IsNotNull(column1);
                Assert.IsTrue(column1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(column1.ColumnText, "absolutely not true#2");

                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.English);
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.English);
                Assert.AreEqual<string>(column2.ColumnText, "total scrap!");
    
                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.Greek);
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(column2.ColumnText, "τελείως μάπα");
                #endregion


                #region REMOVE LANGUAGE
                surveyManager.RemoveSurveyLanguage(survey1.SurveyId, BuiltinLanguages.English);
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, BuiltinLanguages.Greek);
                Assert.IsNotNull(survey1);
                Assert.IsNull(surveyManager.GetSurveyById(survey1.SurveyId, BuiltinLanguages.English));


                Assert.IsNull(surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.English));

                column1 = surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.Greek);
                Assert.IsNotNull(column1);
                Assert.IsTrue(column1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(column1.ColumnText, "absolutely not true#2");

                Assert.IsNull(surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.English));

                column2 = surveyManager.GetQuestionColumnById(column2.Survey, column2.Question, column2.ColumnId, BuiltinLanguages.Greek);
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.TextsLanguage == BuiltinLanguages.Greek);
                Assert.AreEqual<string>(column2.ColumnText, "τελείως μάπα");
                #endregion



                //Delete
                surveyManager.DeleteQuestionColumn(column1);
                Assert.IsNull(surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.Greek));
                Assert.IsNull(surveyManager.GetQuestionColumnById(column1.Survey, column1.Question, column1.ColumnId, BuiltinLanguages.English));

                q2 = surveyManager.GetQuestionById(q2.Survey, q2.QuestionId, q2.TextsLanguage);
                Assert.IsTrue(q2.ColumnsSequence == 2);
                Assert.IsTrue(q2.OptionsSequence == 0);
                Assert.IsTrue(surveyManager.GetQuestionColumns(q2).Count == 1);
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
