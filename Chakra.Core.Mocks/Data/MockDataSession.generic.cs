using System;
using System.Collections.Generic;
using System.Text;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    public class MockDataSession<TScenario, TScenarioOption> : IMockDataSession<TScenario>
        where TScenario : IScenario
        where TScenarioOption : class, IScenarioOption, new()
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion        

        /// <summary>
        /// Execute resolution of repository interface using specified clas
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <returns>Returns repository instance</returns>
        public TRepositoryInterface ResolveRepository<TRepositoryInterface>()
            where TRepositoryInterface : IRepository
        {
            //Utilizzo il metodo presente sull'helper
            return RepositoryHelper.Resolve<TRepositoryInterface, IMockRepository>(this);
        }

        /// <summary>
        /// Active transaction on session
        /// </summary>
        public IDataTransaction Transaction { get; private set; }

        private TScenarioOption _ScenarioOption;

        public TScenarioOption ScenarioOption
        {
            get
            {
                if (_ScenarioOption == null)
                {
                    _ScenarioOption = new TScenarioOption();
                }
                return _ScenarioOption;
            }
        }

        /// <summary>
        /// Active scenario
        /// </summary>
        public TScenario Scenario
        {
            get
            {
                return (TScenario)ScenarioOption.GetInstance();
            }
        }

        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public IDataTransaction BeginTransaction()
        {
            //Cast to interface, then generate data transaction
            var castedDataSession = this as IMockDataSession;
            return new MockDataTransaction(castedDataSession);
        }

        /// <summary>
        /// Executes convert of session instance on specified type
        /// </summary>
        /// <typeparam name="TOutput">Target type</typeparam>
        /// <returns>Returns converted instance</returns>
        public TOutput As<TOutput>()
            where TOutput : class
        {
            //Se il tipo di destinazione non è lo stesso dell'istanza 
            //corrente emetto un'eccezione per indicare errore
            if (GetType() != typeof(TOutput))
                throw new InvalidCastException(string.Format("Unable to convert data session of " +
                    "type '{0}' to requested type '{1}'.", GetType().FullName, typeof(TOutput).FullName));

            //Eseguo la conversione e ritorno
            return this as TOutput;
        }

        /// <summary>
        /// Set active transaction on current data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        public void SetActiveTransaction(IDataTransaction dataTransaction)
        {
            //Arguments validation
            if (dataTransaction == null) throw new ArgumentNullException(nameof(dataTransaction));

            //Set transaction
            Transaction = dataTransaction;
        }

        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MockDataSession()
        {
            //Richiamo i dispose implicito
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Explici dispose
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        protected virtual void Dispose(bool isDisposing)
        {
            //If already disposed, return
            if (_IsDisposed)
                return;

            //Is is explicit dispose
            if (isDisposing)
            {
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;
        }
    }
}
