using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks
{
    /// <summary>
    /// Repository for Department on mock engine
    /// </summary>
    [Repository]
    public class MockDepartmentRepository: MockRepositoryBase<Department, IChakraScenario>, IDepartmentRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        public MockDepartmentRepository(IDataSession dataSession) 
            : base(dataSession, sc => sc.Departments) { }
    }
}
