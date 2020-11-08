namespace Valis.Core
{
    internal static class BuiltinSystemUsers
    {
        public static readonly VLSystemUser Installer = new VLSystemUser { UserId = 1, DefaultLanguage = 0, FirstName = "Installer", LastName = "Account", IsBuiltIn = true};
        public static readonly VLSystemUser SystemAdmin = new VLSystemUser { UserId = 2, DefaultLanguage = 0, FirstName = "SystemAdmin", LastName = "Account", IsBuiltIn = true };
        public static readonly VLSystemUser Developer = new VLSystemUser { UserId = 3, DefaultLanguage = 43, FirstName = "Developer", LastName = "Account", IsBuiltIn = true };
        public static readonly VLSystemUser Admin = new VLSystemUser { UserId = 4, DefaultLanguage = 43, FirstName = "Administrator", LastName = "Account", IsBuiltIn = true };
    }
}
