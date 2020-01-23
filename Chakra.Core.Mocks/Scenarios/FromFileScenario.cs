using System;

namespace ZenProgramming.Chakra.Core.Mocks.Scenarios
{
    /// <summary>
    /// Represents base class for scenario loaded from file
    /// </summary>
    internal abstract class FromFileScenarioBase : IScenario
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">Source file path</param>
        protected FromFileScenarioBase(string filePath)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
        }

        /// <summary>
        /// Executes initialization and loading 
        /// of entities on scenario
        /// </summary>
        public void InitializeEntities()
        {
            throw new NotImplementedException("Lettura da file di un JSON con il contenuto delle informazioni");

        }

        /// <summary>
        /// Executes initialization of assets 
        /// (files, folders, configurations) on scenario
        /// </summary>
        public virtual void InitializeAssets()
        {
            //Questo metodo può essere sovrascritto nella classe derivata
        }
    }
}
