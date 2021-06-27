using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Repositories;
using Xunit;
using ZenProgramming.Chakra.Core.Async.Data;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Async.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;

namespace ZenProgramming.Chakra.Core.EntityFramework.Tests
{
    public class EntityFrameworkRepositoryTests
    {
        [Fact]
        public void VerifyThatRepositoryCanBeCreated()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSession<ChakraDbContext>>();
            var DataSession = SessionFactory.OpenSession();

            var repository = DataSession.ResolveRepository<IPersonRepository>();
            Assert.True(repository is EfPersonRepository);
        }

        [Fact]
        public void VerifyThatRepositoryCanBeCreatedAsync()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSession<ChakraDbContext>>();
            var DataSession = SessionFactory.OpenSession();

            var repository = DataSession.ResolveRepository<ICarRepository>();
            Assert.True(repository is EfCarRepository);
        }

        [Fact]
        public async Task VerifyThatTransactionCanBeCommittedAsync()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSessionAsync<ChakraDbContext>>();
            var DataSession = SessionFactory.OpenSession();

            var repository = DataSession.ResolveRepository<ICarRepository>();
            Assert.True(repository is EfCarRepository);
            var transaction = DataSession.BeginTransaction();
            await transaction.CommitAsync();
        }

        [Fact]
        public async Task VerifyThatTransactionCanBeRollbackAsync()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSessionAsync<ChakraDbContext>>();
            var DataSession = SessionFactory.OpenSession();

            var repository = DataSession.ResolveRepository<ICarRepository>();
            Assert.True(repository is EfCarRepository);
            var transaction = DataSession.BeginTransaction();
            await transaction.RollBackAsync();
        }
    }
}
