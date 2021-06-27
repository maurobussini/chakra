using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.Async.Data
{
    /// <summary>
    /// EntityFramework data transaction async extensions
    /// </summary>
    public static class DataTransactionAsyncExtension
    {
        /// <summary>
        /// Commit async implementation for IDataTransaction
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        /// <returns></returns>
        public static Task CommitAsync(this IDataTransaction dataTransaction)
        {
            if (dataTransaction is IDataTransactionAsync _dt)
            {
                return _dt.CommitAsync();
            }
            throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
                                           $"interface '{typeof(IDataTransactionAsync).FullName}'.");
        }

        /// <summary>
        /// Rollback async implementation for IDataTransaction
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        public static Task RollBackAsync(this IDataTransaction dataTransaction)
        {
            if (dataTransaction is IDataTransactionAsync _dt)
            {
                return _dt.RollBackAsync();
            }
            throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
                                           $"interface '{typeof(IDataTransactionAsync).FullName}'.");
        }
    }
}