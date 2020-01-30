using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;

namespace ZenProgramming.Chakra.Core.Mocks.Tests
{
    public class MockRepositoryTests
    {

        [Fact]
        public void VerifyThatRepositoryCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<MockDataSession<
                IChakraScenario, ScopedScenarioOption<SimpleScenario>>>();
            var DataSession = SessionFactory.OpenSession();

            var repository = DataSession.ResolveRepository<IPersonRepository>();
            Assert.True(repository is MockPersonRepository);
        }        
    }
}
