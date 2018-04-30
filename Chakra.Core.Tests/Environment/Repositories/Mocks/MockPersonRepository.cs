using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Data.Repositories.Mockups;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;
using ZenProgramming.Chakra.Core.Tests.Environment.Scenarios;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks
{
    [Repository]
    public class MockPersonRepository: MockupRepositoryBase<Person, IChakraScenario>, IPersonRepository
    {
        public MockPersonRepository(IDataSession dataSession) 
            : base(dataSession, sc => sc.Persons) { }
    }
}
