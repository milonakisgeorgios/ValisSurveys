using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class GetQuestions : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "QuestionText");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);

            var libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = libraryManager.GetLibraryQuestions(BuiltinLibraryQuestionCategories.CommonQuestions.CategoryId, pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);
            
            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;


            var rows = items.Select(c => new
            {
                c.QuestionId,
                c.Category,
                c.QuestionType,
                c.IsRequired,
                c.RequiredBehavior,
                c.RequiredMinLimit,
                c.RequiredMaxLimit,
                c.OptionalInputBox,
                c.RandomizeOptionsSequence,
                c.RandomizeColumnSequence,
                c.OneResponsePerColumn,
                c.UseDateTimeControls,
                c.AddResetLink,
                c.ValidationBehavior,
                c.RandomBehavior,
                c.OtherFieldType,
                c.TextsLanguage,
                c.QuestionText,
                c.CreateDT,
                c.LastUpdateDT
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