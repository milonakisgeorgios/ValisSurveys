using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{

    [TestClass]
    public class SystemTests01 : AdminBaseClass
    {
        class TestManager : VLManagerBase
        {
            public TestManager(IAccessToken accessToken) : base(accessToken)
            {

            }

            public new bool ValidatePermissions(params VLPermissions[] requiredPermissions)
            {
                return base.ValidatePermissions(requiredPermissions);
            }
            public new void CheckPermissions(params VLPermissions[] requiredPermissions)
            {
                base.CheckPermissions(requiredPermissions);
            }
        }

        [TestMethod, Description("BuiltinRoles Roles!")]
        public void RolesTest01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                Assert.IsTrue(systemManager.GetRoles().Count == 4);//το σύστημα βλέπει ρόλους απο id >= 10

                #region Δεν μπορούμε να διαγράψουμε builtin roles
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteRole(BuiltinRoles.SystemAdmin.RoleId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteRole(BuiltinRoles.Developer.RoleId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteRole(BuiltinRoles.Administrator.RoleId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteRole(BuiltinRoles.PowerClient.RoleId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteRole(BuiltinRoles.Client.RoleId); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteRole(BuiltinRoles.DemoClient.RoleId); }, typeof(VLException));
                #endregion

                #region Δεν μπορούμε να κάνουμε update builtin roles
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateRole(BuiltinRoles.SystemAdmin); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateRole(BuiltinRoles.Developer); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateRole(BuiltinRoles.Administrator); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateRole(BuiltinRoles.PowerClient); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateRole(BuiltinRoles.Client); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateRole(BuiltinRoles.DemoClient); }, typeof(VLException));
                #endregion

                #region Δεν μπορούμε να δημιουργήσουμε ρόλους με ονόματα που ήδη υπάρχουν
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole(BuiltinRoles.SystemAdmin.Name); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole(BuiltinRoles.Developer.Name); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole(BuiltinRoles.Administrator.Name); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole(BuiltinRoles.PowerClient.Name); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole(BuiltinRoles.Client.Name); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole(BuiltinRoles.DemoClient.Name); }, typeof(VLException));
                #endregion


                var systemAdminRole = systemManager.GetRoleById(BuiltinRoles.SystemAdmin.RoleId);
                Assert.IsNotNull(systemAdminRole);
                Assert.IsTrue((VLPermissions.ManageSystem & systemAdminRole.Permissions) == VLPermissions.ManageSystem);
                Assert.IsTrue((VLPermissions.Developer & systemAdminRole.Permissions) == VLPermissions.Developer);
                Assert.IsTrue((VLPermissions.SystemService & systemAdminRole.Permissions) == VLPermissions.SystemService);

                Assert.IsTrue((VLPermissions.EnumerateSecurity & systemAdminRole.Permissions) == VLPermissions.EnumerateSecurity);
                Assert.IsTrue((VLPermissions.ManageSecurity & systemAdminRole.Permissions) == VLPermissions.ManageSecurity);

                Assert.IsTrue((VLPermissions.EnumerateSystemParameters & systemAdminRole.Permissions) == VLPermissions.EnumerateSystemParameters);
                Assert.IsTrue((VLPermissions.ManageSystemParameters & systemAdminRole.Permissions) == VLPermissions.ManageSystemParameters);

                Assert.IsTrue((VLPermissions.EnumerateBuildingBlocks & systemAdminRole.Permissions) == VLPermissions.EnumerateBuildingBlocks);
                Assert.IsTrue((VLPermissions.ManageBuidingBlocks & systemAdminRole.Permissions) == VLPermissions.ManageBuidingBlocks);

                Assert.IsTrue((VLPermissions.EnumerateThemes & systemAdminRole.Permissions) == VLPermissions.EnumerateThemes);
                Assert.IsTrue((VLPermissions.ManageThemes & systemAdminRole.Permissions) == VLPermissions.ManageThemes);

                Assert.IsTrue((VLPermissions.EnumerateRenders & systemAdminRole.Permissions) == VLPermissions.EnumerateRenders);
                Assert.IsTrue((VLPermissions.ManageRenders & systemAdminRole.Permissions) == VLPermissions.ManageRenders);

                Assert.IsTrue((VLPermissions.EnumerateClients & systemAdminRole.Permissions) == VLPermissions.EnumerateClients);
                Assert.IsTrue((VLPermissions.ManageClients & systemAdminRole.Permissions) == VLPermissions.ManageClients);


                var developerRole = systemManager.GetRoleById(BuiltinRoles.Developer.RoleId);
                Assert.IsNotNull(developerRole);
                Assert.IsTrue((VLPermissions.ManageSystem & developerRole.Permissions) == VLPermissions.ManageSystem);
                Assert.IsTrue((VLPermissions.Developer & developerRole.Permissions) == VLPermissions.Developer);
                Assert.IsTrue((VLPermissions.SystemService & developerRole.Permissions) == VLPermissions.SystemService);

                Assert.IsTrue((VLPermissions.EnumerateSecurity & developerRole.Permissions) == VLPermissions.EnumerateSecurity);
                Assert.IsTrue((VLPermissions.ManageSecurity & developerRole.Permissions) == VLPermissions.ManageSecurity);

                Assert.IsTrue((VLPermissions.EnumerateSystemParameters & developerRole.Permissions) == VLPermissions.EnumerateSystemParameters);
                Assert.IsTrue((VLPermissions.ManageSystemParameters & developerRole.Permissions) == VLPermissions.ManageSystemParameters);

                Assert.IsTrue((VLPermissions.EnumerateBuildingBlocks & developerRole.Permissions) == VLPermissions.EnumerateBuildingBlocks);
                Assert.IsTrue((VLPermissions.ManageBuidingBlocks & developerRole.Permissions) == VLPermissions.ManageBuidingBlocks);

                Assert.IsTrue((VLPermissions.EnumerateThemes & developerRole.Permissions) == VLPermissions.EnumerateThemes);
                Assert.IsTrue((VLPermissions.ManageThemes & developerRole.Permissions) == VLPermissions.ManageThemes);

                Assert.IsTrue((VLPermissions.EnumerateRenders & developerRole.Permissions) == VLPermissions.EnumerateRenders);
                Assert.IsTrue((VLPermissions.ManageRenders & developerRole.Permissions) == VLPermissions.ManageRenders);

                Assert.IsTrue((VLPermissions.EnumerateClients & systemAdminRole.Permissions) == VLPermissions.EnumerateClients);
                Assert.IsTrue((VLPermissions.ManageClients & systemAdminRole.Permissions) == VLPermissions.ManageClients);


                var administratorRole = systemManager.GetRoleById(BuiltinRoles.Administrator.RoleId);
                Assert.IsNotNull(administratorRole);
                Assert.IsTrue((VLPermissions.ManageSystem & administratorRole.Permissions) == VLPermissions.ManageSystem);
                Assert.IsFalse((VLPermissions.Developer & administratorRole.Permissions) == VLPermissions.Developer);
                Assert.IsFalse((VLPermissions.SystemService & administratorRole.Permissions) == VLPermissions.SystemService);

                Assert.IsTrue((VLPermissions.EnumerateSecurity & administratorRole.Permissions) == VLPermissions.EnumerateSecurity);
                Assert.IsTrue((VLPermissions.ManageSecurity & administratorRole.Permissions) == VLPermissions.ManageSecurity);

                Assert.IsTrue((VLPermissions.EnumerateSystemParameters & administratorRole.Permissions) == VLPermissions.EnumerateSystemParameters);
                Assert.IsFalse((VLPermissions.ManageSystemParameters & administratorRole.Permissions) == VLPermissions.ManageSystemParameters);

                Assert.IsTrue((VLPermissions.EnumerateBuildingBlocks & administratorRole.Permissions) == VLPermissions.EnumerateBuildingBlocks);
                Assert.IsTrue((VLPermissions.ManageBuidingBlocks & administratorRole.Permissions) == VLPermissions.ManageBuidingBlocks);

                Assert.IsTrue((VLPermissions.EnumerateThemes & administratorRole.Permissions) == VLPermissions.EnumerateThemes);
                Assert.IsFalse((VLPermissions.ManageThemes & administratorRole.Permissions) == VLPermissions.ManageThemes);

                Assert.IsTrue((VLPermissions.EnumerateRenders & administratorRole.Permissions) == VLPermissions.EnumerateRenders);
                Assert.IsFalse((VLPermissions.ManageRenders & administratorRole.Permissions) == VLPermissions.ManageRenders);

                Assert.IsTrue((VLPermissions.EnumerateClients & administratorRole.Permissions) == VLPermissions.EnumerateClients);
                Assert.IsTrue((VLPermissions.ManageClients & administratorRole.Permissions) == VLPermissions.ManageClients);

            }
            finally
            {
                var roles = systemManager.GetRoles();
                foreach(var item in roles)
                {
                    if (item.IsBuiltIn == false)
                        systemManager.DeleteRole(item.RoleId);
                }
            }
        }

        [TestMethod, Description("CRUD operations for Roles!")]
        public void RolesTest02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                Assert.IsTrue(systemManager.GetRoles().Count == 4);//το σύστημα βλέπει ρόλους απο id >= 10


                //Δημιουργούμε ένα νέο ρόλο:
                var role01 = systemManager.CreateRole("ρόλος1", permissions: VLPermissions.Developer | VLPermissions.ManageSecurity);
                #region
                Assert.IsNotNull(role01);
                Assert.AreEqual(role01.Name, "ρόλος1");
                Assert.IsNull(role01.Description);
                Assert.IsTrue(role01.Permissions == (VLPermissions.Developer | VLPermissions.ManageSecurity));
                Assert.IsFalse(role01.IsBuiltIn);
                var svdRole01 = systemManager.GetRoleById(role01.RoleId);
                Assert.AreEqual(role01, svdRole01);
                svdRole01 = systemManager.GetRoleByName(role01.Name);
                Assert.AreEqual(role01, svdRole01);
                #endregion

                Assert.IsTrue(systemManager.GetRoles().Count == 5);

                #region Δεν μπορούμε να δημιουργήσουμε ρόλους με ονόματα που ήδη υπάρχουν
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole("ρόλος1"); }, typeof(VLException));
                #endregion

                //Update
                role01.Name = BuiltinRoles.Administrator.Name;
                _EXECUTEAndCATCHType(delegate { systemManager.CreateRole("ρόλος1"); }, typeof(VLException));

                role01.Name = "RoussoRole";
                role01.Description = "This is a Rousso Level 2 Role!";
                role01.Permissions = VLPermissions.ClientFullControl | VLPermissions.Developer | VLPermissions.ClientImportLists;
                role01 = systemManager.UpdateRole(role01);
                #region
                Assert.IsNotNull(role01);
                Assert.AreEqual(role01.Name, "RoussoRole");
                Assert.AreEqual(role01.Description, "This is a Rousso Level 2 Role!");
                Assert.IsTrue(role01.Permissions == (VLPermissions.ClientFullControl | VLPermissions.Developer | VLPermissions.ClientImportLists));
                Assert.IsFalse(role01.IsBuiltIn);
                svdRole01 = systemManager.GetRoleById(role01.RoleId);
                Assert.AreEqual(role01, svdRole01);
                svdRole01 = systemManager.GetRoleByName(role01.Name);
                Assert.AreEqual(role01, svdRole01);
                #endregion



                //Delete
                systemManager.DeleteRole(role01);
                Assert.IsNull(systemManager.GetRoleById(role01.RoleId));


                Assert.IsTrue(systemManager.GetRoles().Count == 4);
            }
            finally
            {
                var roles = systemManager.GetRoles();
                foreach (var item in roles)
                {
                    if (item.IsBuiltIn == false)
                        systemManager.DeleteRole(item.RoleId);
                }
            }
        }


        [TestMethod, Description("CheckPermissions & ValidatePermissions!")]
        public void RolesTest03()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                VLAccessToken _atoken = new VLAccessToken(admin);

                //Δημιουργούμε ένα νέο ρόλο:
                var role01 = systemManager.CreateRole("ρόλος1", permissions: VLPermissions.Developer | VLPermissions.ManageSecurity);
                Assert.IsNotNull(role01);
                _atoken.Permissions = role01.Permissions;

                var _manager = new TestManager(_atoken);
                Assert.IsFalse(_manager.HasPermissions(VLPermissions.ManageSystem));
                Assert.IsFalse(_manager.ValidatePermissions(VLPermissions.ManageSystem));
                _EXECUTEAndCATCHType(delegate { _manager.CheckPermissions(VLPermissions.ManageSystem); }, typeof(VLAccessDeniedException));


                Assert.IsFalse(_manager.HasPermissions(VLPermissions.ManageSystem|VLPermissions.Developer));
                Assert.IsFalse(_manager.ValidatePermissions(VLPermissions.ManageSystem | VLPermissions.Developer));
                _EXECUTEAndCATCHType(delegate { _manager.CheckPermissions(VLPermissions.ManageSystem | VLPermissions.Developer); }, typeof(VLAccessDeniedException));

                Assert.IsFalse(_manager.HasPermissions(VLPermissions.ManageSystem | VLPermissions.Developer | VLPermissions.ManageSecurity));
                Assert.IsFalse(_manager.ValidatePermissions(VLPermissions.ManageSystem | VLPermissions.Developer | VLPermissions.ManageSecurity));
                _EXECUTEAndCATCHType(delegate { _manager.CheckPermissions(VLPermissions.ManageSystem | VLPermissions.Developer | VLPermissions.ManageSecurity); }, typeof(VLAccessDeniedException));


                Assert.IsTrue(_manager.HasPermissions(VLPermissions.Developer | VLPermissions.ManageSecurity));
                Assert.IsTrue(_manager.ValidatePermissions(VLPermissions.Developer | VLPermissions.ManageSecurity));
                _EXECUTE_SUCCESS(delegate { _manager.ValidatePermissions(VLPermissions.Developer | VLPermissions.ManageSecurity); });


                Assert.IsTrue(_manager.ValidatePermissions(VLPermissions.ManageSystemParameters, VLPermissions.Developer | VLPermissions.ManageSecurity));
                _EXECUTE_SUCCESS(delegate { _manager.ValidatePermissions(VLPermissions.ManageSystemParameters, VLPermissions.Developer | VLPermissions.ManageSecurity); });

                Assert.IsTrue(_manager.ValidatePermissions(VLPermissions.ManageThemes, VLPermissions.ManageSystemParameters | VLPermissions.ManageSecurity, VLPermissions.Developer | VLPermissions.ManageSecurity));
                _EXECUTE_SUCCESS(delegate { _manager.ValidatePermissions(VLPermissions.ManageThemes, VLPermissions.ManageSystemParameters | VLPermissions.ManageSecurity, VLPermissions.Developer | VLPermissions.ManageSecurity); });




            }
            finally
            {
                var roles = systemManager.GetRoles();
                foreach (var item in roles)
                {
                    if (item.IsBuiltIn == false)
                        systemManager.DeleteRole(item.RoleId);
                }
            }
        }


    }
}
