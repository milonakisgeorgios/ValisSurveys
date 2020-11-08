using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.ClientLists.Contacts
{
    public class Create : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var listId = TryParseInt32(context, "listId", true);
            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            var contact = new VLContact();
            contact.ListId = listId;
            contact.Organization = TryParseString(context, "Organization", false);
            contact.Title = TryParseString(context, "Title", false);
            contact.Department = TryParseString(context, "Department", false);
            contact.FirstName = TryParseString(context, "FirstName", false);
            contact.LastName = TryParseString(context, "LastName", false);
            contact.Email = TryParseString(context, "Email", true);
            contact.Comment = TryParseString(context, "Comment", false);

            //we create the contact:
            contact = systemManager.CreateContact(contact);

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