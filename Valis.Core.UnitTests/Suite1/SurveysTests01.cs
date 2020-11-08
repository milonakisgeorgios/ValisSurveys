using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class SurveysTests01 : AdminBaseClass
    {
        [TestMethod, Description("CRUD operations for VLSurvey")]
        public void SurveyTest01()
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

                //Δεν υπάρχει κανένα survey
                Assert.IsTrue(surveyManager.GetSurveys().Count == 0);

                //Δημιουργούμε ένα νέο survey:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #1", "Risk assessment");
                #region
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.Client == client1.ClientId);
                Assert.IsNull(survey1.Folder);
                Assert.IsTrue(survey1.PublicId.Length == 32);
                Assert.AreEqual<string>(survey1.Title, "Questionnaire #1");
                Assert.IsTrue(survey1.Theme == BuiltinThemes.Default.ThemeId);
                Assert.IsNull(survey1.Logo);
                Assert.IsTrue(survey1.AttributeFlags == 0);
                Assert.IsFalse(survey1.IsBuiltIn);
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<String>(survey1.ShowTitle, "Risk assessment");
                Assert.IsNull(survey1.HeaderHtml);
                Assert.IsNull(survey1.WelcomeHtml);
                Assert.IsNull(survey1.GoodbyeHtml);
                Assert.IsNull(survey1.FooterHtml);
                Assert.AreEqual<String>(survey1.StartButton, "Start");
                Assert.AreEqual<String>(survey1.PreviousButton, "Previous");
                Assert.AreEqual<String>(survey1.NextButton, "Next");
                Assert.AreEqual<String>(survey1.DoneButton, "Submit");

                var svdSurvey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.AreEqual<VLSurvey>(survey1, svdSurvey1);
                svdSurvey1 = surveyManager.GetSurveyByPublicId(survey1.PublicId);
                Assert.AreEqual<VLSurvey>(survey1, svdSurvey1);
                #endregion


                //Κάνουμε update το survey:
                survey1.WelcomeHtml = "Welcome my clients!!!!";
                survey1.PreviousButton = "Πίσω";
                survey1.NextButton = "Μπροστά";
                survey1.Title = "Employees survey";
                survey1.PublicId = "empolyee12345";
                survey1 = surveyManager.UpdateSurvey(survey1);
                #region
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.Client == client1.ClientId);
                Assert.IsNull(survey1.Folder);
                Assert.AreEqual<string>(survey1.PublicId, "empolyee12345");
                Assert.AreEqual<string>(survey1.Title, "Employees survey");
                Assert.IsTrue(survey1.Theme == BuiltinThemes.Default.ThemeId);
                Assert.IsNull(survey1.Logo);
                Assert.IsTrue(survey1.AttributeFlags == 0);
                Assert.IsFalse(survey1.IsBuiltIn);
                Assert.IsTrue(survey1.PageSequence == 1);
                Assert.IsTrue(survey1.QuestionSequence == 0);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<String>(survey1.ShowTitle, "Risk assessment");
                Assert.AreEqual<string>(survey1.WelcomeHtml, "Welcome my clients!!!!");
                Assert.AreEqual<String>(survey1.StartButton, "Start");
                Assert.AreEqual<string>(survey1.PreviousButton, "Πίσω");
                Assert.AreEqual<string>(survey1.NextButton, "Μπροστά");
                Assert.AreEqual<String>(survey1.DoneButton, "Submit");

                svdSurvey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.AreEqual<VLSurvey>(survey1, svdSurvey1);
                svdSurvey1 = surveyManager.GetSurveyByPublicId(survey1.PublicId);
                Assert.AreEqual<VLSurvey>(survey1, svdSurvey1);
                #endregion

                //τώρα έχουμε 1 (ένα) survey:
                Assert.IsTrue(surveyManager.GetSurveys().Count == 1);

                //Δημιουργούμε ένα ακόμα νέο survey:
                var survey2 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #2", "Εκτίμηση περιβάλλοντος");
                #region
                Assert.IsNotNull(survey2);
                Assert.IsTrue(survey2.Client == client1.ClientId);
                Assert.IsNull(survey2.Folder);
                Assert.IsTrue(survey2.PublicId.Length == 32);
                Assert.AreEqual<string>(survey2.Title, "Questionnaire #2");
                Assert.IsTrue(survey2.Theme == BuiltinThemes.Default.ThemeId);
                Assert.IsNull(survey2.Logo);
                Assert.IsTrue(survey2.AttributeFlags == 0);
                Assert.IsFalse(survey2.IsBuiltIn);
                Assert.IsTrue(survey2.PageSequence == 1);
                Assert.IsTrue(survey2.QuestionSequence == 0);
                Assert.IsTrue(survey2.PrimaryLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(survey2.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<String>(survey2.ShowTitle, "Εκτίμηση περιβάλλοντος");
                Assert.IsNull(survey2.WelcomeHtml);
                Assert.AreEqual<String>(survey2.PreviousButton, "Previous");
                Assert.AreEqual<String>(survey2.NextButton, "Next");
                Assert.AreEqual<String>(survey2.DoneButton, "Submit");

                var svdSurvey2 = surveyManager.GetSurveyById(survey2.SurveyId);
                Assert.AreEqual<VLSurvey>(survey2, svdSurvey2);
                svdSurvey2 = surveyManager.GetSurveyByPublicId(survey2.PublicId);
                Assert.AreEqual<VLSurvey>(survey2, svdSurvey2);
                #endregion


                //τώρα έχουμε 2 (δύο) surveys:
                Assert.IsTrue(surveyManager.GetSurveys().Count == 2);


                //Δεν μπορούμε να δημιουργήσουμε survey με PublicId που να υπάρχει ήδη:
                survey2.PublicId = survey1.PublicId;
                _EXECUTEAndCATCH(delegate 
                { 
                    surveyManager.UpdateSurvey(survey2); 
                });

                //Δεν μπορούμε να δημιουργήσουμε survey με PublicId που να υπάρχει ήδη:
                var survey3 = new VLSurvey();
                survey3.Client = client1.ClientId;
                    survey3.Title = "survey number 4";
                    survey3.ShowTitle = "Survey Demo #4";
                    survey3.PublicId = survey1.PublicId;
                    survey3.Theme = BuiltinThemes.Default.ThemeId;
                _EXECUTEAndCATCH(delegate
                {

                    survey3 = surveyManager.CreateSurvey(survey3);
                });
                //Εάν αλλάξουμε το publicID, μπρούμε να δημιουργήσουμε΄survey:
                survey3.PublicId = Guid.NewGuid().ToString("N");
                survey3 = surveyManager.CreateSurvey(survey3);
                Assert.IsNotNull(survey3);



                //τώρα έχουμε 3 (τρία) surveys:
                Assert.IsTrue(surveyManager.GetSurveys().Count == 3);


                //Ελέγχουμε το page στοιχειωδώς:
                Int32 totalRows = 0;
                var surveys = surveyManager.GetSurveys(1, 18, ref totalRows);
                Assert.IsTrue(surveys.Count == 3);
                Assert.IsTrue(totalRows == 3);

                //Ελέγχουμε το filtering:
                surveys = surveyManager.GetSurveys("where PublicId='empolyee12345'");
                Assert.IsTrue(surveys.Count == 1);
                Assert.AreEqual<string>(surveys[0].PublicId, "empolyee12345");



                //DELETE
                surveyManager.DeleteSurvey(survey1);
                //τώρα έχουμε 2 surveys:
                Assert.IsTrue(surveyManager.GetSurveys().Count == 2);

                //DELETE
                surveyManager.DeleteSurvey(survey2);
                //τώρα έχουμε 1 survey:
                Assert.IsTrue(surveyManager.GetSurveys().Count == 1);

                //DELETE
                surveyManager.DeleteSurvey(survey3);
                //τώρα δεν έχουμε κανένα survey:
                Assert.IsTrue(surveyManager.GetSurveys().Count == 0);

                Assert.IsNull(surveyManager.GetSurveyById(survey3.SurveyId));
                Assert.IsNull(surveyManager.GetSurveyById(survey2.SurveyId));
                Assert.IsNull(surveyManager.GetSurveyById(survey1.SurveyId));
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


        [TestMethod, Description("Language operations for VLSurvey")]
        public void SurveyTest02()
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

                //δημιουργούμε ένα survey στην γερμανική γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #1", "German ShowTitle", "German WelcomeText", textsLanguage: BuiltinLanguages.German);
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.German);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.German);
                
                //Τώρα θα προσθέσουμε στο survey μία ακόμα γλώσσα:
                var survey2 = surveyManager.AddSurveyLanguage(survey1.SurveyId, survey1.TextsLanguage, BuiltinLanguages.English);
                Assert.IsNotNull(survey2);
                Assert.IsTrue(survey2.PrimaryLanguage == BuiltinLanguages.German);
                Assert.IsTrue(survey2.TextsLanguage == BuiltinLanguages.English);

                //Τώρα θα προσθέσουμε στο survey μία ακόμα γλώσσα:
                var survey3 = surveyManager.AddSurveyLanguage(survey1.SurveyId, survey1.TextsLanguage, BuiltinLanguages.Greek);
                Assert.IsNotNull(survey3);
                Assert.IsTrue(survey3.PrimaryLanguage == BuiltinLanguages.German);
                Assert.IsTrue(survey3.TextsLanguage == BuiltinLanguages.Greek);


                //Ζητάμε όλα τα variants (μεταφράσεις) για αυτό το survey:
                var variants = surveyManager.GetSurveyVariantsById(survey1.SurveyId);
                Assert.IsTrue(variants.Count == 3);

                //Refresh
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                survey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                survey3 = surveyManager.GetSurveyById(survey3.SurveyId, survey3.TextsLanguage);


                var svdSurvey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.AreEqual<VLSurvey>(survey1, svdSurvey1);
                var svdSurvey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.AreEqual<VLSurvey>(survey2, svdSurvey2);
                var svdSurvey3 = surveyManager.GetSurveyById(survey3.SurveyId, survey3.TextsLanguage);
                Assert.AreEqual<VLSurvey>(survey3, svdSurvey3);


                //Κάνουμε αλλαγές σε κάθε variant:
                survey1.ShowTitle = "GERMAN_SHOWTITLE";
                survey1.WelcomeHtml = "GERMAN_WELCOME_TEXT";
                survey1 = surveyManager.UpdateSurvey(survey1);

                survey2.ShowTitle = "ENGLISH_SHOWTITLE";
                survey2.WelcomeHtml = "ENGLISH_WELCOME_TEXT";
                survey2 = surveyManager.UpdateSurvey(survey2);

                survey3.ShowTitle = "ΕΛΛΗΝΙΚΟΣ_SHOTTITLE";
                survey3.WelcomeHtml = "ΕΛΛΗΝΙΚΟΣ_WELCOME_TEXT";
                survey3 = surveyManager.UpdateSurvey(survey3);

                svdSurvey1 = surveyManager.GetSurveyById(survey1.SurveyId, survey1.TextsLanguage);
                Assert.AreEqual<VLSurvey>(survey1, svdSurvey1);
                svdSurvey2 = surveyManager.GetSurveyById(survey2.SurveyId, survey2.TextsLanguage);
                Assert.AreEqual<VLSurvey>(survey2, svdSurvey2);
                svdSurvey3 = surveyManager.GetSurveyById(survey3.SurveyId, survey3.TextsLanguage);
                Assert.AreEqual<VLSurvey>(survey3, svdSurvey3);




                //Δεν μπορούμε να διαγράξουμε την γλώσσα εκείνη η οποία είναι η PRIMARYLANGUAGE του survey:
                _EXECUTEAndCATCH(delegate { 
                    surveyManager.RemoveSurveyLanguage(survey1.SurveyId, BuiltinLanguages.German); 
                });

                //διαγράφουμε το Ελληνικό variant:
                surveyManager.RemoveSurveyLanguage(survey1.SurveyId, BuiltinLanguages.Greek);

                //Ζητάμε όλα τα variants (μεταφράσεις) για αυτό το survey:
                variants = surveyManager.GetSurveyVariantsById(survey1.SurveyId);
                Assert.IsTrue(variants.Count == 2);


                //διαγράφουμε το Αγγλικό variant:
                surveyManager.RemoveSurveyLanguage(survey1.SurveyId, BuiltinLanguages.English);

                //Ζητάμε όλα τα variants (μεταφράσεις) για αυτό το survey:
                variants = surveyManager.GetSurveyVariantsById(survey1.SurveyId);
                Assert.IsTrue(variants.Count == 1);
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


        [TestMethod, Description("Language meanings for VLSurvey")]
        public void SurveyTest03()
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

                //δημιουργούμε ένα survey στην Default γλώσσα:
                var survey0 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #1", "Invariant ShowTitle", "Invariant WelcomeText", textsLanguage: BuiltinLanguages.DefaultLanguage);
                Assert.IsNotNull(survey0);
                Assert.IsTrue(survey0.PrimaryLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(survey0.TextsLanguage == BuiltinLanguages.Greek);

                //δημιουργούμε ένα survey στην Invariant γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #1", "Invariant ShowTitle", "Invariant WelcomeText", textsLanguage: BuiltinLanguages.Invariant);
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.Invariant);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.Invariant);
                
                //δημιουργούμε ένα survey στην English γλώσσα:
                var survey2 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #2", "English ShowTitle", "English WelcomeText", textsLanguage: BuiltinLanguages.English);
                Assert.IsNotNull(survey2);
                Assert.IsTrue(survey2.PrimaryLanguage == BuiltinLanguages.English);
                Assert.IsTrue(survey2.TextsLanguage == BuiltinLanguages.English);
                
                //δημιουργούμε ένα survey στην French γλώσσα:
                var survey3 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #3", "French ShowTitle", "French WelcomeText", textsLanguage: BuiltinLanguages.French);
                Assert.IsNotNull(survey3);
                Assert.IsTrue(survey3.PrimaryLanguage == BuiltinLanguages.French);
                Assert.IsTrue(survey3.TextsLanguage == BuiltinLanguages.French);
                
                //δημιουργούμε ένα survey στην German γλώσσα:
                var survey4 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #4", "German ShowTitle", "German WelcomeText", textsLanguage: BuiltinLanguages.German);
                Assert.IsNotNull(survey4);
                Assert.IsTrue(survey4.PrimaryLanguage == BuiltinLanguages.German);
                Assert.IsTrue(survey4.TextsLanguage == BuiltinLanguages.German);
                
                //δημιουργούμε ένα survey στην Greek γλώσσα:
                var survey5 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #5", "Greek ShowTitle", "Greek WelcomeText", textsLanguage: BuiltinLanguages.Greek);
                Assert.IsNotNull(survey5);
                Assert.IsTrue(survey5.PrimaryLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(survey5.TextsLanguage == BuiltinLanguages.Greek);
                
                //δημιουργούμε ένα survey στην French γλώσσα:
                var survey6 = surveyManager.CreateSurvey(client1.ClientId, "Questionnaire #6", "French ShowTitle", "French WelcomeText", textsLanguage: BuiltinLanguages.French);
                Assert.IsNotNull(survey6);
                Assert.IsTrue(survey6.PrimaryLanguage == BuiltinLanguages.French);
                Assert.IsTrue(survey6.TextsLanguage == BuiltinLanguages.French);




                //
                Assert.IsTrue(surveyManager.GetSurveysCount() == 7);                                                //μετράει  όλα τα surveys
                Assert.IsTrue(surveyManager.GetSurveysCount(textsLanguage: BuiltinLanguages.Invariant) == 1);       //μετράει τα ελληνικά variants
                Assert.IsTrue(surveyManager.GetSurveysCount(textsLanguage: BuiltinLanguages.English) == 1);         //μετράει τα English variants
                Assert.IsTrue(surveyManager.GetSurveysCount(textsLanguage: BuiltinLanguages.French) == 2);          //μετράει τα French variants
                Assert.IsTrue(surveyManager.GetSurveysCount(textsLanguage: BuiltinLanguages.PrimaryLanguage) == 7); //μετράει όλα τα surveys
                Assert.IsTrue(surveyManager.GetSurveysCount(textsLanguage: BuiltinLanguages.German) == 1);          //μετράει τα French variants



                Assert.IsTrue(surveyManager.GetSurveys().Count == 7);                                                //μετράει  όλα τα surveys
                Assert.IsTrue(surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.Invariant).Count == 1);       //μετράει τα ελληνικά variants
                Assert.IsTrue(surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.English).Count == 1);         //μετράει τα English variants
                Assert.IsTrue(surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.French).Count == 2);          //μετράει τα French variants
                Assert.IsTrue(surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage).Count == 7); //μετράει όλα τα surveys
                Assert.IsTrue(surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.German).Count == 1);          //μετράει τα French variants
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


        [TestMethod, Description("VLSurvey.DesignVersion")]
        public void SurveyTest04()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);

                //δημιουργούμε ένα survey στην Default γλώσσα:
                var survey1 = surveyManager.CreateSurvey(client.ClientId, "Questionnaire #1", "my ShowTitle", "my WelcomeText", textsLanguage: BuiltinLanguages.Greek);
                Assert.IsNotNull(survey1);
                Assert.IsTrue(survey1.PrimaryLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(survey1.TextsLanguage == BuiltinLanguages.Greek);
                Assert.IsTrue(survey1.DesignVersion == 0);                      //<-------

                //Τραβάμε την πρώτη σελίδα που δημιουργείται αυτόματα
                var page1 = surveyManager.GetFirstSurveyPage(survey1);

                
                //Δημιουργούμε μία ερώτηση:
                var question1 = surveyManager.CreateQuestion(page1, QuestionType.SingleLine, "my first question!");
                Assert.IsNotNull(question1);
                Assert.IsTrue(question1.Survey == survey1.SurveyId);
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 1);


                //Δημιουργούμε μία ακόμα ερώτηση:
                var question2 = surveyManager.CreateQuestion(page1, QuestionType.OneFromMany, "Where are you from?");
                Assert.IsNotNull(question2);
                Assert.IsTrue(question2.Survey == survey1.SurveyId);
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 2);



                //Δημιουργούμε μία ακόμα ερώτηση:
                var question3 = surveyManager.CreateQuestion(page1, QuestionType.MatrixOnePerRow, "Choose you ranks?");
                Assert.IsNotNull(question3);
                Assert.IsTrue(question3.Survey == survey1.SurveyId);
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 3);


                //question2
                //{
                    //Προσθέτω Options:
                    var option2_1 = surveyManager.CreateQuestionOption(survey1.SurveyId, question2.QuestionId, "Athens");
                    Assert.IsNotNull(option2_1);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 4);

                    //Προσθέτω Options:
                    var option2_2 = surveyManager.CreateQuestionOption(survey1.SurveyId, question2.QuestionId, "Patra");
                    Assert.IsNotNull(option2_2);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 5);

                    //Προσθέτω Options:
                    var option2_3 = surveyManager.CreateQuestionOption(survey1.SurveyId, question2.QuestionId, "Kalamata");
                    Assert.IsNotNull(option2_3);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 6);
                //}

                //question3
                //{
                    //Προσθέτω Options:
                    var option3_1 = surveyManager.CreateQuestionOption(survey1.SurveyId, question3.QuestionId, "Mathematica");
                    Assert.IsNotNull(option3_1);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 7);

                    //Προσθέτω Options:
                    var option3_2 = surveyManager.CreateQuestionOption(survey1.SurveyId, question3.QuestionId, "Chemistry");
                    Assert.IsNotNull(option3_2);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 8);

                    //Προσθέτω Options:
                    var option3_3 = surveyManager.CreateQuestionOption(survey1.SurveyId, question3.QuestionId, "Biology");
                    Assert.IsNotNull(option3_3);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 9);


                    //Προσθέτω column:
                    var col3_1 = surveyManager.CreateQuestionColumn(survey1.SurveyId, question3.QuestionId, "5");
                    Assert.IsNotNull(col3_1);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 10);

                    var col3_2 = surveyManager.CreateQuestionColumn(survey1.SurveyId, question3.QuestionId, "6");
                    Assert.IsNotNull(col3_2);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 11);

                    var col3_3 = surveyManager.CreateQuestionColumn(survey1.SurveyId, question3.QuestionId, "7");
                    Assert.IsNotNull(col3_3);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 12);

                    var col3_4 = surveyManager.CreateQuestionColumn(survey1.SurveyId, question3.QuestionId, "8");
                    Assert.IsNotNull(col3_4);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 13);

                    var col3_5 = surveyManager.CreateQuestionColumn(survey1.SurveyId, question3.QuestionId, "9");
                    Assert.IsNotNull(col3_5);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 14);

                    var col3_6 = surveyManager.CreateQuestionColumn(survey1.SurveyId, question3.QuestionId, "10");
                    Assert.IsNotNull(col3_6);
                    //Αλλαξε το DesignVersion:
                    survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                    Assert.IsTrue(survey1.DesignVersion == 15);
                //}

                
                //διαγράφουμε μία ερώτηση:
                surveyManager.DeleteQuestion(question1);
                Assert.IsNull(surveyManager.GetQuestionById(question1.Survey, question1.QuestionId));
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 16);
                
                //Διαγράφουμε options:
                surveyManager.DeleteQuestionOption(option2_3);
                Assert.IsNull(surveyManager.GetQuestionOptionById(option2_3.Survey, option2_3.Question, option2_3.OptionId));
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 17);

                //Διαγράφουμε options:
                surveyManager.DeleteQuestionOption(option3_2);
                Assert.IsNull(surveyManager.GetQuestionOptionById(option3_2.Survey, option3_2.Question, option3_2.OptionId));
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 18);


                //διαγράφουμε κολώνες:
                surveyManager.DeleteQuestionColumn(col3_5);
                Assert.IsNull(surveyManager.GetQuestionColumnById(col3_5.Survey, col3_5.Question, col3_5.ColumnId));
                //Αλλαξε το DesignVersion:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.DesignVersion == 19);
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
