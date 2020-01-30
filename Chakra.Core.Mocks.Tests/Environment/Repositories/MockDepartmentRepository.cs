using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Repositories.Mocks
{
    [Repository]
    public class MockDepartmentRepository: MockRepositoryBase<Department, IChakraScenario>, IDepartmentRepository
    {
        public MockDepartmentRepository(IDataSession dataSession) 
            : base(dataSession, sc => sc.Departments) { }
    }
}
