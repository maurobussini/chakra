using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.EntityFramework.Extensions;

namespace ZenProgramming.Chakra.Core.EntityFramework.Data.Repositories
{
    /// <summary>
    /// Represents base repository for access to storage based on EntityFramework
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TDbContext">Type of database context</typeparam>
    public abstract class EntityFrameworkRepositoryBase<TEntity, TDbContext> : IRepository<TEntity>, IEntityFrameworkRepository
        where TEntity : class, IEntity, new()
        where TDbContext: DbContext, new()
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Protected properties
        /// <summary>
        /// Get entity framework data session
        /// </summary>
        protected EntityFrameworkDataSession<TDbContext> DataSession { get; }

        /// <summary>
        /// Collection of entities
        /// </summary>
        protected DbSet<TEntity> Collection { get; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        /// <param name="collectionReference">Collection reference</param>
        protected EntityFrameworkRepositoryBase(IDataSession dataSession, Func<TDbContext, DbSet<TEntity>> collectionReference)
        {
            //Eseguo la validazione degli argomenti
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));
            if (collectionReference == null) throw new ArgumentNullException(nameof(collectionReference));

            //Tento il cast della sessione generica ad EntityFramework
            var efSession = dataSession as EntityFrameworkDataSession<TDbContext>;
            if (efSession == null)
                throw new InvalidCastException(
                    $"Specified session of type '{dataSession.GetType().FullName}' cannot be converted to type '{typeof(EntityFrameworkDataSession<TDbContext>).FullName}'.");

            //Imposto la proprietà della sessione
            DataSession = efSession;

            //Recupero il riferimento alla collezione
            Collection = collectionReference(DataSession.Context);    
        }        

        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression)
        {
            //Validazione argomenti
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            //Eseguo l'estrazione dell'elemento singolo
            return Collection
                .Where(expression)
                .SingleOrDefault();
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
        public IList<TEntity> Fetch(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null, int? maximumRows = null,
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
                .ToList();
        }

        /// <summary>
        /// Fetch list of entities matching criteria on repository
        /// </summary>
        /// <param name="select">Select expression</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="selectFilterExpression">Select filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public IList<TProjection> FetchWithProjection<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null,
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
                .ToList();
        }

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public int Count(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            //Query con filtro e paginazione
            IQueryable<TEntity> query = Collection;

            //Se ho un filtro, lo imposto
            if (filterExpression != null)
                query = query.Where(filterExpression);

            //Conto gli elementi
            return query.Count();
        }

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="entity">Entity to save</param>
        public void Save(TEntity entity)
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Utilizzo l'helper per il salvataggio
            RepositoryHelper.Save(entity, DataSession, s =>
            {
                //Se l'entità è in modifica, non cè bisogno di fare nulla
                if (entity.GetId() != null)
                    return;

                //Aggiungo l'elemento al set
                Collection.Add(entity);
            });
        }

        /// <summary>
        /// Execute validation on the specified entity and
        /// returns a boolean result for the operation
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>If valid, returns true</returns>
        public bool IsValid(TEntity entity)
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Eseguo la validazione utilizzando il metodo base
            IList<ValidationResult> results = Validate(entity);

            //La validazione ha successo se non ci sono errori
            return (results.Count == 0);
        }

        /// <summary>
        /// Execute a validation on properties of the entity specified
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Returns a list of validaton results</returns>
        public IList<ValidationResult> Validate(TEntity entity)
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Eseguo la validazione delle informazioni usando l'helper
            IList<ValidationResult> validationResults = RepositoryHelper.Validate(entity, DataSession);

            //Mando in uscita la lista
            return validationResults;
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public void Delete(TEntity entity)
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Se l'elemento non ha id, emetto eccezione
            if (entity.GetId() == null) throw new InvalidOperationException(
                $"Unable to delete entity of type '{typeof(TEntity).FullName}' with invalid id.");

            //Rimuovo l'elemento dal set
            Collection.Remove(entity);
        }

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~EntityFrameworkRepositoryBase()
		{
            //Richiamo i dispose implicito
			Dispose(false);
		}

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Eseguo una dispose esplicita
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        protected virtual void Dispose(bool isDisposing)
        {
            //Se l'oggetto è già rilasciato, esco
            if (_IsDisposed)
                return;

            //Se è richiesto il rilascio esplicito
            if (isDisposing)
            {
                //RIlascio della logica non finalizzabile
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }
    }
}
