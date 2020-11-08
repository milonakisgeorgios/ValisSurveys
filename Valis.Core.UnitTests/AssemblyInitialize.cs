using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests
{
    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            AdminBaseClass._initialize(context);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            AdminBaseClass._cleanup();
        }
    }
}
