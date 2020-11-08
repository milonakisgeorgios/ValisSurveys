using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists
{
    public class GetPage : WebApiHandler
    {
        static string[] whitelist = new string[] { "ClientId", "ListId", "Name", "TotalContacts", "CreationDT", "LastUpdateDT" };

        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            {
                #region check value
                /*
                 * To  sortIndex πρέπει να ειναι η ονομασία ενός απο τα Public Properties του anonymous type που στέλνουμε σαν απάντηση:
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
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);


            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetClientLists(accessToken.ClientId.Value, pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;

            var rows = items.Select(c => new
            {
                c.Client,
                c.ListId,
                c.Name,
                c.TotalContacts,
                CreationDT = accessToken.ConvertTimeFromUtc(c.CreateDT),
                LastUpdateDT = accessToken.ConvertTimeFromUtc(c.LastUpdateDT)
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
    }
}