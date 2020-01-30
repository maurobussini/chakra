namespace ZenProgramming.Chakra.Core.Mocks.Scenarios.Options
{
    /// <summary>
    /// Interface for scenario option
    /// </summary>
    public interface IScenarioOption
    {
        /// <summary>
        /// Get instance of scenario working
        /// </summary>
        /// <returns>Returns scenario instance</returns>
        IScenario GetInstance();
    }
}
