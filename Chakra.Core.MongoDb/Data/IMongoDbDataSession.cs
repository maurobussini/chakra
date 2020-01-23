using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.MongoDb.Data.Options;

namespace ZenProgramming.Chakra.Core.MongoDb.Data
{
    /// <summary>
    /// MongoDB data session interface
    /// </summary>
    /// <typeparam name="TMongoDbOptions">Type of options</typeparam>
    public interface IMongoDbDataSession<TMongoDbOptions> : IDataSession
        where TMongoDbOptions : class, IMongoDbOptions, new()
    {
        /// <summary>
        /// MongoDB options
        /// </summary>
        TMongoDbOptions Options { get; }

        /// <summary>
        /// Set active transaction in current data session
        /// </summary>
        /// <param name="transaction">Active transaction</param>
        void SetActiveTransaction(MongoDbDataTransaction<TMongoDbOptions> transaction);
    }
}
