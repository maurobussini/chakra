using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data.Repositories
{
    /// <summary>
    /// Base class for repositories with mock engine
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TScenarioInterface">Type of scenario interface (ex: IChakraScenario)</typeparam>
    public static class MockRepositoryAsyncExtensions
    {
        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        public static Task<TEntity> GetSingleAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity, new() =>
            Task.FromResult(repository.GetSingle(expression));

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
        public static Task<List<TEntity>> FetchAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
            where TEntity : class, IEntity, new() =>
                Task.FromResult(repository.Fetch(filterExpression, startRowIndex, maximumRows, sortExpression, isDescending).ToList());

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
        public static Task<List<TProjection>> FetchWithProjectionAsync<TEntity, TProjection>(this IRepository<TEntity> repository, Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,
            Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
            where TEntity : class, IEntity, new() =>
            Task.FromResult(repository.FetchWithProjection(select, filterExpression, selectFilterExpression, startRowIndex, maximumRows, sortExpression, isDescending).ToList());

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public static Task<int> CountAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filterExpression = null)
            where TEntity : class, IEntity, new() =>
            Task.FromResult(repository.Count(filterExpression));

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="entity">Entity to save</param>
        public static Task SaveAsync<TEntity>(IRepository<TEntity> repository, TEntity entity)
             where TEntity : class, IEntity, new()
        {
            repository.Save(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="entity">Entity to delete</param>
        public static Task DeleteAsync<TEntity>(IRepository<TEntity> repository, TEntity entity)
             where TEntity : class, IEntity, new()
        {
            repository.Delete(entity);
            return Task.CompletedTask;
        }
    }
}
