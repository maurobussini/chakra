using Xunit;
using ZenProgramming.Chakra.Core.Persistences;
using ZenProgramming.Chakra.Core.Tests.Environment.Persistences;

namespace ZenProgramming.Chakra.Core.Tests
{
    public class PersistenceTests
    {
        [Fact]
        public void ShouldInitializePersistence()
        {
            var credentials = PersistenceInitializerFactory.Fetch<Credential>();
            Assert.True(credentials?.Count > 0);
        }
    }
}
