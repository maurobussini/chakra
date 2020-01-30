namespace ZenProgramming.Chakra.Core.Mocks.Scenarios.Options
{
    /// <summary>
    /// Interface for scenario option with "scoped" life of scenario (a new 
    /// instance of scenario is create on the first request of
    /// data session, then the instance is singleton)
    /// </summary>
    /// <typeparam name="TScenarioImplementation">Type of scenario implementation</typeparam>
    public class ScopedScenarioOption<TScenarioImplementation> : IScenarioOption<TScenarioImplementation>, IScopedScenarioOption
        where TScenarioImplementation : class, IScenario, new()
    {
        //Singleton scenario
        private static TScenarioImplementation _SingletonInstance;

        /// <summary>
        /// Get instance of scenario working
        /// </summary>
        /// <returns>Returns scenario instance</returns>
        public IScenario GetInstance()
        {
            //If instance does not exists
            if (_SingletonInstance == null)
            {
                //Create instance
                _SingletonInstance = new TScenarioImplementation();

                //Initialize entities and assets                
                _SingletonInstance.InitializeEntities();
                _SingletonInstance.InitializeAssets();
            }

            //Returns instance
            return _SingletonInstance;
        }
    }
}
