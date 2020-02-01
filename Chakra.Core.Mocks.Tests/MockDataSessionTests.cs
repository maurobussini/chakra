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
        public void ShouldMockDataSessionBeOpenedWithRegisterDefaultUsingImplicitScopedMode()
        {
            //Register data session using "implicit mode" (scoped)
            SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario>>();

            //Open session
            IDataSession session = SessionFactory.OpenSession();

            //Check that session is created and is valid
            Assert.NotNull(session);
            Assert.True(session is IMockDataSession);
        }

        [Fact]
        public void ShouldMockDataSessionBeOpenedWithRegisterDefaultUsingExplicitScopedMode()
        {
            //Register data session using "implicit mode" (scoped)
            SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario, ScopedScenarioOption<SimpleScenario>>>();

            //Open session
            IDataSession session = SessionFactory.OpenSession();

            //Check that session is created and is valid
            Assert.NotNull(session);
            Assert.True(session is IMockDataSession);
        }

        [Fact]
        public void ShouldMockDataSessionBeOpenedDirectlyUsingImplicitScopedMode()
        {
            //Open session directly
            IDataSession session = SessionFactory.OpenSession<MockDataSession<SimpleScenario>>();

            //Check that session is created and is valid
            Assert.NotNull(session);
            Assert.True(session is IMockDataSession);
        }

        [Fact]
        public void ShouldMockDataSessionBeOpenedDirectlyUsingExplicitScopedMode() 
        {
            //Open session directly
            IDataSession session = SessionFactory.OpenSession<MockDataSession<SimpleScenario, ScopedScenarioOption<SimpleScenario>>>();

            //Check that session is created and is valid
            Assert.NotNull(session);
            Assert.True(session is IMockDataSession);
        }
    }
}
