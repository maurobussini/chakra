using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Repositories;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using ZenProgramming.Chakra.Core.Tests;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

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

        //[Fact]
        //public void VerifyThatModernEntityBaseGeneratesAutomaticallyIdentifierOnCreate() 
        //{
        //    SessionFactory.RegisterDefaultDataSession<EntityFrameworkDataSession<ChakraDbContext>>();
        //    var DataSession = SessionFactory.OpenSession();

        //    var repository = DataSession.ResolveRepository<IDepartmentRepository>();

        //    //Creation of new entity with data but no Identifier
        //    Department entity = new Department
        //    {
        //        Code = "001",
        //        Name = "Department 001"
        //    };

        //    //Executes in transation
        //    using (var t = DataSession.BeginTransaction()) 
        //    {
        //        //Save of entity
        //        repository.Save(entity);

        //        //Commit on transation
        //        t.Commit();
        //    }

        //    //Identifier should be generate
        //    Assert.True(!string.IsNullOrEmpty(entity.Id));
        //}
    }
}
