using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.EntityFramework.Async.Data.Repositories;
using ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Repositories
{
    [Repository]
    public class EfDepartmentRepository : EntityFrameworkRepositoryBase<Department, ChakraDbContext>, IDepartmentRepository
    {
        public EfDepartmentRepository(IDataSession dataSession) 
            : base(dataSession, dbc => dbc.Departments) { }
    }

    [Repository]
    public class EfCarRepository : EntityFrameworkRepositoryBaseAsync<Car, ChakraDbContext>, ICarRepository
    {
        public EfCarRepository(IDataSession dataSession) 
            : base(dataSession, dbc => dbc.Cars) { }
    }
}
