using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Async.Data;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Diagnostic;
using ZenProgramming.Chakra.Core.Mocks.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    /// <summary>
    /// Data transaction implementation for mockup
    /// </summary>
    public class MockDataTransactionAsync : MockDataTransaction, IDataTransactionAsync
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MockDataTransactionAsync(IMockDataSession dataSession): base(dataSession)
        {
        }

        /// <summary>
        /// Executes commit async of transaction
        /// </summary>
        public Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes rollback of transaction
        /// </summary>
        public Task RollBackAsync()
        {
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
