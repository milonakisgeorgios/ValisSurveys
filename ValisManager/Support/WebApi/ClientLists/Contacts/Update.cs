using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists.Contacts
{
    public class Update : WebApiHandler
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

            contact.Organization = TryParseString(context, "Organization", false, contact.Organization);
            contact.Title = TryParseString(context, "Title", false, contact.Title);
            contact.Department = TryParseString(context, "Department", false, contact.Department);
            contact.FirstName = TryParseString(context, "FirstName", false, contact.FirstName);
            contact.LastName = TryParseString(context, "LastName", false, contact.LastName);
            contact.Email = TryParseString(context, "Email", false, contact.Email);
            contact.Comment = TryParseString(context, "Comment", false, contact.Comment);

            contact = systemManager.UpdateContact(contact);

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
                contact.CreateDT
            };

            var response = JsonConvert.SerializeObject(_item, Formatting.None);
            context.Response.Write(response);
        }
    }
}