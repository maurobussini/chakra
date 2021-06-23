using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ZenProgramming.Chakra.Core.Async.Data.Repositories;
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
    public abstract class MongoDbRepositoryBaseAsync<TEntity, TMongoDbOptions> :
        MongoDbRepositoryBase<TEntity, TMongoDbOptions>,
        IRepositoryAsync<TEntity>
        where TEntity : class, IEntity, new()
        where TMongoDbOptions: class, IMongoDbOptions, new()
    {

        protected MongoDbRepositoryBaseAsync(IDataSession dataSession) : base(dataSession){}
        /// <summary>
        /// Assign primary identifier to newely created entity
        /// </summary>
        /// <param name="repository">MongoDb repository</param>
        /// <param name="entity">Entity instance</param>
        async Task AssignPrimaryIdentifierAsync(TEntity entity)
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
                var count = await CountAsync();

                //Predisposizione al massimo valore
                int max = 0;

                //Se non ho elementi il massimo è 0; se ho già
                //elementi devo contare il massimo già presente
                if (count != 0)
                {
                    //Conteggio il massimo
                    max = await MaxAsync<int>("Id");
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
        /// <param name="repository">MongoDb repository</param>
        /// <param name="fieldName">Field name</param>
        /// <returns>Returns value</returns>
        async Task<TOutput> MaxAsync<TOutput>(string fieldName)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            //Definizione del campo su cui lavorare
            var idFieldDefinition = new StringFieldDefinition<TEntity, object>(fieldName);

            //Recupero il primo elemento con valore massimo
            var elementWithMaxValue = await Collection
                .Find(Builders<TEntity>.Filter.Empty)
                .Sort(Builders<TEntity>.Sort.Descending(idFieldDefinition))
                .Limit(1)
                .FirstOrDefaultAsync();

            //Se l'elemento non esiste, ritorno il default
            if (elementWithMaxValue == null)
                return default;

            //Recupero il valore della proprietà            
            return (TOutput) elementWithMaxValue.GetId();
        }

        /// <summary>
        /// Get single entity using expression
        /// </summary>
        /// <param name="repository">MongoDb repository</param>
        /// <param name="expression">Search expression</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            //Validazione argomenti
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            //Eseguo l'estrazione dell'elemento singolo
            return Collection
                .FindSync(expression)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Fetch list of entities matching criteria on repository
        /// </summary>
        /// <param name="repository">MongoDb repository</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<List<TEntity>> FetchAsync(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null, int? maximumRows = null,
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
            return query.ToListAsync();
        }

        /// <summary>
        /// Fetch with projection list of entities matching criteria on repository
        /// </summary>
        /// <param name="repository">MongoDb repository</param>
        /// <param name="select">Select expression</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <param name="selectFilterExpression">Select filter expression</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <param name="sortExpression">Filter expression</param>
        /// <param name="isDescending">Is descending sorting</param>
        /// <returns>Returns list of all available entities</returns>
        public Task<List<TProjection>> FetchWithProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> @select, Expression<Func<TEntity, bool>> filterExpression = null,
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

            return projectionQuery.ToListAsync();
        }

        /// <summary>
        /// Count entities matching criteria on repository
        /// </summary>
        /// <param name="repository">MongoDb repository</param>
        /// <param name="filterExpression">Filter expression</param>
        /// <returns>Returns count</returns>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression = null)
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
                countAsLong = await (filterExpression != null
                    ? Collection.CountDocumentsAsync(filterExpression)
                    : Collection.CountDocumentsAsync(Builders<TEntity>.Filter.Empty));

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
        /// <param name="repository">MongoDb repository</param>
        /// <param name="entity">Entity to save</param>
        public Task SaveAsync(TEntity entity)
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Utilizzo l'helper per il salvataggio
            return RepositoryHelper.SaveAsync(entity, DataSession, async s =>
            {
                //Se l'entità ha un identificatore nullo, aggiungo
                if (entity.GetId() == null)
                {
                    //Eseguo l'assegnazione dell'identificatore primario
                    await AssignPrimaryIdentifierAsync(entity);

                    //Inserisco ed esco
                    await Collection.InsertOneAsync(entity);
                    return;
                }

                //In caso contrario, compongo il filtro per individuare
                //univocamente l'entità in modo che possa essere rimpiazzata
                FieldDefinition<TEntity, object> idFieldDefinition = new StringFieldDefinition<TEntity, object>("Id");
                var uniqueFilter = Builders<TEntity>.Filter.Eq(idFieldDefinition, entity.GetId());

                //Eseguo il replace
                await Collection.FindOneAndReplaceAsync(uniqueFilter, entity);
            });
        }

        /// <summary>
        /// Executes delete of entity
        /// </summary>
        /// <param name="repository">MongoDb repository</param>
        /// <param name="entity">Entity to delete</param>
        public Task DeleteAsync(TEntity entity)
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
            return Collection.DeleteOneAsync(uniqueFilter);
        }
    }
}
