using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Contexts;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Repositories.EntityFramework
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
