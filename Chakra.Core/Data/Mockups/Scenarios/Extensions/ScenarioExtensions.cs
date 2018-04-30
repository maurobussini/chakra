using System;

namespace ZenProgramming.Chakra.Core.Data.Mockups.Scenarios.Extensions
{
    /// <summary>
    /// Contains extensions of interface "IScenario"
    /// </summary>
    public static class ScenarioExtensions
    {
        /// <summary>
        /// Executes cast of specified instance to target type
        /// </summary>
        /// <typeparam name="TScenario">Target type</typeparam>
        /// <param name="instance">Instance</param>
        /// <returns>Returns casted type</returns>
        public static TScenario OfType<TScenario>(this IScenario instance)
            where TScenario : IScenario
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            try
            {
                //Tento il cast esplicito, all'interno di un blocco
                //try/catch per gestire eventuali problemi di conversione
                return (TScenario)instance;
            }
            catch (InvalidCastException exc)
            {
                //Sollevo l'evento di cast invalido
                throw new InvalidCastException(string.Format("Unable to " + 
                    "cast instance of type '{0}' to target type '{1}'.", 
                    instance.GetType().FullName, typeof(TScenario).FullName), exc);
            }
        }
    }
}
