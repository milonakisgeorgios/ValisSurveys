using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class AddQnaFilterWithUserInputs : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var viewId = TryParseGuid(context, "viewId");
                var questionId = TryParseInt16(context, "questionId");
                var comparisonOperator = (ComparisonOperator)TryParseByte(context, "operator");
                var userinput1 = TryParseString(context, "userinput1");
                var userinput2 = TryParseString(context, "userinput2", false, null);

                //Φτιάχνουμε ένα surveyManager:
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                //Βρισκουμε την επιλεγμένη όψη:
                var selectedview = surveyManager.GetViewById(viewId);


                //Δημιουργούμε και το φίλτρο:
                var filter = surveyManager.AddFilter(selectedview, questionId, new VLFilterDetail { Operator = comparisonOperator, UserInput1 = userinput1, UserInput2 = userinput2 });



                //empty json object
                context.Response.Write("{}");
            }
            catch
            {
                throw;
            }
        }
    }
}