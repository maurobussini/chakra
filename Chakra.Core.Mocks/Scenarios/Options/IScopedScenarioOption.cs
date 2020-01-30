namespace ZenProgramming.Chakra.Core.Mocks.Scenarios.Options
{
    /// <summary>
    /// Interface for scenario option with "scoped" life of scenario (a new 
    /// instance of scenario is create on the first request of
    /// data session, then the instance is singleton)
    /// </summary>
    public interface IScopedScenarioOption : IScenarioOption
    {
    }
}
