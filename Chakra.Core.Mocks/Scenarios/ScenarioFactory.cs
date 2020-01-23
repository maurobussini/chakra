using System;

namespace ZenProgramming.Chakra.Core.Mocks.Scenarios
{
    /// <summary>
    /// Factory for default scenario
    /// </summary>
    public static class ScenarioFactory
    {
        /// <summary>
        /// Instance holder
        /// </summary>
        private static IScenario _Default;
        private readonly static object lockObj = new object();

        /// <summary>
        /// Get default scenario instance
        /// </summary>
        public static IScenario Default
        {
            get
            {
                //Se non ho inizializzato, emetto eccezione
                if (_Default == null)
                    throw new InvalidOperationException("Unable to use scenario factory without " +
                        "execute a proper initialization. Please invoke 'ScenarioFactory.Initialize' method before access " + 
                        "default scenario instance available on factory.");

                //Ritorno l'istanza di default
                return _Default;
            }
        }

        /// <summary>
        /// Initialize default instance
        /// </summary>
        /// <param name="scenario">Scenario</param>
        public static void Initialize(IScenario scenario)
        {
            //Validazione argomenti
            if (scenario == null) throw new ArgumentNullException(nameof(scenario));

            lock (lockObj) 
            {
                //Imposto l'istanza
                _Default = scenario;

                //Richiamo le funzioni di inizializzazione
                _Default.InitializeEntities();
                _Default.InitializeAssets();
            }            
        }
    }
}
