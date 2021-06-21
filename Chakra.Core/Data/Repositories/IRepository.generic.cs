using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.Data.Repositories
{
    /// <summary>
    /// Interface for generic repository of entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public partial interface IRepository<TEntity> : IRepository
        where TEntity : class, IEntity, new()
    {
        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> expression);

        /// <summary>
<<<<<<< HEAD
        /// Fetch list of entities matching criteria on repository
=======
        /// Fetch and project list of entities matching criteria on repository
>>>>>>> 02b6da9... description typo
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        IList<TEntity> Fetch(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null,
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
        IList<TProjection> FetchWithProjection<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null,
            int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false);

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        int Count(Expression<Func<TEntity, bool>> filterExpression = null);

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="entity">Entity to save</param>
        void Save(TEntity entity);

        /// <summary>
        /// Execute validation on the specified entity and
        /// returns a boolean result for the operation
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>If valid, returns true</returns>
        bool IsValid(TEntity entity);

        /// <summary>
        /// Execute a validation on properties of the entity specified
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Returns a list of validaton results</returns>
        IList<ValidationResult> Validate(TEntity entity);

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(TEntity entity);
    }
}