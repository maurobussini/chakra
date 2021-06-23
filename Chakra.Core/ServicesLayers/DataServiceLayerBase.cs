using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.ServicesLayers
{
    /// <summary>
    /// Represents base service layer for interact with data
    /// </summary>
    public abstract class DataServiceLayerBase : ServiceLayerBase
    {
        /// <summary>
        /// Active data session
        /// </summary>
        protected IDataSession DataSession { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        protected DataServiceLayerBase(IDataSession dataSession)
        {
            //Eseguo la validazione degli argomenti
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));

            //Valorizzo la proprietà protetta
            DataSession = dataSession;
        }

        /// <summary>
        /// Executes validation of provided entity using repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="repository">Repository instance</param>
        /// <returns>Returns list of validation results</returns>
        protected virtual IList<ValidationResult> ValidateEntity<TEntity>(TEntity entity, IRepository<TEntity> repository)
             where TEntity : class, IEntity, new()
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            //In transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Eseguo la validazione dell'oggetto
                IList<ValidationResult> results = repository.Validate(entity);

                //Eseguo il rollback
                t.Rollback();

                //Ritorno la lista delle validazioni
                return results;
            }
        }

        /// <summary>
        /// Execute save of entity on specified repository, executing a validation
        /// of contained data, initialization and/or update (if needed)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="repository">Repository</param>
        /// <param name="actionOnCreate">Action to execute on entity on create</param>
        /// <param name="actionOnUpdate">Action to execute on entity on update</param>
        /// <returns>Returns validation result</returns>
        protected virtual IList<ValidationResult> SaveEntity<TEntity>(TEntity entity, IRepository<TEntity> repository,
            Action<TEntity> actionOnCreate = null, Action<TEntity> actionOnUpdate = null)
            where TEntity : class, IEntity, new()
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            //In transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Se l'oggetto non ha id primario, e la funzione di 
                //actionCreate è valorizzata, la lancio
                if (entity.GetId() == null && actionOnCreate != null)
                    actionOnCreate(entity);

                //Se l'id è valorizzato e ho la funzione di actionUpdate
                if (entity.GetId() != null && actionOnUpdate != null)
                    actionOnUpdate(entity);

                //Eseguo la validazione dell'oggetto
                IList<ValidationResult> results = repository.Validate(entity);

                //Se ho errori di validazione, rollback ed esco
                if (results.Count > 0)
                {
                    //Eseguo il rollback e ritorno
                    t.Rollback();
                    return results;
                }

                //Eseguo il salvataggio dell'oggetto
                repository.Save(entity);
                t.Commit();

                //Ritorno successo
                return results;
            }
        }

        /// <summary>
        /// Execute delete of specified entity
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="repository">Repository instance</param>
        /// <returns>Returns validation results</returns>
        protected virtual IList<ValidationResult> DeleteEntity<TEntity>(TEntity entity, IRepository<TEntity> repository)
            where TEntity : class, IEntity, new()
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            //In transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Predispongo le validazioni di uscita
                IList<ValidationResult> results = new List<ValidationResult>();

                //Se l'entità non ha un id primario non si può cancellare
                if (entity.GetId() == null) throw new InvalidOperationException(
                    $"Cannot delete entity of type '{typeof(TEntity).FullName}' with null identifier.");

                //Eseguo il delete dell'oggetto
                repository.Delete(entity);
                t.Commit();

                //Ritorno successo
                return results;
            }
        }

        /// <summary>
        /// Get single entity using id
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="repository">Repository</param>
        /// <returns>Returns entity or null</returns>
        protected virtual TEntity GetSingleEntity<TEntity>(object id, IRepository<TEntity> repository)
            where TEntity : class, IEntity, new()
        {
            //Validazione argomenti
            if (id == null) throw new ArgumentNullException(nameof(id));

            //In transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Eseguo il salvataggio dell'oggetto
                var result = repository.GetSingle(e => e.GetId() == id);
                t.Commit();
                return result;
            }
        }

		/// <summary>
		/// Get single entity using provided expression and repository instance
		/// </summary>
		/// <typeparam name="TEntity">Type of entity</typeparam>
		/// <param name="filter">Filter</param>
		/// <param name="repository">Repository instance</param>
		/// <returns>Returns entity or null</returns>
		protected TEntity GetSingleEntity<TEntity>(Expression<Func<TEntity, bool>> filter, IRepository<TEntity> repository)
			where TEntity : class, IEntity, new()
		{
			//Validazioni argomenti
			if (filter == null) throw new ArgumentNullException(nameof(filter));
			if (repository == null) throw new ArgumentNullException(nameof(repository));

			//Eseguo in transazione
			using (var t = DataSession.BeginTransaction())
			{
				//Recupero i dati, commit ed uscita
				var result = repository.GetSingle(filter);
				t.Commit();
				return result;
			}
		}

        /// <summary>
        /// FetchAndProject list of entities of provided type
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="filter">Filter</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sort">Sort expression</param>
        /// <param name="isDescending">Is descending</param>
        /// <param name="repository">Repository instance</param>
        /// <returns>Returns list of elements</returns>
        protected IList<TEntity> FetchEntities<TEntity>(Expression<Func<TEntity, bool>> filter,
			int? startRowIndex, int? maximumRows,
			Expression<Func<TEntity, object>> sort, bool isDescending,
			IRepository<TEntity> repository)
			where TEntity : class, IEntity, new()
		{
			//Validazioni argomenti
			if (repository == null) throw new ArgumentNullException(nameof(repository));

			//Eseguo in transazione
			using (var t = DataSession.BeginTransaction())
			{
				//Recupero i dati, commit ed uscita
				var result = repository.Fetch(filter, startRowIndex, maximumRows, sort, isDescending);
				t.Commit();
				return result;
			}
		}

		/// <summary>
		/// Count entities of provided type
		/// </summary>
		/// <typeparam name="TEntity">Type of entity</typeparam>
		/// <param name="filter">Filter</param>
		/// <param name="repository">Repository instance</param>
		/// <returns>Returns count of elements</returns>
		protected int CountEntities<TEntity>(Expression<Func<TEntity, bool>> filter,
			IRepository<TEntity> repository)
			where TEntity : class, IEntity, new()
		{
			//Validazioni argomenti
			if (repository == null) throw new ArgumentNullException(nameof(repository));

			//Eseguo in transazione
			using (var t = DataSession.BeginTransaction())
			{
				//Recupero i dati, commit ed uscita
				var result = repository.Count(filter);
				t.Commit();
				return result;
			}
		}
	}
}
