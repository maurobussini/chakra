using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.MongoDb.Data.Options;

namespace ZenProgramming.Chakra.Core.MongoDb.Data
{
    /// <summary>
    /// Entity Framework implementation of data session
    /// </summary>
    /// <typeparam name="TMongoDbOptions">Type of options</typeparam>
    public class MongoDbDataSessionAsync<TMongoDbOptions> : MongoDbDataSession<TMongoDbOptions>
        where TMongoDbOptions : class, IMongoDbOptions, new()
    {        
        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        public override IDataTransaction BeginTransaction()
        {
            //Ritorno un'istanza della transazone di Entity Framework
            return new MongoDbDataTransactionAsync<TMongoDbOptions>(this);
        }
        
        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MongoDbDataSessionAsync()
		{
            //Richiamo i dispose implicito
			Dispose(false);
		}
    }
}
