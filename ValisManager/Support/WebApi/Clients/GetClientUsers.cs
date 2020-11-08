using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Clients
{
    public class GetClientUsers : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var clientId = TryParseInt32(context, "ClientId", true);
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);


            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetclientUserViews(clientId, pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;
            
            var rows = items.Select(c => new
            {
                c.Client,
                c.UserId,
                c.DefaultLanguageName,
                c.Title,
                c.Department,
                c.FirstName,
                c.LastName,
                c.CountryName,
                c.Prefecture,
                c.Town,
                c.Address,
                c.Zip,
                c.Telephone1,
                c.Telephone2,
                c.Email,
                c.IsActive,
                c.IsBuiltIn,
                c.RoleName,
                c.LastActivityDate,
                c.LogOnToken,
                c.IsApproved,
                c.IsLockedOut,
                c.LastLoginDate,
                c.CreateDT
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