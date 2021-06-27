using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Async.Data;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.MongoDb.Data.Options;

namespace ZenProgramming.Chakra.Core.MongoDb.Data
{
    /// <summary>
    /// Represents MongoDb implementation of data transaction
    /// </summary>
    /// <typeparam name="TMongoDbOptions">Type of MongoDb options</typeparam>
    public class MongoDbDataTransactionAsync<TMongoDbOptions> : MongoDbDataTransaction<TMongoDbOptions>, IDataTransactionAsync
        where TMongoDbOptions : class, IMongoDbOptions, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Data session instance</param>
        public MongoDbDataTransactionAsync(IMongoDbDataSession<TMongoDbOptions> dataSession): base(dataSession) { }

        /// <summary>
        /// Execute commit or active transaction
        /// </summary>
        public Task CommitAsync()
        {
           this.Commit();
           return Task.CompletedTask;
        }

        /// <summary>
        /// Execute rollback on active transaction
        /// </summary>
        public Task RollBackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MongoDbDataTransactionAsync()
		{
            //Richiamo i dispose implicito
			Dispose(false);
		}
    }
}
