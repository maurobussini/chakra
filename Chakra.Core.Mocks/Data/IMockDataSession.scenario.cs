using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Data
{
    /// <summary>
    /// Generic interface for data session on "Mock" engine
    /// that owns a specific scenario for data
    /// </summary>
    /// <typeparam name="TScenario">Type of scenario</typeparam>
    public interface IMockDataSession<TScenario> : IMockDataSession
        where TScenario : IScenario
    {
        /// <summary>
        /// Scenario instance
        /// </summary>
        TScenario Scenario { get; }
    }
}
