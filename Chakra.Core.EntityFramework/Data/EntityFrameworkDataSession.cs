using System;
using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories;

namespace ZenProgramming.Chakra.Core.EntityFramework.Data
{
    /// <summary>
    /// Entity Framework implementation of data session
    /// </summary>
    public class EntityFrameworkDataSession<TDbContext> : IEntityFrameworkDataSession<TDbContext>
        where TDbContext: DbContext, new()
    {        
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Public properties
        /// <summary>
        /// Database context
        /// </summary>
        public TDbContext Context { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityFrameworkDataSession()
        {
            //Inizializzo il contesto del database
            Context = new TDbContext();
        }

        /// <summary>
        /// Set active transaction on current instance
        /// </summary>
        /// <param name="transaction">Transaction instance</param>
        public void SetActiveTransaction(EntityFrameworkDataTransaction<TDbContext> transaction)
        {
            //Imposto la transazione
            Transaction = transaction;
        }

        /// <summary>
        /// Execute resolution of repository interface using specified class
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <returns>Returns repository instance</returns>
        public TRepositoryInterface ResolveRepository<TRepositoryInterface>()
            where TRepositoryInterface : IRepository
        {
            //Utilizzo il metodo presente sull'helper
            return RepositoryHelper.Resolve<TRepositoryInterface, IEntityFrameworkRepository>(this);
        }

        /// <summary>
        /// Execute resolution of repository interface using specified interface type
        /// </summary>
        /// <returns>Returns repository instance</returns>
        public IRepository ResolveRepository<TRepositoryInterface>(Type repositoryInterface)
            where TRepositoryInterface : IRepository
        {
            //Utilizzo il metodo presente sull'helper
            return RepositoryHelper.Resolve<IEntityFrameworkRepository>(repositoryInterface, this);
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
            return new EntityFrameworkDataTransaction<TDbContext>(this);
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
        ~EntityFrameworkDataSession()
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
                //Se il contesto non è nullo
                if (Context != null)
                {
                    //Eseguo il dispose e lo annullo
                    Context.Dispose();
                    Context = null;
                }
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }        
    }
}
