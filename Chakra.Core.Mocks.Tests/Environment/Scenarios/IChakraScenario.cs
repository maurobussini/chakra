using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios
{
    public interface IChakraScenario: IScenario
    {
        IList<Person> Persons { get; set; }
    }
}
