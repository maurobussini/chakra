using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;

namespace ZenProgramming.Chakra.Core.EntityFramework.Tests
{
    public class DataSessionTests
    {
        [Fact]
        public void VerifyThatEntityFrameworkDataSessionCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSession<ChakraDbContext>>();
            IDataSession session = SessionFactory.OpenSession();
            Assert.True(session is EntityFrameworkDataSession<ChakraDbContext>);
        }
    }
}
