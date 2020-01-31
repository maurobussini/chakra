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
        public void ShouldCreateRepositoryWithRegisterDefaultDataSessionAsTransient()
        {
            //Register as transient
            SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario, TransientScenarioOption<SimpleScenario>>>();

            //Open session and create repository
            var dataSession = SessionFactory.OpenSession();
            var repository = dataSession.ResolveRepository<IPersonRepository>();

            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is MockPersonRepository);

            //Clean
            repository.Dispose();
            dataSession.Dispose();
        }

        [Fact]
        public void ShouldCreateRepositoryWithRegisterDefaultDataSessionAsScoped()
        {
            //Register as transient
            SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario, ScopedScenarioOption<SimpleScenario>>>();

            //Open session and create repository
            var dataSession = SessionFactory.OpenSession();
            var repository = dataSession.ResolveRepository<IPersonRepository>();


            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is MockPersonRepository);

            //Clean
            repository.Dispose();
            dataSession.Dispose();
        }

        [Fact]
        public void ShouldCreateRepositoryWithRegisterDefaultDataSessionAsImplicit()
        {
            //Register as transient
            SessionFactory.RegisterDefaultDataSession<MockDataSession<SimpleScenario>>();

            //Open session and create repository
            var dataSession = SessionFactory.OpenSession();
            var repository = dataSession.ResolveRepository<IPersonRepository>();

            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is MockPersonRepository);

            //Clean
            repository.Dispose();
            dataSession.Dispose();
        }

        [Fact]
        public void ShouldCreateRepositoryWithDirectOpenAsTransient()
        {
            //Open session and create repository
            var dataSession = SessionFactory.OpenSession<MockDataSession<SimpleScenario, TransientScenarioOption<SimpleScenario>>>();
            var repository = dataSession.ResolveRepository<IPersonRepository>();

            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is MockPersonRepository);

            //Clean
            repository.Dispose();
            dataSession.Dispose();
        }

        [Fact]
        public void ShouldCreateRepositoryWithDirectOpenAsScoped()
        {
            //Open session and create repository
            var dataSession = SessionFactory.OpenSession<MockDataSession<SimpleScenario, ScopedScenarioOption<SimpleScenario>>>();
            var repository = dataSession.ResolveRepository<IPersonRepository>();


            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is MockPersonRepository);

            //Clean
            repository.Dispose();
            dataSession.Dispose();
        }

        [Fact]
        public void ShouldCreateRepositoryWithDirectOpenAsImplicit()
        {
            //Open session and create repository
            var dataSession = SessionFactory.OpenSession<MockDataSession<SimpleScenario>>();
            var repository = dataSession.ResolveRepository<IPersonRepository>();


            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is MockPersonRepository);

            //Clean
            repository.Dispose();
            dataSession.Dispose();
        }
    }
}
