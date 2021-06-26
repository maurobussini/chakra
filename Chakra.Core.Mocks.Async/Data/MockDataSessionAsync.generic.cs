using System;
using ZenProgramming.Chakra.Core.Async.Data.Repositories;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    /// <summary>
    /// Generic implementation of data session on "Mock" engine with injectable scenario option
    /// </summary>
    /// <typeparam name="TScenarioImplementation">Type of scenario instance (ex: SimpleScenario)</typeparam>
    /// <typeparam name="TScenarioOption">Type of scenario option (ex: TransientScenarioOption[SimpleScenario])</typeparam>
    public class MockDataSessionAsync<TScenarioImplementation, TScenarioOption> : MockDataSession<TScenarioImplementation, TScenarioOption>, IMockDataSessionAsync
        where TScenarioImplementation : class, IScenario, new()
        where TScenarioOption : class, IScenarioOption<TScenarioImplementation>, new()
    {
   
       
        public override IDataTransaction BeginTransaction()
        {
            //Cast to interface, then generate data transaction
            
            if (this is IMockDataSessionAsync castedDataSession)
            {
                return new MockDataTransactionAsync(castedDataSession);
            }
            throw new InvalidCastException($"Unable to cast type of '{this.GetType().FullName}' to " +
                                           $"interface '{typeof(IMockDataSessionAsync).FullName}'.");
        }

        /// <summary>
        /// Set active transaction on current data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        public override void SetActiveTransaction(IDataTransaction dataTransaction)
        {
            // cast
            if (dataTransaction is IDataTransactionAsync dt)
            {
                Transaction = dt;
                return;
            }
            throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
                                           $"interface '{typeof(IDataTransactionAsync).FullName}'.");
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
