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
    /// <summary>
    /// Generic implementation of data session on "Mock" engine with injectable scenario option
    /// </summary>
    /// <typeparam name="TScenarioImplementation">Type of scenario instance (ex: SimpleScenario)</typeparam>
    /// <typeparam name="TScenarioOption">Type of scenario option (ex: TransientScenarioOption[SimpleScenario])</typeparam>
    public class MockDataSession<TScenarioImplementation, TScenarioOption> : IMockDataSession<TScenarioImplementation, TScenarioOption>
        where TScenarioImplementation : class, IScenario, new()
        where TScenarioOption : class, IScenarioOption<TScenarioImplementation>, new()
    {
        #region Private fields
        private bool _IsDisposed;
        private TScenarioOption _Option;
        #endregion        
        
        #region Public properties
        /// <summary>
        /// Active transaction on session
        /// </summary>
        public IDataTransaction Transaction { get; private set; }

        /// <summary>
        /// Option for scenario
        /// </summary>
        public TScenarioOption Option 
        {
            get 
            {
                if (_Option == null)
                    _Option = new TScenarioOption();
                return _Option;
            }
        }

        /// <summary>
        /// Active scenario
        /// </summary>
        public TScenarioImplementation Scenario
        {
            get
            {
                //Get instance of scenario from option
                var scenario = Option.GetInstance();

                //Safe cast to specified scenario
                if (!(scenario is TScenarioImplementation casted))
                    throw new InvalidProgramException($"Scenario provided inside option " +
                        $"is of type '{scenario.GetType().FullName}' but should be of type '{typeof(TScenarioImplementation).FullName}'");

                //Returns instance 
                return casted;
            }
        }
        #endregion

        /// <summary>
        /// Get (or creates) scenario instance
        /// </summary>
        /// <returns>Returns instance of scenario</returns>
        public IScenario GetScenario()
        {
            //Get instance of scenario from option
            return Option.GetInstance();
        }

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
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public virtual IDataTransaction BeginTransaction()
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
                throw new InvalidCastException("Unable to convert data session of " +
                                               $"type '{GetType().FullName}' to requested type '{typeof(TOutput).FullName}'.");

            //Eseguo la conversione e ritorno
            return this as TOutput;
        }

        /// <summary>
        /// Set active transaction on current data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        public virtual void SetActiveTransaction(IDataTransaction dataTransaction)
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
