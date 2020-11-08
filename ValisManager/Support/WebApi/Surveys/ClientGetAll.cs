using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Surveys
{
    /// <summary>
    /// Επιστρέφει πίσω τα Surveys που αντιστοιχούν στο συγκεκριμένο πελάτη μας!
    /// </summary>
    public class ClientGetAll : WebApiHandler
    {
        static string[] whitelist = new string[] { "SurveyId", "Title", "Folder", "PublicId", "PrimaryLanguage", "CreateDT", "LastUpdateDT", "TextsLanguage", "ShowTitle", "IsBuiltIn", "HasCollectors", "HasResponses", "RecordedResponses" };

        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                /*
                 *  page: the requested page (default value page) 
                    rows: the number of rows requested (default value rows) 
                    sort: the sorting column (default value sidx) 
                    order: the sort order (default value sord) 
                    search: the search indicator (default value _search) 
                    nd: the time passed to the request (for IE browsers not to cache the request) (default value nd) 
                    id: the name of the id when POST-ing data in editing modules (default value id) 
                    oper: the operation parameter (default value oper) 
                    editoper: the name of operation when the data is POST-ed in edit mode (default value edit) 
                    addoper: the name of operation when the data is posted in add mode (default value add) 
                    deloper: the name of operation when the data is posted in delete mode (default value del) 
                    totalrows: the number of the total rows to be obtained from server - see rowTotal (default value totalrows) 
                    subgridid: the name passed when we click to load data in the subgrid (default value id) 
                 */
                var pageIndex = TryParseInt32(context, "page", false, 1);
                var pageSize = TryParseInt32(context, "rows", false, 10);
                var sortIndex = TryParseString(context, "sidx", false, "Name");
                {
                    #region check value
                    /*
                 * To  sortIndex πρέπει να ειναι η ονομασία ενός απο τα Public Properties του anonymous type που στέλνουμε σαν απάντηση:
                 * ClientId, ListId, ContactId, Organization, Title, FirstName, LastName, Email, IsOptedOut, IsBouncedEmail, CreationDT:
                 * 
                 */
                    bool isok = false;
                    foreach (var item in whitelist)
                    {
                        if (item.Equals(sortIndex, System.StringComparison.OrdinalIgnoreCase))
                            isok = true;
                    }
                    if (!isok)
                        throw new ArgumentException(string.Format("Parameter's value is invalid. value = '{0}'.", sortIndex.Replace("'", "''")), "sortIndex");
                    #endregion
                }
                var sortOrder = TryParseString(context, "sord", false, "asc");
                {
                    #region check value
                    if (!sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase) && !sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(string.Format("Parameter's value is invalid. valud = '{0}'.", sortOrder.Replace("'", "''")), "sortOrder");
                    }
                    #endregion
                }

                var manager = VLSurveyManager.GetAnInstance(accessToken);
                var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);

                int totalRecords = 0;
                var items = manager.GetSurveys(pageIndex, pageSize, ref totalRecords, null, orderByClause);

                int totalpages = totalRecords / pageSize;
                if (totalpages * pageSize < totalRecords)
                    totalpages++;

                var rows = items.Select(c => new
                {
                    c.SurveyId,
                    c.Title,
                    c.Folder,
                    c.PublicId,
                    c.PrimaryLanguage,
                    c.SupportedLanguagesIds,
                    CreateDT = accessToken.ConvertTimeFromUtc(c.CreateDT).ToString(Utilities.DateTime_Format_Human),
                    LastUpdateDT = accessToken.ConvertTimeFromUtc(c.LastUpdateDT).ToString(Utilities.DateTime_Format_Human),
                    c.TextsLanguage,
                    c.ShowTitle,
                    c.IsBuiltIn,
                    c.HasCollectors,
                    c.HasResponses,
                    c.RecordedResponses
                }).ToArray();

                var data = new
                {
                    total = totalpages,     //total pages for the query
                    page = pageIndex,       //current page of the query
                    records = totalRecords, //total number of records for the query
                    rows
                };


                var response = JsonConvert.SerializeObject(data, Formatting.None);
                context.Response.Write(response);
            }
            catch
            {
                throw;
            }
        }
    }
}