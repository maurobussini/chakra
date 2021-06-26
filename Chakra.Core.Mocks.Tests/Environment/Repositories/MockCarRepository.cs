using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Mocks.Async.Data.Repositories;
using ZenProgramming.Chakra.Core.Mocks.Tests.Environment.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;

namespace ZenProgramming.Chakra.Core.Mocks.Tests.Environment.Repositories
{
    /// <summary>
    /// Repository for Person on mock engine
    /// </summary>
    [Repository]
    public class MockCarRepository: MockRepositoryBaseAsync<Car, IChakraScenario>, ICarRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        public MockCarRepository(IDataSessionAsync dataSession) 
            : base(dataSession, sc => sc.Cars) { }
    }
}