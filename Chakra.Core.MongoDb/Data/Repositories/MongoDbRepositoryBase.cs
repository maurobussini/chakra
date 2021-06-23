using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Data.Repositories.Helpers;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.MongoDb.Data.Options;

namespace ZenProgramming.Chakra.Core.MongoDb.Data.Repositories
{
    /// <summary>
    /// Represents base repository for access to storage based on MongoDb
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TMongoDbOptions">Type of options</typeparam>
    public abstract class MongoDbRepositoryBase<TEntity, TMongoDbOptions> : IRepository<TEntity>, IMongoDbRepository
        where TEntity : class, IEntity, new()
        where TMongoDbOptions: class, IMongoDbOptions, new()
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Public properties
        /// <summary>
        /// Get entity framework data session
        /// </summary>
        public MongoDbDataSession<TMongoDbOptions> DataSession { get; }

        /// <summary>
        /// Collection of elements managed by repository
        /// </summary>
        public IMongoCollection<TEntity> Collection { get; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Active data session</param>
        protected MongoDbRepositoryBase(IDataSession dataSession)
        {
            //Eseguo la validazione degli argomenti
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));

            //Tento il cast della sessione generica ad MongoDb
            var mongoSession = dataSession as MongoDbDataSession<TMongoDbOptions>;

            //Imposto la proprietà della sessione
            DataSession = mongoSession ?? throw new InvalidCastException(
                    $"Specified session of type '{dataSession.GetType().FullName}' cannot " + 
                    $"be converted to type '{typeof(MongoDbDataSession<TMongoDbOptions>).FullName}'.");

            //Recupero la collezione su cui lavorare
            Collection = mongoSession.Database.GetCollection<TEntity>(typeof(TEntity).Name);
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
                .FindSync(expression)
                .SingleOrDefault();
        }

        /// <summary>
        /// Fetch and project list of entities matching criteria on repository
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
            //Applico il filtro sulla collezione; se il filtro non è impostato
            //definisco un builder vuoto che ritorna sempre tutti gli elementi
            IFindFluent<TEntity, TEntity> query = filterExpression != null 
                ? Collection.Find(filterExpression)
                : Collection.Find(Builders<TEntity>.Filter.Empty);
            
            //Se ho un ordinamento
            if (sortExpression != null)
            {
                //Applico ascendente o discendente
                SortDefinition<TEntity> sortDefinition = isDescending
                    ? Builders<TEntity>.Sort.Descending(sortExpression)
                    : Builders<TEntity>.Sort.Ascending(sortExpression);

                //Applico l'ordinamento
                query = query.Sort(sortDefinition);
            }

            //Se ho impostato la riga di inizio
            if (startRowIndex != null)
                query = query.Skip(startRowIndex.Value);

            //Se ho impostato la riga di fine
            if (maximumRows != null)
                query = query.Limit(maximumRows.Value);
            
            //Ritorno il valore
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

            var query = Collection.AsQueryable();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }
            
            //Se ho un ordinamento
            if (sortExpression != null)
            {
                //Applico ascendente o discendente
                query = isDescending
                    ? query.OrderByDescending(sortExpression)
                    : query.OrderBy(sortExpression);
            }
            
            //Eseguo la proiezione dei dati
            var projectionQuery = query.Select(select);
            
            //Se ho un filtro sulla proiezione, lo imposto
            if (selectFilterExpression != null)
                projectionQuery = projectionQuery.Where(selectFilterExpression);
            
            //Se ho impostato la riga di inizio
            if (startRowIndex != null)
                projectionQuery = projectionQuery.Skip(startRowIndex.Value);

            //Se ho impostato la riga di fine
            if (maximumRows != null)
                projectionQuery = projectionQuery.Take(maximumRows.Value);
            
            //Ritorno il valore

            return projectionQuery.ToList();
        }

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public int Count(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            //Definizione del valore di uscita
            long countAsLong = 0;

            //Per mantenere la compatibilità con l'interfaccia
            //del repository base, eseguo un cast del long del mongo
            //ad intero: in caso di errore segnalo il problema
            try
            {
                //Applico una condizione sulla collezione; se il filtro non è impostato
                //definisco un builder vuoto che ritorna tutti gli elementi
                countAsLong = filterExpression != null
                    ? Collection.CountDocuments(filterExpression)
                    : Collection.CountDocuments(Builders<TEntity>.Filter.Empty);

                //Tento il cast ad intero
                return (int) countAsLong;
            }
            catch (InvalidCastException exc)
            {
                //Sollevo l'eccezione esplicativa
                throw new NotSupportedException("Result returned by MongoDb driver is a " + 
                    $"'long' with value {countAsLong} and cannot be casted as 'int'.", exc);
            }
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
                //Se l'entità ha un identificatore nullo, aggiungo
                if (entity.GetId() == null)
                {
                    //Eseguo l'assegnazione dell'identificatore primario
                    AssignPrimaryIdentifier(entity);

                    //Inserisco ed esco
                    Collection.InsertOne(entity);
                    return;
                }

                //In caso contrario, compongo il filtro per individuare
                //univocamente l'entità in modo che possa essere rimpiazzata
                FieldDefinition<TEntity, object> idFieldDefinition = new StringFieldDefinition<TEntity, object>("Id");
                var uniqueFilter = Builders<TEntity>.Filter.Eq(idFieldDefinition, entity.GetId());

                //Eseguo il replace
                Collection.FindOneAndReplace(uniqueFilter, entity);
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
            if (entity is IModernEntity modern)
            {
                //Assegno il guid generato
                modern.Id = Guid.NewGuid().ToString("D");
                return;
            }

            //Se è un'entità classica con intero
            if (entity is IEntity<int> classic)
            {
                //Conteggio gli elementi a database
                var count = Count();

                //Predisposizione al massimo valore
                int max = 0;

                //Se non ho elementi il massimo è 0; se ho già
                //elementi devo contare il massimo già presente
                if (count != 0)
                {
                    //Conteggio il massimo
                    max = Max<int>("Id");
                }

                //Incremento il massimo di uno ed assegno
                classic.Id = max + 1;
            }

            //In tutti gli altri casi emetto eccezione perchè non è una casistica implementata
            throw new NotSupportedException("Automatic assigment of primary identifier is only available " +
                                            "for entities that implements 'IModernEntity' or 'IEntity<int>'; please override this method " +
                                            "in order to provide a custom generator for primary identifier based on your needs");
        }

        /// <summary>
        /// Get maximum value on entity
        /// </summary>
        /// <typeparam name="TOutput">Type of output</typeparam>
        /// <param name="fieldName">Field name</param>
        /// <returns>Returns value</returns>
        protected TOutput Max<TOutput>(string fieldName)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            //Definizione del campo su cui lavorare
            var idFieldDefinition = new StringFieldDefinition<TEntity, object>(fieldName);

            //Recupero il primo elemento con valore massimo
            var elementWithMaxValue = Collection
                .Find(Builders<TEntity>.Filter.Empty)
                .Sort(Builders<TEntity>.Sort.Descending(idFieldDefinition))
                .Limit(1)
                .FirstOrDefault();

            //Se l'elemento non esiste, ritorno il default
            if (elementWithMaxValue == null)
                return default;

            //Recupero il valore della proprietà            
            return (TOutput) elementWithMaxValue.GetId();
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
            return results.Count == 0;
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

            //Compongo il filtro per individuare univocamente l'elemento
            FieldDefinition<TEntity, object> idFieldDefinition = new StringFieldDefinition<TEntity, object>("Id");
            var uniqueFilter = Builders<TEntity>.Filter.Eq(idFieldDefinition, entity.GetId());

            //Eseguo il remove dell'elemento
            Collection.DeleteOne(uniqueFilter);
        }

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~MongoDbRepositoryBase()
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
