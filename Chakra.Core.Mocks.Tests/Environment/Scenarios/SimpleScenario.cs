using Chakra.Core.Tests.Environment.Entities;
using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios
{
    /// <summary>
    /// Implementation of simple scenario that implements ICharkaScenario
    /// </summary>
    public class SimpleScenario: IChakraScenario
    {
        /// <summary>
        /// Persons
        /// </summary>
        public IList<Person> Persons { get; set; }

        /// <summary>
        /// Departments
        /// </summary>
        public IList<Department> Departments { get; set; }

        /// <summary>
        /// Constructor (should be parameterless)
        /// </summary>
        public SimpleScenario()
        {
            //Initialize lists
            Persons = new List<Person>();
            Departments = new List<Department>();
        }

        /// <summary>
        /// Executes initialization and loading 
        /// of entities on scenario
        /// </summary>
        public void InitializeEntities()
        {
            //Create a person and add to list
            var john = new Person
            {
                Id = Persons.Count + 1, 
                IsMale = true, 
                Name = "John", 
                Surname = "Doe"
            };
            Persons.Add(john);

            //Create few departments
            var fin = new Department
            {
                Id = Guid.NewGuid().ToString("D"),
                Code = "FIN",
                Name = "Finance"
            };
            var rd = new Department
            {
                Id = Guid.NewGuid().ToString("D"),
                Code = "R&D",
                Name = "Research and Development"
            };
            var hr = new Department
            {
                Id = Guid.NewGuid().ToString("D"),
                Code = "HR",
                Name = "Human Resources"
            };

            //...and use "Push" extension method for push and create
            //auto-magically primary Id if they are missing
            this.Push(s => s.Departments, fin, rd, hr);
        }

        /// <summary>
        /// Executes initialization of assets 
        /// (files, folders, configurations) on scenario
        /// </summary>
        public void InitializeAssets()
        {            
            //None in this case
        }
    }
}
