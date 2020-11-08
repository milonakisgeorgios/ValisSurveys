using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class ViewTests02 : SurveyFacilityBaseClass
    {

        /// <summary>
        /// 
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests02_01()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("Delta S.A.", BuiltinCountries.Greece, "delta", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                var survey = CreateSurvey2(surveyManager, client1, "my survey!!!");
                
                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);



                //Δημιουργούμε viewFilters:
                var filter1 = surveyManager.AddFilter(defView, 6, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 });
                //Εκτελούμε την MakeViewFilterStatements
                var qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 1);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=6 and ((SelectedOption=1))))");
                //Προσθέτουμε ένα ακόμα viewFilter:
                var filter2 = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2 }); //QuestionType.OneFromMany
                //Εκτελούμε την MakeViewFilterStatements
                qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 2);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=6 and ((SelectedOption=1))))");
                Assert.AreEqual<string>(qfilters[1], "where (([Question]=5 and ((SelectedOption=1) or (SelectedOption=2))))");
                //Προσθέτουμε ένα ακόμα viewFilter:
                var filter3 = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2, SelectedColumn = 9 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 9 });
                //Εκτελούμε την MakeViewFilterStatements
                qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 3);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=6 and ((SelectedOption=1))))");
                Assert.AreEqual<string>(qfilters[1], "where (([Question]=5 and ((SelectedOption=1) or (SelectedOption=2))))");
                Assert.AreEqual<string>(qfilters[2], "where ((Question=8 and ((SelectedOption=1 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=2 and (SelectedColumn=9)) and (SelectedOption=3 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=4 and (SelectedColumn=9)))))");


                //διαγράφουμε τα viewFilters:
                surveyManager.DeleteViewFiltersForView(defView);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);
                //Δημιουργούμε νέα φίλτρα:
                filter1 = surveyManager.AddFilter(defView, 6, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 });
                filter2 = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2 }); //QuestionType.OneFromMany
                filter3 = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2, SelectedColumn = 9 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 9 });
                //Εκτελούμε την MakeViewFilterStatements
                qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 3);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=6 and ((SelectedOption=1))))");
                Assert.AreEqual<string>(qfilters[1], "where (([Question]=5 and ((SelectedOption=1) or (SelectedOption=2))))");
                Assert.AreEqual<string>(qfilters[2], "where ((Question=8 and ((SelectedOption=1 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=2 and (SelectedColumn=9)) and (SelectedOption=3 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=4 and (SelectedColumn=9)))))");



                //διαγράφουμε τα viewFilters:
                surveyManager.DeleteViewFiltersForView(defView);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);
                //Δημιουργούμε νέα φίλτρα:
                filter1 = surveyManager.AddFilter(defView, 6, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 });
                filter2 = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2 }); //QuestionType.OneFromMany
                filter3 = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2, SelectedColumn = 9 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 4 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 5 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 3, SelectedColumn = 6 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 9 });
                //Εκτελούμε την MakeViewFilterStatements
                qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 3);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=6 and ((SelectedOption=1))))");
                Assert.AreEqual<string>(qfilters[1], "where (([Question]=5 and ((SelectedOption=1) or (SelectedOption=2))))");
                Assert.AreEqual<string>(qfilters[2], "where ((Question=8 and ((SelectedOption=1 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=2 and (SelectedColumn=9)) and (SelectedOption=3 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=4 and (SelectedColumn=9)))))");
                //Δημιουργούμε νέα φίλτρα:
                var filter4 = surveyManager.AddFilter(defView, 4, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "12/18/2013", UserInput2 = "12/28/2013" });       //QuestionType.Date
                //Εκτελούμε την MakeViewFilterStatements
                qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 4);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=6 and ((SelectedOption=1))))");
                Assert.AreEqual<string>(qfilters[1], "where (([Question]=5 and ((SelectedOption=1) or (SelectedOption=2))))");
                Assert.AreEqual<string>(qfilters[2], "where ((Question=8 and ((SelectedOption=1 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=2 and (SelectedColumn=9)) and (SelectedOption=3 and (SelectedColumn=4 or SelectedColumn=5 or SelectedColumn=6)) and (SelectedOption=4 and (SelectedColumn=9)))))");
                Assert.AreEqual<string>(qfilters[3], "where (([Question]=4 and ((convert(date, UserInput, 101) between convert(date, '12/18/2013', 101) and convert(date, '12/28/2013', 101)))))");


                //διαγράφουμε τα viewFilters:
                surveyManager.DeleteViewFiltersForView(defView);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);
                //Δημιουργούμε νέα φίλτρα:
                filter1 = surveyManager.AddFilter(defView, 2, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "12", UserInput2 = "45" });
                qfilters = surveyManager.MakeViewFilterStatements(defView, surveyManager.GetViewFilters(defView));
                Assert.IsTrue(qfilters.Count == 1);
                Assert.AreEqual<string>(qfilters[0], "where (([Question]=2 and ((cast(UserInput as int) between 12 and 45))))");
            }
            finally
            {
                #region
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
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests02_02()
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
                CreateResponsesForSurvey2(surveyManager, survey);


                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                surveyManager.DeleteViewFiltersForView(defView);
                defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 0);


                //Τραβάμε τα responses με βάση αυτό το view
                var responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 820);



                //Δημιουργούμε ένα filter: (Αυτοί που έχουν απαντήσει ότι είναι απο το UK)
                var filter1 = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 });//From where are you from? (UK);
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 197);
                //Δημιουργούμε ένα filter: (Αυτοί που έχουν απαντήσει ότι έχουν φίλους το option2 ή to option5):
                var filter2 = surveyManager.AddFilter(defView, 6, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 5 });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 61);
                //Δημιουργούμε ένα filter: (αυτοί των οποίων η ηλικία είναι μεταξύ 10 και 50 χρονων:
                var filter3 = surveyManager.AddFilter(defView, 2, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "10", UserInput2 = "50" });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 44);
                //Δημιουργούμε ένα filter (Που να έχουν απαντήσει οτιδήποτε για βαθμό των μαθηματικών: )
                var filter4 = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4 });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 28);
                //Δημιουργούμε ένα filter (Που να έχουν απαντήσει για βαθμό των μαθηματικών 7,8,9,10: )
                var filter4a = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 7 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 8 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 9 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 10 });


                //Απενεργοποιούμε το filter4:
                filter4 = surveyManager.DisableViewFilter(filter4);
                Assert.IsFalse(filter4.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 11);


                //Απενεργοποιούμε το filter2
                filter2 = surveyManager.DisableViewFilter(filter2);
                Assert.IsFalse(filter2.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 28);


                //Ενεργοποιούμε το filter2:
                filter2 = surveyManager.EnableViewFilter(filter2);
                Assert.IsTrue(filter2.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 11);

                //Απενεργοποιούμε το filter1
                filter1 = surveyManager.DisableViewFilter(filter1);
                Assert.IsFalse(filter1.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 39);
            }
            finally
            {
                #region
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
                #endregion
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests02_03()
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
                CreateResponsesForSurvey2(surveyManager, survey);


                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                surveyManager.DeleteViewFiltersForView(defView);
                defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 0);


                //Τραβάμε τα responses με βάση αυτό το view
                var responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 820);




                //Δημιουργούμε ένα filter: (Αυτοί που το βάρος τους είναι λιγότερο απο 20)
                var filter1 = surveyManager.AddFilter(defView, 3, new VLFilterDetail { Operator = ComparisonOperator.Less, UserInput1 = "20" });//What is your weight?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 196);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 1);
                //Διαγράφουμε το φίλτρο
                surveyManager.DeleteViewFilter(filter1);
                Assert.IsNull(surveyManager.GetViewFilterById(filter1.FilterId));
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);
                //Δημιουργούμε ένα filter: (Αυτοί που το βάρος τους είναι μεγαλύτερο απο 13.6599)
                filter1 = surveyManager.AddFilter(defView, 3, new VLFilterDetail { Operator = ComparisonOperator.Greater, UserInput1 = "13.6599" });//What is your weight?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 745);
                //Δημιουργούμε ένα filter: (Αυτοί που το βάρος τους είναι μεγαλύτερο απο 13.66)
                var filter2 = surveyManager.AddFilter(defView, 3, new VLFilterDetail { Operator = ComparisonOperator.Greater, UserInput1 = "13.66" });//What is your weight?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 684);

                //Διαγράφουμε όλα τα φίλτρα:
                surveyManager.DeleteViewFiltersForView(defView);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);

                //Δημιουργούμε ένα filter: (Αυτοί που το βάρος τους είναι μεταξύ απο 20)
                filter1 = surveyManager.AddFilter(defView, 3, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "18.24", UserInput2 = "57.06" });//What is your weight?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 194);

                //Διαγράφουμε όλα τα φίλτρα:
                surveyManager.DeleteViewFiltersForView(defView);
                Assert.IsTrue(surveyManager.GetViewFilters(defView).Count == 0);



                //Θέλουμε όσους γενήθηκαν την '01/31/1994':
                filter1 = surveyManager.AddFilter(defView, 4, new VLFilterDetail { Operator = ComparisonOperator.Equals, UserInput1 = "01/31/1994" });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 69);

                surveyManager.DeleteViewFiltersForView(defView);
                filter1 = surveyManager.AddFilter(defView, 4, new VLFilterDetail { Operator = ComparisonOperator.Greater, UserInput1 = "01/31/1994" });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 0);

                surveyManager.DeleteViewFiltersForView(defView);
                filter1 = surveyManager.AddFilter(defView, 4, new VLFilterDetail { Operator = ComparisonOperator.GreaterOrEqual, UserInput1 = "01/31/1994" });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 69);


                surveyManager.DeleteViewFiltersForView(defView);
                filter1 = surveyManager.AddFilter(defView, 4, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "07/11/1972", UserInput2 = "07/05/1978" });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 280);

            }
            finally
            {
                #region
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
                #endregion
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        [TestMethod, Description("")]
        public void ViewTests02_04()
        {
            var surveyManager = VLSurveyManager.GetAnInstance(admin);
            var systemManager = VLSystemManager.GetAnInstance(admin);


            try
            {
                #region Δημιουργούμε έναν πελάτη:
                var client1 = systemManager.CreateClient("MatterWare S.A.", BuiltinCountries.Greece, "MaBs", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                #endregion

                var survey = CreateSurvey2(surveyManager, client1, "my survey!!!");
                CreateResponsesForSurvey2(surveyManager, survey);


                //Διαβάζουμε το defaultView:
                var defView = surveyManager.GetDefaultView(survey);
                surveyManager.DeleteViewFiltersForView(defView);
                defView = surveyManager.GetDefaultView(survey);
                Assert.IsNotNull(defView);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 0);


                //Τραβάμε όλα τα responses με βάση αυτό το view
                var responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 820);
                //Τραβάμε τα responses σελίδα/σελίδα με βάση αυτό το view:   
                Int32 totalRows = surveyManager.GetViewResponsesCount(defView);
                Assert.IsTrue(totalRows == 820);
                for (int i = 1; i <= 82; i++)
                {
                    var pagedResponses = surveyManager.GetViewResponses(defView, i, 10, ref totalRows);

                    Assert.IsTrue(totalRows == 820);
                    Assert.IsTrue(pagedResponses.Count == 10);
                }




                //Φτιάχνω φίλτρα :
                var filter1 = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 });//From where are you from? (UK);
                var filter2 = surveyManager.AddFilter(defView, 6, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2 });//Who is your friend? ()
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 30);
                for (int i = 1; i <= 3; i++)
                {
                    var pagedResponses = surveyManager.GetViewResponses(defView, i, 10, ref totalRows);

                    Assert.IsTrue(totalRows == 30);
                    Assert.IsTrue(pagedResponses.Count == 10);
                }

                for (int i = 1; i <= 30; i++)
                {
                    var pagedResponses = surveyManager.GetViewResponses(defView, i, 1, ref totalRows);

                    Assert.IsTrue(totalRows == 30);
                    Assert.IsTrue(pagedResponses.Count == 1);
                }

            }
            finally
            {
                #region
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
                #endregion
            }
        }

    }
}
