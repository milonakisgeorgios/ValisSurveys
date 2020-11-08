using System;
using System.Collections.ObjectModel;

namespace Valis.Core
{
    /// <summary>
    /// Οι σελίδες EndSurvey, Thankyou και Disqualification έχουν καρφωτά PageId, διαφορετικά απο τις
    /// κανονικές για να μπορούμε να τις καταλαβαίνουμε και να τις ξεχωρίζουμε απο τις κανονικές σελίδες
    /// του survey.
    /// </summary>
    public static class BuiltinVirtualPages
    {
        public static readonly VLSurveyPage EndSurveyPage = new VLSurveyPage() { PageId = -1, ShowTitle = "EndSurvey", DisplayOrder = 1000, CreateDT = Utility.UtcNow(), LastUpdateDT = Utility.UtcNow()};
        public static readonly VLSurveyPage GoodbyPage = new VLSurveyPage() { PageId = -2, ShowTitle = "Thankyou Page", DisplayOrder = 1001, CreateDT = Utility.UtcNow(), LastUpdateDT = Utility.UtcNow() };
        public static readonly VLSurveyPage DisqualificationPage = new VLSurveyPage() { PageId = -3, ShowTitle = "Disqualification Page", DisplayOrder = 1002, CreateDT = Utility.UtcNow(), LastUpdateDT = Utility.UtcNow() };

        public static void AddVirtualPagesToCollection(Int32 surveyId, Collection<VLSurveyPage> pages)
        {
            pages.Add(new VLSurveyPage() { Survey = surveyId, PageId = -1, ShowTitle = "EndSurvey", DisplayOrder = 1000, CreateDT = Utility.UtcNow(), LastUpdateDT = Utility.UtcNow() });
            pages.Add(new VLSurveyPage() { Survey = surveyId, PageId = -2, ShowTitle = "Thankyou Page", DisplayOrder = 1001, CreateDT = Utility.UtcNow(), LastUpdateDT = Utility.UtcNow() });
            pages.Add(new VLSurveyPage() { Survey = surveyId, PageId = -3, ShowTitle = "Disqualification Page", DisplayOrder = 1002, CreateDT = Utility.UtcNow(), LastUpdateDT = Utility.UtcNow() });
        }
    }
}
