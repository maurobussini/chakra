using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    /// <summary>
    /// Interface for data session on "Mock" engine
    /// </summary>
    public interface IMockDataSessionAsync : IMockDataSessionBase, IDataSessionAsync
    {   
        /// <summary>
        /// Set active transation on data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        void SetActiveTransaction(IDataTransactionAsync dataTransaction);
    }
}
