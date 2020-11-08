using System;

namespace Valis.Core.UnitTests
{
    public class SurveyFacilityBaseClass : AdminBaseClass
    {
        protected VLSurvey CreateSurvey1(VLSurveyManager surveyManager, VLClient client, string surveyTitle = "Employee survey")
        {
            //Δημιουργούμε ένα Survey (και αυτόματα δημιουργείται ένα default view για αυτή):
            var survey = surveyManager.CreateSurvey(client, surveyTitle, textsLanguage: BuiltinLanguages.English);
            survey.HeaderHtml = "Employee survey";
            survey.WelcomeHtml = "Dear staff members,<br><br>please fill out the questionnaire as soon as possible and try to answer all questions including personal data. By all means your data will be processed and evaluated anonymously.<br><br>When the survey is completed you will get informed about the overall result in detail. Your superior will inform you about the results of the department.<br><br>Your participation is very important! Only a high participation rate leads to significant results.<br><br>Thank you for your cooperation and commitment!<br>";
            survey.GoodbyeHtml = "we thank you!!!";
            survey.StartButton = "Start Survey";
            survey.PreviousButton = "Back";
            survey.NextButton = "Next";
            survey.DoneButton = "Submit";
            survey = surveyManager.UpdateSurvey(survey);

            var page1 = surveyManager.GetFirstSurveyPage(survey);
            page1.ShowTitle = "Task and working environment";
            page1 = surveyManager.UpdateSurveyPage(page1);
            var page2 = surveyManager.CreateSurveyPage(survey, "Further education and training");
            var page3 = surveyManager.CreateSurveyPage(survey, "Cooperation with the superior");
            var page4 = surveyManager.CreateSurveyPage(survey, string.Empty);
            var page5 = surveyManager.CreateSurveyPage(survey, "General attitude");
            var page6 = surveyManager.CreateSurveyPage(survey, "The company");
            var page7 = surveyManager.CreateSurveyPage(survey, "Statistical data");
            var page8 = surveyManager.CreateSurveyPage(survey, "General statements:");


            survey = surveyManager.AddSurveyLanguage(survey.SurveyId, BuiltinLanguages.English, BuiltinLanguages.Greek);
            survey.HeaderHtml = "Γκαλοπ Εργαζομένων";
            survey.WelcomeHtml = "Αγαπητοί υπάλληλοι....";
            survey.GoodbyeHtml = "σας ευχαριστούμε";
            survey.StartButton = "Ξεκινήστε";
            survey.PreviousButton = "Προηγούμενο";
            survey.NextButton = "Επόμενο";
            survey.DoneButton = "Τερμα!";
            survey = surveyManager.UpdateSurvey(survey);

            page1 = surveyManager.GetSurveyPageById(survey.SurveyId, page1.PageId, BuiltinLanguages.Greek.LanguageId);
            page1.ShowTitle = "Καθηκοντα και εργασιακό περιβάλλον";
            page1 = surveyManager.UpdateSurveyPage(page1);
            page2 = surveyManager.GetSurveyPageById(survey.SurveyId, page2.PageId, BuiltinLanguages.Greek.LanguageId);
            page2.ShowTitle = "Επιπλέον μόρφωση και εκπαίδευση";
            page2 = surveyManager.UpdateSurveyPage(page2);
            page3 = surveyManager.GetSurveyPageById(survey.SurveyId, page3.PageId, BuiltinLanguages.Greek.LanguageId);
            page3.ShowTitle = "Συνεργασία με τον ανώτερο";
            page3 = surveyManager.UpdateSurveyPage(page3);
            page4 = surveyManager.GetSurveyPageById(survey.SurveyId, page4.PageId, BuiltinLanguages.Greek.LanguageId);
            page4.ShowTitle = "";
            page4 = surveyManager.UpdateSurveyPage(page4);
            page5 = surveyManager.GetSurveyPageById(survey.SurveyId, page5.PageId, BuiltinLanguages.Greek.LanguageId);
            page5.ShowTitle = "Γενικό πνεύμα";
            page5 = surveyManager.UpdateSurveyPage(page5);
            page6 = surveyManager.GetSurveyPageById(survey.SurveyId, page6.PageId, BuiltinLanguages.Greek.LanguageId);
            page6.ShowTitle = "Η εταιρεία";
            page6 = surveyManager.UpdateSurveyPage(page6);
            page7 = surveyManager.GetSurveyPageById(survey.SurveyId, page7.PageId, BuiltinLanguages.Greek.LanguageId);
            page7.ShowTitle = "Στατιστικά δεδομένα";
            page7 = surveyManager.UpdateSurveyPage(page7);
            page8 = surveyManager.GetSurveyPageById(survey.SurveyId, page8.PageId, BuiltinLanguages.Greek.LanguageId);
            page8.ShowTitle = "Γενικές δηλώσεις";
            page8 = surveyManager.UpdateSurveyPage(page8);




            //1. QUESTION: Please fill out the following chart  
            var question1 = surveyManager.CreateQuestion(survey.SurveyId, page1.PageId, QuestionType.MatrixOnePerRow, "Please fill out the following chart", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question1.QuestionId, "absolutely not true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question1.QuestionId, "not true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question1.QuestionId, "partly true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question1.QuestionId, "true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question1.QuestionId, "absolutely true", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question1.QuestionId, "The time is sufficient for performing my tasks");
            surveyManager.CreateQuestionOption(survey.SurveyId, question1.QuestionId, "We help each other in my working environment");
            surveyManager.CreateQuestionOption(survey.SurveyId, question1.QuestionId, "My job allows me to use my knowledge and skills");
            surveyManager.CreateQuestionOption(survey.SurveyId, question1.QuestionId, "The working atmosphere in my working environment is good");
            surveyManager.CreateQuestionOption(survey.SurveyId, question1.QuestionId, "I am satisfied with the external conditions at my workstation");

            //2. QUESTION: What needs to be improved at your workstation?  (Several answers are possible)
            var question2 = surveyManager.CreateQuestion(survey.SurveyId, page1.PageId, QuestionType.ManyFromMany, "What needs to be improved at your workstation", BuiltinLanguages.English);
            surveyManager.CreateQuestionOption(survey.SurveyId, question2.QuestionId, "nothing");
            surveyManager.CreateQuestionOption(survey.SurveyId, question2.QuestionId, "ventilation");
            surveyManager.CreateQuestionOption(survey.SurveyId, question2.QuestionId, "more daylight");
            surveyManager.CreateQuestionOption(survey.SurveyId, question2.QuestionId, "better illumination");
            surveyManager.CreateQuestionOption(survey.SurveyId, question2.QuestionId, "cafeteria");
            surveyManager.CreateQuestionOption(survey.SurveyId, question2.QuestionId, "equipment workstation");

            //3. QUESTION: Please fill out the following chart  
            var question3 = surveyManager.CreateQuestion(survey.SurveyId, page2.PageId, QuestionType.MatrixOnePerRow, "Please fill out the following chart?", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question3.QuestionId, "absolutely not true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question3.QuestionId, "not true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question3.QuestionId, "partly true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question3.QuestionId, "true", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question3.QuestionId, "absolutely true", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question3.QuestionId, "The company offers me enough possibilties for further development");
            surveyManager.CreateQuestionOption(survey.SurveyId, question3.QuestionId, "I am satisfied with the offered options for further education and training");

            //4. QUESTION: Please fill out the following chart  
            var question4 = surveyManager.CreateQuestion(survey.SurveyId, page3.PageId, QuestionType.MatrixOnePerRow, "Please fill out the following chart?", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question4.QuestionId, "i totally disagree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question4.QuestionId, "I disagree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question4.QuestionId, "I partly agree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question4.QuestionId, "I agree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question4.QuestionId, "I absolutely agree", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question4.QuestionId, "My superior promotes my according to my skills");
            surveyManager.CreateQuestionOption(survey.SurveyId, question4.QuestionId, "My superior gives me frank and honest feedback");
            surveyManager.CreateQuestionOption(survey.SurveyId, question4.QuestionId, "My superior assesses my performance in a fair way");
            surveyManager.CreateQuestionOption(survey.SurveyId, question4.QuestionId, "My superior encourages me sufficiantly");

            //5. QUESTION: Please fill out the following chart  
            var question5 = surveyManager.CreateQuestion(survey.SurveyId, page4.PageId, QuestionType.MatrixOnePerRow, "Please fill out the following chart?", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question5.QuestionId, "i totally disagree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question5.QuestionId, "I disagree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question5.QuestionId, "I partly agree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question5.QuestionId, "I agree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question5.QuestionId, "I absolutely agree", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question5.QuestionId, "I have enough information do do my job well");
            surveyManager.CreateQuestionOption(survey.SurveyId, question5.QuestionId, "We exchange information and experiences sufficiantly in my working environment");
            surveyManager.CreateQuestionOption(survey.SurveyId, question5.QuestionId, "The management informs the staff about current developments and decisions adequately");
            surveyManager.CreateQuestionOption(survey.SurveyId, question5.QuestionId, "The management informs timely about current developments and decisions");

            //6. QUESTION:   All in all how satisfied with the company are you at present? 
            var question6 = surveyManager.CreateQuestion(survey.SurveyId, page5.PageId, QuestionType.OneFromMany, "All in all how satisfied with the company are you at present?", BuiltinLanguages.English);
            surveyManager.CreateQuestionOption(survey.SurveyId, question6.QuestionId, "very dissatisfied");
            surveyManager.CreateQuestionOption(survey.SurveyId, question6.QuestionId, "dissatisfied");
            surveyManager.CreateQuestionOption(survey.SurveyId, question6.QuestionId, "partly satisfied");
            surveyManager.CreateQuestionOption(survey.SurveyId, question6.QuestionId, "satisfied");
            surveyManager.CreateQuestionOption(survey.SurveyId, question6.QuestionId, "very satisfied");

            //7. QUESTION: Please fill out the following chart  
            var question7 = surveyManager.CreateQuestion(survey.SurveyId, page6.PageId, QuestionType.MatrixOnePerRow, "Please fill out the following chart?", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question7.QuestionId, "i totally disagree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question7.QuestionId, "I disagree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question7.QuestionId, "I partly agree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question7.QuestionId, "I agree", BuiltinLanguages.English);
            surveyManager.CreateQuestionColumn(survey.SurveyId, question7.QuestionId, "I absolutely agree", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question7.QuestionId, "The cooperation with the colleagues in the other departments is good");
            surveyManager.CreateQuestionOption(survey.SurveyId, question7.QuestionId, "Our company will be successful also in the future");
            surveyManager.CreateQuestionOption(survey.SurveyId, question7.QuestionId, "I feel solidarity with the company also beyond my job related commitment");
            surveyManager.CreateQuestionOption(survey.SurveyId, question7.QuestionId, "I can recommend my employer");

            //8. QUESTION: Which department do you work in? 
            var question8 = surveyManager.CreateQuestion(survey.SurveyId, page7.PageId, QuestionType.OneFromMany, "Which department do you work in?", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question8.QuestionId, "A");
            surveyManager.CreateQuestionOption(survey.SurveyId, question8.QuestionId, "B");
            surveyManager.CreateQuestionOption(survey.SurveyId, question8.QuestionId, "C");
            surveyManager.CreateQuestionOption(survey.SurveyId, question8.QuestionId, "D");

            //9. QUESTION: How long have you been working in the company? 
            var question9 = surveyManager.CreateQuestion(survey.SurveyId, page7.PageId, QuestionType.OneFromMany, "How long have you been working in the company?", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question9.QuestionId, "< 1 year");
            surveyManager.CreateQuestionOption(survey.SurveyId, question9.QuestionId, "1-3 years");
            surveyManager.CreateQuestionOption(survey.SurveyId, question9.QuestionId, "3-10 years");
            surveyManager.CreateQuestionOption(survey.SurveyId, question9.QuestionId, "> 11 years");


            //10. QUESTION: Do you hold staff responsibility?
            var question10 = surveyManager.CreateQuestion(survey.SurveyId, page7.PageId, QuestionType.OneFromMany, "Do you hold staff responsibility?", BuiltinLanguages.English);

            surveyManager.CreateQuestionOption(survey.SurveyId, question10.QuestionId, "Yes");
            surveyManager.CreateQuestionOption(survey.SurveyId, question10.QuestionId, "No");

            //11. QUESTION: Statistical data
            var question11 = surveyManager.CreateQuestion(survey.SurveyId, page7.PageId, QuestionType.Composite, "Statistical data", BuiltinLanguages.English);
            
            var question12 = surveyManager.CreateQuestion(survey.SurveyId, page7.PageId, QuestionType.Integer, "Age", BuiltinLanguages.English);
            question12.MasterQuestion = question11.QuestionId;
            question12 = surveyManager.UpdateQuestion(question12);

            var question13 = surveyManager.CreateQuestion(survey.SurveyId, page7.PageId, QuestionType.DropDown, "Gender", BuiltinLanguages.English);
            question13.MasterQuestion = question11.QuestionId;
            question13 = surveyManager.UpdateQuestion(question13);

            //12. QUESTION: General statements:  
            var question14 = surveyManager.CreateQuestion(survey.SurveyId, page8.PageId, QuestionType.MultipleLine, "General statements:", BuiltinLanguages.English);


            return survey;
        }
        protected VLSurvey CreateSurvey2(VLSurveyManager surveyManager, VLClient client, string surveyTitle = "Questionnaire #1", string surveyShowTitle = "Risk assessment")
        {

            //Δημιουργούμε ένα Survey (και αυτόματα δημιουργείται ένα default view για αυτή):
            var survey = surveyManager.CreateSurvey(client, surveyTitle, surveyShowTitle);

            //Αυτή είναι η σελίδα που δημιουργήθηκε αυτόματα:
            var page1 = surveyManager.GetFirstSurveyPage(survey);
            var page2 = surveyManager.CreateSurveyPage(survey, null, "This is the second page");
            var page3 = surveyManager.CreateSurveyPage(survey, null, "This is the third page");


            //Δημιουργούμε ερωτήσεις στην page1:
            var question1 = surveyManager.CreateQuestion(page1, QuestionType.SingleLine, "What is your name?");
            var question2 = surveyManager.CreateQuestion(page1, QuestionType.Integer, "What is your age?");
            var question3 = surveyManager.CreateQuestion(page1, QuestionType.Decimal, "What is your weight?");
            var question4 = surveyManager.CreateQuestion(page1, QuestionType.Date, "What is your birthday?");

            //Δημιουργούμε ερωτήσεις στην page2:
            var question5 = surveyManager.CreateQuestion(page2, QuestionType.OneFromMany, "From where are you?");
            {
                var option5_01 = surveyManager.CreateQuestionOption(question5, "Greece");
                var option5_02 = surveyManager.CreateQuestionOption(question5, "UK");
                var option5_03 = surveyManager.CreateQuestionOption(question5, "USA");
                var option5_04 = surveyManager.CreateQuestionOption(question5, "Bulgaria");
            }
            var question6 = surveyManager.CreateQuestion(page2, QuestionType.OneFromMany, "Who is your friend?");
            {
                var option6_01 = surveyManager.CreateQuestionOption(question6, "John");
                var option6_02 = surveyManager.CreateQuestionOption(question6, "Bruto");
                var option6_03 = surveyManager.CreateQuestionOption(question6, "Makis");
                var option6_04 = surveyManager.CreateQuestionOption(question6, "Anna");
                var option6_05 = surveyManager.CreateQuestionOption(question6, "George");
                var option6_06 = surveyManager.CreateQuestionOption(question6, "Tasos");
            }
            var question7 = surveyManager.CreateQuestion(page2, QuestionType.ManyFromMany, "What Cars do you like?");
            {
                var option7_01 = surveyManager.CreateQuestionOption(question7, "Subaru");
                var option7_02 = surveyManager.CreateQuestionOption(question7, "Jaguar");
                var option7_03 = surveyManager.CreateQuestionOption(question7, "Chevrolet");
                var option7_04 = surveyManager.CreateQuestionOption(question7, "Lamborghini");
                var option7_05 = surveyManager.CreateQuestionOption(question7, "Ford");
                var option7_06 = surveyManager.CreateQuestionOption(question7, "Nissan");
                var option7_07 = surveyManager.CreateQuestionOption(question7, "Mazda");
                var option7_08 = surveyManager.CreateQuestionOption(question7, "Citroen");
            }

            //Δημιουργούμε ερωτήσεις στην page3:
            var question8 = surveyManager.CreateQuestion(page3, QuestionType.MatrixOnePerRow, "Βαθμολογήστε την απόδοσή σας σε κάθε μάθημα:");
            {
                var option8_01 = surveyManager.CreateQuestionOption(question8, "Mathematica");
                var option8_02 = surveyManager.CreateQuestionOption(question8, "Chemistry");
                var option8_03 = surveyManager.CreateQuestionOption(question8, "Biology");
                var option8_04 = surveyManager.CreateQuestionOption(question8, "Physics");
                var column8_01 = surveyManager.CreateQuestionColumn(question8, "1");
                var column8_02 = surveyManager.CreateQuestionColumn(question8, "2");
                var column8_03 = surveyManager.CreateQuestionColumn(question8, "3");
                var column8_04 = surveyManager.CreateQuestionColumn(question8, "4");
                var column8_05 = surveyManager.CreateQuestionColumn(question8, "5");
                var column8_06 = surveyManager.CreateQuestionColumn(question8, "6");
                var column8_07 = surveyManager.CreateQuestionColumn(question8, "7");
                var column8_08 = surveyManager.CreateQuestionColumn(question8, "8");
                var column8_09 = surveyManager.CreateQuestionColumn(question8, "9");
                var column8_10 = surveyManager.CreateQuestionColumn(question8, "10");
            }

            var question9 = surveyManager.CreateQuestion(page3, QuestionType.OneFromMany, "Are you satisfied?");
            {
                var option9_01 = surveyManager.CreateQuestionOption(question9, "Yes");
                var option9_02 = surveyManager.CreateQuestionOption(question9, "No");
            }

            var question10 = surveyManager.CreateQuestion(page3, QuestionType.MatrixManyPerRow, "Kino");
            {
                var option10_01 = surveyManager.CreateQuestionOption(question10, "Row1");
                var option10_02 = surveyManager.CreateQuestionOption(question10, "Row2");
                var option10_03 = surveyManager.CreateQuestionOption(question10, "Row3");
                var option10_04 = surveyManager.CreateQuestionOption(question10, "Row4");
                var option10_05 = surveyManager.CreateQuestionOption(question10, "Row5");
                var option10_06 = surveyManager.CreateQuestionOption(question10, "Row6");
                var column10_01 = surveyManager.CreateQuestionColumn(question10, "col1");
                var column10_02 = surveyManager.CreateQuestionColumn(question10, "col2");
                var column10_03 = surveyManager.CreateQuestionColumn(question10, "col3");
                var column10_04 = surveyManager.CreateQuestionColumn(question10, "col4");
                var column10_05 = surveyManager.CreateQuestionColumn(question10, "col5");
            }

            return survey;
        }
        protected void CreateResponseDetailsForSurvey2(VLSurveyManager surveyManager, VLResponse response, Random rnd)
        {
            var names = new string[] { "George", "John", "Adam", "Nick", "Mike", "Richard" };
            var ages = new string[] { "45", "34", "67", "22", "34", "39", "56", "55", "23", "27" };
            var weights = new string[] { "75.99", "57.06", "61.92", "18.24", "04.52", "13.66", "86.93", "29.38", "57.96", "84.88", "79.97", "62.20" };
            var dates = new string[] { "06/28/1971", "11/25/1971", "07/11/1972", "08/09/1973", "06/24/1974", "07/05/1978", "09/05/1983", "03/05/1985", "10/04/1988", "09/07/1990", "12/03/1993", "01/31/1994" };
            var cars = new Object[] { new Int32[] { 1, 3, 4 }, new Int32[] { 1 }, new Int32[] { 6 }, new Int32[] { 5, 1, 8 }, new Int32[] { 2, 7 }, new Int32[] { 6, 8 } };


            //page1
            surveyManager.CreateResponseDetail(response.ResponseId, 1, null, null, names[rnd.Next(0, names.Length)]);           //"What is your name?
            surveyManager.CreateResponseDetail(response.ResponseId, 2, null, null, ages[rnd.Next(0, ages.Length)]);                //What is your age?
            surveyManager.CreateResponseDetail(response.ResponseId, 3, null, null, weights[rnd.Next(0, weights.Length)]);
            surveyManager.CreateResponseDetail(response.ResponseId, 4, null, null, dates[rnd.Next(0, dates.Length)]);
            //page2
            surveyManager.CreateResponseDetail(response.ResponseId, 5, (byte)rnd.Next(1, 5));       //From where are you?
            surveyManager.CreateResponseDetail(response.ResponseId, 6, (byte)rnd.Next(1, 7));       //Who is your friend?

            var ci = rnd.Next(0, 6);                                                               //What Cars do you like?
            Int32[] _cars = cars[ci] as Int32[];
            for (int i = 0; i < _cars.Length; i++)
            {
                surveyManager.CreateResponseDetail(response.ResponseId, 7, (byte)_cars[i]);
            }

            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 8, 1, (byte)rnd.Next(1, 11));//Βαθμολογήστε την απόδοσή σας σε κάθε μάθημα:
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 8, 2, (byte)rnd.Next(1, 11));//Βαθμολογήστε την απόδοσή σας σε κάθε μάθημα:
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 8, 3, (byte)rnd.Next(1, 11));//Βαθμολογήστε την απόδοσή σας σε κάθε μάθημα:
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 8, 4, (byte)rnd.Next(1, 11));//Βαθμολογήστε την απόδοσή σας σε κάθε μάθημα:

            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 9, (byte)rnd.Next(1, 3));                               //Are you satisfied?


            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 1, 1);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 2, 1);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 3, 1);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 4, 1);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 5, 1);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 6, 1);

            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 1, 2);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 2, 2);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 3, 2);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 4, 2);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 5, 2);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 6, 2);

            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 1, 3);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 2, 3);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 3, 3);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 4, 3);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 5, 3);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 6, 3);

            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 1, 4);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 2, 4);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 3, 4);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 4, 4);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 5, 4);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 6, 4);

            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 1, 5);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 2, 5);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 3, 5);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 4, 5);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 5, 5);
            if (rnd.Next(0, 2) == 1) surveyManager.CreateResponseDetail(response.ResponseId, 10, 6, 5);
        }
        protected void CreateResponsesForSurvey2(VLSurveyManager surveyManager, VLSurvey survey, Int32 responses1 = 200, Int32 responses2=120, Int32 responses3=500)
        {
            var rnd = new System.Random(965522113);
            var collector01 = surveyManager.CreateCollector(survey, CollectorType.WebLink, "collector01");
            var collector02 = surveyManager.CreateCollector(survey, CollectorType.WebLink, "collector02");
            var collector03 = surveyManager.CreateCollector(survey, CollectorType.WebLink, "collector03");

            for (int i = 1; i <= responses1; i++)
            {
                var openDate = new DateTime(2013, 8, 4, 12, 0, 0).AddHours(rnd.Next(0, 64));
                var response = surveyManager.CreateResponse(survey.SurveyId, collector01.CollectorId, null, openDate, openDate.AddMinutes(rnd.Next(4, 1441)));

                CreateResponseDetailsForSurvey2(surveyManager, response, rnd);
            }
            for (int i = 1; i <= responses2; i++)
            {
                var openDate = new DateTime(2013, 8, 8, 12, 0, 0).AddHours(rnd.Next(0, 64));
                var response = surveyManager.CreateResponse(survey.SurveyId, collector02.CollectorId, null, openDate, openDate.AddMinutes(rnd.Next(4, 1441)));

                CreateResponseDetailsForSurvey2(surveyManager, response, rnd);
            }
            for (int i = 1; i <= responses3; i++)
            {
                var openDate = new DateTime(2013, 8, 12, 12, 0, 0).AddHours(rnd.Next(0, 120));
                var response = surveyManager.CreateResponse(survey.SurveyId, collector03.CollectorId, null, openDate, openDate.AddMinutes(rnd.Next(4, 1441)));

                CreateResponseDetailsForSurvey2(surveyManager, response, rnd);
            }

        }


    }
}
