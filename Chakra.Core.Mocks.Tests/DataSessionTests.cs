using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;

namespace ZenProgramming.Chakra.Core.Tests
{
    public class DataSessionTests
    {
        [Fact]
        public void VerifyThatMockDataSessionCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<MockDataSession>();
            IDataSession session = SessionFactory.OpenSession();
            Assert.True(session is MockDataSession);
        }

    }
}
