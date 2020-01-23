using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks
{
    [Repository]
    public class MockPersonRepository: MockRepositoryBase<Person, IChakraScenario>, IPersonRepository
    {
        public MockPersonRepository(IDataSession dataSession) 
            : base(dataSession, sc => sc.Persons) { }
    }
}
