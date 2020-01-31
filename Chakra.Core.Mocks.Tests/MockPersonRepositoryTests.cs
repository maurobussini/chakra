using System;
using System.Linq;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;
using ZenProgramming.Chakra.Core.Mocks.Tests.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;

namespace Chakra.Core.Mocks.Tests
{
    public class MockPersonRepositoryTests : IDisposable
    {
        private IDataSession _DataSession;

        public MockPersonRepositoryTests()
        {
            //Register default session and open
            _DataSession = SessionFactory.OpenSession<MockDataSession<
                SimpleScenario,
                TransientScenarioOption<SimpleScenario>>>();
        }

        [Fact]
        public void ShouldCountAtLeastOneElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<IPersonRepository>();

            //Execute operation
            var result = repository.Count();

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void ShouldFetchAtLeastOneElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<IPersonRepository>();

            //Execute operation
            var result = repository.Fetch();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void ShouldCreateIncrementElements()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<IPersonRepository>();

            //Get number of elements before create
            var countBefore = _DataSession.GetScenario<IChakraScenario>().Persons.Count;

            //Define new element
            var element = new Person 
            {
                Name = "Jane", 
                Surname = "Doe"
            };

            //Execute operation
            repository.Save(element);

            //Get number of elements after create
            var countAfter = _DataSession.GetScenario<IChakraScenario>().Persons.Count;

            //Assert
            Assert.Equal(countBefore + 1, countAfter);
        }

        [Fact]
        public void ShouldUpdateSaveChangedElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<IPersonRepository>();

            //Get existing element
            var all = repository.Fetch();
            var existing = all.First();

            //Change name of existing element
            var existingId = existing.Id;
            existing.Name = "Jack";

            //Execute operation of update (using save)
            repository.Save(existing);

            //Get element from scenario
            var fromScenario = _DataSession.GetScenario<IChakraScenario>()
                .Persons.SingleOrDefault(e => e.Id == existingId);

            //Assert
            Assert.Equal(existing.Name, fromScenario.Name);
        }

        [Fact]
        public void ShouldDeleteDecrementElements()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<IPersonRepository>();

            //Get number of elements before create
            var countBefore = _DataSession.GetScenario<IChakraScenario>().Persons.Count;

            //Get existing element
            var all = repository.Fetch();
            var existing = all.First();

            //Execute operation
            repository.Delete(existing);

            //Get number of elements after create
            var countAfter = _DataSession.GetScenario<IChakraScenario>().Persons.Count;

            //Assert
            Assert.Equal(countBefore - 1, countAfter);
        }

        public void Dispose()
        {
            //Release datasession and scenario
            _DataSession.Dispose();
        }
    }
}
