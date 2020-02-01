using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks
{
    /// <summary>
    /// Repository for Person on mock engine
    /// </summary>
    [Repository]
    public class MockPersonRepository: MockRepositoryBase<Person, IChakraScenario>, IPersonRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        public MockPersonRepository(IDataSession dataSession) 
            : base(dataSession, sc => sc.Persons) { }
    }
}
