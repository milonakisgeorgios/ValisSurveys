using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class GetAll : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var surveyId = TryParseInt32(context, "surveyId", true);
                var pageIndex = TryParseInt32(context, "page", false, 1);
                var pageSize = TryParseInt32(context, "rows", false, 10);
                var sortIndex = TryParseString(context, "sidx", false, "Name");
                var sortOrder = TryParseString(context, "sord", false, "asc");

                var manager = VLSurveyManager.GetAnInstance(accessToken);

                int totalRecords = 0;
                var items = manager.GetCollectors(surveyId, pageIndex, pageSize, ref totalRecords);

                int totalpages = totalRecords / pageSize;
                if (totalpages * pageSize < totalRecords)
                    totalpages++;


                var rows = items.Select(c => new
                {
                    c.CollectorId,
                    c.Survey,
                    c.CollectorType,
                    c.Name,
                    c.AttributeFlags,
                    c.Status,
                    c.Responses,
                    c.SupportedLanguagesIds,
                    c.PrimaryLanguage,
                    c.TextsLanguage,
                    c.CreditType,
                    CreateDT = accessToken.ConvertTimeFromUtc(c.CreateDT).ToString(Utilities.DateTime_Format_Human),
                    LastUpdateDT = accessToken.ConvertTimeFromUtc(c.LastUpdateDT).ToString(Utilities.DateTime_Format_Human)
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