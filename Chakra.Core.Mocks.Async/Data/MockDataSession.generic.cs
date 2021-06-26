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
   
        #region Public properties

        /// <summary>
        /// Active transaction on session
        /// </summary>
        public IDataTransactionAsync TransactionAsync { get; private set; }

        
        #endregion

       
        /// <summary>
        /// Execute resolution of repository interface using specified clas
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <returns>Returns repository instance</returns>
        public TRepositoryInterface ResolveRepositoryAsync<TRepositoryInterface>()
            where TRepositoryInterface : IRepositoryAsync
        {
            //Utilizzo il metodo presente sull'helper
            return Core.Async.Data.Repositories.Helpers.RepositoryHelper.Resolve<TRepositoryInterface, IMockRepositoryAsync>(this);
        }

        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public IDataTransactionAsync BeginTransactionAsync()
        {
            //Cast to interface, then generate data transaction
            var castedDataSession = this as IMockDataSessionAsync;
            return new MockDataTransactionAsync(castedDataSession);
        }
        
        
        /// <summary>
        /// Set active transaction on current data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        public void SetActiveTransactionAsync(IDataTransactionAsync dataTransaction)
        {
            //Set transaction
            TransactionAsync = dataTransaction ?? throw new ArgumentNullException(nameof(dataTransaction));
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
