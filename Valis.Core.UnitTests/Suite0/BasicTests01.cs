using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite0
{
    [TestClass]
    public class BasicTests01 : SimpleBaseClass
    {


        [TestMethod, Description("Check the valisSystem's section name")]
        public void ConnectivityTest01()
        {
            //we check the ValisSystem's static properties:
            Assert.AreEqual<string>(ValisSystem.SectionName, "valisSystem");
            Assert.IsNotNull(ValisSystem.Settings);
        }



        [TestMethod, Description("Check the builtin accounts")]
        public void ConnectivityTest02()
        {
            ValisSystem vsystem = new ValisSystem();

            Assert.IsNull(vsystem.LogOnUser(null, null));
            Assert.IsNull(vsystem.LogOnUser("sysadmin", string.Empty));
            Assert.IsNull(vsystem.LogOnUser(string.Empty, "tolk!3n"));
            Assert.IsNull(vsystem.LogOnUser("qeqeqeqwe", "qweqqe"));

            //WE CHECK THE builtin sysadmin ACCOUNT:
            var sysAdmin = vsystem.LogOnUser("sysadmin", "tolk!3n");
            Assert.IsNotNull(sysAdmin);
            Assert.IsTrue(sysAdmin.Principal == 2);
            Assert.IsTrue(sysAdmin.PrincipalType == PrincipalType.SystemUser);
            Assert.IsTrue(sysAdmin.IsBuiltIn);
            Assert.IsTrue(sysAdmin.DefaultLanguage == BuiltinLanguages.Invariant.LanguageId);
            //ValidateAccessToken
            var sysAdmin2 = vsystem.ValidateAccessToken(sysAdmin.AccessTokenId);
            Assert.IsNotNull(sysAdmin2);
            Assert.AreEqual<VLAccessToken>(sysAdmin, sysAdmin2);

            vsystem.LogOffUser(sysAdmin);
            sysAdmin2 = vsystem.ValidateAccessToken(sysAdmin.AccessTokenId);
            Assert.IsNull(sysAdmin2);


            //WE CHECK THE builtin developer ACCOUNT:
            var dev = vsystem.LogOnUser("developer", "tolk!3n");
            Assert.IsNotNull(dev);
            Assert.IsTrue(dev.Principal == 3);
            Assert.IsTrue(dev.IsBuiltIn);
            Assert.IsTrue(dev.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
            //ValidateAccessToken
            var dev2 = vsystem.ValidateAccessToken(dev.AccessTokenId);
            Assert.IsNotNull(dev2);
            Assert.AreEqual<VLAccessToken>(dev, dev2);

            vsystem.LogOffUser(dev);
            dev2 = vsystem.ValidateAccessToken(dev.AccessTokenId);
            Assert.IsNull(dev2);


            //WE CHECK the builtin administrator ACCOUNT:
            var admin = vsystem.LogOnUser("admin", "tolk!3n");
            Assert.IsNotNull(admin);
            Assert.IsTrue(admin.Principal == 4);
            Assert.IsTrue(admin.IsBuiltIn);
            Assert.IsTrue(admin.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
            //ValidateAccessToken
            var admin2 = vsystem.ValidateAccessToken(admin.AccessTokenId);
            Assert.IsNotNull(admin2);
            Assert.AreEqual<VLAccessToken>(admin, admin2);

            vsystem.LogOffUser(admin);
            admin2 = vsystem.ValidateAccessToken(admin.AccessTokenId);
            Assert.IsNull(admin2);

        }



    }
}
