using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios
{
    public class SimpleScenario: IChakraScenario
    {
        public IList<Person> Persons { get; set; }

        public SimpleScenario()
        {
            Persons = new List<Person>();
        }

        public void InitializeEntities()
        {
            Persons.Add(new Person
            {
                Id = Persons.Count + 1, 
                IsMale = true, 
                Name = "Mauro", 
                Surname = "Bussini"
            });
        }

        public void InitializeAssets()
        {            
        }
    }
}
