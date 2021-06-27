using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Async.Data.Repositories;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories;
using ZenProgramming.Chakra.Core.EntityFramework.Extensions;

namespace ZenProgramming.Chakra.Core.EntityFramework.Async.Data.Repositories
{
    public abstract class EntityFrameworkRepositoryBaseAsync<TEntity, TDbContext> :
        EntityFrameworkRepositoryBase<TEntity, TDbContext>,
        IRepositoryAsync<TEntity>
        where TEntity : class, IEntity, new()
        where TDbContext: DbContext, new()
    {
        
        protected EntityFrameworkRepositoryBaseAsync(IDataSession dataSession, Func<TDbContext, DbSet<TEntity>> collectionReference) : base(dataSession, collectionReference) { }

        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            //Validazione argomenti
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            //Eseguo l'estrazione dell'elemento singolo
            return Collection
                .Where(expression)
                .SingleOrDefaultAsync();
        }

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
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
        {
            //Query con filtro e paginazione
            IQueryable<TEntity> query = Collection;

            //Se ho un filtro, lo imposto
            if (filterExpression != null)
                query = query.Where(filterExpression).AsQueryable();

            //Se specificato, applico anche il sort
            if (sortExpression != null)
                query = isDescending
                    ? query.OrderByDescending(sortExpression)
                    : query.OrderBy(sortExpression);

            //Eseguo l'estrazione dati
            return query
                .Paging(startRowIndex, maximumRows)
                .ToListAsync();
        }

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
        public Task<List<TProjection>> FetchWithProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null,
            int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
        {
            //Query con filtro e paginazione
            IQueryable<TEntity> query = Collection;

            //Se ho un filtro, lo imposto
            if (filterExpression != null)
                query = query.Where(filterExpression).AsQueryable();

            //Se specificato, applico anche il sort
            if (sortExpression != null)
                query = isDescending
                    ? query.OrderByDescending(sortExpression)
                    : query.OrderBy(sortExpression);

            //Eseguo la proiezione dei dati
            var projectionQuery = query
                .Select(select).AsQueryable();

            //Se ho un filtro sulla proiezione, lo imposto
            if (selectFilterExpression != null)
                projectionQuery = projectionQuery.Where(selectFilterExpression).AsQueryable();

            return projectionQuery
                .Paging(startRowIndex, maximumRows)
                .ToListAsync();
        }

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            //Query con filtro e paginazione
            IQueryable<TEntity> query = Collection;

            //Se ho un filtro, lo imposto
            if (filterExpression != null)
                query = query.Where(filterExpression);

            //Conto gli elementi
            return query.CountAsync();
        }

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="entity">Entity to save</param>
        public Task SaveAsync(TEntity entity)
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Utilizzo l'helper per il salvataggio
            return Core.Async.Data.Repositories.Helpers.RepositoryHelper.SaveAsync(entity, DataSession,async s =>
            {
                //Se l'entità è in modifica, non cè bisogno di fare nulla
                if (entity.GetId() != null)
                    return;

                //Aggiungo l'elemento al set
                await Collection.AddAsync(entity);
            });
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public Task DeleteAsync(TEntity entity)
        {
            // Deletion is not an async method
            // Call base method
            Delete(entity);
            return Task.CompletedTask;
        }

    }
    
}
