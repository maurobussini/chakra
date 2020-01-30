using System;
using System.Collections.Generic;
using System.Linq;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Extensions;

namespace ZenProgramming.Chakra.Core.Mocks.Scenarios.Extensions
{
    /// <summary>
    /// Contains extensions of interface "IScenario"
    /// </summary>
    public static class ScenarioExtensions
    {
        /// <summary>
        /// Executes cast of specified instance to target type
        /// </summary>
        /// <typeparam name="TScenario">Target type</typeparam>
        /// <param name="instance">Instance</param>
        /// <returns>Returns casted type</returns>
        public static TScenario OfType<TScenario>(this IScenario instance)
            where TScenario : IScenario
        {
            //Arguments validation
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            try
            {
                //Tento il cast esplicito, all'interno di un blocco
                //try/catch per gestire eventuali problemi di conversione
                return (TScenario)instance;
            }
            catch (InvalidCastException exc)
            {
                //Sollevo l'evento di cast invalido
                throw new InvalidCastException(string.Format("Unable to " + 
                    "cast instance of type '{0}' to target type '{1}'.", 
                    instance.GetType().FullName, typeof(TScenario).FullName), exc);
            }
        }

        /// <summary>
        /// Pushes provided entity to specified list
        /// </summary>
        /// <typeparam name="TEntity">Entity or contract</typeparam>
        /// <typeparam name="TScenario">Type of scenario</typeparam>
        /// <param name="instance">Scenario instance</param>
        /// <param name="targetCollection">Target collection</param>
        /// <param name="entityToAppend">Entity or contract to push</param>
        public static void Push<TEntity, TScenario>(this TScenario instance, Func<TScenario, IList<TEntity>> targetCollection, TEntity entityToAppend)
            where TEntity : class, new()
            where TScenario : IScenario
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (targetCollection == null) throw new ArgumentNullException(nameof(targetCollection));
            if (entityToAppend == null) throw new ArgumentNullException(nameof(entityToAppend));

            //Recupero la collezione target
            var collection = targetCollection(instance);

            //Tento la conversione ad IEntity
            var modernEntity = entityToAppend as IModernEntity;

            //Se l'elemento è un una ModernEntity e il suo id non è già valorizzato
            if (modernEntity != null && string.IsNullOrEmpty(modernEntity.Id))
            {
                //Procedo generando l'identificatore
                string generatedId = GenerateNextId(collection);

                //Applico sull'entità
                modernEntity.Id = generatedId;
            }
            else
            {
                //Se è un'entità classica con intero
                IEntity<int> classic = entityToAppend as IEntity<int>;
                if (classic != null)
                {
                    //Genero il nuovo id utilizzando il massimo già presente ed aggiungendo 1
                    int? max = collection.Count == 0
                        ? 0
                        : collection
                            .Cast<IEntity<int>>()
                            .Max(e => e.Id);

                    //Se trovo un max nullo, emetto eccezione
                    if (max == null) throw new InvalidProgramException("Found entity with invalid id.");

                    //Incremento il massimo di uno ed assegno
                    classic.Id = max + 1;
                }
            }


            //Aggiungo l'entità in coda
            collection.Add(entityToAppend);
        }

        /// <summary>
        /// Pushes provided list of entities to specified list
        /// </summary>
        /// <typeparam name="TEntity">Type of entity or contract</typeparam>
        /// <typeparam name="TScenario">Type of scenario</typeparam>
        /// <param name="instance">Scenario instance</param>
        /// <param name="targetCollection">Target collection</param>
        /// <param name="multipleEntitiesToAppend">List of entities to push</param>
        public static void Push<TEntity, TScenario>(this TScenario instance, Func<TScenario, IList<TEntity>> targetCollection, params TEntity[] multipleEntitiesToAppend)
            where TEntity : class, new()
            where TScenario : IScenario
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (targetCollection == null) throw new ArgumentNullException(nameof(targetCollection));
            if (multipleEntitiesToAppend == null) throw new ArgumentNullException(nameof(multipleEntitiesToAppend));

            //Iterazione sulle entità in lista e utilizzo del metodo base
            multipleEntitiesToAppend.Each(e => Push(instance, targetCollection, e));
        }

        /// <summary>
        /// Generates next unique progressive identifier
        /// using entity type and list of already created entities
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="actualList">Currently archived entities</param>
        /// <returns>Returns generated id</returns>
        private static string GenerateNextId<TEntity>(IList<TEntity> actualList)
        {
            //Conteggio le entità in lista
            int count = actualList.Count;

            //Progressivo paddato
            string progressive = (count + 1).ToString().PadLeft(4, '0');

            //Il pattern è il tipo con un progressivo 4 cifre
            return $"{typeof(TEntity).Name}-{progressive}";
        }
    }
}
