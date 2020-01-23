using ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;

namespace ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Repositories
{
    [Repository]
    public class EfPersonRepository : EntityFrameworkRepositoryBase<Person, ChakraDbContext>, IPersonRepository
    {
        public EfPersonRepository(IDataSession dataSession) 
            : base(dataSession, dbc => dbc.Persons)
        {

            //DataSession.Context
        }

    }
}
