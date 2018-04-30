using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;
using ZenProgramming.Chakra.Core.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories.EntityFramework;

namespace ZenProgramming.Chakra.Core.Tests
{
    [TestClass]
    public class EntityFrameworkRepositoryTests
    {
        private IDataSession DataSession { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSession<ChakraDbContext>>();
            DataSession = SessionFactory.OpenSession();
        }

        [TestMethod]
        public void VerifyThatRepositoryCanBeCreated()
        {
            var repository = DataSession.ResolveRepository<IPersonRepository>();
            Assert.IsTrue(repository is EfPersonRepository);
        }

        [TestMethod]
        public void VerifyThatRepositoryFetchAllPersons()
        {
            var repository = DataSession.ResolveRepository<IPersonRepository>();
            var data = repository.Fetch();
            Assert.IsTrue(data != null && data.Count > 0);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DataSession.Dispose();
        }
    }
}
