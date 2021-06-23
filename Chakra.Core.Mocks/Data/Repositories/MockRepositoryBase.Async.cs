using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZenProgramming.Chakra.Core.Mocks.Data.Repositories
{
    /// <summary>
    /// Base class for repositories with mock engine
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TScenarioInterface">Type of scenario interface (ex: IChakraScenario)</typeparam>
    public abstract partial class MockRepositoryBase<TEntity, TScenarioInterface>
    {
        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression) =>
            Task.FromResult(this.GetSingle(expression));

        /// <summary>
        /// Fetch list of entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<List<TEntity>> FetchAsync(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false) =>
                Task.FromResult(this.Fetch(filterExpression,startRowIndex,maximumRows,sortExpression,isDescending).ToList());
        
        /// <summary>
        /// Fetch with projection list of entities matching criteria on repository
        /// </summary>
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
            Task.FromResult(this.FetchWithProjection(select,filterExpression,selectFilterExpression,startRowIndex,maximumRows,sortExpression,isDescending).ToList());

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression = null) =>
            Task.FromResult(this.Count(filterExpression));
        
        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="entity">Entity to save</param>
        public Task SaveAsync(TEntity entity)
        {
            this.Save(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public Task DeleteAsync(TEntity entity)
        {
            this.Delete(entity);
            return Task.CompletedTask;
        }
    }
}
