using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class SurveyResponsesTest01 : SurveyFacilityBaseClass
    {


        [TestMethod, Description("testing VLRuntimeSession")]
        public void SurveyResponsesTest01_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            try
            {
                Assert.IsTrue(surveyManager.GetSessions().Count == 0);

                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);

                //We create a survey:
                var survey1 = surveyManager.CreateSurvey(client.ClientId, "ερωτηματολόγιο - demo1", "ερωτηματολόγιο - demo1");
                Assert.IsNotNull(survey1);


                //Δημιουργούμε ένα session:
                var session1 = surveyManager.CreateSession(survey1.SurveyId, RuntimeRequestType.Preview);
                Assert.IsTrue(surveyManager.GetSessions().Count == 1);

                session1["key1"] = 34;
                session1["key2"] = "George";
                session1["key3"] = null;
                session1["key4"] = 34.5;

                Assert.AreEqual(session1["key1"], 34);
                Assert.AreEqual(session1["key2"], "George");
                Assert.AreEqual(session1["key3"], null);
                Assert.AreEqual(session1["key4"], 34.5);

                //το αποθηκεύουμε:
                surveyManager.ReleaseSession(session1);
                //Read:
                var svdSession1 = surveyManager.AcquireSession(session1.SessionId);
                Assert.IsNotNull(svdSession1);
                Assert.IsTrue(svdSession1.Survey == survey1.SurveyId);
                foreach (string key in session1.Keys)
                {
                    Assert.AreEqual(session1[key], svdSession1[key]);
                }


                //Δημιουργούμε ένα session:
                var session2 = surveyManager.CreateSession(survey1.SurveyId, RuntimeRequestType.Preview);
                Assert.IsTrue(surveyManager.GetSessions().Count == 2);

                for (int i = 1; i <= 100; i++)
                {
                    session2["key" + i.ToString()] = i;
                }
                //το αποθηκεύουμε:
                surveyManager.ReleaseSession(session2);
                //Read:
                var svdSession2 = surveyManager.AcquireSession(session2.SessionId);
                Assert.IsNotNull(svdSession2);
                foreach (string key in session2.Keys)
                {
                    Assert.AreEqual(session2[key], svdSession2[key]);
                }



                Assert.IsTrue(surveyManager.GetSessions().Count == 2);
                surveyManager.DeleteSession(session1.SessionId);
                Assert.IsTrue(surveyManager.GetSessions().Count == 1);
                surveyManager.DeleteSession(session2.SessionId);
                Assert.IsTrue(surveyManager.GetSessions().Count == 0);

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
                surveyManager.DeleteAllSessions();

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }
        }


        [TestMethod, Description("testing VLResponse & VLResponseDetail")]
        public void SurveyResponsesTest01_02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);



                var survey = CreateSurvey1(surveyManager, client);

            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    var responses = surveyManager.GetResponses(item.SurveyId);
                    foreach (var a in responses)
                    {
                        surveyManager.DeleteResponse(a);
                    }

                    if (item.IsBuiltIn)
                        continue;

                    surveyManager.DeleteSurvey(item);
                }
                surveyManager.DeleteAllSessions();

                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }

            }
        }

        [TestMethod, Description("testing VLResponse & VLResponseDetail")]
        public void SurveyResponsesTest01_02b()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client);

                //τραβάμε απο την βάση μας ένα γνωστό survey
                var survey1 = CreateSurvey1(surveyManager, client);
                Assert.IsNotNull(survey1);

                Assert.IsTrue(surveyManager.GetResponses(survey1.SurveyId).Count == 0);

                var _now = Utility.RoundToSeconds(Utility.UtcNow());
                //Δημιουργούμε μία απάντηση:
                var response = surveyManager.CreateResponse(survey1.SurveyId, null, null, _now);
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Survey == survey1.SurveyId);
                Assert.IsNull(response.Collector);
                Assert.IsNull(response.Recipient);
                Assert.IsTrue(response.OpenDate == _now);
                Assert.IsNull(response.CloseDate);
                Assert.IsTrue(response.AttributeFlags == 0);
                var svdResponse = surveyManager.GetResponseById(response.ResponseId);
                Assert.AreEqual<VLResponse>(response, svdResponse);


                var question1 = surveyManager.GetQuestionById(survey1.SurveyId, 1);
                Assert.IsNotNull(question1);
                //RuntimeSession[QstnID_1_OptID_1_] = 1 
                var detail_1_1 = surveyManager.CreateResponseDetail(response.ResponseId, question1.QuestionId, 1, 1);
                #region
                Assert.IsTrue(detail_1_1.Response == response.ResponseId);
                Assert.IsTrue(detail_1_1.Question == question1.QuestionId);
                Assert.IsTrue(detail_1_1.SelectedOption == 1);
                Assert.IsTrue(detail_1_1.SelectedColumn == 1);
                Assert.IsNull(detail_1_1.UserInput);
                var svdDetail = surveyManager.GetResponseDetail(response.ResponseId, question1.QuestionId, 1, 1);
                Assert.AreEqual<VLResponseDetail>(detail_1_1, svdDetail);
                #endregion
                //RuntimeSession[QstnID_1_OptID_2_] = 3 
                var detail_1_2 = surveyManager.CreateResponseDetail(response.ResponseId, question1.QuestionId, 2, 3);
                #region
                Assert.IsTrue(detail_1_2.Response == response.ResponseId);
                Assert.IsTrue(detail_1_2.Question == question1.QuestionId);
                Assert.IsTrue(detail_1_2.SelectedOption == 2);
                Assert.IsTrue(detail_1_2.SelectedColumn == 3);
                Assert.IsNull(detail_1_2.UserInput);
                #endregion
                //RuntimeSession[QstnID_1_OptID_3_] = 2 
                var detail_1_3 = surveyManager.CreateResponseDetail(response.ResponseId, question1.QuestionId, 3, 2);
                #region
                Assert.IsTrue(detail_1_3.Response == response.ResponseId);
                Assert.IsTrue(detail_1_3.Question == question1.QuestionId);
                Assert.IsTrue(detail_1_3.SelectedOption == 3);
                Assert.IsTrue(detail_1_3.SelectedColumn == 2);
                Assert.IsNull(detail_1_3.UserInput);
                #endregion
                //RuntimeSession[QstnID_1_OptID_4_] = 4 
                var detail_1_4 = surveyManager.CreateResponseDetail(response.ResponseId, question1.QuestionId, 4, 4);
                //RuntimeSession[QstnID_1_OptID_5_] = 5 
                var detail_1_5 = surveyManager.CreateResponseDetail(response.ResponseId, question1.QuestionId, 5, 5);


                var question2 = surveyManager.GetQuestionById(survey1.SurveyId, 2);
                Assert.IsNotNull(question2);
                //RuntimeSession[QstnID_2_OptID_2_] = 1 
                var detail_2_2 = surveyManager.CreateResponseDetail(response.ResponseId, question2.QuestionId, 2, 1);
                #region
                Assert.IsTrue(detail_2_2.Response == response.ResponseId);
                Assert.IsTrue(detail_2_2.Question == question2.QuestionId);
                Assert.IsTrue(detail_2_2.SelectedOption == 2);
                Assert.IsTrue(detail_2_2.SelectedColumn == 1);
                Assert.IsNull(detail_2_2.UserInput);
                #endregion
                //RuntimeSession[QstnID_2_OptID_3_] = 1 
                var detail_2_3 = surveyManager.CreateResponseDetail(response.ResponseId, question2.QuestionId, 3, 1);
                //RuntimeSession[QstnID_2_OptID_4_] = 1 
                var detail_2_4 = surveyManager.CreateResponseDetail(response.ResponseId, question2.QuestionId, 4, 1);
                //RuntimeSession[QstnID_2_OptID_7_] = 1 
                //RuntimeSession[QstnID_2_OptID_7_SingleLine_] = χαρτί τουαλέτας
                var detail_2_7 = surveyManager.CreateResponseDetail(response.ResponseId, question2.QuestionId, 7, 1, "χαρτί τουαλέτας");
                #region
                Assert.IsTrue(detail_2_7.Response == response.ResponseId);
                Assert.IsTrue(detail_2_7.Question == question2.QuestionId);
                Assert.IsTrue(detail_2_7.SelectedOption == 7);
                Assert.IsTrue(detail_2_7.SelectedColumn == 1);
                Assert.AreEqual<string>(detail_2_7.UserInput, "χαρτί τουαλέτας");
                #endregion



                var question3 = surveyManager.GetQuestionById(survey1.SurveyId, 3);
                Assert.IsNotNull(question3);
                //RuntimeSession[QstnID_3_OptID_1_] = 3
                var detail_3_1 = surveyManager.CreateResponseDetail(response.ResponseId, question3.QuestionId, 1, 3);
                //RuntimeSession[QstnID_3_OptID_2_] = 4 
                var detail_3_2 = surveyManager.CreateResponseDetail(response.ResponseId, question3.QuestionId, 2, 4);



                var question4 = surveyManager.GetQuestionById(survey1.SurveyId, 4);
                Assert.IsNotNull(question4);
                //RuntimeSession[QstnID_4_OptID_1_] = 5 
                var detail_4_1 = surveyManager.CreateResponseDetail(response.ResponseId, question4.QuestionId, 1, 5);
                //RuntimeSession[QstnID_4_OptID_2_] = 4 
                var detail_4_2 = surveyManager.CreateResponseDetail(response.ResponseId, question4.QuestionId, 2, 4);
                //RuntimeSession[QstnID_4_OptID_3_] = 4 
                var detail_4_3 = surveyManager.CreateResponseDetail(response.ResponseId, question4.QuestionId, 3, 4);
                //RuntimeSession[QstnID_4_OptID_4_] = 5 
                var detail_4_4 = surveyManager.CreateResponseDetail(response.ResponseId, question4.QuestionId, 4, 5);


                var question5 = surveyManager.GetQuestionById(survey1.SurveyId, 5);
                Assert.IsNotNull(question5);
                //RuntimeSession[QstnID_5_OptID_1_] = 4 
                var detail_5_1 = surveyManager.CreateResponseDetail(response.ResponseId, question5.QuestionId, 1, 4);
                //RuntimeSession[QstnID_5_OptID_2_] = 3 
                var detail_5_2 = surveyManager.CreateResponseDetail(response.ResponseId, question5.QuestionId, 2, 3);
                //RuntimeSession[QstnID_5_OptID_3_] = 3 
                var detail_5_3 = surveyManager.CreateResponseDetail(response.ResponseId, question5.QuestionId, 3, 2);


                var question6 = surveyManager.GetQuestionById(survey1.SurveyId, 6);
                Assert.IsNotNull(question6);
                //RuntimeSession[QstnID_6_] = 3 
                var detail_6_1 = surveyManager.CreateResponseDetail(response.ResponseId, question6.QuestionId, 3);
                #region
                Assert.IsTrue(detail_6_1.Response == response.ResponseId);
                Assert.IsTrue(detail_6_1.Question == question6.QuestionId);
                Assert.IsTrue(detail_6_1.SelectedOption == 3);
                Assert.IsNull(detail_6_1.SelectedColumn);
                Assert.IsNull(detail_6_1.UserInput);
                #endregion


                var question7 = surveyManager.GetQuestionById(survey1.SurveyId, 7);
                Assert.IsNotNull(question7);
                //RuntimeSession[QstnID_7_OptID_3_] = 5 
                var detail_7_1 = surveyManager.CreateResponseDetail(response.ResponseId, question7.QuestionId, 3);
                //RuntimeSession[QstnID_7_OptID_4_] = 5 
                var detail_7_2 = surveyManager.CreateResponseDetail(response.ResponseId, question7.QuestionId, 4);


                var question8 = surveyManager.GetQuestionById(survey1.SurveyId, 8);
                Assert.IsNotNull(question8);
                //RuntimeSession[QstnID_8_] = 2 
                var detail_8_1 = surveyManager.CreateResponseDetail(response.ResponseId, question8.QuestionId, 2);
                #region
                Assert.IsTrue(detail_8_1.Response == response.ResponseId);
                Assert.IsTrue(detail_8_1.Question == question8.QuestionId);
                Assert.IsTrue(detail_8_1.SelectedOption == 2);
                Assert.IsNull(detail_8_1.SelectedColumn);
                Assert.IsNull(detail_8_1.UserInput);
                #endregion

                var question9 = surveyManager.GetQuestionById(survey1.SurveyId, 9);
                Assert.IsNotNull(question9);
                //RuntimeSession[QstnID_9_] = 1 
                var detail_9_1 = surveyManager.CreateResponseDetail(response.ResponseId, question9.QuestionId, 1);

                var question10 = surveyManager.GetQuestionById(survey1.SurveyId, 10);
                Assert.IsNotNull(question10);
                //RuntimeSession[QstnID_10_] = 1 
                var detail_10_1 = surveyManager.CreateResponseDetail(response.ResponseId, question10.QuestionId, 1);


                var question12 = surveyManager.GetQuestionById(survey1.SurveyId, 12);
                Assert.IsNotNull(question12);
                //RuntimeSession[QstnID_12_] = 56 
                var detail_12_1 = surveyManager.CreateResponseDetail(response.ResponseId, question12.QuestionId, 1, 0, "56");


                var question13 = surveyManager.GetQuestionById(survey1.SurveyId, 13);
                Assert.IsNotNull(question13);
                //RuntimeSession[QstnID_13_] = 2 
                var detail_13_1 = surveyManager.CreateResponseDetail(response.ResponseId, question13.QuestionId, 2);


                var question14 = surveyManager.GetQuestionById(survey1.SurveyId, 14);
                Assert.IsNotNull(question14);
                //RuntimeSession[QstnID_14_] = όλα καλά, συνεχίστε έτσι!!!! 
                var detail_14_1 = surveyManager.CreateResponseDetail(response.ResponseId, question14.QuestionId, 1, 0, "όλα καλά, συνεχίστε έτσι!!!!");
                #region
                Assert.IsTrue(detail_14_1.Response == response.ResponseId);
                Assert.IsTrue(detail_14_1.Question == question14.QuestionId);
                Assert.IsTrue(detail_14_1.SelectedOption == 1);
                Assert.IsTrue(detail_14_1.SelectedColumn == 0);
                Assert.AreEqual<string>(detail_14_1.UserInput, "όλα καλά, συνεχίστε έτσι!!!!");
                #endregion


                //διαγράφουμε την απάντηση:
                surveyManager.DeleteResponse(response.ResponseId);

            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    var responses = surveyManager.GetResponses(item.SurveyId);
                    foreach (var a in responses)
                    {
                        surveyManager.DeleteResponse(a);
                    }

                    if (item.IsBuiltIn)
                        continue;

                    surveyManager.DeleteSurvey(item);
                }
                surveyManager.DeleteAllSessions();
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }
        }



        [TestMethod, Description("testing Surveys.RecordedResponses")]
        public void SurveyResponsesTest01_03()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);
            var surveyManager = VLSurveyManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                //We create a survey:
                var survey1 = CreateSurvey1(surveyManager, client);
                Assert.IsNotNull(survey1);
                //Τραβάμε τις ερωτήσεις:
                var questions = surveyManager.GetQuestionsForSurvey(survey1);
                Assert.IsTrue(questions.Count == 14);

                Assert.IsTrue(surveyManager.GetResponses(survey1.SurveyId).Count == 0);
                Assert.IsTrue(survey1.RecordedResponses == 0);
                Assert.IsFalse(survey1.HasResponses);



                var _now = Utility.RoundToSeconds(Utility.UtcNow());
                //Δημιουργούμε ένα Response:
                var response1 = surveyManager.CreateResponse(survey1.SurveyId, null, null, _now);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 1);
                Assert.IsTrue(survey1.HasResponses);

                //Δημιουργούμε ένα Response:
                var response2 = surveyManager.CreateResponse(survey1.SurveyId, null, null, _now);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 2);
                Assert.IsTrue(survey1.HasResponses);

                //Δημιουργούμε ένα Response:
                var response3 = surveyManager.CreateResponse(survey1.SurveyId, null, null, _now);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 3);
                Assert.IsTrue(survey1.HasResponses);


                //Δημιουργούμε ένα collector:
                var collector = surveyManager.CreateCollector(survey1, CollectorType.WebLink, "test1");
                Assert.IsTrue(collector.Status == CollectorStatus.Open);
                Assert.IsTrue(collector.Responses == 0);

                //Δημιουργούμε ένα Response:
                var response4 = surveyManager.CreateResponse(survey1.SurveyId, collector.CollectorId, null, _now);
                //εχει αλλάξει ο collector:
                collector = surveyManager.GetCollectorById(collector.CollectorId);
                Assert.IsTrue(collector.Responses == 1);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 4);
                Assert.IsTrue(survey1.HasResponses);

                //Δημιουργούμε ένα Response:
                var response5 = surveyManager.CreateResponse(survey1.SurveyId, collector.CollectorId, null, _now);
                //εχει αλλάξει ο collector:
                collector = surveyManager.GetCollectorById(collector.CollectorId);
                Assert.IsTrue(collector.Responses == 2);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 5);
                Assert.IsTrue(survey1.HasResponses);

                //Δημιουργούμε ένα Response:
                var response6 = surveyManager.CreateResponse(survey1.SurveyId, collector.CollectorId, null, _now);
                //εχει αλλάξει ο collector:
                collector = surveyManager.GetCollectorById(collector.CollectorId);
                Assert.IsTrue(collector.Responses == 3);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 6);
                Assert.IsTrue(survey1.HasResponses);

                Assert.IsTrue(surveyManager.GetResponses(survey1.SurveyId).Count == 6);
                //διαγράφουμε ένα response:
                surveyManager.DeleteResponse(response5.ResponseId);
                Assert.IsTrue(surveyManager.GetResponses(survey1.SurveyId).Count == 5);

                //εχει αλλάξει ο collector:
                collector = surveyManager.GetCollectorById(collector.CollectorId);
                Assert.IsTrue(collector.Responses == 2);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 5);
                Assert.IsTrue(survey1.HasResponses);


                //διαγράφουμε ta ρεσπονσεσ του collector:
                surveyManager.ClearResponsesForCollector(collector.CollectorId);

                //εχει αλλάξει ο collector:
                collector = surveyManager.GetCollectorById(collector.CollectorId);
                Assert.IsTrue(collector.Responses == 0);
                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 3);
                Assert.IsTrue(survey1.HasResponses);

                //Διαγράφουμε όλα τα responses:
                var responses = surveyManager.GetResponses(survey1.SurveyId);
                foreach (var r in responses)
                {
                    surveyManager.DeleteResponse(r.ResponseId);
                }

                //Εχει αλλάξει το survey1:
                survey1 = surveyManager.GetSurveyById(survey1.SurveyId);
                Assert.IsTrue(survey1.RecordedResponses == 0);
                Assert.IsFalse(survey1.HasResponses);
            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    var collectors = surveyManager.GetCollectors(item);
                    foreach (var c in collectors)
                    {
                        if(c.Status == CollectorStatus.Open)
                        {
                            surveyManager.CloseCollector(c);
                        }
                    }
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
