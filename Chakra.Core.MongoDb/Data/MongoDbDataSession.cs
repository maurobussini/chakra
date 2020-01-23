using System;
using MongoDB.Driver;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.MongoDb.Data.Options;
using ZenProgramming.Chakra.Core.MongoDb.Data.Repositories;

namespace ZenProgramming.Chakra.Core.MongoDb.Data
{
    /// <summary>
    /// Entity Framework implementation of data session
    /// </summary>
    /// <typeparam name="TMongoDbOptions">Type of options</typeparam>
    public class MongoDbDataSession<TMongoDbOptions> : IMongoDbDataSession<TMongoDbOptions>
        where TMongoDbOptions : class, IMongoDbOptions, new()
    {        
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Public properties
        /// <summary>
        /// Provider options
        /// </summary>
        public TMongoDbOptions Options { get; private set; }

        /// <summary>
        /// MongoDb client
        /// </summary>
        public MongoClient Client { get; }

        /// <summary>
        /// Database instance
        /// </summary>
        public IMongoDatabase Database { get; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoDbDataSession()
        {
            //Creazione di una instanza delle opzioni
            Options = new TMongoDbOptions();

            //Recupero la strnga di connessione la database e creo l'URL
            var mongoUrl = MongoUrl.Create(Options.ConnectionString);
            string databaseName = mongoUrl.DatabaseName;

            //Istanzio il client con MongoDb
            Client = new MongoClient(mongoUrl);
            Database = Client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Set active transaction on current instance
        /// </summary>
        /// <param name="transaction">Transaction instance</param>
        public void SetActiveTransaction(MongoDbDataTransaction<TMongoDbOptions> transaction)
        {
            //Imposto la transazione
            Transaction = transaction;
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
            return RepositoryHelper.Resolve<TRepositoryInterface, IMongoDbRepository>(this);
        }

        /// <summary>
        /// Active transaction on data session
        /// </summary>
        public IDataTransaction Transaction { get; private set; }

        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public IDataTransaction BeginTransaction()
        {
            //Ritorno un'istanza della transazone di Entity Framework
            return new MongoDbDataTransaction<TMongoDbOptions>(this);
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
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MongoDbDataSession()
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
                //Annullo le opzioni
                Options = null;
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }        
    }
}
