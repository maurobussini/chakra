using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;
using Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Repositories
{
    [Repository]
    public class EfDepartmentRepository : EntityFrameworkRepositoryBase<Department, ChakraDbContext>, IDepartmentRepository
    {
        public EfDepartmentRepository(IDataSession dataSession) 
            : base(dataSession, dbc => dbc.Departments) { }
    }
}
