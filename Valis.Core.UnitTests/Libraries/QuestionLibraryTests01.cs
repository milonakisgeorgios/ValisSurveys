using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Libraries
{
    [TestClass]
    public class QuestionLibraryTests01 : AdminBaseClass
    {

        [TestMethod, Description("CRUD tests for LibraryQuestionCategories")]
        public void QuestionLibraryTests01_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var libraryManager = VLLibraryManager.GetAnInstance(admin);

            try
            {
                //Στο σύστημα υπάρχει μία κατηγορία ερωτήσεων:
                Assert.IsTrue(libraryManager.GetLibraryQuestionCategories().Count == 1);
                //υπάρχει σε τρείς γλώσσες:
                Assert.IsNotNull(libraryManager.GetLibraryQuestionCategoryById(1, BuiltinLanguages.Invariant));
                Assert.IsNotNull(libraryManager.GetLibraryQuestionCategoryById(1, BuiltinLanguages.English));
                Assert.IsNotNull(libraryManager.GetLibraryQuestionCategoryById(1, BuiltinLanguages.Greek));

                //Δημιουργούμε μία νέα κατηγορία:
                var category2 = libraryManager.CreateLibraryQuestionCategory("Δημογραφικές ερωτήσεις", BuiltinLanguages.Greek);
                Assert.IsNotNull(category2);
                Assert.AreEqual<string>(category2.Name, "Δημογραφικές ερωτήσεις");
                Assert.IsFalse(category2.IsBuiltIn);
                Assert.IsTrue(category2.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                var svdCategory2 = libraryManager.GetLibraryQuestionCategoryById(category2.CategoryId, category2.TextsLanguage);
                Assert.AreEqual<VLLibraryQuestionCategory>(category2, svdCategory2);


                //εχουμε δύο εγγραφές στο σύστημα:
                Assert.IsTrue(libraryManager.GetLibraryQuestionCategories().Count == 2);


                //Δεν επιτρέπεται να δημιουργήσουμε κατηγορία με το ίδιο όνομα:
                _EXECUTEAndCATCHType(delegate { libraryManager.CreateLibraryQuestionCategory("Δημογραφικές ερωτήσεις", BuiltinLanguages.Greek);  }, typeof(VLException));
               
 
                //Κάνουμε update ανα γλώσσα
                var category2el = libraryManager.GetLibraryQuestionCategoryById(category2.CategoryId, BuiltinLanguages.Greek);
                Assert.IsTrue(category2el.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                category2el.Name = "greek23";
                category2el = libraryManager.UpdateLibraryQuestionCategory(category2el);
                Assert.IsTrue(category2el.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>(category2el.Name, "greek23");


                var category2en = libraryManager.GetLibraryQuestionCategoryById(category2.CategoryId, BuiltinLanguages.English);
                Assert.IsTrue(category2en.TextsLanguage == BuiltinLanguages.English.LanguageId);
                category2en.Name = "fasdfwrfwf";
                category2en = libraryManager.UpdateLibraryQuestionCategory(category2en);
                Assert.IsTrue(category2en.TextsLanguage == BuiltinLanguages.English.LanguageId);
                Assert.AreEqual<string>(category2en.Name, "fasdfwrfwf");


                var category2inv = libraryManager.GetLibraryQuestionCategoryById(category2.CategoryId, BuiltinLanguages.Invariant);
                Assert.IsTrue(category2inv.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                category2inv.Name = "!#$!#E1234!@#Q@e";
                category2inv = libraryManager.UpdateLibraryQuestionCategory(category2inv);
                Assert.IsTrue(category2inv.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.AreEqual<string>(category2inv.Name, "!#$!#E1234!@#Q@e");


                //Δεν επιτρέπεται να δημιουργήσουμε κατηγορία με το ίδιο όνομα:
                _EXECUTEAndCATCHType(delegate { libraryManager.CreateLibraryQuestionCategory("greek23", BuiltinLanguages.Greek); }, typeof(VLException));

                //Δεν επιτρέπεται να δημιουργήσουμε κατηγορία με το ίδιο όνομα:
                _EXECUTEAndCATCHType(delegate { libraryManager.CreateLibraryQuestionCategory("fasdfwrfwf", BuiltinLanguages.Greek); }, typeof(VLException));

                //Δεν επιτρέπεται να δημιουργήσουμε κατηγορία με το ίδιο όνομα:
                _EXECUTEAndCATCHType(delegate { libraryManager.CreateLibraryQuestionCategory("!#$!#E1234!@#Q@e", BuiltinLanguages.Greek); }, typeof(VLException));
               
            }
            finally
            {
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
            }
        }


        [TestMethod, Description("CRUD tests for LibraryQuestions")]
        public void QuestionLibraryTests01_02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var libraryManager = VLLibraryManager.GetAnInstance(admin);

            try
            {
                //Στο σύστημα υπάρχει μία κατηγορία ερωτήσεων:
                var categories = libraryManager.GetLibraryQuestionCategories();
                Assert.IsTrue(categories.Count == 1);
                var category1 = categories[0];
                //για αυτή την κατηγορία, δεν έχουμε καθόλου ερωτήσεις:
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 0);

                //Δημιουργούμε μία ακόμα κατηγορία
                var category2  = libraryManager.CreateLibraryQuestionCategory("my-category");
                Assert.IsNotNull(category2);
                //για αυτή την κατηγορία, δεν έχουμε καθόλου ερωτήσεις:
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category2.CategoryId).Count == 0);


                //We create a new question:
                var question1 = libraryManager.CreateLibraryQuestion(category1.CategoryId, QuestionType.SingleLine, "What is your name?");
                Assert.IsNotNull(question1);
                Assert.IsTrue(question1.Category == category1.CategoryId);
                Assert.IsTrue(question1.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(question1.QuestionType == QuestionType.SingleLine);
                Assert.AreEqual<string>("What is your name?", question1.QuestionText);
                var svdQuestion1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.AreEqual<VLLibraryQuestion>(question1, svdQuestion1);

                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 1);
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category2.CategoryId).Count == 0);



                /*
                 * Οταν δημιουργούμε ένα item στην Library, τότε αυτό δημιουργεί variances σε όλες τις υποστηριζόμενες 
                 * γλώσσες του συστήματος μας, τις χρειαστούμε δεν τις χρειαστούμε.
                 */
                {
                    var _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                    _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.Bulgarian.LanguageId);
                    _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.English.LanguageId);
                    _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.French.LanguageId);
                    _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.German.LanguageId);
                    _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                    _tq = libraryManager.GetLibraryQuestionById(question1.QuestionId, BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_tq.TextsLanguage == BuiltinLanguages.Russian.LanguageId);
                }



                //We create a new question:
                var question2 = libraryManager.CreateLibraryQuestion(category1.CategoryId, QuestionType.SingleLine, "What is your age?");
                Assert.IsNotNull(question2);
                Assert.IsTrue(question2.Category == category1.CategoryId);
                Assert.IsTrue(question2.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(question2.QuestionType == QuestionType.SingleLine);
                Assert.AreEqual<string>("What is your age?", question2.QuestionText);
                var svdQuestion2 = libraryManager.GetLibraryQuestionById(question2.QuestionId, question2.TextsLanguage);
                Assert.AreEqual<VLLibraryQuestion>(question2, svdQuestion2);


                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 2);
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category2.CategoryId).Count == 0);



                //We create a new question:
                var question3 = libraryManager.CreateLibraryQuestion(category2.CategoryId, QuestionType.MultipleLine, "What is your comment?");
                Assert.IsNotNull(question3);
                Assert.IsTrue(question3.Category == category2.CategoryId);
                Assert.IsTrue(question3.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(question3.QuestionType == QuestionType.MultipleLine);
                Assert.AreEqual<string>("What is your comment?", question3.QuestionText);
                var svdQuestion3 = libraryManager.GetLibraryQuestionById(question3.QuestionId, question3.TextsLanguage);
                Assert.AreEqual<VLLibraryQuestion>(question3, svdQuestion3);



                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 2);
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category2.CategoryId).Count == 1);


                //Δεν επιτρέπεται να διαγράψουμε μία κατηγορία όταν έχει ερωτήσεις απο κάτω της:
                _EXECUTEAndCATCHType(delegate { libraryManager.DeleteLibraryQuestionCategory(category2.CategoryId); }, typeof(VLException));

                //Κάνουμε update
                question2.QuestionText = "tralalala!!!";
                question2 = libraryManager.UpdateLibraryQuestion(question2);
                Assert.IsTrue(question2.Category == category1.CategoryId);
                Assert.IsTrue(question2.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(question2.QuestionType == QuestionType.SingleLine);
                Assert.AreEqual<string>("tralalala!!!", question2.QuestionText);
                svdQuestion2 = libraryManager.GetLibraryQuestionById(question2.QuestionId, question2.TextsLanguage);
                Assert.AreEqual<VLLibraryQuestion>(question2, svdQuestion2);

                //διαγραφή
                libraryManager.DeleteLibraryQuestion(question2.QuestionId);
                Assert.IsNull(libraryManager.GetLibraryQuestionById(question2.QuestionId, question2.TextsLanguage));


                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 1);
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category2.CategoryId).Count == 1);
            }
            finally
            {
                var categories = libraryManager.GetLibraryQuestionCategories();
                foreach (var item in categories)
                {
                    var questions = libraryManager.GetLibraryQuestions(item.CategoryId);
                    foreach(var q in questions)
                    {
                        libraryManager.DeleteLibraryQuestion(q.QuestionId);
                    }


                    if (item.IsBuiltIn)
                        continue;

                    libraryManager.DeleteLibraryQuestionCategory(item);
                }
            }
        }


        [TestMethod, Description("CRUD tests for LibraryOptions")]
        public void QuestionLibraryTests01_03()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var libraryManager = VLLibraryManager.GetAnInstance(admin);

            try
            {
                //Στο σύστημα υπάρχει μία κατηγορία ερωτήσεων:
                var categories = libraryManager.GetLibraryQuestionCategories();
                Assert.IsTrue(categories.Count == 1);
                var category1 = categories[0];
                //για αυτή την κατηγορία, δεν έχουμε καθόλου ερωτήσεις:
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 0);


                //We create a new question:
                var question1 = libraryManager.CreateLibraryQuestion(category1.CategoryId, QuestionType.DropDown, "Choose your age:");
                Assert.IsNotNull(question1);
                Assert.IsTrue(question1.Category == category1.CategoryId);
                Assert.IsTrue(question1.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(question1.QuestionType == QuestionType.DropDown);
                Assert.AreEqual<string>("Choose your age:", question1.QuestionText);
                Assert.IsTrue(question1.OptionsSequence == 0);
                Assert.IsTrue(question1.ColumnsSequence == 0);


                //Δεν υπάρχει κανένα option ακόμα:
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 0);

                //Create New Option:
                var option1 = libraryManager.CreateLibraryQuestionOption(question1, "first choice (10)!");
                Assert.IsNotNull(option1);
                Assert.IsTrue(option1.OptionId == 1);
                Assert.IsTrue(option1.Question == question1.QuestionId);
                Assert.IsTrue(option1.OptionText == "first choice (10)!");
                var svdOption1 = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, option1.TextsLanguage);
                Assert.AreEqual<VLLibraryOption>(option1, svdOption1);

                //
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 1);

                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 1);
                Assert.IsTrue(question1.ColumnsSequence == 0);

                //Create New Option:
                var option2 = libraryManager.CreateLibraryQuestionOption(question1, "2nd choice (12)!");
                Assert.IsTrue(option2.OptionId == 2);
                Assert.IsTrue(option2.Question == question1.QuestionId);
                Assert.IsTrue(option2.OptionText == "2nd choice (12)!");
                //Create New Option:
                var option3 = libraryManager.CreateLibraryQuestionOption(question1, "3rd choice (14)!");
                Assert.IsTrue(option3.OptionId == 3);
                Assert.IsTrue(option3.Question == question1.QuestionId);
                Assert.IsTrue(option3.OptionText == "3rd choice (14)!");

                //
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 3);

                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 3);
                Assert.IsTrue(question1.ColumnsSequence == 0);



                //UPDATE
                Assert.IsTrue(option2.OptionText == "2nd choice (12)!");
                option2.OptionText = "tralalalala!";
                option2 = libraryManager.UpdateLibraryQuestionOption(option2);
                Assert.IsTrue(option2.OptionId == 2);
                Assert.IsTrue(option2.Question == question1.QuestionId);
                Assert.IsTrue(option2.OptionText == "tralalalala!");


                //DELETE
                libraryManager.DeleteLibraryQuestionOption(option2);
                Assert.IsNull(libraryManager.GetLibraryQuestionOptionById(option2.Question, option2.OptionId, option2.TextsLanguage));


                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 2);

                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 3);
                Assert.IsTrue(question1.ColumnsSequence == 0);
            }
            finally
            {
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
            }
        }


        [TestMethod, Description("Translation tests for LibraryOptions")]
        public void QuestionLibraryTests01_04()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var libraryManager = VLLibraryManager.GetAnInstance(admin);

            try
            {
                #region δημιουργούμε μία ερώτηση
                //Στο σύστημα υπάρχει μία κατηγορία ερωτήσεων:
                var categories = libraryManager.GetLibraryQuestionCategories();
                Assert.IsTrue(categories.Count == 1);
                var category1 = categories[0];
                //για αυτή την κατηγορία, δεν έχουμε καθόλου ερωτήσεις:
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 0);

                //We create a new question:
                var question1 = libraryManager.CreateLibraryQuestion(category1.CategoryId, QuestionType.DropDown, "Choose your age:");
                Assert.IsNotNull(question1);
                #endregion

                //Create New Option:
                var option1 = libraryManager.CreateLibraryQuestionOption(question1, "first choice (10)!");
                Assert.IsNotNull(option1);
                //Create New Option:
                var option2 = libraryManager.CreateLibraryQuestionOption(question1, "2nd choice (12)!");
                Assert.IsNotNull(option2);

                //Εχουμε δύο options:
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 2);
                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 2);
                Assert.IsTrue(question1.ColumnsSequence == 0);

                /*
                 * Οταν δημιουργούμε ένα item στην Library, τότε αυτό δημιουργεί variances σε όλες τις υποστηριζόμενες 
                 * γλώσσες του συστήματος μας, τις χρειαστούμε δεν τις χρειαστούμε.
                 */
                #region
                var _option1_inv = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(_option1_inv.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(_option1_inv.OptionText == "first choice (10)!");
                var _option1_bul = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Bulgarian.LanguageId);
                Assert.IsTrue(_option1_bul.TextsLanguage == BuiltinLanguages.Bulgarian.LanguageId);
                Assert.IsTrue(_option1_bul.OptionText == "first choice (10)!");
                var _option1_eng = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(_option1_eng.TextsLanguage == BuiltinLanguages.English.LanguageId);
                Assert.IsTrue(_option1_eng.OptionText == "first choice (10)!");
                var _option1_fra = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.French.LanguageId);
                Assert.IsTrue(_option1_fra.TextsLanguage == BuiltinLanguages.French.LanguageId);
                Assert.IsTrue(_option1_fra.OptionText == "first choice (10)!");
                var _option1_deu = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.German.LanguageId);
                Assert.IsTrue(_option1_deu.TextsLanguage == BuiltinLanguages.German.LanguageId);
                Assert.IsTrue(_option1_deu.OptionText == "first choice (10)!");
                var _option1_ell = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(_option1_ell.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.IsTrue(_option1_ell.OptionText == "first choice (10)!");
                var _option1_rus = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Russian.LanguageId);
                Assert.IsTrue(_option1_rus.TextsLanguage == BuiltinLanguages.Russian.LanguageId);
                Assert.IsTrue(_option1_rus.OptionText == "first choice (10)!");
                #endregion



                /*
                 * Μεταφράζουμε το invariant:
                 */
                _option1_inv.OptionText = "Αυτή έίναι ή invariant μετάφραση!!";
                libraryManager.UpdateLibraryQuestionOption(_option1_inv);

                #region ελέγχουμε
                {
                    _option1_inv = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_option1_inv.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_option1_inv.OptionText == "Αυτή έίναι ή invariant μετάφραση!!");
                    _option1_bul = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_option1_bul.TextsLanguage == BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_option1_bul.OptionText == "first choice (10)!");
                    _option1_eng = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_option1_eng.TextsLanguage == BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_option1_eng.OptionText == "first choice (10)!");
                    _option1_fra = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_option1_fra.TextsLanguage == BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_option1_fra.OptionText == "first choice (10)!");
                    _option1_deu = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_option1_deu.TextsLanguage == BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_option1_deu.OptionText == "first choice (10)!");
                    _option1_ell = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_option1_ell.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_option1_ell.OptionText == "first choice (10)!");
                    _option1_rus = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_option1_rus.TextsLanguage == BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_option1_rus.OptionText == "first choice (10)!");
                }
                #endregion



                /*
                 * Μεταφράζουμε το γαλλικό:
                 */
                _option1_fra.OptionText = "Αυτή έίναι ή Γαλλική μετάφραση, @#$324!!";
                libraryManager.UpdateLibraryQuestionOption(_option1_fra);

                #region ελέγχουμε
                {
                    _option1_inv = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_option1_inv.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_option1_inv.OptionText == "Αυτή έίναι ή invariant μετάφραση!!");
                    _option1_bul = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_option1_bul.TextsLanguage == BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_option1_bul.OptionText == "first choice (10)!");
                    _option1_eng = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_option1_eng.TextsLanguage == BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_option1_eng.OptionText == "first choice (10)!");
                    _option1_fra = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_option1_fra.TextsLanguage == BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_option1_fra.OptionText == "Αυτή έίναι ή Γαλλική μετάφραση, @#$324!!");
                    _option1_deu = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_option1_deu.TextsLanguage == BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_option1_deu.OptionText == "first choice (10)!");
                    _option1_ell = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_option1_ell.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_option1_ell.OptionText == "first choice (10)!");
                    _option1_rus = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_option1_rus.TextsLanguage == BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_option1_rus.OptionText == "first choice (10)!");
                }
                #endregion



                /*
                 * Μεταφράζουμε το ελληνικό:
                 */
                _option1_ell.OptionText = "Πρώτη επιλογή (10)!";
                libraryManager.UpdateLibraryQuestionOption(_option1_ell);

                #region ελέγχουμε
                {
                    _option1_inv = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_option1_inv.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                    Assert.IsTrue(_option1_inv.OptionText == "Αυτή έίναι ή invariant μετάφραση!!");
                    _option1_bul = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_option1_bul.TextsLanguage == BuiltinLanguages.Bulgarian.LanguageId);
                    Assert.IsTrue(_option1_bul.OptionText == "first choice (10)!");
                    _option1_eng = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_option1_eng.TextsLanguage == BuiltinLanguages.English.LanguageId);
                    Assert.IsTrue(_option1_eng.OptionText == "first choice (10)!");
                    _option1_fra = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_option1_fra.TextsLanguage == BuiltinLanguages.French.LanguageId);
                    Assert.IsTrue(_option1_fra.OptionText == "Αυτή έίναι ή Γαλλική μετάφραση, @#$324!!");
                    _option1_deu = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_option1_deu.TextsLanguage == BuiltinLanguages.German.LanguageId);
                    Assert.IsTrue(_option1_deu.OptionText == "first choice (10)!");
                    _option1_ell = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_option1_ell.TextsLanguage == BuiltinLanguages.Greek.LanguageId);
                    Assert.IsTrue(_option1_ell.OptionText == "Πρώτη επιλογή (10)!");
                    _option1_rus = libraryManager.GetLibraryQuestionOptionById(option1.Question, option1.OptionId, BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_option1_rus.TextsLanguage == BuiltinLanguages.Russian.LanguageId);
                    Assert.IsTrue(_option1_rus.OptionText == "first choice (10)!");
                }
                #endregion
            }
            finally
            {
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
            }
        }

        [TestMethod, Description("CRUD tests for LibraryColumns")]
        public void QuestionLibraryTests01_05()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var libraryManager = VLLibraryManager.GetAnInstance(admin);


            try
            {
                //Στο σύστημα υπάρχει μία κατηγορία ερωτήσεων:
                var categories = libraryManager.GetLibraryQuestionCategories();
                Assert.IsTrue(categories.Count == 1);
                var category1 = categories[0];
                //για αυτή την κατηγορία, δεν έχουμε καθόλου ερωτήσεις:
                Assert.IsTrue(libraryManager.GetLibraryQuestions(category1.CategoryId).Count == 0);


                //We create a new question:
                var question1 = libraryManager.CreateLibraryQuestion(category1.CategoryId, QuestionType.MatrixOnePerRow, "Please fill out the following chart:");
                Assert.IsTrue(question1.TextsLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.IsTrue(question1.QuestionType == QuestionType.MatrixOnePerRow);
                Assert.AreEqual<string>("Please fill out the following chart:", question1.QuestionText);
                Assert.IsTrue(question1.OptionsSequence == 0);
                Assert.IsTrue(question1.ColumnsSequence == 0);


                //Δεν υπάρχει κανένα option ακόμα:
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 0);

                var option1 = libraryManager.CreateLibraryQuestionOption(question1, "The time is sufficient for performing my tasks");
                var option2 = libraryManager.CreateLibraryQuestionOption(question1, "We help each other in my working environment");
                var option3 = libraryManager.CreateLibraryQuestionOption(question1, "My job allows me to use my knowledge and skills");
                var option4 = libraryManager.CreateLibraryQuestionOption(question1, "The working atmosphere in my working environment is good");
                var option5 = libraryManager.CreateLibraryQuestionOption(question1, "I am satisfied with the external conditions at my workstation");

                //τώρα έχουμε 5 options:
                Assert.IsTrue(libraryManager.GetLibraryQuestionOptions(question1).Count == 5);


                //Δεν υπάρχει καμμία column ακόμα:
                Assert.IsTrue(libraryManager.GetLibraryQuestionColumns(question1).Count == 0);

                //Δημιουργούμε την πρώτη κολώνα μας:
                var column1 = libraryManager.CreateLibraryQuestionColumn(question1, "absolutely not true");
                Assert.IsNotNull(column1);
                Assert.IsTrue(column1.Question == question1.QuestionId);
                Assert.IsTrue(column1.ColumnId == 1);
                Assert.IsTrue(column1.ColumnText == "absolutely not true");
                var svdColumn1 = libraryManager.GetLibraryQuestionColumnById(column1.Question, column1.ColumnId, column1.TextsLanguage);
                Assert.AreEqual<VLLibraryColumn>(column1, svdColumn1);
                //Εχουμε μία κολώνα:
                Assert.IsTrue(libraryManager.GetLibraryQuestionColumns(question1).Count == 1);
                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 5);
                Assert.IsTrue(question1.ColumnsSequence == 1);

                //not true
                var column2 = libraryManager.CreateLibraryQuestionColumn(question1, "not true");
                Assert.IsNotNull(column2);
                Assert.IsTrue(column2.Question == question1.QuestionId);
                Assert.IsTrue(column2.ColumnId == 2);
                Assert.IsTrue(column2.ColumnText == "not true");
                var svdColumn2 = libraryManager.GetLibraryQuestionColumnById(column2.Question, column2.ColumnId, column2.TextsLanguage);
                Assert.AreEqual<VLLibraryColumn>(column2, svdColumn2);
                //Εχουμε δύο κολώνες:
                Assert.IsTrue(libraryManager.GetLibraryQuestionColumns(question1).Count == 2);
                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 5);
                Assert.IsTrue(question1.ColumnsSequence == 2);

                //partly true
                var column3 = libraryManager.CreateLibraryQuestionColumn(question1, "partly true");
                Assert.IsNotNull(column3);
                Assert.IsTrue(column3.Question == question1.QuestionId);
                Assert.IsTrue(column3.ColumnId == 3);
                Assert.IsTrue(column3.ColumnText == "partly true");
                var svdColumn3 = libraryManager.GetLibraryQuestionColumnById(column3.Question, column3.ColumnId, column3.TextsLanguage);
                Assert.AreEqual<VLLibraryColumn>(column3, svdColumn3);
                //Εχουμε τρείς κολώνες:
                Assert.IsTrue(libraryManager.GetLibraryQuestionColumns(question1).Count == 3);
                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 5);
                Assert.IsTrue(question1.ColumnsSequence == 3);

                //true
                var column4 = libraryManager.CreateLibraryQuestionColumn(question1, "true");
                var svdColumn4 = libraryManager.GetLibraryQuestionColumnById(column4.Question, column4.ColumnId, column4.TextsLanguage);
                Assert.AreEqual<VLLibraryColumn>(column4, svdColumn4);
                //absolutely true
                var column5 = libraryManager.CreateLibraryQuestionColumn(question1, "absolutely true");
                var svdColumn5 = libraryManager.GetLibraryQuestionColumnById(column5.Question, column5.ColumnId, column5.TextsLanguage);
                Assert.AreEqual<VLLibraryColumn>(column5, svdColumn5);

                //Εχουμε πέντε (5) κολώνες:
                Assert.IsTrue(libraryManager.GetLibraryQuestionColumns(question1).Count == 5);
                question1 = libraryManager.GetLibraryQuestionById(question1.QuestionId, question1.TextsLanguage);
                Assert.IsTrue(question1.OptionsSequence == 5);
                Assert.IsTrue(question1.ColumnsSequence == 5);



            }
            finally
            {
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
            }
        }


    }
}
