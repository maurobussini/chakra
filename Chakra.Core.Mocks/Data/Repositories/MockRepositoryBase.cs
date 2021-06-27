using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Mocks.Data.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Data.Repositories
{
    /// <summary>
    /// Base class for repositories with mock engine
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TScenarioInterface">Type of scenario interface (ex: IChakraScenario)</typeparam>
    public abstract class MockRepositoryBase<TEntity, TScenarioInterface> : MockRepositoryRoot<TEntity, TScenarioInterface>
        where TEntity : class, IEntity, new()
        where TScenarioInterface : IScenario
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        /// <param name="entitiesExpression">Entities expression</param>
        protected MockRepositoryBase(IDataSession dataSession,
            Func<TScenarioInterface, IList<TEntity>> entitiesExpression)
        : base(
            dataSession.AsMockDataSession,
            entitiesExpression)
        { }
    }
}