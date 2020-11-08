using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Logins
{
    public class GetLogins : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");

            /*Τραβάμε τα search criteria:*/
            //var _search_PrincipalType = TryParseByte(context, "PrincipalType", false);
            //var _search_ClientName = TryParseString(context, "ClientName", false);
            //var _search_User = TryParseString(context, "User", false);
            //var _search_LogOnToken = TryParseString(context, "LogOnToken", false);
            //var _search_EnterDt = TryParseString(context, "EnterDt", false);
            //var _search_LeaveDt = TryParseString(context, "LeaveDt", false);


            /*Φτιάχνουμε το OrderBy*/
            string orderByClause = string.Empty;
            if (sortIndex == "User")
            {
                orderByClause = string.Format("order by LastName {0}, FirstName {0}", sortOrder);
            }
            else
            {
                orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);
            }
            /*Φτιάχνουμε το Whereclause*/
            StringBuilder whereclause = new StringBuilder("where 1=1");
            if (!string.IsNullOrEmpty(context.Request.Params["PrincipalType"]))
            {
                var _search_PrincipalType = (PrincipalType)TryParseByte(context, "PrincipalType", true);
                whereclause.AppendFormat(" and PrincipalType = {0}", ((byte)_search_PrincipalType).ToString(CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrEmpty(context.Request.Params["ClientName"]))
            {
                var _search_ClientName = TryParseString(context, "ClientName", true);
                whereclause.AppendFormat(" and upper(ClientName) like '%{0}%'", _search_ClientName.Replace("'", "''").ToUpperInvariant());
            }
            if (!string.IsNullOrEmpty(context.Request.Params["User"]))
            {
                var _search_User = TryParseString(context, "User", false);
                whereclause.AppendFormat(" and upper(LastName) like '%{0}%'", _search_User.Replace("'", "''").ToUpperInvariant());
            }
            if (!string.IsNullOrEmpty(context.Request.Params["LogOnToken"]))
            {
                var _search_LogOnToken = TryParseString(context, "LogOnToken", false);
                whereclause.AppendFormat(" and upper(LogOnToken) like '%{0}%'", _search_LogOnToken.Replace("'", "''").ToUpperInvariant());
            }
            if (!string.IsNullOrEmpty(context.Request.Params["EnterDt"]))
            {
                try
                {
                    var _search_EnterDt = TryParseDateTime(context, "EnterDt", false, null, "dd/MM/yyyy");
                    //we want the datetime in ISO8601
                    whereclause.AppendFormat(" and EnterDt>='{0}'", accessToken.ConvertTimeToUtc( _search_EnterDt.Value).ToString("s"));
                }
                catch
                {
                }

            }




            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetLogins(pageIndex, pageSize, ref totalRecords, whereclause.ToString(), orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;


            var rows = items.Select(c => new
            {
                c.LoginId,
                c.PrincipalType,
                c.Principal,
                c.ClientId,
                c.Language,
                c.Permissions,
                EnterDt = accessToken.ConvertTimeFromUtc(c.EnterDt),
                LeaveDt = c.LeaveDt.HasValue ? (DateTime?)accessToken.ConvertTimeFromUtc(c.LeaveDt.Value) : (DateTime?)null,
                c.IPAddress,
                c.TimeZoneId,
                c.IsOK,
                c.IsCleared,
                c.ClientName,
                c.FirstName,
                c.LastName,
                c.Email,
                c.Role,
                c.LogOnToken
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