using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    /// <summary>
    /// Interface for data session on "Mock" engine
    /// </summary>
    public interface IMockDataSession : IDataSession
    {
        /// <summary>
        /// Set active transation on data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        void SetActiveTransaction(IDataTransaction dataTransaction);
    }
}
