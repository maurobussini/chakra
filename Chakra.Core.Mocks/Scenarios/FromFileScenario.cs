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
            //Arguments validation
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
        }

        /// <summary>
        /// Executes initialization and loading 
        /// of entities on scenario
        /// </summary>
        public void InitializeEntities()
        {
            //TODO: Should read a local file (ex. YAML, JSON) to build scenario
            throw new NotImplementedException("TODO: Should read a local file (ex. YAML, JSON) to build scenario");
        }

        /// <summary>
        /// Executes initialization of assets 
        /// (files, folders, configurations) on scenario
        /// </summary>
        public virtual void InitializeAssets()
        {
            //Not required here, but overridable
        }
    }
}
