using System;
using System.Collections.ObjectModel;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class AddQnaFilterWithOptions : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var viewId = TryParseGuid(context, "viewId");
                var questionId = TryParseInt16(context, "questionId");
                var selectedOptions = TryParseString(context, "options");


                /*Τώρα πρέπει να απομονώσουμε ένα - ένα τα options:*/
                var _options = selectedOptions.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
                byte[] options = new byte[_options.Length];
                for(int i=0; i<_options.Length; i++)
                {
                    options[i] = byte.Parse(_options[i]);
                }

                //Φτιάχνουμε ένα surveyManager:
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                //Βρισκουμε την επιλεγμένη όψη:
                var selectedview = surveyManager.GetViewById(viewId);

                //Δημιουργούμε τα filterDetails:
                Collection<VLFilterDetail> details = new Collection<VLFilterDetail>();
                for (int i = 0; i < options.Length; i++)
                {
                    details.Add(new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = options[i] });
                }
                //Δημιουργούμε και το φίλτρο:
                var filter = surveyManager.AddFilter(selectedview, questionId, details);



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