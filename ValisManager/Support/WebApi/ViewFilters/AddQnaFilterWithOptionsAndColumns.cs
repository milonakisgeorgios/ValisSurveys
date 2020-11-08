using System;
using System.Collections.ObjectModel;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ViewFilters
{
    public class AddQnaFilterWithOptionsAndColumns : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var viewId = TryParseGuid(context, "viewId");
                var questionId = TryParseInt16(context, "questionId");
                var conc_rows = TryParseString(context, "rows");             //fltrOption_8_1_1,fltrOption_8_2_2,fltrOption_8_3_3
                var conc_cols = TryParseString(context, "columns");       //fltrColumn_8_9_1,fltrColumn_8_10_1,fltrColumn_8_8_2,fltrColumn_8_9_2,fltrColumn_8_10_2,fltrColumn_8_5_3,fltrColumn_8_6_3,fltrColumn_8_7_3,fltrColumn_8_8_3

                /*σπάμε rows & columns:*/
                var rows = conc_rows.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var columns = conc_cols.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


                //Φτιάχνουμε ένα surveyManager:
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                //Βρισκουμε την επιλεγμένη όψη:
                var selectedview = surveyManager.GetViewById(viewId);


                //Δημιουργούμε τα filterDetails:
                Collection<VLFilterDetail> details = new Collection<VLFilterDetail>();
                foreach (var row in rows)
                {
                    var tokens = row.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    Int32 question = Int32.Parse(tokens[1]);
                    byte option = byte.Parse(tokens[2]);
                    byte section = byte.Parse(tokens[3]);

                    foreach (var col in columns)
                    {
                        var tokens2 = col.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        Int32 question2 = Int32.Parse(tokens2[1]);
                        byte column = byte.Parse(tokens2[2]);
                        byte section2 = byte.Parse(tokens2[3]);

                        if (question == question2 && section == section2)
                        {
                            details.Add(new VLFilterDetail { Operator = ComparisonOperator.IsChecked, SelectedOption = option, SelectedColumn = column });
                        }
                    }
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