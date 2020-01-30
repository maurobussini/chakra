namespace ZenProgramming.Chakra.Core.Mocks.Scenarios.Options
{
    /// <summary>
    /// Interface of scenario option with "transient" life (a new instance 
    /// for each new data session will be created)
    /// </summary>
    public interface ITransientScenarioOption : IScenarioOption
    {
    }
}
