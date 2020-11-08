using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AdminBaseClass : SimpleBaseClass
    {
        protected static ValisSystem valisSystem;
        protected static VLAccessToken sysadmin;
        protected static VLAccessToken dev;
        protected static VLAccessToken admin;

        protected const string _ImportFilesPath = "TestFiles\\ImportFiles";
        protected const string _ImagesPath = "TestFiles\\Images";

        public static void _initialize(TestContext context)
        {
            System.Diagnostics.Debug.WriteLine("AdminBaseClass::_initialize");
            //Δημιουργούμε ένα καινούργιο ValisSystem
            valisSystem = new ValisSystem();

            //τώρα κάνουμε login στο σύστημά μας
            sysadmin = valisSystem.LogOnUser("sysadmin", "tolk!3n");
            Assert.IsNotNull(sysadmin);

            dev = valisSystem.LogOnUser("developer", "tolk!3n");
            Assert.IsNotNull(dev);

            admin = valisSystem.LogOnUser("admin", "tolk!3n");
            Assert.IsNotNull(admin);
        }


        public static void _cleanup()
        {
            System.Diagnostics.Debug.WriteLine("AdminBaseClass::_cleanup");
            if (sysadmin != null)
            {
                valisSystem.LogOffUser(sysadmin);
                sysadmin = null;
            }
            if (dev != null)
            {
                valisSystem.LogOffUser(dev);
                dev = null;
            }
            if(admin != null)
            {
                valisSystem.LogOffUser(admin);
                admin = null;
            }
        }


    }
}
