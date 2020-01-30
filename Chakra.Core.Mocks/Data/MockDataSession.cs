using System;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    //  /// <summary>
    //  /// Data session for mockup
    //  /// </summary>
    //  [Obsolete("Used in 2.0 => use version 3.0")]
    //  public class MockDataSession<TScenario> : IDataSession
    //      where TScenario: IScenario
    //  {
    //      #region Static fields
    //      private static Type _DefaultScenario = null;

    //      [Obsolete]
    //      private static Lazy<TScenario> _ScenarioInstance = new Lazy<TScenario>(InitializeScenario);

    //      [Obsolete]
    //      private static readonly object lockObject = new object();

    //      [Obsolete]
    //      /// <summary>
    //      /// Initialize scenario in singleton instance
    //      /// </summary>
    //      /// <returns>Returns instance</returns>
    //      private static TScenario InitializeScenario() 
    //      {
    //          //New instance of scenario
    //          var instance = (TScenario)Activator.CreateInstance(_DefaultScenario);

    //          //Inizialize entities and assets
    //          instance.InitializeEntities();
    //          instance.InitializeAssets();

    //          //Returns instance
    //          return instance;
    //      }

    //      [Obsolete]
    //      public static void SetScenarioImplementation<T>()
    //      {
    //          lock (lockObject) 
    //          {
    //              _DefaultScenario = typeof(T);
    //          }

    //      }

    //      [Obsolete]
    //      public void DestroyScenarioInstance() 
    //      {
    //          lock (lockObject)
    //          {
    //              //Re-initialize scenario instance
    //              _ScenarioInstance = new Lazy<TScenario>(InitializeScenario);
    //          }
    //      }
    //      #endregion


    //      #region Private fields
    //      private bool _IsDisposed;
    //      #endregion        

    //      /// <summary>
    //      /// Execute resolution of repository interface using specified clas
    //      /// </summary>
    //      /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
    //      /// <returns>Returns repository instance</returns>
    //      public TRepositoryInterface ResolveRepository<TRepositoryInterface>()
    //          where TRepositoryInterface : IRepository
    //      {
    //          //Utilizzo il metodo presente sull'helper
    //          return RepositoryHelper.Resolve<TRepositoryInterface, IMockRepository>(this);
    //      }

    //      /// <summary>
    //      /// Active transaction on session
    //      /// </summary>
    //      public IDataTransaction Transaction { get; private set; }

    //      /// <summary>
    //      /// Active scenario
    //      /// </summary>
    //      public TScenario Scenario 
    //      { 
    //          get 
    //          {
    //              if (_TmpScenarioInstance != null)
    //                  return _TmpScenarioInstance;

    //              if (_DefaultScenario == null)
    //                  throw new InvalidProgramException("Scenario implementation invalid");

    //              _TmpScenarioInstance = (TScenario)Activator.CreateInstance(_DefaultScenario);
    //              _TmpScenarioInstance.InitializeAssets();
    //              _TmpScenarioInstance.InitializeEntities();
    //              return _TmpScenarioInstance;



    //              //Get singleton instance
    //              //return _ScenarioInstance.Value; 
    //          } 
    //      }
    //      private TScenario _TmpScenarioInstance;

    //      /// <summary>
    //      /// Begin new transaction on active session
    //      /// </summary>
    //      /// <returns>Returns data transaction instance</returns>
    //      public IDataTransaction BeginTransaction()
    //      {
    //          //Se la sessione è già attiva, ignoro
    //          var dataTransaction = new MockDataTransaction<TScenario>(this);

    //          //Mando in uscita la struttura
    //          return dataTransaction;
    //      }

    //      /// <summary>
    //      /// Executes convert of session instance on specified type
    //      /// </summary>
    //      /// <typeparam name="TOutput">Target type</typeparam>
    //      /// <returns>Returns converted instance</returns>
    //      public TOutput As<TOutput>() 
    //          where TOutput : class
    //      {
    //          //Se il tipo di destinazione non è lo stesso dell'istanza 
    //          //corrente emetto un'eccezione per indicare errore
    //          if (GetType() != typeof(TOutput))
    //              throw new InvalidCastException(string.Format("Unable to convert data session of " +
    //                  "type '{0}' to requested type '{1}'.", GetType().FullName, typeof(TOutput).FullName));

    //          //Eseguo la conversione e ritorno
    //          return this as TOutput;
    //      }

    //      /// <summary>
    //      /// Set active transaction on current data session
    //      /// </summary>
    //      /// <param name="dataTransaction">Data transaction</param>
    //      public void SetActiveTransaction(IDataTransaction dataTransaction)
    //      {
    //          //Validazione argomenti
    //          if (dataTransaction == null) throw new ArgumentNullException(nameof(dataTransaction));

    //          //Imposto la transazione
    //          Transaction = dataTransaction;
    //      }

    //      /// <summary>
    //      /// Finalizer that ensures the object is correctly disposed of.
    ///// </summary>
    //      ~MockDataSession()
    //{
    //          //Richiamo i dispose implicito
    //	Dispose(false);
    //}

    //      /// <summary>
    //      /// Performs application-defined tasks associated with freeing, 
    //      /// releasing, or resetting unmanaged resources.
    //      /// </summary>
    //      public void Dispose()
    //      {
    //          //Eseguo una dispose esplicita
    //          Dispose(true);
    //          GC.SuppressFinalize(this);
    //      }

    //      /// <summary>
    //      /// Performs application-defined tasks associated with freeing, 
    //      /// releasing, or resetting unmanaged resources.
    //      /// </summary>
    //      /// <param name="isDisposing">Explicit dispose</param>
    //      protected virtual void Dispose(bool isDisposing)
    //      {
    //          //Se l'oggetto è già rilasciato, esco
    //          if (_IsDisposed)
    //              return;

    //          //Se è richiesto il rilascio esplicito
    //          if (isDisposing)
    //          {                
    //          }

    //          //Marco il dispose e invoco il GC
    //          _IsDisposed = true;            
    //      }
    //  }





    public class MockDataSession<TScenario> : MockDataSession<TScenario, ScopedScenarioOption<TScenario>>
        where TScenario : class, IScenario, new()
    { 
    }

    public class MockDataSession<TScenario, TScenarioOption> : IMockDataSession<TScenario>
        where TScenario : IScenario
        where TScenarioOption: class, IScenarioOption, new()
    {
        #region Static fields
        //[Obsolete]
        //private static Type _DefaultScenario = null;

        //[Obsolete]
        //private static Lazy<TScenario> _ScenarioInstance = new Lazy<TScenario>(InitializeScenario);

        //[Obsolete]
        //private static readonly object lockObject = new object();

        //[Obsolete]
        ///// <summary>
        ///// Initialize scenario in singleton instance
        ///// </summary>
        ///// <returns>Returns instance</returns>
        //private static TScenario InitializeScenario()
        //{
        //    //New instance of scenario
        //    var instance = (TScenario)Activator.CreateInstance(_DefaultScenario);

        //    //Inizialize entities and assets
        //    instance.InitializeEntities();
        //    instance.InitializeAssets();

        //    //Returns instance
        //    return instance;
        //}

        //[Obsolete]
        //public static void SetScenarioImplementation<T>()
        //{
        //    lock (lockObject)
        //    {
        //        _DefaultScenario = typeof(T);
        //    }

        //}

        //[Obsolete]
        //public void DestroyScenarioInstance()
        //{
        //    lock (lockObject)
        //    {
        //        //Re-initialize scenario instance
        //        _ScenarioInstance = new Lazy<TScenario>(InitializeScenario);
        //    }
        //}
        #endregion


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

        private TScenarioOption _ScenarioOption = null;
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

        private static TScenario _ScenarioInstanceStatic;
        private TScenario _ScenarioInstanceLocal;

        /// <summary>
        /// Active scenario
        /// </summary>
        public TScenario Scenario
        {
            get
            {
                return (TScenario)ScenarioOption.GetInstance();

                ////Se è transient, uso quello locale
                //if (ScenarioOption is ITransientScenarioOption)
                //{
                //    if (_ScenarioInstanceLocal == null) 
                //    {
                //        _ScenarioInstanceLocal= Activator.CreateInstance(ScenarioOption.)
                //    }
                //}

                ////Se l'opzione è di tipo Scoped
                //if (ScenarioOption is IScopedScenarioOption) 
                //{
                    
                //}

                //if (_TmpScenarioInstance != null)
                //    return _TmpScenarioInstance;

                //if (_DefaultScenario == null)
                //    throw new InvalidProgramException("Scenario implementation invalid");

                //_TmpScenarioInstance = (TScenario)Activator.CreateInstance(_DefaultScenario);
                //_TmpScenarioInstance.InitializeAssets();
                //_TmpScenarioInstance.InitializeEntities();
                //return _TmpScenarioInstance;



                //Get singleton instance
                //return _ScenarioInstance.Value; 
            }
        }
        private TScenario _TmpScenarioInstance;

        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public IDataTransaction BeginTransaction()
        {
            //Se la sessione è già attiva, ignoro
            var castedDataSession = this as IMockDataSession;
            var dataTransaction = new MockDataTransaction(castedDataSession);

            //Mando in uscita la struttura
            return dataTransaction;
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
            //Validazione argomenti
            if (dataTransaction == null) throw new ArgumentNullException(nameof(dataTransaction));

            //Imposto la transazione
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
            //Eseguo una dispose esplicita
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
            //Se l'oggetto è già rilasciato, esco
            if (_IsDisposed)
                return;

            //Se è richiesto il rilascio esplicito
            if (isDisposing)
            {
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;
        }
    }
}
