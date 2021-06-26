using ZenProgramming.Chakra.Core.Mocks.Scenarios;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data
{
    /// <summary>
    /// Generic interface for data session on "Mock" engine
    /// that owns a specific scenario for data
    /// </summary>
    /// <typeparam name="TScenarioOption">Type of scenario option</typeparam>
    public interface IMockDataSessionAsync<TScenarioInstance, TScenarioOption> : IMockDataSessionAsync<TScenarioInstance>
        where TScenarioInstance: IScenario
        where TScenarioOption : IScenarioOption
    {
        /// <summary>
        /// Scenario option
        /// </summary>
        TScenarioOption Option { get; }
    }
}
