using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Mocks.Async.Data.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data.Repositories
{
    public abstract class MockRepositoryBaseAsync<TEntity, TScenarioInterface> : MockRepositoryRoot<TEntity,TScenarioInterface>,
        IMockRepositoryAsync
        where TEntity : class, IEntity, new()
        where TScenarioInterface: IScenario
    {
        protected MockRepositoryBaseAsync(IDataSessionAsync dataSession,
            Func<TScenarioInterface, IList<TEntity>> entitiesExpression) : base(dataSession.AsMockDataSessionAsync,
                entitiesExpression)
        {
        }

        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression) =>
            Task.FromResult(GetSingle(expression));

        /// <summary>
        /// Fetch list of entities matching criteria on repository
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<List<TEntity>> FetchAsync(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false) =>
                Task.FromResult(Fetch(filterExpression, startRowIndex, maximumRows, sortExpression, isDescending).ToList());

        /// <summary>
        /// Fetch with projection list of entities matching criteria on repository
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="select">Select expression</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="selectFilterExpression">Select filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<List<TProjection>> FetchWithProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,
            Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false) =>
            Task.FromResult(FetchWithProjection(select, filterExpression, selectFilterExpression, startRowIndex, maximumRows, sortExpression, isDescending).ToList());

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression = null) =>
            Task.FromResult(Count(filterExpression));

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="entity">Entity to save</param>
        public Task SaveAsync(TEntity entity)
        {
            Save(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="entity">Entity to delete</param>
        public Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }
    }
}
