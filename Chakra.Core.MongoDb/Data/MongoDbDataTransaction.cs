using System;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.MongoDb.Data.Options;

namespace ZenProgramming.Chakra.Core.MongoDb.Data
{

    public interface IMongoDbTransaction<TMongoDbOptions> : IDataTransaction
        where TMongoDbOptions : class, IMongoDbOptions, new()
    {

    }
    /// <summary>
    /// Represents MongoDb implementation of data transaction
    /// </summary>
    /// <typeparam name="TMongoDbOptions">Type of MongoDb options</typeparam>
    public class MongoDbDataTransaction<TMongoDbOptions> : IMongoDbTransaction<TMongoDbOptions>
        where TMongoDbOptions : class, IMongoDbOptions, new()
    {
        #region Private field
        private bool _IsDisposed;
        protected readonly IMongoDbDataSession<TMongoDbOptions> _DataSession;
        #endregion

        #region Public properties
        /// <summary>
        /// Is active
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Current transaction was rolled back
        /// </summary>
        public bool WasRolledBack { get; protected set; }

        /// <summary>
        /// Current transaction was rolled committed
        /// </summary>
        public bool WasCommitted { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Data session instance</param>
        public MongoDbDataTransaction(IMongoDbDataSession<TMongoDbOptions> dataSession)
        {
            //Validazione argomenti
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));

            //Imposto la mirror
            _DataSession = dataSession;

            //Se già esiste una transanzione sull'holder, esco
            if (_DataSession.Transaction != null)
                return;

            //Imposto l'istanza corrente
            IsActive = true;
            _DataSession.SetActiveTransaction(this);
        }

        /// <summary>
        /// Execute commit or active transaction
        /// </summary>
        public void Commit()
        {
            //Se l'istanza della transazione presente nel session holder
            //non è l'istanza attuale, non eseguo alcuna operazione
            if (_DataSession.Transaction != this)
                return;

            //Se è già stato committato, emetto eccezione
            if (WasCommitted)
                throw new InvalidOperationException("Current transaction was already committed.");

            //Se è stato rollbackato, emetto eccezione
            if (WasRolledBack)
                throw new InvalidOperationException("Current transaction was already rolled back.");

            //ATTENZIONE! In un database documentale non esiste il concetto di
            //transazione quindo le operazioni di rollback e commit sono fittizie

            //Imposto il flag di commit
            WasCommitted = true;

            //Rimuovo l'istanza di transazione
            IsActive = false;
            _DataSession.SetActiveTransaction(null);
        }

        /// <summary>
        /// Execute rollback on active transaction
        /// </summary>
        public void Rollback()
        {
            //Se l'istanza della transazione presente nel session holder
            //non è l'istanza attuale, non eseguo alcuna operazione
            if (_DataSession.Transaction != this)
                return;

            //Se è stato rollbackato, emetto eccezione
            if (WasRolledBack)
                throw new InvalidOperationException("Current transaction was already rolled back.");

            //Se è già stato committato, emetto eccezione
            if (WasCommitted)
                throw new InvalidOperationException("Current transaction was already committed.");

            //ATTENZIONE! In un database documentale non esiste il concetto di
            //transazione quindo le operazioni di rollback e commit sono fittizie

            //Imposto il flag di rollback
            WasRolledBack = true;

            //Rimuovo l'istanza di transazione
            IsActive = false;
            _DataSession.SetActiveTransaction(null);
        }

        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MongoDbDataTransaction()
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
                //Nessun rilascio, in questo caso
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }    
    }
}
