using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Payments
{
    public class GetPaymentsView1 : WebApiHandler
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
            var items = systemManager.GetPaymentsView1(clientId, pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;


            foreach(var item in items)
            {
                #region TotalCredits
                if (item.CreditType == CreditType.EmailType)
                {
                    if (item.Quantity == 1)
                    {
                        item.TotalCredits = "1 email";
                    }
                    else
                    {
                        item.TotalCredits = string.Format("{0} emails", item.Quantity);
                    }
                }
                else if (item.CreditType == CreditType.ResponseType)
                {
                    if (item.Quantity == 1)
                    {
                        item.TotalCredits = "1 response";
                    }
                    else
                    {
                        item.TotalCredits = string.Format("{0} responses", item.Quantity);
                    }
                }
                else if (item.CreditType == CreditType.ClickType)
                {
                    if (item.Quantity == 1)
                    {
                        item.TotalCredits = "1 click";
                    }
                    else
                    {
                        item.TotalCredits = string.Format("{0} clicks", item.Quantity);
                    }
                }
                #endregion

                #region UsedCredits
                if (item.CreditType == CreditType.EmailType)
                {
                    if (item.QuantityUsed == 0)
                    {
                        item.UsedCredits = "0";
                    }
                    else if (item.QuantityUsed == 1)
                    {
                        item.UsedCredits = "1";
                    }
                    else
                    {
                        item.UsedCredits = string.Format("{0}", item.QuantityUsed);
                    }
                }
                else if (item.CreditType == CreditType.ResponseType)
                {
                    if (item.QuantityUsed == 0)
                    {
                        item.UsedCredits = "0";
                    }
                    else if (item.QuantityUsed == 1)
                    {
                        item.UsedCredits = "1";
                    }
                    else
                    {
                        item.UsedCredits = string.Format("{0}", item.QuantityUsed);
                    }
                }
                else if (item.CreditType == CreditType.ClickType)
                {
                    if (item.QuantityUsed == 0)
                    {
                        item.UsedCredits = "0";
                    }
                    else if (item.QuantityUsed == 1)
                    {
                        item.UsedCredits = "1 ";
                    }
                    else
                    {
                        item.UsedCredits = string.Format("{0}", item.QuantityUsed);
                    }
                }
                #endregion

                #region RestCredits
                var _restQuantity = item.Quantity - item.QuantityUsed;
                if (item.CreditType == CreditType.EmailType)
                {
                    if (_restQuantity == 0)
                    {
                        item.RestCredits = "0";
                    }
                    else if (_restQuantity == 1)
                    {
                        item.RestCredits = "1";
                    }
                    else
                    {
                        item.RestCredits = _restQuantity.ToString();
                    }
                }
                else if (item.CreditType == CreditType.ResponseType)
                {
                    if (_restQuantity == 0)
                    {
                        item.RestCredits = "0";
                    }
                    else if (_restQuantity == 1)
                    {
                        item.RestCredits = "1";
                    }
                    else
                    {
                        item.RestCredits = _restQuantity.ToString();
                    }
                }
                else if (item.CreditType == CreditType.ClickType)
                {
                    if (_restQuantity == 0)
                    {
                        item.RestCredits = "0";
                    }
                    else if (_restQuantity == 1)
                    {
                        item.RestCredits = "1";
                    }
                    else
                    {
                        item.RestCredits = _restQuantity.ToString();
                    }
                }
                #endregion

            }


            var data = new
            {
                total = totalpages,     //total pages for the query
                page = pageIndex,       //current page of the query
                records = totalRecords, //total number of records for the query
                rows = items
            };


            var response = JsonConvert.SerializeObject(data, Formatting.None);
            context.Response.Write(response);
        }
    }
}