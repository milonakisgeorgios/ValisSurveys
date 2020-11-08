using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class ViewTests01 : SurveyFacilityBaseClass
    {

        /// <summary>
        /// Ελεγχος κύκλου ζωής για defaultView/viewPage/viewQuestion
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests01_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                //Δημιουργούμε ένα Survey (και αυτόματα δημιουργείται ένα default view για αυτή):
                var survey1 = surveyManager.CreateSurvey(client1, "Questionnaire #1", "Risk assessment");
                Assert.IsNotNull(survey1);

                //Οταν δημιουργείται ένα survey, αμέσως δημιουργείται και ένα default view:
                Assert.IsTrue(surveyManager.GetViews(survey1).Count == 1);
                Assert.IsTrue(surveyManager.GetViewsCount(survey1.SurveyId) == 1);


                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey1);
                Assert.IsNotNull(defView);
                Assert.IsTrue(defView.Client == survey1.Client);
                Assert.IsTrue(defView.Survey == survey1.SurveyId);
                Assert.IsTrue(defView.IsDefaultView);
                var svdDefView = surveyManager.GetViewById(defView.ViewId);
                Assert.AreEqual<VLView>(defView, svdDefView);


                //Οταν δημιουργούμε ένα survey, δημιουργείται αυτόματα και ένα SurveyPage
                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 1);
                var spage1 = surveyManager.GetSurveyPages(survey1)[0];
                Assert.IsNotNull(spage1);
                //Εχει δημιουργηθεί και το αντίστοιχο viewPage για το DefaultPage:
                Assert.IsTrue(surveyManager.GetViewPages(defView).Count == 1);
                var vpage1 = surveyManager.GetViewPagebyId(defView.ViewId, spage1.Survey, spage1.PageId);
                Assert.IsNotNull(vpage1);
                Assert.IsTrue(vpage1.ViewId == defView.ViewId);
                Assert.IsTrue(vpage1.Survey == spage1.Survey);
                Assert.IsTrue(vpage1.Page == spage1.PageId);
                Assert.IsTrue(vpage1.ShowResponses);


                //Δημιουργούμε μία νέα σελίδα ερωτημάτων στο survey:
                var spage2 = surveyManager.CreateSurveyPage(survey1, null, "In this first section, we'd like to learn more about some of your roles, beliefs, and attitudes as well as some of the activities that you do as the parent of a school-aged child.");
                Assert.IsNotNull(spage2);
                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 2);
                //Εχει δημιουργηθεί και το αντίστοιχο viewPage για το DefaultPage:
                Assert.IsTrue(surveyManager.GetViewPages(defView).Count == 2);
                var vpage2 = surveyManager.GetViewPagebyId(defView.ViewId, spage2.Survey, spage2.PageId);
                Assert.IsNotNull(vpage2);
                Assert.IsTrue(vpage2.ViewId == defView.ViewId);
                Assert.IsTrue(vpage2.Survey == spage2.Survey);
                Assert.IsTrue(vpage2.Page == spage2.PageId);
                Assert.IsTrue(vpage2.ShowResponses);


                //Δημιουργούμε ένα question στην page1:
                var question1 = surveyManager.CreateQuestion(spage1, QuestionType.SingleLine, "this is Question #1");
                Assert.IsTrue(question1 != null);
                //Εχει δημιουργηθεί και το αντίστοιχο ViewQuestion για το DefaultView:
                Assert.IsTrue(surveyManager.GetViewQuestions(defView).Count == 1);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage1).Count == 1);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage2).Count == 0);
                var vquestion1 = surveyManager.GetViewQuestionById(defView, question1);
                Assert.IsNotNull(vquestion1);
                Assert.IsTrue(vquestion1.ViewId == defView.ViewId);
                Assert.IsTrue(vquestion1.Survey == survey1.SurveyId);
                Assert.IsTrue(vquestion1.Question == question1.QuestionId);
                Assert.IsTrue(vquestion1.ShowResponses);


                //Δημιουργούμε ακόμα ένα question στην page1:
                var question2 = surveyManager.CreateQuestion(spage1, QuestionType.SingleLine, "this is Question #2");
                Assert.IsTrue(question2 != null);
                //Εχει δημιουργηθεί και το αντίστοιχο ViewQuestion για το DefaultView:
                Assert.IsTrue(surveyManager.GetViewQuestions(defView).Count == 2);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage1).Count == 2);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage2).Count == 0);
                var vquestion2 = surveyManager.GetViewQuestionById(defView, question2);
                Assert.IsNotNull(vquestion2);
                Assert.IsTrue(vquestion2.ViewId == defView.ViewId);
                Assert.IsTrue(vquestion2.Survey == survey1.SurveyId);
                Assert.IsTrue(vquestion2.Question == question2.QuestionId);
                Assert.IsTrue(vquestion2.ShowResponses);

                //Δημιουργούμε ένα collector για το survey1:
                var collector1 = surveyManager.CreateCollector(survey1, CollectorType.WebLink, "web_collector01");
                Assert.IsNotNull(collector1);
                Assert.IsTrue(collector1.Status == CollectorStatus.Open);
                //εχει δημιουργηθεί και το αντίστοιχο ViewCollection για το DefaultView:
                Assert.IsTrue(surveyManager.GetViewCollectors(defView).Count == 1);
                var vcollector1 = surveyManager.GetViewCollectorById(defView.ViewId, collector1.CollectorId);
                Assert.IsNotNull(vcollector1);
                Assert.IsTrue(vcollector1.ViewId == defView.ViewId);
                Assert.IsTrue(vcollector1.Collector == collector1.CollectorId);
                Assert.IsTrue(vcollector1.Survey == collector1.Survey);
                Assert.IsTrue(vcollector1.CollectorType == collector1.CollectorType);
                Assert.IsTrue(vcollector1.CollectorName == collector1.Name);
                Assert.IsTrue(vcollector1.IncludeResponses);


                //Κλείνουμε τον collector
                collector1 = surveyManager.CloseCollector(collector1);
                Assert.IsTrue(collector1.Status == CollectorStatus.Close);

                //διαγραφή μίας ερώτησης
                surveyManager.DeleteQuestion(question2);
                Assert.IsNull(surveyManager.GetQuestionById(question2.Survey, question2.QuestionId));
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(survey1).Count == 1);
                //διαγράφτηκε και τo ατίστοιχο viewQuestion:
                Assert.IsTrue(surveyManager.GetViewQuestions(defView).Count == 1);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage1).Count == 1);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage2).Count == 0);
                Assert.IsNull(surveyManager.GetViewQuestionById(defView, question2));

                //διαγραφή της σελίδας spage1:
                surveyManager.DeleteSurveyPage(spage1);
                Assert.IsNull(surveyManager.GetSurveyPageById(spage1.Survey, spage1.PageId));
                Assert.IsTrue(surveyManager.GetSurveyPages(survey1).Count == 1);
                Assert.IsNull(surveyManager.GetQuestionById(question1.Survey, question1.QuestionId));
                Assert.IsTrue(surveyManager.GetQuestionsForSurvey(survey1).Count == 0);
                //διαγράφτηκαν και τα αντίστοιχα entities στο defview:
                Assert.IsTrue(surveyManager.GetViewPages(defView).Count == 1);
                Assert.IsNull(surveyManager.GetViewPagebyId(defView.ViewId, spage1.Survey, spage1.PageId));
                Assert.IsTrue(surveyManager.GetViewQuestions(defView).Count == 0);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage1).Count == 0);
                Assert.IsTrue(surveyManager.GetViewPageQuestions(vpage2).Count == 0);
                Assert.IsNull(surveyManager.GetViewQuestionById(defView, question1));


                //διαγραφή του collector:
                surveyManager.DeleteCollector(collector1);
                Assert.IsNull(surveyManager.GetCollectorById(collector1.CollectorId));
                //
                Assert.IsTrue(surveyManager.GetViewCollectors(defView).Count == 0);
                Assert.IsNull(surveyManager.GetViewCollectorById(defView.ViewId, collector1.CollectorId));

            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

                    var collectors = surveyManager.GetCollectors(item);
                    foreach (var c in collectors)
                    {
                        if (c.Status == CollectorStatus.Open)
                        {
                            surveyManager.CloseCollector(c);
                        }
                    }

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
        /// Κύκλος ζωής για ViewFilters
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests01_02()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                var survey = CreateSurvey2(surveyManager, client1, "my survey!!!");
                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);

                //το view δεν έχει κανένα φίλτρο:
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 0);
                Assert.IsFalse(defView.FilteringByQuestionInUse);

                //Δημιουργούμε ένα φίλτρο:
                var filter1 = surveyManager.AddFilter(defView, 1, new VLFilterDetail { Operator = ComparisonOperator.Equals, UserInput1 = "george" } );
                Assert.IsTrue(filter1.Survey == defView.Survey);
                Assert.IsTrue(filter1.ViewId == defView.ViewId);
                Assert.IsTrue(filter1.ApplyOrder == 1);
                Assert.IsTrue(filter1.Question == 1);
                Assert.IsTrue(filter1.FilterDetails.Count == 1);
                Assert.IsTrue(filter1.FilterDetails[0].Operator == ComparisonOperator.Equals);
                Assert.IsNull(filter1.FilterDetails[0].SelectedOption);
                Assert.IsNull(filter1.FilterDetails[0].SelectedColumn);
                Assert.IsTrue(filter1.FilterDetails[0].UserInput1 == "george");
                Assert.IsNull(filter1.FilterDetails[0].UserInput2);
                Assert.IsTrue(filter1.IsActive);
                Assert.IsTrue(filter1.IsRule);
                Assert.IsNull(filter1.LogicalOperator);
                var svdFilter1 = surveyManager.GetViewFilterById(filter1.FilterId);
                Assert.AreEqual<VLViewFilter>(filter1, svdFilter1);

                //Τώρα το view έχει ένα φίλτρο:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 1);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 1);
                Assert.IsTrue(defView.FilteringByQuestionInUse);

                //Δημιουργούμε ένα φίλτρο:                
                var filter2 = surveyManager.AddFilter(defView, 2, new Collection<VLFilterDetail> { new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "12", UserInput2="39" } }); //QuestionType.Integer
                Assert.IsTrue(filter2.Survey == defView.Survey);
                Assert.IsTrue(filter2.ViewId == defView.ViewId);
                Assert.IsTrue(filter2.ApplyOrder == 2);
                Assert.IsTrue(filter2.Question == 2);
                Assert.IsTrue(filter2.FilterDetails.Count == 1);
                Assert.IsTrue(filter2.FilterDetails[0].Operator == ComparisonOperator.Between);
                Assert.IsNull(filter2.FilterDetails[0].SelectedOption);
                Assert.IsNull(filter2.FilterDetails[0].SelectedColumn);
                Assert.IsTrue(filter2.FilterDetails[0].UserInput1 == "12");
                Assert.IsTrue(filter2.FilterDetails[0].UserInput2 == "39");
                Assert.IsTrue(filter2.IsActive);
                Assert.IsTrue(filter2.IsRule);
                Assert.IsNull(filter2.LogicalOperator);
                var svdfilter2 = surveyManager.GetViewFilterById(filter2.FilterId);
                Assert.AreEqual<VLViewFilter>(filter2, svdfilter2);
                //Τώρα το view έχει δύο (2) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 2);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 2);
                Assert.IsTrue(defView.FilteringByQuestionInUse);

                //Δημιουργούμε ένα φίλτρο:
                var filter3 = surveyManager.AddFilter(defView, 3, new VLFilterDetail { Operator = ComparisonOperator.GreaterOrEqual, UserInput1 = "64.8" });//QuestionType.Decimal
                Assert.IsTrue(filter3.Survey == defView.Survey);
                Assert.IsTrue(filter3.ViewId == defView.ViewId);
                Assert.IsTrue(filter3.ApplyOrder == 3);
                Assert.IsTrue(filter3.Question == 3);
                Assert.IsTrue(filter3.FilterDetails.Count == 1);
                Assert.IsTrue(filter3.FilterDetails[0].Operator == ComparisonOperator.GreaterOrEqual);
                Assert.IsNull(filter3.FilterDetails[0].SelectedOption);
                Assert.IsNull(filter3.FilterDetails[0].SelectedColumn);
                Assert.IsTrue(filter3.FilterDetails[0].UserInput1 == "64.8");
                Assert.IsNull(filter3.FilterDetails[0].UserInput2);
                Assert.IsTrue(filter3.IsActive);
                Assert.IsTrue(filter3.IsRule);
                Assert.IsNull(filter3.LogicalOperator);
                var svdfilter3 = surveyManager.GetViewFilterById(filter3.FilterId);
                Assert.AreEqual<VLViewFilter>(filter3, svdfilter3);
                //Τώρα το view έχει τρία (3) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 3);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 3);
                Assert.IsTrue(defView.FilteringByQuestionInUse);


                var filter4 = surveyManager.AddFilter(defView, 4, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "12/18/2013", UserInput2 = "12/28/2013" });       //QuestionType.Date
                Assert.IsTrue(filter4.Survey == defView.Survey);
                Assert.IsTrue(filter4.ViewId == defView.ViewId);
                Assert.IsTrue(filter4.ApplyOrder == 4);
                Assert.IsTrue(filter4.Question == 4);
                Assert.IsTrue(filter4.FilterDetails.Count == 1);
                Assert.IsTrue(filter4.FilterDetails[0].Operator == ComparisonOperator.Between);
                Assert.IsNull(filter4.FilterDetails[0].SelectedOption);
                Assert.IsNull(filter4.FilterDetails[0].SelectedColumn);
                Assert.IsTrue(filter4.FilterDetails[0].UserInput1 == "12/18/2013");
                Assert.IsTrue(filter4.FilterDetails[0].UserInput2 == "12/28/2013");
                Assert.IsTrue(filter4.IsActive);
                Assert.IsTrue(filter4.IsRule);
                Assert.IsNull(filter4.LogicalOperator);
                var svdfilter4 = surveyManager.GetViewFilterById(filter4.FilterId);
                Assert.AreEqual<VLViewFilter>(filter4, svdfilter4);
                //Τώρα το view έχει τέσσερα (4) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 4);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 4);
                Assert.IsTrue(defView.FilteringByQuestionInUse);


                var filter5a = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4 }); //QuestionType.OneFromMany
                Assert.IsTrue(filter5a.Survey == defView.Survey);
                Assert.IsTrue(filter5a.ViewId == defView.ViewId);
                Assert.IsTrue(filter5a.ApplyOrder == 5);
                Assert.IsTrue(filter5a.Question == 5);
                Assert.IsTrue(filter5a.FilterDetails.Count == 2);

                Assert.IsTrue(filter5a.FilterDetails[0].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter5a.FilterDetails[0].SelectedOption == 1);
                Assert.IsNull(filter5a.FilterDetails[0].SelectedColumn);
                Assert.IsNull(filter5a.FilterDetails[0].UserInput1);
                Assert.IsNull(filter5a.FilterDetails[0].UserInput2);
                Assert.IsTrue(filter5a.FilterDetails[1].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter5a.FilterDetails[1].SelectedOption == 4);
                Assert.IsNull(filter5a.FilterDetails[1].SelectedColumn);
                Assert.IsNull(filter5a.FilterDetails[1].UserInput1);
                Assert.IsNull(filter5a.FilterDetails[1].UserInput2);

                Assert.IsTrue(filter5a.IsActive);
                Assert.IsTrue(filter5a.IsRule);
                Assert.IsNull(filter5a.LogicalOperator);
                var svdfilter5a = surveyManager.GetViewFilterById(filter5a.FilterId);
                Assert.AreEqual<VLViewFilter>(filter5a, svdfilter5a);
                //Τώρα το view έχει πέντε (5) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 5);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 5);
                Assert.IsTrue(defView.FilteringByQuestionInUse);


                var filter6 = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2, SelectedColumn = 9 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 9 });
                var sql = filter6.ViewFilterSql;
                Assert.IsTrue(filter6.Survey == defView.Survey);
                Assert.IsTrue(filter6.ViewId == defView.ViewId);
                Assert.IsTrue(filter6.ApplyOrder == 6);
                Assert.IsTrue(filter6.Question == 8);
                Assert.IsTrue(filter6.FilterDetails.Count == 8);

                Assert.IsTrue(filter6.FilterDetails[0].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[0].SelectedOption == 1);
                Assert.IsTrue(filter6.FilterDetails[0].SelectedColumn == 4);
                Assert.IsNull(filter6.FilterDetails[0].UserInput1);
                Assert.IsNull(filter6.FilterDetails[0].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[1].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[1].SelectedOption == 1);
                Assert.IsTrue(filter6.FilterDetails[1].SelectedColumn == 5);
                Assert.IsNull(filter6.FilterDetails[1].UserInput1);
                Assert.IsNull(filter6.FilterDetails[1].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[2].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[2].SelectedOption == 1);
                Assert.IsTrue(filter6.FilterDetails[2].SelectedColumn == 6);
                Assert.IsNull(filter6.FilterDetails[2].UserInput1);
                Assert.IsNull(filter6.FilterDetails[2].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[3].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[3].SelectedOption == 2);
                Assert.IsTrue(filter6.FilterDetails[3].SelectedColumn == 9);
                Assert.IsNull(filter6.FilterDetails[3].UserInput1);
                Assert.IsNull(filter6.FilterDetails[3].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[4].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[4].SelectedOption == 3);
                Assert.IsTrue(filter6.FilterDetails[4].SelectedColumn == 4);
                Assert.IsNull(filter6.FilterDetails[4].UserInput1);
                Assert.IsNull(filter6.FilterDetails[4].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[5].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[5].SelectedOption == 3);
                Assert.IsTrue(filter6.FilterDetails[5].SelectedColumn == 5);
                Assert.IsNull(filter6.FilterDetails[5].UserInput1);
                Assert.IsNull(filter6.FilterDetails[5].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[6].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[6].SelectedOption == 3);
                Assert.IsTrue(filter6.FilterDetails[6].SelectedColumn == 6);
                Assert.IsNull(filter6.FilterDetails[6].UserInput1);
                Assert.IsNull(filter6.FilterDetails[6].UserInput2);
                
                Assert.IsTrue(filter6.FilterDetails[7].Operator == ComparisonOperator.IsChecked);
                Assert.IsTrue(filter6.FilterDetails[7].SelectedOption == 4);
                Assert.IsTrue(filter6.FilterDetails[7].SelectedColumn == 9);
                Assert.IsNull(filter6.FilterDetails[7].UserInput1);
                Assert.IsNull(filter6.FilterDetails[7].UserInput2);

                Assert.IsTrue(filter6.IsActive);
                Assert.IsTrue(filter6.IsRule);
                Assert.IsNull(filter6.LogicalOperator);
                var svdfilter6 = surveyManager.GetViewFilterById(filter6.FilterId);
                Assert.AreEqual<VLViewFilter>(filter6, svdfilter6);
                //Τώρα το view έχει έξι (6) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 6);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 6);
                Assert.IsTrue(defView.FilteringByQuestionInUse);


                //Διαγράφουμε ένα φίλτρο:
                surveyManager.DeleteViewFilter(filter3);
                Assert.IsNull(surveyManager.GetViewFilterById(filter3.FilterId));

                filter1 = surveyManager.GetViewFilterById(filter1.FilterId);
                filter2 = surveyManager.GetViewFilterById(filter2.FilterId);
                filter4 = surveyManager.GetViewFilterById(filter4.FilterId);
                filter5a = surveyManager.GetViewFilterById(filter5a.FilterId);
                filter6 = surveyManager.GetViewFilterById(filter6.FilterId);

                Assert.IsTrue(filter1.ApplyOrder == 1);
                Assert.IsTrue(filter2.ApplyOrder == 2);
                Assert.IsTrue(filter4.ApplyOrder == 3);
                Assert.IsTrue(filter5a.ApplyOrder == 4);
                Assert.IsTrue(filter6.ApplyOrder == 5);


                //Τώρα το view έχει πέντε (5) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 5);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 5);
                Assert.IsTrue(defView.FilteringByQuestionInUse);

                //Τώρα απενεργοποιούμε τα φίλτρα:
                filter1 = surveyManager.DisableViewFilter(filter1);
                filter2 = surveyManager.DisableViewFilter(filter2);
                filter4 = surveyManager.DisableViewFilter(filter4);
                filter5a = surveyManager.DisableViewFilter(filter5a);
                {
                    //Τώρα το view έχει πέντε (5) φίλτρα:
                    defView = surveyManager.GetViewById(defView.ViewId);
                    Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 5);
                    Assert.IsTrue(defView.NumberOfQuestionFilters == 5);
                    Assert.IsTrue(defView.FilteringByQuestionInUse);
                }
                //Απενεργοποιούμε και το τελευταίο φίλτρο:
                filter6 = surveyManager.DisableViewFilter(filter6);
                {
                    //Τώρα το view έχει πέντε (5) φίλτρα:
                    defView = surveyManager.GetViewById(defView.ViewId);
                    Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 5);
                    Assert.IsTrue(defView.NumberOfQuestionFilters == 5);
                    Assert.IsFalse(defView.FilteringByQuestionInUse);                   //<!---------
                }




                //Διαγράφουμε ένα ακόμα φίλτρο:
                surveyManager.DeleteViewFilter(filter1);
                Assert.IsNull(surveyManager.GetViewFilterById(filter1.FilterId));

                filter2 = surveyManager.GetViewFilterById(filter2.FilterId);
                filter4 = surveyManager.GetViewFilterById(filter4.FilterId);
                filter5a = surveyManager.GetViewFilterById(filter5a.FilterId);
                filter6 = surveyManager.GetViewFilterById(filter6.FilterId);

                Assert.IsTrue(filter2.ApplyOrder == 1);
                Assert.IsTrue(filter4.ApplyOrder == 2);
                Assert.IsTrue(filter5a.ApplyOrder == 3);
                Assert.IsTrue(filter6.ApplyOrder == 4);                   //<!---------

                //Τώρα το view έχει τέσερα (4) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 4);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 4);
                Assert.IsFalse(defView.FilteringByQuestionInUse);


                //Ενεργοποιούμε ένα απο τα φίλτρα:
                filter4 = surveyManager.EnableViewFilter(filter4);
                Assert.IsTrue(filter4.IsActive);
                //Τώρα το view έχει τέσερα (4) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 4);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 4);
                Assert.IsTrue(defView.FilteringByQuestionInUse);                   //<!---------

                //Διαγράφουμε το φίλτρο που ενεργοποιήσαμε:
                surveyManager.DeleteViewFilter(filter4);
                Assert.IsNull(surveyManager.GetViewFilterById(filter4.FilterId));

                filter2 = surveyManager.GetViewFilterById(filter2.FilterId);
                filter5a = surveyManager.GetViewFilterById(filter5a.FilterId);
                filter6 = surveyManager.GetViewFilterById(filter6.FilterId);

                Assert.IsTrue(filter2.ApplyOrder == 1);
                Assert.IsTrue(filter5a.ApplyOrder == 2);
                Assert.IsTrue(filter6.ApplyOrder == 3);

                //Τώρα το view έχει τρία (3) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 3);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 3);
                Assert.IsFalse(defView.FilteringByQuestionInUse);                   //<!---------


                //Ενεργοποιούμε τα φίλτρα:
                filter2 = surveyManager.EnableViewFilter(filter2);
                Assert.IsTrue(filter2.IsActive);
                filter5a = surveyManager.EnableViewFilter(filter5a);
                Assert.IsTrue(filter5a.IsActive);
                filter6 = surveyManager.EnableViewFilter(filter6);
                Assert.IsTrue(filter6.IsActive);
                //Τώρα το view έχει τρία (3) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 3);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 3);
                Assert.IsTrue(defView.FilteringByQuestionInUse);                   //<!---------


                //Διαγράφουμε ένα φίλτρο:
                surveyManager.DeleteViewFilter(filter6);
                Assert.IsNull(surveyManager.GetViewFilterById(filter6.FilterId));

                filter2 = surveyManager.GetViewFilterById(filter2.FilterId);
                filter5a = surveyManager.GetViewFilterById(filter5a.FilterId);

                Assert.IsTrue(filter2.ApplyOrder == 1);
                Assert.IsTrue(filter5a.ApplyOrder == 2);
                //Τώρα το view έχει δύο (2) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 2);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 2);
                Assert.IsTrue(defView.FilteringByQuestionInUse);                   //<!---------


                //Απενεργοποιούμε ένα φίλτρο:
                filter2 = surveyManager.DisableViewFilter(filter2);
                Assert.IsFalse(filter2.IsActive);
                //Τώρα το view έχει δύο (2) φίλτρα:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 2);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 2);
                Assert.IsTrue(defView.FilteringByQuestionInUse);                   //<!---------

                //Διαγράφουμε ένα φίλτρο
                surveyManager.DeleteViewFilter(filter5a);
                Assert.IsNull(surveyManager.GetViewFilterById(filter5a.FilterId));

                filter2 = surveyManager.GetViewFilterById(filter2.FilterId);
                Assert.IsTrue(filter2.ApplyOrder == 1);
                Assert.IsFalse(filter2.IsActive);

                //Τώρα το view έχει ένα (1) φίλτρο:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 1);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 1);
                Assert.IsFalse(defView.FilteringByQuestionInUse);                   //<!---------



                //ενεργοποιούμε το φίλτρο
                filter2 = surveyManager.EnableViewFilter(filter2);
                Assert.IsTrue(filter2.IsActive);
                //Τώρα το view έχει ένα (1) φίλτρο:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 1);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 1);
                Assert.IsTrue(defView.FilteringByQuestionInUse);                   //<!---------


                //Διαγράφουμε και το τελευταίο φίλτρο:
                surveyManager.DeleteViewFilter(filter2);
                Assert.IsNull(surveyManager.GetViewFilterById(filter2.FilterId));
                //Τώρα το view δεν έχει κανένα φίλτρο:
                defView = surveyManager.GetViewById(defView.ViewId);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 0);
                Assert.IsFalse(defView.FilteringByQuestionInUse);                   //<!---------

            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

                    var collectors = surveyManager.GetCollectors(item);
                    foreach (var c in collectors)
                    {
                        if (c.Status == CollectorStatus.Open)
                        {
                            surveyManager.CloseCollector(c);
                        }
                    }

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
        /// Filtering by ViewCollectors
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests01_03()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                var survey = CreateSurvey2(surveyManager, client1, "my survey!!!");
                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);
                Assert.IsFalse(defView.FilteringByCollectorInUse);
                Assert.IsFalse(defView.EnableFilteringByCollector);

                //Το defaultView δεν διαθέτει κανένα ViewCollectors:
                Assert.IsTrue(surveyManager.GetViewCollectors(defView).Count == 0);

                //Δημιουργούμε τρείς collectors για το survey μας:
                var collector01 = surveyManager.CreateCollector(survey, CollectorType.WebLink, "collector01");
                var collector02 = surveyManager.CreateCollector(survey, CollectorType.WebLink, "collector02");
                var collector03 = surveyManager.CreateCollector(survey, CollectorType.WebLink, "collector03");

                //Τώρα το defaultView διαθέτει τρείς (3) ViewCollectors, και όλοι έχουν το IncludeResponses == true:
                var vcollectors = surveyManager.GetViewCollectors(defView);
                Assert.IsTrue(vcollectors.Count == 3);
                foreach (var vc in vcollectors)
                {
                    Assert.IsTrue(vc.IncludeResponses);
                }

                var vcollector1 = surveyManager.GetViewCollectorById(defView.ViewId, collector01.CollectorId);
                var vcollector2 = surveyManager.GetViewCollectorById(defView.ViewId, collector02.CollectorId);
                var vcollector3 = surveyManager.GetViewCollectorById(defView.ViewId, collector03.CollectorId);

                //Θέλουμε να ενεργοποιήσουμε φιλτράρισμα των responses ανα collector. Θέλουμε τα responses απο collector01 & collector03:
                vcollector1.IncludeResponses = true;
                vcollector1 = surveyManager.UpdateViewCollector(vcollector1);
                Assert.IsTrue(vcollector1.IncludeResponses);
                vcollector2.IncludeResponses = false;
                vcollector2 = surveyManager.UpdateViewCollector(vcollector2);
                Assert.IsFalse(vcollector2.IncludeResponses);
                vcollector3.IncludeResponses = true;
                vcollector3 = surveyManager.UpdateViewCollector(vcollector3);
                Assert.IsTrue(vcollector3.IncludeResponses);
                defView = surveyManager.EnableFilteringByCollector(defView);
                Assert.IsTrue(defView.EnableFilteringByCollector);
                Assert.IsFalse(defView.FilteringByCollectorInUse);

                //Θέλουμε να απενεργοποιήσουμε το FilteringByCollector:
                defView = surveyManager.DisableFilteringByCollector(defView);
                Assert.IsFalse(defView.EnableFilteringByCollector);
                Assert.IsFalse(defView.FilteringByCollectorInUse);




                //Θέλουμε να ενεργοποιήσουμε φιλτράρισμα των responses ανα collector. Θέλουμε τα responses απο collector02:
                vcollector1.IncludeResponses = false;
                vcollector1 = surveyManager.UpdateViewCollector(vcollector1);
                Assert.IsFalse(vcollector1.IncludeResponses);
                vcollector2.IncludeResponses = true;
                vcollector2 = surveyManager.UpdateViewCollector(vcollector2);
                Assert.IsTrue(vcollector2.IncludeResponses);
                vcollector3.IncludeResponses = false;
                vcollector3 = surveyManager.UpdateViewCollector(vcollector3);
                Assert.IsFalse(vcollector3.IncludeResponses);
                defView = surveyManager.EnableFilteringByCollector(defView);
                Assert.IsTrue(defView.EnableFilteringByCollector);
                Assert.IsFalse(defView.FilteringByCollectorInUse);

                //Θέλουμε να απενεργοποιήσουμε το FilteringByCollector:
                defView = surveyManager.DisableFilteringByCollector(defView);
                Assert.IsFalse(defView.EnableFilteringByCollector);
                Assert.IsFalse(defView.FilteringByCollectorInUse);

            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

                    var collectors = surveyManager.GetCollectors(item);
                    foreach (var c in collectors)
                    {
                        if (c.Status == CollectorStatus.Open)
                        {
                            surveyManager.CloseCollector(c);
                        }
                    }

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
        /// FilteringByTimePeriodInUse & FilteringByResponseTimeInUse
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests01_04()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                var survey = CreateSurvey2(surveyManager, client1, "my survey!!!");
                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);
                Assert.IsFalse(defView.FilteringByResponseTimeInUse);
                Assert.IsFalse(defView.EnableFilteringByResponseTime);
                Assert.IsFalse(defView.FilteringByTimePeriodInUse);
                Assert.IsFalse(defView.EnableFilteringByTimePeriod);


                //Θέλουμε να ενεργοποιήσουμε φιλτράρισμα των responses που συμπληρώθηκαν μεταξύ δύο ημερομηνιών:
                var timePeriodStart = Utility.RoundToSeconds(DateTime.Now.AddDays(-12));
                var timePeriodEnd = Utility.RoundToSeconds(DateTime.Now);
                defView = surveyManager.AddTimePeriodFilter(defView, timePeriodStart, timePeriodEnd);
                Assert.IsTrue(defView.EnableFilteringByTimePeriod);
                Assert.IsTrue(defView.FilteringByTimePeriodInUse);
                Assert.IsTrue(defView.TimePeriodStart == timePeriodStart);
                Assert.IsTrue(defView.TimePeriodEnd == timePeriodEnd);
                //Θέλουμε να απενεργοποιήσουμε
                defView = surveyManager.DisableFilteringByTimePeriod(defView);
                Assert.IsFalse(defView.EnableFilteringByTimePeriod);
                Assert.IsTrue(defView.FilteringByTimePeriodInUse);
                Assert.IsTrue(defView.TimePeriodStart == timePeriodStart);
                Assert.IsTrue(defView.TimePeriodEnd == timePeriodEnd);
                //Θέλουμε να το διαγράψουμε
                defView = surveyManager.DeleteTimePeriodFilter(defView);
                Assert.IsFalse(defView.EnableFilteringByTimePeriod);
                Assert.IsFalse(defView.FilteringByTimePeriodInUse);
                Assert.IsNull(defView.TimePeriodStart);
                Assert.IsNull(defView.TimePeriodEnd);

                //Θέλουμε να ενεργοποιήσουμε φιλτράρισμα των responses που συμπληρώθηκαν σε λιγότερο χρόνο απο δύο ώρες:
                defView = surveyManager.AddResponseTimeFilter(defView, ResponseTimeOperator.LessOrEqual, 1, ResponseTimeUnit.Hour);
                Assert.IsTrue(defView.EnableFilteringByResponseTime);
                Assert.IsTrue(defView.FilteringByResponseTimeInUse);
                Assert.IsTrue(defView.TotalResponseTime == 1);
                Assert.IsTrue(defView.TotalResponseTimeOperator == ResponseTimeOperator.LessOrEqual);
                Assert.IsTrue(defView.TotalResponseTimeUnit == ResponseTimeUnit.Hour);
                //Θέλουμε να απενεργοποιήσουμε
                defView = surveyManager.DisableFilteringByTotalResponseTime(defView);
                Assert.IsFalse(defView.EnableFilteringByResponseTime);
                Assert.IsTrue(defView.FilteringByResponseTimeInUse);
                Assert.IsTrue(defView.TotalResponseTime == 1);
                Assert.IsTrue(defView.TotalResponseTimeOperator == ResponseTimeOperator.LessOrEqual);
                Assert.IsTrue(defView.TotalResponseTimeUnit == ResponseTimeUnit.Hour);
                //Θέλουμε να ακυρώσουμε
                defView = surveyManager.DeleteResponseTimeFilter(defView);
                Assert.IsFalse(defView.EnableFilteringByResponseTime);
                Assert.IsFalse(defView.FilteringByResponseTimeInUse);
                Assert.IsNull(defView.TotalResponseTime);
                Assert.IsNull(defView.TotalResponseTimeOperator);
                Assert.IsNull(defView.TotalResponseTimeUnit);
            }
            finally
            {
                var surveys = surveyManager.GetSurveys(textsLanguage: BuiltinLanguages.PrimaryLanguage);
                foreach (var item in surveys)
                {
                    if (item.IsBuiltIn)
                        continue;

                    var collectors = surveyManager.GetCollectors(item);
                    foreach (var c in collectors)
                    {
                        if (c.Status == CollectorStatus.Open)
                        {
                            surveyManager.CloseCollector(c);
                        }
                    }

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
