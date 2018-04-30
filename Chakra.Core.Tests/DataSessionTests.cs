using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.EntityFramework.Data;
using ZenProgramming.Chakra.Core.Tests.Environment.Contexts;

namespace ZenProgramming.Chakra.Core.Tests
{
    [TestClass]
    public class DataSessionTests
    {
        [TestMethod]
        public void VerifyThatMockDataSessionCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<MockupDataSession>();
            IDataSession session = SessionFactory.OpenSession();
            Assert.IsTrue(session is MockupDataSession);
        }

        [TestMethod]
        public void VerifyThatEntityFrameworkDataSessionCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSession<ChakraDbContext>>();
            IDataSession session = SessionFactory.OpenSession();
            Assert.IsTrue(session is EntityFrameworkDataSession<ChakraDbContext>);
        }
    }
}
