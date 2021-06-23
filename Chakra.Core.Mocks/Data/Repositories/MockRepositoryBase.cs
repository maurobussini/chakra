using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Data.Extensions;
using ZenProgramming.Chakra.Core.Mocks.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Data.Repositories
{
    /// <summary>
    /// Base class for repositories with mock engine
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TScenarioInterface">Type of scenario interface (ex: IChakraScenario)</typeparam>
    public abstract class MockRepositoryBase<TEntity, TScenarioInterface> : IRepository<TEntity>, IMockRepository
        where TEntity : class, IEntity, new()
        where TScenarioInterface: IScenario
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Protected properties
        /// <summary>
        /// Get mock data session
        /// </summary>
        protected IMockDataSession DataSession { get; }

        /// <summary>
        /// List of mocked entities
        /// </summary>
        protected IList<TEntity> MockedEntities { get; }

		/// <summary>
		/// Used scenario instance
		/// </summary>
		protected TScenarioInterface Scenario { get; }
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataSession">Active data session</param>
		/// <param name="entitiesExpression">Entities expression</param>
		protected MockRepositoryBase(IDataSession dataSession, Func<TScenarioInterface, IList<TEntity>> entitiesExpression)        
        {
            //Arguments validation
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));

            //Convert to IMockDataSession
            var mockupSession = dataSession.AsMockDataSession();

            //Get scenario from data session
            IScenario scenarioFromDataSession = mockupSession.GetScenario();

            //Try cast of scenario to provided interface
            if (!(scenarioFromDataSession is TScenarioInterface castedScenario))
                throw new InvalidCastException("Scenario contained on data session is of " + 
                    $"type '{scenarioFromDataSession.GetType().FullName}' and cannot be converted "+ 
                    $"to type '{typeof(TScenarioInterface).FullName}'.");

            //Set data session and scenario
            DataSession = mockupSession;
            Scenario = castedScenario;

            //Initialize working mocked entities
            MockedEntities = entitiesExpression(Scenario);
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
        /// Fetch list of entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public IList<TEntity> Fetch(Expression<Func<TEntity, bool>> filterExpression = null,  int? startRowIndex = null, 
            int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
        {            
            //Query base
            IList<TEntity> query = MockedEntities;
                
            //Se ho un filtro, compilo l'expression e lo imposto
            if (filterExpression != null)
            {
                //Compilo l'espressione di filtro
                var compiled = filterExpression.Compile();

                //Filtro e converto
                query = query
                    .Where(compiled)
                    .ToList();
            }            

            //Se specificato, applico anche il sort
            if (sortExpression != null)
            {
                //Compilo l'espressione di sort
                var compiledSort = sortExpression.Compile();

                //Accodo all'enumeble precedente
                query = isDescending
                    ? query.OrderByDescending(compiledSort).ToList()
                    : query.OrderBy(compiledSort).ToList();
            }

            //Imposto la paginazione
            query = query.Paging(startRowIndex, maximumRows).ToList();

            //Eseguo l'estrazione dati
            return query.ToList();
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
        public IList<TProjection> FetchWithProjection<TProjection>(Expression<Func<TEntity, TProjection>> select, Expression<Func<TEntity, bool>> filterExpression = null,
            Expression<Func<TProjection, bool>> selectFilterExpression = null, int? startRowIndex = null, int? maximumRows = null,
            Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false)
        {
            //Query base
            IList<TEntity> query = MockedEntities;
                
            //Se ho un filtro, compilo l'expression e lo imposto
            if (filterExpression != null)
            {
                //Compilo l'espressione di filtro
                var compiled = filterExpression.Compile();

                //Filtro e converto
                query = query
                    .Where(compiled)
                    .ToList();
            }            

            //Se specificato, applico anche il sort
            if (sortExpression != null)
            {
                //Compilo l'espressione di sort
                var compiledSort = sortExpression.Compile();

                //Accodo all'enumeble precedente
                query = isDescending
                    ? query.OrderByDescending(compiledSort).ToList()
                    : query.OrderBy(compiledSort).ToList();
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
        public int Count(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            //Query base
            var query = MockedEntities;

            //Se ho un filtro, compilo l'expression e lo imposto
            if (filterExpression != null)
            {
                //Compilo l'espressione di filtro
                var compiled = filterExpression.Compile();

                //Filtro e converto
                query = query
                    .Where(compiled)
                    .ToList();
            }

            //Recupero il numero totale di elementi
            return query.Count();
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
                    //Remove element
                    Delete(entity);
                }

                //Accodo l'entità
                MockedEntities.Add(entity);


                //ATTENTION! In case of update, do nothing because
                //entity is already on the list and the reference is updated
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
                return;
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
            var onMemory = MockedEntities.SingleOrDefault(e => e.GetId().Equals(entity.GetId()));

            //Se trovata, la rimuovo
            if (onMemory != null)
                MockedEntities.Remove(onMemory);
        }

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MockRepositoryBase()
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
