using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SystemUser
{
    public class GetSystemUsers : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);


            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetSystemUserViews(pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;
            
            var rows = items.Select(c => new
            {
                c.UserId,
                c.DefaultLanguageName,
                c.FirstName,
                c.LastName,
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