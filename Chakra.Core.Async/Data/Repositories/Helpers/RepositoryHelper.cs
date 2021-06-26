using System;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.Async.Data.Repositories.Helpers
{
    /// <summary>
    /// Helper for repository
    /// </summary>
    public static class RepositoryHelper
    {
        
        /// <summary>
        /// Execute create or update of specified entity
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity to save</param>
        /// <param name="dataSession">Data session</param>
        /// <param name="saveMethod">Save method</param>
        public static Task SaveAsync<TEntity>(TEntity entity, IDataSession dataSession, Func<IDataSession,Task> saveMethod)
            where TEntity : class, IEntity
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));
            if (saveMethod == null) throw new ArgumentNullException(nameof(saveMethod));

            // check/update entity values and properties
            ZenProgramming.Chakra.Core.Data.Repositories.Helpers.RepositoryHelper.ValidateAndRectify(entity, dataSession);

            //Mando in esecuzione il metodo di salvataggio
            return saveMethod(dataSession);
        }
        
            /// <summary>
        /// Resolve a repository dependency using provided
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <typeparam name="TSpecificDataSessionRepository">Type of interface of repository specific for data session implementation</typeparam>
        /// <param name="dataSession">Active data session</param>
        /// <returns>Returns concrete instance of repository</returns>
        public static TRepositoryInterface Resolve<TRepositoryInterface, TSpecificDataSessionRepository>(IDataSessionAsync dataSession)
            where TRepositoryInterface : IRepositoryAsync
            where TSpecificDataSessionRepository: IRepositoryAsync
        {
            #region NEW VERSION (2.0.11)
            ////Utilizzo il metodo base
            //return (TRepositoryInterface)Resolve<TSpecificDataSessionRepository>(typeof(TRepositoryInterface), dataSession);
            #endregion

            #region OLD VERSION (2.0.10)

            //Tento il recupero del tipo implementato dall'interfaccia
            var implementedType = ZenProgramming.Chakra.Core.Data.Repositories.Helpers.RepositoryHelper.FindImplementedType<TRepositoryInterface, TSpecificDataSessionRepository>();

            //Se non ho nessun elemento, emetto eccezione
            if (implementedType == null)
                throw new InvalidProgramException("Unable to find concrete types that " +
                                                  $"implements repository interface '{typeof(TRepositoryInterface).FullName}' and data session interface '{typeof(TSpecificDataSessionRepository).FullName}' on current application " +
                                                  "domain. Please verify also that implementation if marked with attribute '[Repository]'.");

            //Eseguo la creazione dell'istanza della classe di repository specifico
            object instance = Activator.CreateInstance(implementedType, dataSession);

            //Se l'istanza non è convertibile a repostory, emetto eccezione
            if (!(instance is TRepositoryInterface))
                throw new InvalidCastException($"Unable to cast type of '{instance.GetType().FullName}' to " +
                                               $"interface '{typeof(TRepositoryInterface).FullName}'.");

            //Ritorno l'istanza
            return (TRepositoryInterface)instance;

            #endregion
        }

    }
}
