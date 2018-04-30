namespace ZenProgramming.Chakra.Core.Data.Mockups.Scenarios
{
    /// <summary>
    /// Represents interface for create a user test scenario
    /// </summary>
    public interface IScenario
    {
        /// <summary>
        /// Executes initialization and loading 
        /// of entities on scenario
        /// </summary>
        void InitializeEntities();

        /// <summary>
        /// Executes initialization of assets 
        /// (files, folders, configurations) on scenario
        /// </summary>
        void InitializeAssets();
    }
}
