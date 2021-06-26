using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Async.Data;
using ZenProgramming.Chakra.Core.Mocks.Async.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;
using ZenProgramming.Chakra.Core.Mocks.Tests.Environment.Scenarios;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;
using ZenProgramming.Chakra.Core.Tests.Environment.Repositories;

namespace ZenProgramming.Chakra.Core.Mocks.Tests
{
    public class MockCarRepositoryTests : IDisposable
    {
        private IDataSessionAsync _DataSession { get; set; }

        public MockCarRepositoryTests() 
        {
            //Register default session and open
            _DataSession = SessionFactoryAsync.OpenSession<MockDataSessionAsync<
                SimpleScenario,
                TransientScenarioOption<SimpleScenario>>>();
        }

        [Fact]
        public async Task ShouldCountAtLeastOneElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();

            //Execute operation
            var result = await repository.CountAsync();

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void ShouldFetchAtLeastOneElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();

            //Execute operation
            var result = repository.Fetch();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task ShouldFetchAtLeastOneElementAsync()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();

            //Execute operation
            var result = await repository.FetchAsync();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }



        [Fact]
        public async Task ShouldFetchWithProjectionReturnsAtLeastOneElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();

            //Execute operation
            var result = await repository.FetchWithProjectionAsync(x=>new{x.Name});

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
        
        [Fact]
        public void ShouldCreateIncrementElements()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();
            
            //Get number of elements before create
            var countBefore = _DataSession.GetScenario<IChakraScenario>().Cars.Count;

            //Define new element
            var element = new Car 
            {
                Brand = "Pagani", 
                Name = "Zonda"
            };

            //Execute operation
            repository.Save(element);

            //Get number of elements after create
            var countAfter = _DataSession.GetScenario<IChakraScenario>().Cars.Count;

            //Assert
            Assert.Equal(countBefore + 1, countAfter);
        }


        [Fact]
        public async Task ShouldCreateTransactionAndCommitAsync()
        {
            
            using var transaction = _DataSession.BeginTransaction();

            await transaction.CommitAsync();
        }

        [Fact]
        public async Task ShouldCreateIncrementElementsAsync()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();
            
            //Get number of elements before create
            var countBefore = _DataSession.GetScenario<IChakraScenario>().Cars.Count;

            //Define new element
            var element = new Car 
            {
                Brand = "Pagani", 
                Name = "Zonda"
            };

            //Execute operation
            await repository.SaveAsync(element);

            //Get number of elements after create
            var countAfter = _DataSession.GetScenario<IChakraScenario>().Cars.Count;

            //Assert
            Assert.Equal(countBefore + 1, countAfter);
        }

        [Fact]
        public async Task ShouldUpdateSaveChangedElement()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();

            //Get existing element
            var all = await repository.FetchAsync();
            var existing = all.First();

            //Change name of existing element
            var existingId = existing.Id;
            existing.Name = existing.Name + " updated!";

            //Execute operation of update (using save)
            await repository.SaveAsync(existing);

            //Get element from scenario
            var fromScenario = _DataSession.GetScenario<IChakraScenario>()
                .Cars.SingleOrDefault(e => e.Id == existingId);

            //Assert
            Assert.Equal(existing.Name, fromScenario.Name);
        }
        
        [Fact]
        public async Task ShouldDeleteDecrementElements()
        {
            //Resolve repo
            var repository = _DataSession.ResolveRepository<ICarRepository>();

            //Get number of elements before create
            var countBefore = _DataSession.GetScenario<IChakraScenario>().Cars.Count;

            //Get existing element
            var all = await repository.FetchAsync();
            var existing = all.First();

            //Execute operation
            await repository.DeleteAsync(existing);

            //Get number of elements after create
            var countAfter = _DataSession.GetScenario<IChakraScenario>().Cars.Count;

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