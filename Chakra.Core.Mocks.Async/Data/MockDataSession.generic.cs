using System;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Mocks.Async.Data;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    /// <summary>
    /// Generic implementation of data session on "Mock" engine with injectable scenario option
    /// </summary>
    /// <typeparam name="TScenarioImplementation">Type of scenario instance (ex: SimpleScenario)</typeparam>
    /// <typeparam name="TScenarioOption">Type of scenario option (ex: TransientScenarioOption[SimpleScenario])</typeparam>
    public class MockDataSessionAsync<TScenarioImplementation, TScenarioOption> : MockDataSession<TScenarioImplementation, TScenarioOption>
        where TScenarioImplementation : class, IScenario, new()
        where TScenarioOption : class, IScenarioOption<TScenarioImplementation>, new()
    {
        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public override IDataTransaction BeginTransaction()
        {
            //Cast to interface, then generate data transaction
            var castedDataSession = this as IMockDataSession;
            return new MockDataTransactionAsync(castedDataSession);
        }
 
        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MockDataSessionAsync()
        {
            //Richiamo i dispose implicito
            Dispose(false);
        }     
    }
}
