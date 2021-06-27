using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.EntityFramework.Data;

namespace ZenProgramming.Chakra.Core.EntityFramework.Async.Data
{
    /// <summary>
    /// Entity Framework implementation of data session
    /// </summary>
    public class EntityFrameworkDataSessionAsync<TDbContext> : EntityFrameworkDataSession<TDbContext>
        where TDbContext: DbContext, new()
    {
        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public override IDataTransaction BeginTransaction()
        {
            //Ritorno un'istanza della transazone di Entity Framework
            return new EntityFrameworkDataTransactionAsync<TDbContext>(this);
        }

        
        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~EntityFrameworkDataSessionAsync()
		{
            //Richiamo i dispose implicito
			Dispose(false);
		}
    }
}
