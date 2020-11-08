using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite2
{
    [TestClass]
    public class ViewSummary01 : SurveyFacilityBaseClass
    {

        [TestMethod, Description("")]
        public void ViewSummary01_01()
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
                defView = surveyManager.RevertView(defView);
                Assert.IsTrue(defView.NumberOfQuestionFilters == 0);

                //Τραβάμε τα responses με βάση αυτό το view
                var responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 820);
                //τραβάμε το summary αυτού του view:
                var summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 820);


                //Δημιουργούμε ένα filter: (Αυτοί που έχουν απαντήσει ότι είναι απο το UK)
                var filter1 = surveyManager.AddFilter(defView, 5, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 1 });//From where are you from? (UK);
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 197);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 197);

                //Δημιουργούμε ένα filter: (Αυτοί που έχουν απαντήσει ότι έχουν φίλους το option2 ή to option5):
                var filter2 = surveyManager.AddFilter(defView, 6, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 2 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 5 });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 61);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 61);

                //Δημιουργούμε ένα filter: (αυτοί των οποίων η ηλικία είναι μεταξύ 10 και 50 χρονων:
                var filter3 = surveyManager.AddFilter(defView, 2, new VLFilterDetail { Operator = ComparisonOperator.Between, UserInput1 = "10", UserInput2 = "50" });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 44);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 44);
                //Δημιουργούμε ένα filter (Που να έχουν απαντήσει οτιδήποτε για βαθμό των μαθηματικών: )
                var filter4 = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4 });
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 28);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 28);

                //Δημιουργούμε ένα filter (Που να έχουν απαντήσει για βαθμό των μαθηματικών 7,8,9,10: )
                var filter4a = surveyManager.AddFilter(defView, 8, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 7 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 8 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 9 }, new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = 4, SelectedColumn = 10 });
                //Απενεργοποιούμε το filter4:
                filter4 = surveyManager.DisableViewFilter(filter4);
                Assert.IsFalse(filter4.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 11);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 11);


                //Απενεργοποιούμε το filter2
                filter2 = surveyManager.DisableViewFilter(filter2);
                Assert.IsFalse(filter2.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 28);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 28);


                //Ενεργοποιούμε το filter2:
                filter2 = surveyManager.EnableViewFilter(filter2);
                Assert.IsTrue(filter2.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 11);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 11);


                //Απενεργοποιούμε το filter1
                filter1 = surveyManager.DisableViewFilter(filter1);
                Assert.IsFalse(filter1.IsActive);
                //Responses ?
                responses = surveyManager.GetViewResponses(defView);
                Assert.IsTrue(responses.Count == 39);
                summary = surveyManager.GetViewSummary(defView);
                Assert.IsTrue(summary.RecordedResponses == 820);
                Assert.IsTrue(summary.FilteredResponses == 39);
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
