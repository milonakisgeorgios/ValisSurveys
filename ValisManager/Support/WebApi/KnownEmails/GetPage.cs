using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.KnownEmails
{
    public class GetPage : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var _search = TryParseBoolean(context, "_search", false, false);
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);

            /*Φτιάχνουμε το Whereclause*/
            StringBuilder whereclause = new StringBuilder("where 1=1");
            if (_search == true)
            {
                if (!string.IsNullOrEmpty(context.Request.Params["RegisterDt"]))
                {
                    try
                    {
                        var _search_RegisterDt = TryParseDateTime(context, "RegisterDt", false, null, "dd/MM/yyyy");
                        //we want the datetime in ISO8601
                        whereclause.AppendFormat(" and RegisterDt >= '{0}'", accessToken.ConvertTimeToUtc(_search_RegisterDt.Value).ToString("s"));
                    }
                    catch
                    {
                    }
                }
                if (!string.IsNullOrEmpty(context.Request.Params["Name"]))
                {
                    var _search_ClientName = TryParseString(context, "Name", true);
                    whereclause.AppendFormat(" and upper(Name) like '%{0}%'", _search_ClientName.Replace("'", "''").ToUpperInvariant());
                }
                if (!string.IsNullOrEmpty(context.Request.Params["EmailAddress"]))
                {
                    var _search_EmailAddress = TryParseString(context, "EmailAddress", true);
                    whereclause.AppendFormat(" and upper(EmailAddress) like '%{0}%'", _search_EmailAddress.Replace("'", "''").ToUpperInvariant());
                }
                if (!string.IsNullOrEmpty(context.Request.Params["IsVerified"]))
                {
                    var _search_IsVerified = TryParseBoolean(context, "IsVerified", true);
                    if(_search_IsVerified)
                    {
                        whereclause.Append(" and IsVerified = 1");
                    }
                    else
                    {
                        whereclause.Append(" and IsVerified = 0");
                    }
                }
                if (!string.IsNullOrEmpty(context.Request.Params["IsOptedOut"]))
                {
                    var _search_IsOptedOut = TryParseBoolean(context, "IsOptedOut", true);
                    if (_search_IsOptedOut)
                    {
                        whereclause.Append(" and IsOptedOut = 1");
                    }
                    else
                    {
                        whereclause.Append(" and IsOptedOut = 0");
                    }
                }
                if (!string.IsNullOrEmpty(context.Request.Params["IsBounced"]))
                {
                    var _search_IsBounced = TryParseBoolean(context, "IsBounced", true);
                    if (_search_IsBounced)
                    {
                        whereclause.Append(" and IsBounced = 1");
                    }
                    else
                    {
                        whereclause.Append(" and IsBounced = 0");
                    }
                }
            }


            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetKnownEmailExs(pageIndex, pageSize, ref totalRecords, whereclause.ToString(), orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;


            var rows = items.Select(c => new
            {
                c.Client,
                c.EmailId,
                c.EmailAddress,
                c.LocalPart,
                c.DomainPart,
                c.RegisterDt,
                c.IsDomainOK,
                c.IsVerified,
                c.IsOptedOut,
                c.IsBounced,
                c.VerifiedDt,
                c.OptedOutDt,
                c.ClientName,
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