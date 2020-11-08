namespace Valis.Core
{
    /// <summary>
    /// Τα πρώτα primary keys (0, 1, 2, ,3, 4, 5, 6, 7, 8, 9) είναι αόρατα απο την διαχείριση του συστήματος
    /// </summary>
    internal static class BuiltinRoles
    {
        public static VLRole SystemAdmin = new VLRole { RoleId = 1, Name = "SystemAdmin", Description = "The role of a SystemAdmin", Permissions = (VLPermissions)131041, IsBuiltIn = true };
        public static VLRole Developer = new VLRole { RoleId = 2, Name = "Developer", Description = "The role of a Developer", Permissions = (VLPermissions)131043, IsBuiltIn = true };
        public static VLRole Administrator = new VLRole { RoleId = 10, Name = "Administrator", Description = "The role of an Administrator", Permissions = (VLPermissions)110305, IsBuiltIn = true };
        public static VLRole PowerClient = new VLRole { RoleId = 15, Name = "PowerClient", Description = "The role of a PowerClient", Permissions = (VLPermissions)549751619584L, IsBuiltIn = true };
        public static VLRole Client = new VLRole { RoleId = 20, Name = "Client", Description = "The role of a Client", Permissions = (VLPermissions)549705482240L, IsBuiltIn = true };
        public static VLRole DemoClient = new VLRole { RoleId = 22, Name = "DemoClient", Description = "The role of a Demo-Client", Permissions = (VLPermissions)173493190656L, IsBuiltIn = true };
    }
}
