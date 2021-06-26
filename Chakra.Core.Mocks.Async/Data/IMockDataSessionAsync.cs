using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    /// <summary>
    /// Interface for data session on "Mock" engine
    /// </summary>
    public interface IMockDataSessionAsync : IMockDataSession, IDataSessionAsync
    {   
        /// <summary>
        /// Set active transation on data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        void SetActiveTransactionAsync(IDataTransactionAsync dataTransaction);
    }
}
