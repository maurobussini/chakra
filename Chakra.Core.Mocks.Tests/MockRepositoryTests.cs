using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks;

namespace ZenProgramming.Chakra.Core.Mocks.Tests
{
    public class MockRepositoryTests
    {

        [Fact]
        public void VerifyThatRepositoryCanBeCreated()
        {
            var scenario = new SimpleScenario();
            ScenarioFactory.Initialize(scenario);
            SessionFactory.RegisterDefaultDataSession<MockDataSession>();
            var DataSession = SessionFactory.OpenSession();

            var repository = DataSession.ResolveRepository<IPersonRepository>();
            Assert.True(repository is MockPersonRepository);
        }
    }
}
