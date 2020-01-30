using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;
using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;

namespace ZenProgramming.Chakra.Core.Tests
{
    public class MockDataSessionTests
    {
        [Fact]
        public void VerifyThatMockDataSessionCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario>>();
            IDataSession session = SessionFactory.OpenSession();
            Assert.True(session is MockDataSession<SimpleScenario>);
        }

        [Fact]
        public void ShouldMockDataSessionBeOpenedUsingRegisteredDefault()
        {
            SessionFactory.RegisterDefaultDataSession<MockDataSession<IChakraScenario, ScopedScenarioOption<SimpleScenario>>>();
            IDataSession session = SessionFactory.OpenSession();
            Assert.NotNull(session);
        }


        [Fact]
        public void ShouldMockDataSessionBeCreatedUsingExplicitOnOpen()
        {
            IDataSession session = SessionFactory.OpenSession<MockDataSession<IChakraScenario, ScopedScenarioOption<SimpleScenario>>>();
            Assert.NotNull(session);
        }

        [Fact]
        public void ShouldMockDataSessionWithScopedScenarioBeCreated() 
        {
            
        }
    }
}
