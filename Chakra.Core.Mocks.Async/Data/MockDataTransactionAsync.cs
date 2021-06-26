using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{

    public static class MockDataTransactionAsyncExtension
    {
        public static Task CommitAsync(this IDataTransaction dataTransaction)
        {
            if (dataTransaction is IDataTransactionAsync _dt)
            {
                return _dt.CommitAsync();
            }
            throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
                                           $"interface '{typeof(IDataTransactionAsync).FullName}'.");
        }

        public static Task RollBackAsync(this IDataTransaction dataTransaction)
        {
            if (dataTransaction is IDataTransactionAsync _dt)
            {
                return _dt.RollBackAsync();
            }
            throw new InvalidCastException($"Unable to cast type of '{dataTransaction.GetType().FullName}' to " +
                                           $"interface '{typeof(IDataTransactionAsync).FullName}'.");
        }

        //public static Task CommitAsync(this MockDataTransaction mockDataTransaction)
        //{
        //    // Call base method
        //    this.Commit();
        //    return Task.CompletedTask;
        //}
    }

    /// <summary>
    /// Data transaction implementation for mockup
    /// </summary>
    public class MockDataTransactionAsync : MockDataTransaction, IDataTransactionAsync
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MockDataTransactionAsync(IMockDataSessionAsync dataSession): base(dataSession) {}

        /// <summary>
        /// Executes commit of transaction
        /// </summary>
        public Task CommitAsync()
        {
            // Call base method
            this.Commit();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes rollback of transaction
        /// </summary>
        public Task RollBackAsync()
        {
            // Call base method
            this.Rollback();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finalizes that ensures the object is correctly disposed of.
		/// </summary>
        ~MockDataTransactionAsync()
        {
            //Implicit dispose
            Dispose(false);
        }

    }
}
