using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Repositories;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;

namespace ZenProgramming.Chakra.Core.Tests
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
    }
}
