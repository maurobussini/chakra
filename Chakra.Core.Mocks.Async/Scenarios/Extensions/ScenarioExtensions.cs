using System;
using System.Collections.Generic;
using System.Linq;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Async.Data;
using ZenProgramming.Chakra.Core.Mocks.Async.Data.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Data.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Scenarios.Extensions
{
    /// <summary>
    /// Contains extensions of interface "IScenario"
    /// </summary>
    public static class ScenarioExtensions
    {
        /// <summary>
        /// Get scenario of specified type from data session
        /// </summary>
        /// <typeparam name="TScenario">Type of scenario</typeparam>
        /// <param name="instance">Instance</param>
        /// <returns>Returns scenario</returns>
        public static TScenario GetScenario<TScenario>(this IDataSessionAsync instance) 
        {
            //Arguments validation
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Try to cast to IMockDataSession
            IMockDataSessionAsync mockupSession = instance.AsMockDataSessionAsync();

            //Get scenario from data session
            IScenario scenarioFromDataSession = mockupSession.GetScenario();

            //Try cast of scenario to provided type
            if (!(scenarioFromDataSession is TScenario castedScenario))
                throw new InvalidCastException("Scenario contained on data session is of " +
                    $"type '{scenarioFromDataSession.GetType().FullName}' and cannot be converted " +
                    $"to type '{typeof(TScenario).FullName}'.");

            //Return scenario
            return castedScenario;
        }
    }
}
