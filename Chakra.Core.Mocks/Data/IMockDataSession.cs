using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    /// <summary>
    /// Interface for data session on "Mock" engine
    /// </summary>
    public interface IMockDataSession : IDataSession
    {
        /// <summary>
        /// Get (or creates) scenario instance
        /// </summary>
        /// <returns>Returns instance of scenario</returns>
        IScenario GetScenario();

        /// <summary>
        /// Set active transation on data session
        /// </summary>
        /// <param name="dataTransaction">Data transaction</param>
        void SetActiveTransaction(IDataTransaction dataTransaction);
    }
}
