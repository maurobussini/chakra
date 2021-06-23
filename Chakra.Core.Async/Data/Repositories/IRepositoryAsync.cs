using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.Async.Data.Repositories
{
    /// <summary>
    /// Interface for generic repository of entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Fetch list of entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        Task<List<TEntity>> FetchAsync(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null,
            int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false);

        /// <summary>
        /// Fetch list of projections matching criteria on repository
        /// </summary>
        /// <param name="select">Select expression</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="selectFilterExpression">Select filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        Task<List<TProjection>> FetchWithProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null,
            int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false);

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression = null);

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="entity">Entity to save</param>
        Task SaveAsync(TEntity entity);

        ///// <summary>
        ///// Execute validation on the specified entity and
        ///// returns a boolean result for the operation
        ///// </summary>
        ///// <param name="entity">Entity to validate</param>
        ///// <returns>If valid, returns true</returns>
        //bool IsValid(TEntity entity);

        ///// <summary>
        ///// Execute a validation on properties of the entity specified
        ///// </summary>
        ///// <param name="entity">Entity to validate</param>
        ///// <returns>Returns a list of validaton results</returns>
        //IList<ValidationResult> Validate(TEntity entity);

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        Task DeleteAsync(TEntity entity);
    }
}