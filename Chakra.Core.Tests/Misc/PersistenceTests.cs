using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZenProgramming.Chakra.Core.Persistences;
using ZenProgramming.Chakra.Core.Tests.Environment.Persistences;

namespace ZenProgramming.Chakra.Core.Tests.Misc
{
    [TestClass]
    public class PersistenceTests
    {
        [TestMethod]
        public void ShouldInitializePersistence()
        {
            var credentials = PersistenceInitializerFactory.Fetch<Credential>();
            Assert.IsTrue(credentials != null && credentials.Count > 0);
        }
    }
}
