using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class GetOptions : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "question");
            var language = TryParseInt16(context, "language");
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 22);
            var sortIndex = TryParseString(context, "sidx", false, "DisplayOrder");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);


            var libraryManager = VLLibraryManager.GetAnInstance(accessToken);
            var items = libraryManager.GetLibraryQuestionOptions(questionId, language);

            int totalpages = items.Count / pageSize;
            if (totalpages * pageSize < items.Count)
                totalpages++;

            var rows = items.Select(c => new
            {
                c.Question,
                c.OptionId,
                c.OptionText,
                c.OptionType,
                c.OptionValue,
                c.TextsLanguage,
                c.DisplayOrder,
                c.AttributeFlags
            }).ToArray();

            var data = new
            {
                total = totalpages,     //total pages for the query
                page = pageIndex,       //current page of the query
                records = items.Count, //total number of records for the query
                rows
            };


            var response = JsonConvert.SerializeObject(data, Formatting.None);
            context.Response.Write(response);
        }
    }
}