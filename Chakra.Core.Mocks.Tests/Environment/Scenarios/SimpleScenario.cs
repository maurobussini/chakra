using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Mocks.Tests.Environment.Scenarios
{
    /// <summary>
    /// Implementation of simple scenario that implements ICharkaScenario
    /// </summary>
    public class SimpleScenario : IChakraScenario
    {
        /// <summary>
        /// Persons
        /// </summary>
        public IList<Person> Persons { get; set; } = new List<Person>();

        /// <summary>
        /// Departments
        /// </summary>
        public IList<Department> Departments { get; set; } = new List<Department>();

        /// <summary>
        /// Cars
        /// </summary>
        public IList<Car> Cars { get; set; } = new List<Car>();


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

            //Create few departments
            var car1 = new Car
            {
                Id = Guid.NewGuid().ToString("D"),
                Brand = "WV",
                Name = "Golf"
            };
            var car2 = new Car
            {
                Id = Guid.NewGuid().ToString("D"),
                Brand = "Fiat",
                Name = "Panda"
            };
            var car3 = new Car
            {
                Id = Guid.NewGuid().ToString("D"),
                Brand = "Alfa",
                Name = "Quadrifoglio"
            };

            //...and use "Push" extension method for push and create
            //auto-magically primary Id if they are missing
            this.Push(s => s.Cars, car1, car2, car3);
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
