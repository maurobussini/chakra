using System;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.FileSystems;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;

namespace ZenProgramming.Chakra.Core.Data.FileSystems
{
    /// <summary>
    /// File system data session
    /// </summary>
    public class FileSystemDataSession: IDataSession
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
            return RepositoryHelper.Resolve<TRepositoryInterface, IFileSystemRepository>(this);
        }

        /// <summary>
        /// Active transaction
        /// </summary>
        public IDataTransaction Transaction { get; private set; }

        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public IDataTransaction BeginTransaction()
        {
            //Se la sessione è già attiva, ignoro
            var dataTransaction = new FileSystemDataTransaction(this);

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
                throw new InvalidCastException("Unable to convert data session of " +
                                               $"type '{GetType().FullName}' to requested type '{typeof(TOutput).FullName}'.");

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
        ~FileSystemDataSession()
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
