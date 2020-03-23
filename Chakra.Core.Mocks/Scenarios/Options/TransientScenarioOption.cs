namespace ZenProgramming.Chakra.Core.Mocks.Scenarios.Options
{
    /// <summary>
    /// Interface of scenario option with "transient" life (a new instance
    /// for each new data session will be created)
    /// </summary>
    /// <typeparam name="TScenarioImplementation">Type of scenario implementation</typeparam>
    public class TransientScenarioOption<TScenarioImplementation> : IScenarioOption<TScenarioImplementation>
        where TScenarioImplementation : class, IScenario, new()
    {
        //Private instance of scenario
        private TScenarioImplementation _LocalInstance;

        /// <summary>
        /// Get instance of scenario working
        /// </summary>
        /// <returns>Returns scenario instance</returns>
        public TScenarioImplementation GetInstance()
        {
            //If instance does not exists
            if (_LocalInstance == null)
            {
                //Create instance
                _LocalInstance = new TScenarioImplementation();

                //Initialize entities and assets
                _LocalInstance.InitializeAssets();
                _LocalInstance.InitializeEntities();
            }

            //Returns instance
            return _LocalInstance;
        }
    }
}
