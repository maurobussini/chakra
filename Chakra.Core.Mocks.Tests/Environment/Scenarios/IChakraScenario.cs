using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Mocks.Tests.Environment.Scenarios
{
    /// <summary>
    /// Interface for sample mock scenario (for a better
    /// application design use the convention:
    /// => "I" + [application-codename] + "Scenario"
    /// => "ICharkaScenario"
    /// </summary>
    public interface IChakraScenario: IScenario
    {
        /// <summary>
        /// Persons
        /// </summary>
        IList<Person> Persons { get; set; }

        /// <summary>
        /// Departments
        /// </summary>
        IList<Department> Departments { get; set; }
    }
}
