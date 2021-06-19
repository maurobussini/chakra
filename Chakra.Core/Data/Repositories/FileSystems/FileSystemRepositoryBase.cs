using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using ZenProgramming.Chakra.Core.Data.FileSystems;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Extensions;

namespace ZenProgramming.Chakra.Core.Data.Repositories.FileSystems
{
    /// <summary>
    /// Repository base implementation for filesystem
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class FileSystemRepositoryBase<TEntity> : IRepository<TEntity>, IFileSystemRepository
        where TEntity : class, IEntity, new()
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Protected properties
        /// <summary>
        /// Get NHibernate data session
        /// </summary>
        protected FileSystemDataSession DataSession { get; private set; }

        /// <summary>
        /// List of mocked entities
        /// </summary>
        protected IList<TEntity> MockedEntities { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        /// <param name="baseFolder">Base folder for archive</param>
        protected FileSystemRepositoryBase(IDataSession dataSession, string baseFolder)        
        {
            //Validazione argomenti
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));
            if (string.IsNullOrEmpty(baseFolder)) throw new ArgumentNullException(nameof(baseFolder));

            //Tento il cast della sessione generica a FileSystemDataSession
            var fileSystemDataSession = dataSession as FileSystemDataSession;
            if (fileSystemDataSession == null)
                throw new InvalidCastException(
                    $"Specified session of type '{dataSession.GetType().FullName}' cannot be converted to type '{typeof(FileSystemDataSession).FullName}'.");

            //Imposto la proprietà della sessione
            DataSession = fileSystemDataSession;

            //TODO Qui i dati dovrebbero essere prelevati da filesystem, su cartelle specifiche
            //definite per convenzione. Esempio, su oggetti DataFlow o User, ci dovrebbe
            //essere una cartella base che funge da contenitore (es. 'F:\FileSystemStorage')
            //poi tante sottofolder (es. 'DataFlows', 'Users', ecc) e all'interno di esse
            //un file per ciascuna entità, in formato JSON per un rapido recupero dei dati
            //I dati, una volta i memoria, dovrebbero essere riscritti sul filesystem solo
            //nel caso in cui questi cambiano, eventualmente con task asincroni. Valutare
            //se utilizzare NanoDb per eseguire questo tipo di interazione
            MockedEntities = new List<TEntity>();
        }

        /// <summary>
        /// Count all entities on repository
        /// </summary>
        /// <returns>Returns count of all entities</returns>
        public int CountAll()
        {
            //Conteggio tutti gli elementi
            return MockedEntities.Count;
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

            //Compilo l'espressione
            var compiled = expression.Compile();

            //Cerco l'elemento nella lista delle entità mockate
            return MockedEntities
                .SingleOrDefault(compiled);
        }        

        /// <summary>
        /// Fetch list of all entities on repository
        /// </summary>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <returns>Returns list of all available entities</returns>
        public IList<TEntity> FetchAll(int? startRowIndex = null, int? maximumRows = null)
        {
            //Recupero tutte le entità
            return MockedEntities
                .Paging(startRowIndex, maximumRows)
                .ToList();
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
        public IList<TEntity> Fetch(Expression<Func<TEntity, bool>> filterExpression,  int? startRowIndex = null, 
            int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
        {
            //Validazione argomenti
            if (filterExpression == null) throw new ArgumentNullException(nameof(filterExpression));

            //Compilo l'espressione di filtro
            var compiled = filterExpression.Compile();

            //Query con filtro e paginazione
            var query = MockedEntities
                .Where(compiled);

            //Se specificato, applico anche il sort
            if (sortExpression != null)
            {
                //Compilo l'espressione di sort
                var compiledSort = sortExpression.Compile();

                //Accodo all'enumeble precedente
                query = isDescending
                    ? query.OrderByDescending(compiledSort)
                    : query.OrderBy(compiledSort);
            }

            //Eseguo l'estrazione dati
            return query.Paging(startRowIndex, maximumRows).ToList();
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
        public IList<TProjection> Fetch<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,
            Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
        {
            //Validazione argomenti
            if (filterExpression == null) throw new ArgumentNullException(nameof(filterExpression));

            //Compilo l'espressione di filtro
            var compiled = filterExpression.Compile();

            //Query con filtro e paginazione
            var query = MockedEntities
                .Where(compiled);

            //Se specificato, applico anche il sort
            if (sortExpression != null)
            {
                //Compilo l'espressione di sort
                var compiledSort = sortExpression.Compile();

                //Accodo all'enumeble precedente
                query = isDescending
                    ? query.OrderByDescending(compiledSort)
                    : query.OrderBy(compiledSort);
            }

            //Eseguo la proiezione dei dati
            var compiledSelect = select.Compile();
            var projectionQuery = query.Select(compiledSelect);

            //Se ho un filtro sulla proiezione, lo imposto
            if (selectFilterExpression != null)
            {
                var compiledSelectFilterExpression = selectFilterExpression.Compile();
                projectionQuery = projectionQuery.Where(compiledSelectFilterExpression);
            }

            return projectionQuery
                .Paging(startRowIndex, maximumRows)
                .ToList();
        }

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public int Count(Expression<Func<TEntity, bool>> filterExpression)
        {
            //Validazione argomenti
            if (filterExpression == null) throw new ArgumentNullException(nameof(filterExpression));

            //Compilo l'espressione di filtro
            var compiled = filterExpression.Compile();

            //Recupero il numero totale di elementi
            return MockedEntities
                .Count(compiled);
        }

        /// <summary>
        /// Executes save of entity on database
        /// </summary>
        /// <param name="entity">Entity to save</param>
        public void Save(TEntity entity)
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Utilizzo l'helper per eseguire l'operazione
            RepositoryHelper.Save(entity, DataSession, s =>
            {
                //Se l'entità è appena stata creata, la aggiungo
                if (entity.GetId() == null)
                {
                    //Eseguo l'assegnazione dell'identificatore primario
                    AssignPrimaryIdentifier(entity);
                }                
                else
                {
                    //Eseguo la cancellazione dalla memoria
                    Delete(entity);
                }

                //Accodo l'entità
                MockedEntities.Add(entity);
            });
        }

        /// <summary>
        /// Assign primary identifier to newely created entity
        /// </summary>
        /// <param name="entity">Entity instance</param>
        protected virtual void AssignPrimaryIdentifier(TEntity entity)
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Se è un'entità moderna, genero un GUID
            IModernEntity modern = entity as IModernEntity;
            if (modern != null)
            {
                //Assegno il guid generato
                modern.Id = Guid.NewGuid().ToString("D");
                return;
            }

            //Se è un'entità classica con intero
            IEntity<int> classic = entity as IEntity<int>;
            if (classic != null)
            {
                //Genero il nuovo id utilizzando il massimo già presente ed aggiungendo 1
                int? max = MockedEntities.Count == 0 
                    ? 0 
                    : MockedEntities
                        .Cast<IEntity<int>>()
                        .Max(e => e.Id);

                //Se trovo un max nullo, emetto eccezione
                if (max == null) throw new InvalidProgramException("Found entity with invalid id.");

                //Incremento il massimo di uno ed assegno
                classic.Id = max + 1;
            }

            //in tutti gli altri casi emetto eccezione perchè non è una casistica implementata
            throw new NotSupportedException("Automatic assigment of primary identifier is only available " +
                "for entities that implements 'IModernEntity' or 'IEntity<int>'; please override this method " + 
                "in order to provide a custom generator for primary identifier based on your needs");
        }

        /// <summary>
        /// Execute validation on the specified entity and
        /// returns a boolean result for the operation
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>If valid, returns true</returns>
        public bool IsValid(TEntity entity)
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Utilizzo il metodo di validazione
            var validations = Validate(entity);

            //E' valido se non ho validazioni errate
            return validations.Count == 0;
        }

        /// <summary>
        /// Execute a validation on properties of the entity specified
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Returns a list of validaton results</returns>
        public IList<ValidationResult> Validate(TEntity entity)
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Utilizzo l'helper per eseguire l'operazione
            return RepositoryHelper.Validate(entity, DataSession);
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public void Delete(TEntity entity)
        {
            //Validazione argomenti
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Se l'entità ha un id invalido, emetto eccezione
            if (entity.GetId() == null) throw new InvalidProgramException("Specified entity has invalid identifier.");

            //Cerco l'entità all'interno della lista di quelle mockate
            var onMemory = MockedEntities.Single(e => e.GetId() == entity.GetId());

            //Se trovata, la rimuovo
            if (onMemory != null)
                MockedEntities.Remove(onMemory);
        }

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~FileSystemRepositoryBase()
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
                //Rilascio della logica non finalizzabile
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }
    }
}
