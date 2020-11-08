using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class CopySurveyTests01 : SurveyFacilityBaseClass
    {

        void AssertSurveysAreCopies(VLSurvey source, VLSurvey copy)
        {
            Assert.IsTrue(copy.Client == source.Client);
            Assert.IsTrue(copy.SurveyId != source.SurveyId);
            Assert.IsTrue(copy.Folder == source.Folder);
            Assert.IsTrue(copy.PublicId != source.PublicId);
            Assert.IsTrue(copy.Title == source.Title);
            Assert.IsTrue(copy.Theme == source.Theme);
            Assert.IsTrue(copy.Logo == source.Logo);
            //AttributeFlags
            //PageSequence
            //QuestionSequence
            //TicketSequence
            //Assert.IsTrue(copy.PrimaryLanguage == source.PrimaryLanguage);
            //Assert.IsTrue(copy.SupportedLanguagesIds == source.SupportedLanguagesIds);
            //SupportedLanguages
            Assert.IsTrue(copy.QuestionNumberingType == source.QuestionNumberingType);
            Assert.IsTrue(copy.ProgressBarPosition == source.ProgressBarPosition);
            Assert.IsTrue(copy.RequiredHighlightType == source.RequiredHighlightType);
            //DesignVersion
            Assert.IsTrue(copy.RecordedResponses == 0);
            Assert.IsNull(copy.CustomId);
            Assert.IsFalse(copy.IsBuiltIn);

            Assert.IsTrue(copy.ShowLanguageSelector == source.ShowLanguageSelector);
            Assert.IsTrue(copy.ShowWelcomePage == source.ShowWelcomePage);
            Assert.IsTrue(copy.ShowGoodbyePage == source.ShowGoodbyePage);
            Assert.IsTrue(copy.ShowCustomFooter == source.ShowCustomFooter);
            Assert.IsTrue(copy.UsePageNumbering == source.UsePageNumbering);
            Assert.IsTrue(copy.UseQuestionNumbering == source.UseQuestionNumbering);
            Assert.IsTrue(copy.UseProgressBar == source.UseProgressBar);
            Assert.IsTrue(copy.ShowSurveyTitle == source.ShowSurveyTitle);
            Assert.IsTrue(copy.ShowPageTitles == source.ShowPageTitles);
            Assert.IsFalse(copy.SaveFilesInDatabase);
            Assert.IsFalse(copy.HasCollectors);
            Assert.IsFalse(copy.HasResponses);

            //Assert.IsTrue(copy.TextsLanguage == copy.PrimaryLanguage);
            Assert.IsTrue(copy.ShowTitle == source.ShowTitle);
            Assert.IsTrue(copy.HeaderHtml == source.HeaderHtml);
            Assert.IsTrue(copy.WelcomeHtml == source.WelcomeHtml);
            Assert.IsTrue(copy.GoodbyeHtml == source.GoodbyeHtml);
            Assert.IsTrue(copy.FooterHtml == source.FooterHtml);
            Assert.IsTrue(copy.StartButton == source.StartButton);
            Assert.IsTrue(copy.PreviousButton == source.PreviousButton);
            Assert.IsTrue(copy.NextButton == source.NextButton);
            Assert.IsTrue(copy.DoneButton == source.DoneButton);
        }
        void AssertPagesAreCopies(VLSurveyPage source, VLSurveyPage copy)
        {

            Assert.IsTrue(copy.Survey != source.Survey);
            //copy.PageId

            Assert.IsTrue(copy.DisplayOrder == source.DisplayOrder);
            Assert.IsTrue(copy.PreviousPage == source.PreviousPage);
            Assert.IsTrue(copy.NextPage == source.NextPage);
            Assert.IsTrue(copy.AttributeFlags == default(Int32));
            Assert.IsTrue(copy.CustomId == default(string));
            Assert.IsTrue(copy.SkipTo == SkipToBehavior.None);
            Assert.IsNull(copy.SkipToPage);
            Assert.IsNull(copy.SkipToWebUrl);
            //copy.TextsLanguage
            Assert.IsTrue(copy.ShowTitle == source.ShowTitle);
            Assert.IsTrue(copy.Description == source.Description);
        }
        void AssertQuestionsAreCopies(VLSurveyQuestion source, VLSurveyQuestion copy)
        {
            Assert.IsTrue(copy.Survey != source.Survey);
            //copy.QuestionId
            //copy.Page
            Assert.IsTrue(copy.MasterQuestion == source.MasterQuestion);
            Assert.IsTrue(copy.DisplayOrder == source.DisplayOrder);
            Assert.IsTrue(copy.QuestionType == source.QuestionType);
            Assert.IsTrue(copy.CustomType == source.CustomType);
            Assert.IsTrue(copy.IsRequired == source.IsRequired);
            Assert.IsTrue(copy.RequiredBehavior == source.RequiredBehavior);
            Assert.IsTrue(copy.RequiredMinLimit == source.RequiredMinLimit);
            Assert.IsTrue(copy.RequiredMaxLimit == source.RequiredMaxLimit);
            Assert.IsTrue(copy.AttributeFlags == default(Int32));

            //copy.TextsLanguage
            Assert.IsTrue(copy.QuestionText == source.QuestionText);
            Assert.IsTrue(copy.Description == source.Description);
            Assert.IsTrue(copy.HelpText == source.HelpText);
            Assert.IsTrue(copy.FrontLabelText == source.FrontLabelText);
            Assert.IsTrue(copy.AfterLabelText == source.AfterLabelText);
            Assert.IsTrue(copy.InsideText == source.InsideText);
            Assert.IsTrue(copy.RequiredMessage == source.RequiredMessage);
            Assert.IsTrue(copy.ValidationMessage == source.ValidationMessage);
            Assert.IsTrue(copy.OtherFieldLabel == source.OtherFieldLabel);
        }

        /// <summary>
        /// αντιγραφή invariant survey
        /// </summary>
        [TestMethod, Description("")]
        public void CopySurveyTests01_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                //We create a survey:
                var source01 = surveyManager.CreateSurvey(client1, "Employee survey", textsLanguage: BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsFalse(source01.ShowWelcomePage);
                source01.ShowWelcomePage = true;
                source01.WelcomeHtml = "This is μήνυμα καλωσορίσματος";
                Assert.IsFalse(source01.ShowGoodbyePage);
                source01.ShowGoodbyePage = true;
                source01.GoodbyeHtml = "αντίο σας, θέλουμε να ξανάρθετε!";
                Assert.IsFalse(source01.ShowCustomFooter);
                source01.ShowCustomFooter = false;
                source01.FooterHtml = "<h1>Απο τον george μυλωνάκη!!</h1>";
                source01 = surveyManager.UpdateSurvey(source01);

                var page01 = surveyManager.GetFirstSurveyPage(source01);
                Assert.IsTrue(page01.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                page01.ShowTitle = "Αυτή είναι η πρώτη σελίδα μου";
                page01.Description = "Με την αφαίρεση τριών σειρών από τους πωρόλιθους του τοίχου σφράγισης, μπροστά από τον δεύτερο διαφραγματικό τοίχο, αποκαλύφθηκαν ολόκληρες οι Καρυάτιδες";
                page01 = surveyManager.UpdateSurveyPage(page01);

                var quesion01 = surveyManager.CreateQuestion(page01, QuestionType.SingleLine, "Hello, πως σε λένε?");
                Assert.IsTrue(quesion01.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);

                //τώρα ο πελάτης μας έχει μόνο ένα survey:
                Assert.IsTrue(surveyManager.GetSurveysCount() == 1);
                Assert.IsTrue(surveyManager.GetSurveys().Count == 1);
                source01 = surveyManager.GetSurveyById(source01.SurveyId, BuiltinLanguages.PrimaryLanguage.LanguageId);
                Assert.IsTrue(source01.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01.PageSequence == 1);
                Assert.IsTrue(source01.QuestionSequence == 1);
                Assert.IsTrue(surveyManager.GetSurveyPages(source01).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(source01).Count == 1);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(source01.SurveyId).Count == 1);
                
                
                //Δημιουργούμε ένα αντίγραφο του source01:
                var copy01 = surveyManager.CopySurvey(source01, null);
                Assert.IsNotNull(copy01);
                AssertSurveysAreCopies(source01, copy01);
                Assert.IsTrue(surveyManager.GetSurveyPages(copy01).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(copy01).Count == 1);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(copy01.SurveyId).Count == 1);
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


        /// <summary>
        /// αντιγραφή survey με δύο γλώσσες!
        /// </summary>
        [TestMethod, Description("")]
        public void CopySurveyTests01_02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                //We create a survey:
                var source01el = surveyManager.CreateSurvey(client1, "Employee survey", textsLanguage: BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(source01el.PrimaryLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsFalse(source01el.ShowWelcomePage);
                source01el.ShowWelcomePage = true;
                source01el.WelcomeHtml = "This is μήνυμα καλωσορίσματος";
                Assert.IsFalse(source01el.ShowGoodbyePage);
                source01el.ShowGoodbyePage = true;
                source01el.GoodbyeHtml = "αντίο σας, θέλουμε να ξανάρθετε!";
                Assert.IsFalse(source01el.ShowCustomFooter);
                source01el.ShowCustomFooter = false;
                source01el.FooterHtml = "<h1>Απο τον george μυλωνάκη!!</h1>";
                source01el = surveyManager.UpdateSurvey(source01el);

                var page01el = surveyManager.GetFirstSurveyPage(source01el);
                Assert.IsTrue(page01el.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                page01el.ShowTitle = "Αυτή είναι η πρώτη σελίδα μου";
                page01el.Description = "Με την αφαίρεση τριών σειρών από τους πωρόλιθους του τοίχου σφράγισης, μπροστά από τον δεύτερο διαφραγματικό τοίχο, αποκαλύφθηκαν ολόκληρες οι Καρυάτιδες";
                page01el = surveyManager.UpdateSurveyPage(page01el);

                var question01el = surveyManager.CreateQuestion(page01el, QuestionType.SingleLine, "Γειά σου, πως σε λένε?");
                Assert.IsTrue(question01el.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                question01el.Description = "μία περιγραφή για την ερώτησή μας!";
                question01el = surveyManager.UpdateQuestion(question01el);

                var question02el = surveyManager.CreateQuestion(page01el, QuestionType.DropDown, "Επελεξε ονομα:");
                Assert.IsTrue(question02el.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                question02el.Description = "Αυτό το όνομα θα χρησιμοποιηθεί κατα την διάρκεια του παιχνιδιού";
                question02el = surveyManager.UpdateQuestion(question02el);
                var option02el1 = surveyManager.CreateQuestionOption(source01el.SurveyId, question02el.QuestionId, "Γιώργος");
                Assert.IsTrue(option02el1.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                var option02el2 = surveyManager.CreateQuestionOption(source01el.SurveyId, question02el.QuestionId, "Γιάννης");
                Assert.IsTrue(option02el2.TextsLanguage == BuiltinLanguages.Greek.LanguageId);


                //Μεταφράζουμε το survey στην αγγλική:
                var source01en = surveyManager.AddSurveyLanguage(source01el.SurveyId, source01el.TextsLanguage, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(source01en.TextsLanguage == BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(source01en.PrimaryLanguage == BuiltinLanguages.Greek.LanguageId);
                source01en.WelcomeHtml = "adfasf 24cv24cv1234cv24crx23";
                source01en.GoodbyeHtml = "!@%!@$RWFSGFERT134t123452`$%!@#$!@#$!@";
                source01en.FooterHtml = "!@#$!@#$!@#<>@#R!@#ER<@#>R!@#ER!@#ER@<>#RQW$EFRQWERQWE";
                source01en = surveyManager.UpdateSurvey(source01en);

                var page01en = surveyManager.GetSurveyPageById(page01el.Survey, page01el.PageId, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(page01en.TextsLanguage == BuiltinLanguages.English.LanguageId);
                page01en.ShowTitle = "this is my first page!";
                page01en.Description = "The iFrame element may be used to display just about any Web content. You have complete control over embedded content, thanks to the Document Object Model";
                page01en = surveyManager.UpdateSurveyPage(page01en);

                var question01en = surveyManager.GetQuestionById(question01el.Survey, question01el.QuestionId, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(question01en.TextsLanguage == BuiltinLanguages.English.LanguageId);
                question01en.QuestionText = "Hello, what is your name?";
                question01en.Description = "a small description foa our question!!";
                question01en = surveyManager.UpdateQuestion(question01en);

                var question02en = surveyManager.GetQuestionById(question02el.Survey, question02el.QuestionId, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(question02en.TextsLanguage == BuiltinLanguages.English.LanguageId);
                question02en.QuestionText = "Chhose a name?";
                question02en.Description = "THis is your name during the game!";
                question02en = surveyManager.UpdateQuestion(question02en);
                var option02en1 = surveyManager.GetQuestionOptionById(option02el1.Survey, option02el1.Question, option02el1.OptionId, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(option02en1.TextsLanguage == BuiltinLanguages.English.LanguageId);
                option02en1.OptionText = "George";
                option02en1 = surveyManager.UpdateQuestionOption(option02en1);
                var option02en2 = surveyManager.GetQuestionOptionById(option02el2.Survey, option02el2.Question, option02el2.OptionId, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(option02en2.TextsLanguage == BuiltinLanguages.English.LanguageId);
                option02en2.OptionText = "John";
                option02en2 = surveyManager.UpdateQuestionOption(option02en2);


                //τώρα ο πελάτης μας έχει μόνο ένα survey:
                Assert.IsTrue(surveyManager.GetSurveysCount() == 1);
                Assert.IsTrue(surveyManager.GetSurveys().Count == 1);
                source01el = surveyManager.GetSurveyById(source01el.SurveyId, BuiltinLanguages.PrimaryLanguage.LanguageId);
                Assert.IsTrue(source01el.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(source01el.PageSequence == 1);
                Assert.IsTrue(source01el.QuestionSequence == 2);
                Assert.IsTrue(surveyManager.GetSurveyPages(source01el).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(source01el).Count == 2);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(source01el.SurveyId).Count == 2);



                //Δημιουργούμε ένα αντίγραφο του source01, αλλά μόνο το Ελληνικό variance:
                var copy01 = surveyManager.CopySurvey(source01el, null, BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(copy01.PrimaryLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(copy01.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(copy01.SupportedLanguages.Count == 1);
                Assert.IsTrue(copy01.SupportsLanguage(BuiltinLanguages.Greek.LanguageId));
                Assert.IsFalse(copy01.SupportsLanguage(BuiltinLanguages.English.LanguageId));
                AssertSurveysAreCopies(source01el, copy01);
                var copyPage01 = surveyManager.GetFirstSurveyPage(copy01);
                AssertPagesAreCopies(page01el, copyPage01);
                var copyQuestion01 = surveyManager.GetQuestionById(copy01.SurveyId, question01el.QuestionId, BuiltinLanguages.Greek.LanguageId);
                AssertQuestionsAreCopies(question01el, copyQuestion01);
                var copyQuestion02 = surveyManager.GetQuestionById(copy01.SurveyId, question02el.QuestionId, BuiltinLanguages.Greek.LanguageId);
                AssertQuestionsAreCopies(question02el, copyQuestion02);
                Assert.IsTrue(surveyManager.GetSurveyPages(copy01).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(copy01).Count == 2);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(copy01.SurveyId).Count == 1);

                //Δημιουργούμε ένα αντίγραφο του source01, αλλά μόνο το English variance:
                var copy02 = surveyManager.CopySurvey(source01el, null, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(copy02.PrimaryLanguage == BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(copy02.TextsLanguage == BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(copy02.SupportedLanguages.Count == 1);
                Assert.IsFalse(copy02.SupportsLanguage(BuiltinLanguages.Greek.LanguageId));
                Assert.IsTrue(copy02.SupportsLanguage(BuiltinLanguages.English.LanguageId));
                AssertSurveysAreCopies(source01en, copy02);
                var copy02Page01 = surveyManager.GetFirstSurveyPage(copy02);
                AssertPagesAreCopies(page01en, copy02Page01);
                var copy02Question01 = surveyManager.GetQuestionById(copy02.SurveyId, question01el.QuestionId, BuiltinLanguages.English.LanguageId);
                AssertQuestionsAreCopies(question01en, copy02Question01);
                var copy02Question02 = surveyManager.GetQuestionById(copy02.SurveyId, question02el.QuestionId, BuiltinLanguages.English.LanguageId);
                AssertQuestionsAreCopies(question02en, copy02Question02);
                Assert.IsTrue(surveyManager.GetSurveyPages(copy02).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(copy02).Count == 2);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(copy02.SurveyId).Count == 1);

                //Δημιουργούμε ένα αντίγραφο σε όλες τις γλώσσες
                var copy03 = surveyManager.CopySurvey(source01el, null, BuiltinLanguages.AllLanguages.LanguageId);
                Assert.IsTrue(copy03.PrimaryLanguage == source01el.PrimaryLanguage);
                Assert.IsTrue(copy03.TextsLanguage == source01el.PrimaryLanguage);
                Assert.IsTrue(copy03.SupportedLanguages.Count == 2);
                Assert.IsTrue(copy03.SupportsLanguage(BuiltinLanguages.Greek.LanguageId));
                Assert.IsTrue(copy03.SupportsLanguage(BuiltinLanguages.English.LanguageId));
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


        /// <summary>
        /// Copy Invariant surveys
        /// </summary>
        [TestMethod, Description("")]
        public void CopySurveyTests01_03()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                //We create a survey:
                var source01el = surveyManager.CreateSurvey(client1, "Employee survey", textsLanguage: BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01el.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsFalse(source01el.ShowWelcomePage);
                source01el.ShowWelcomePage = true;
                source01el.WelcomeHtml = "This is μήνυμα καλωσορίσματος";
                Assert.IsFalse(source01el.ShowGoodbyePage);
                source01el.ShowGoodbyePage = true;
                source01el.GoodbyeHtml = "αντίο σας, θέλουμε να ξανάρθετε!";
                Assert.IsFalse(source01el.ShowCustomFooter);
                source01el.ShowCustomFooter = false;
                source01el.FooterHtml = "<h1>Απο τον george μυλωνάκη!!</h1>";
                source01el = surveyManager.UpdateSurvey(source01el);

                var page01el = surveyManager.GetFirstSurveyPage(source01el);
                Assert.IsTrue(page01el.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                page01el.ShowTitle = "Αυτή είναι η πρώτη σελίδα μου";
                page01el.Description = "Με την αφαίρεση τριών σειρών από τους πωρόλιθους του τοίχου σφράγισης, μπροστά από τον δεύτερο διαφραγματικό τοίχο, αποκαλύφθηκαν ολόκληρες οι Καρυάτιδες";
                page01el = surveyManager.UpdateSurveyPage(page01el);

                var question01el = surveyManager.CreateQuestion(page01el, QuestionType.SingleLine, "Γειά σου, πως σε λένε?");
                Assert.IsTrue(question01el.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                question01el.Description = "μία περιγραφή για την ερώτησή μας!";
                question01el = surveyManager.UpdateQuestion(question01el);

                var question02el = surveyManager.CreateQuestion(page01el, QuestionType.DropDown, "Επελεξε ονομα:");
                Assert.IsTrue(question02el.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                question02el.Description = "Αυτό το όνομα θα χρησιμοποιηθεί κατα την διάρκεια του παιχνιδιού";
                question02el = surveyManager.UpdateQuestion(question02el);
                var option02el1 = surveyManager.CreateQuestionOption(source01el.SurveyId, question02el.QuestionId, "Γιώργος");
                Assert.IsTrue(option02el1.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                var option02el2 = surveyManager.CreateQuestionOption(source01el.SurveyId, question02el.QuestionId, "Γιάννης");
                Assert.IsTrue(option02el2.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);



                
                //τώρα ο πελάτης μας έχει μόνο ένα survey:
                Assert.IsTrue(surveyManager.GetSurveysCount() == 1);
                Assert.IsTrue(surveyManager.GetSurveys().Count == 1);
                source01el = surveyManager.GetSurveyById(source01el.SurveyId, BuiltinLanguages.PrimaryLanguage.LanguageId);
                Assert.IsTrue(source01el.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01el.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                source01el = surveyManager.GetSurveyById(source01el.SurveyId, BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01el.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01el.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                source01el = surveyManager.GetSurveyById(source01el.SurveyId, BuiltinLanguages.DefaultLanguage.LanguageId);
                Assert.IsTrue(source01el.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(source01el.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);

                Assert.IsTrue(source01el.PageSequence == 1);
                Assert.IsTrue(source01el.QuestionSequence == 2);
                Assert.IsTrue(surveyManager.GetSurveyPages(source01el).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(source01el).Count == 2);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(source01el.SurveyId).Count == 1);




                //Δημιουργούμε ένα αντίγραφο του source01:
                var copy01 = surveyManager.CopySurvey(source01el, null, BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(copy01.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(copy01.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(copy01.SupportedLanguages.Count == 1);
                Assert.IsTrue(copy01.SupportsLanguage(BuiltinLanguages.Invariant.LanguageId));
                Assert.IsFalse(copy01.SupportsLanguage(BuiltinLanguages.English.LanguageId));
                Assert.IsFalse(copy01.SupportsLanguage(BuiltinLanguages.Greek.LanguageId));
                AssertSurveysAreCopies(source01el, copy01);
                var copyPage01 = surveyManager.GetFirstSurveyPage(copy01);
                AssertPagesAreCopies(page01el, copyPage01);
                var copyQuestion01 = surveyManager.GetQuestionById(copy01.SurveyId, question01el.QuestionId, BuiltinLanguages.Invariant.LanguageId);
                AssertQuestionsAreCopies(question01el, copyQuestion01);
                var copyQuestion02 = surveyManager.GetQuestionById(copy01.SurveyId, question02el.QuestionId, BuiltinLanguages.Invariant.LanguageId);
                AssertQuestionsAreCopies(question02el, copyQuestion02);
                Assert.IsTrue(surveyManager.GetSurveyPages(copy01).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(copy01).Count == 2);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(copy01.SurveyId).Count == 1);

                //Δημιουργούμε ένα αντίγραφο του source01:
                var copy02 = surveyManager.CopySurvey(source01el, null, BuiltinLanguages.AllLanguages.LanguageId);
                Assert.IsTrue(copy02.PrimaryLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(copy02.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(copy02.SupportedLanguages.Count == 1);
                Assert.IsTrue(copy02.SupportsLanguage(BuiltinLanguages.Invariant.LanguageId));
                Assert.IsFalse(copy02.SupportsLanguage(BuiltinLanguages.English.LanguageId));
                Assert.IsFalse(copy02.SupportsLanguage(BuiltinLanguages.Greek.LanguageId));
                AssertSurveysAreCopies(source01el, copy02);
                var copy02Page01 = surveyManager.GetFirstSurveyPage(copy02);
                AssertPagesAreCopies(page01el, copy02Page01);
                var copy02Question01 = surveyManager.GetQuestionById(copy02.SurveyId, question01el.QuestionId, BuiltinLanguages.Invariant.LanguageId);
                AssertQuestionsAreCopies(question01el, copy02Question01);
                var copy02Question02 = surveyManager.GetQuestionById(copy02.SurveyId, question02el.QuestionId, BuiltinLanguages.Invariant.LanguageId);
                AssertQuestionsAreCopies(question02el, copy02Question02);
                Assert.IsTrue(surveyManager.GetSurveyPages(copy02).Count == 1);
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(copy02).Count == 2);
                Assert.IsTrue(surveyManager.GetSurveyVariantsById(copy02.SurveyId).Count == 1);


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
