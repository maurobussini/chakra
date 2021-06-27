using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Async.Data;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;

namespace ZenProgramming.Chakra.Core.EntityFramework.Async.Data
{
    /// <summary>
    /// Represents Entity Framework implementation of data transaction
    /// </summary>
    /// <typeparam name="TDbContext">Type of DBContext used</typeparam>
    public class EntityFrameworkDataTransactionAsync<TDbContext> : EntityFrameworkDataTransaction<TDbContext>, IDataTransactionAsync
        where TDbContext : DbContext, new()
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Data session instance</param>
        public EntityFrameworkDataTransactionAsync(IEntityFrameworkDataSession<TDbContext> dataSession): base(dataSession)
        { }

        /// <summary>
        /// Execute commit or active transaction
        /// </summary>
        public async Task CommitAsync()
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

            //Eseguo il commit dei cambiamenti sul context
            await _DataSession.Context.SaveChangesAsync();

            //Imposto il flag di commit
            WasCommitted = true;

            //Rimuovo l'istanza di transazione
            IsActive = false;
            _DataSession.SetActiveTransaction(null);
        }
        

        /// <summary>
        /// Execute rollback on active transaction
        /// </summary>
        public Task RollBackAsync()
        {
            // call base method
            this.Rollback();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
        /// </summary>
        ~EntityFrameworkDataTransactionAsync()
        {
            //Richiamo i dispose implicito
            Dispose(false);
        }
    }
}