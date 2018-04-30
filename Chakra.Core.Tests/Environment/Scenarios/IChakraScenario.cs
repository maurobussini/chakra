using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Scenarios
{
    public interface IChakraScenario: IScenario
    {
        IList<Person> Persons { get; set; }
    }
}
