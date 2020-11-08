using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists.Contacts
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var contactId = TryParseInt32(context, "contactId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            var contact = systemManager.GetContactById(contactId);
            if (contact == null)
            {
                throw new VLException(string.Format("There is no Contact with id='{0}'.", contactId));
            }

            var _item = new
            {
                contact.ClientId,
                contact.ListId,
                contact.ContactId,
                contact.Organization,
                contact.Title,
                contact.Department,
                contact.FirstName,
                contact.LastName,
                contact.Email,
                contact.Comment,
                contact.IsOptedOut,
                contact.IsBouncedEmail,
                CreateDT = accessToken.ConvertTimeFromUtc(contact.CreateDT).ToString(Utilities.DateTime_Format_General),
                LastUpdateDT = accessToken.ConvertTimeFromUtc(contact.LastUpdateDT).ToString(Utilities.DateTime_Format_General)
            };

            var response = JsonConvert.SerializeObject(_item, Formatting.None);
            context.Response.Write(response);
        }
    }
}