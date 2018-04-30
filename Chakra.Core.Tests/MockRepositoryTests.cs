using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks;
using ZenProgramming.Chakra.Core.Tests.Environment.Scenarios;

namespace ZenProgramming.Chakra.Core.Tests
{
    [TestClass]
    public class MockRepositoryTests
    {
        private IScenario Scenario { get; set; }
        private IDataSession DataSession { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Scenario = new SimpleScenario();
            ScenarioFactory.Initialize(Scenario);
            SessionFactory.RegisterDefaultDataSession<MockupDataSession>();
            DataSession = SessionFactory.OpenSession();
        }

        [TestMethod]
        public void VerifyThatRepositoryCanBeCreated()
        {
            var repository = DataSession.ResolveRepository<IPersonRepository>();
            Assert.IsTrue(repository is MockPersonRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DataSession.Dispose();
        }
    }
}
