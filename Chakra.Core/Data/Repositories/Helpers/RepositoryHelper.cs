using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using ZenProgramming.Chakra.Core.DataAnnotations;
using ZenProgramming.Chakra.Core.DataAnnotations.Extensions;
using ZenProgramming.Chakra.Core.Entities;
using ZenProgramming.Chakra.Core.Reflection;
using ZenProgramming.Chakra.Core.Security;

namespace ZenProgramming.Chakra.Core.Data.Repositories.Helpers
{
    /// <summary>
    /// Helper for repository
    /// </summary>
    public static class RepositoryHelper
    {
        /// <summary>
        /// Execute a validation on properties of the entity specified
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <param name="dataSession">Session holder used for database validation</param>
        /// <returns>Returns a list of validaton results</returns>
        public static IList<ValidationResult> Validate<TModelEntity>(TModelEntity entity, IDataSession dataSession)
            where TModelEntity : class, IEntity
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //Eseguo la creazione di una lista vuota per contenere gli errori di validazione
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            //Creo un'istanza del contesto di validazione (passando la sessione nel contesto)
            IDictionary<object, object> items = new Dictionary<object, object>();
            items.Add(DataValidationAttributeBase.DataSessionKey, dataSession);
            ValidationContext validationContext = new ValidationContext(entity, null, items);

            //Eseguo la valiazione dell'entità e mando in uscita la lista di errori di validazione
            Validator.TryValidateObject(entity, validationContext, validationResults, true);

            //Mando in uscita la lista
            return validationResults;
        }        

        /// <summary>
        /// Execute create or update of specified entity
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity to save</param>
        /// <param name="dataSession">Data session</param>
        /// <param name="saveMethod">Save method</param>
        public static void Save<TEntity>(TEntity entity, IDataSession dataSession, Action<IDataSession> saveMethod)
            where TEntity : class, IEntity
        {
            //Se non è passato un dato valido, emetto eccezione
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (dataSession == null) throw new ArgumentNullException(nameof(dataSession));
            if (saveMethod == null) throw new ArgumentNullException(nameof(saveMethod));

            //Eseguo la validazione dell'entità da salvare
            IList<ValidationResult> validationResults = Validate(entity, dataSession);

            //Emetto eccezione nel caso in cui non sia possibile eseguire una validazione corretta
            if (validationResults.Count > 0)
                throw new InvalidOperationException(string.Format("The entity '{0}' identified by value '{1}' " + 
                    "cannot be validated and changes won't be commited on database. Please invoke 'Validate' method before " +
                    "submit changes on data domain. Invalid data : '{2}'", entity.GetType().FullName,
                    entity.GetId(), validationResults.ToValidationSummary()));   
         
            //Se l'entità è "Rich", applico le informazioni di sistema
            if (entity is IRichEntity)
            {
                //Eseguo la conversione
                IRichEntity richEntity = (IRichEntity) entity;

                //Recupero il principal corrente. Se non è valido o anonimo
                //imposto stringa vuota, altrimenti il nome di autenticazione
                IPrincipal currentPrincipal = IdentityUtils.ResolvePrincipal();
                string principalName = currentPrincipal == null
                    ? null
                    : currentPrincipal.Identity.IsAuthenticated
                        ? currentPrincipal.Identity.Name
                        : null;

                //Se l'entità non è mai stata salvata sulla base dati, 
                //devo impostare le informazioni di sistema
                if (richEntity.GetId() == null)
                {
                    //Imposto la data corrente per l'inserimento
                    richEntity.CreationTime = DateTime.UtcNow;

                    //Se non ho già un utente, imposto il principal
                    if (string.IsNullOrEmpty(richEntity.CreatedBy))
                        richEntity.CreatedBy = principalName;
                }

                //In tutti i casi imposto la data di aggiornamento
                richEntity.LastUpdateTime = DateTime.UtcNow;

                //Se non ho già un utente, imposto il principal
                if (string.IsNullOrEmpty(richEntity.LastUpdateBy))
                    richEntity.LastUpdateBy = principalName;
            }

            //Mando in esecuzione il metodo di salvataggio
            saveMethod(dataSession);
        }

        /// <summary>
        /// Repository interface map
        /// </summary>
        private class RepositoryInterfaceMap
        {
            /// <summary>
            /// Repository interface type
            /// </summary>
            public Type RepositoryInterfaceType { get; set; }

            /// <summary>
            /// Data session type
            /// </summary>
            public Type DataSessionType { get; set; }

            /// <summary>
            /// Repository implementation type
            /// </summary>
            public Type RepositoryImplementationType { get; set; }
        }        

        /// <summary>
        /// Resolve a repository dependency using provided
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <typeparam name="TSpecificDataSessionRepository">Type of interface of repository specific for data session implementation</typeparam>
        /// <param name="dataSession">Active data session</param>
        /// <returns>Returns concrete instance of repository</returns>
        public static TRepositoryInterface Resolve<TRepositoryInterface, TSpecificDataSessionRepository>(IDataSession dataSession)
            where TRepositoryInterface : IRepository
            where TSpecificDataSessionRepository: IRepository
        {
            #region NEW VERSION (2.0.11)
            ////Utilizzo il metodo base
            //return (TRepositoryInterface)Resolve<TSpecificDataSessionRepository>(typeof(TRepositoryInterface), dataSession);
            #endregion

            #region OLD VERSION (2.0.10)

            //Tento il recupero del tipo implementato dall'interfaccia
            var implementedType = FindImplementedType<TRepositoryInterface, TSpecificDataSessionRepository>();

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

        /// <summary>
        /// Resolve a repository dependency using provided
        /// </summary>
        /// <typeparam name="TSpecificDataSessionRepository">Type of interface of repository specific for data session implementation</typeparam>
        /// <param name="repositoryInterface">Type of repository</param>
        /// <param name="dataSession">Active data session</param>
        /// <returns>Returns concrete instance of repository</returns>
        public static IRepository Resolve<TSpecificDataSessionRepository>(Type repositoryInterface, IDataSession dataSession)
            where TSpecificDataSessionRepository: IRepository
        {
            //Tento il recupero del tipo implementato dall'interfaccia
            var implementedType = FindImplementedType<TSpecificDataSessionRepository>(repositoryInterface);

            //Se non ho nessun elemento, emetto eccezione
            if (implementedType == null)
                throw new InvalidProgramException("Unable to find concrete types that " +
                                                  $"implements repository interface '{repositoryInterface.FullName}' and data session interface '{typeof(TSpecificDataSessionRepository).FullName}' on current application " +
                                                  "domain. Please verify also that implementation if marked with attribute '[Repository]'.");

            //Eseguo la creazione dell'istanza della classe di repository specifico
            object instance = Activator.CreateInstance(implementedType, dataSession);

            //Se l'istanza non è convertibile a repostory, emetto eccezione
            if (!(instance.GetType() == repositoryInterface))
                throw new InvalidCastException($"Unable to cast type of '{instance.GetType().FullName}' to " +
                                               $"interface '{repositoryInterface.FullName}'.");

            //Ritorno l'istanza
            return (IRepository)instance;
        }

        /// <summary>
        /// Maps for repository types
        /// </summary>
        private static readonly Lazy<IList<RepositoryInterfaceMap>> _RepositoryInterfaceMaps = new Lazy<IList<RepositoryInterfaceMap>>(InitializeRepositoryInterfaceMap);

        /// <summary>
        /// Initialize repositories interface maps
        /// </summary>
        /// <returns>Returns list of mappings</returns>
        private static IList<RepositoryInterfaceMap> InitializeRepositoryInterfaceMap() 
        {
            //Istanzio il discover dei tipi esportati con MEF e recupero
            //l'elenco dei tipi che esportano l'interfaccia IRepository
            //var repositoryTypes = ExportableTypeDiscoverer.Catalog.GetExports<IRepository>();
            //ExportableTypeDiscoverer discoverer = new ExportableTypeDiscoverer();
            //var repositoryTypes = discoverer.GetExportedTypes<IRepository>().ToList();
            var repositoryTypes = ExportableTypeDiscoverer.Instance.FetchExportedTypesImplementingInterface<IRepository>();

            //Creo la lista e la inizializzo
            IList<RepositoryInterfaceMap> maps = new List<RepositoryInterfaceMap>();

            //Scorro tutti gli elementi esportati e creo gli oggetti
            foreach (var currentType in repositoryTypes)
                maps.Add(new RepositoryInterfaceMap{ RepositoryImplementationType = currentType });

            //Ritorno la mappa
            return maps;
        }

        /// <summary>
        /// Find implemented type
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <typeparam name="TSpecificDataSessionRepository">Type of interface of repository specific for data session implementation</typeparam>
        /// <returns>Returns implemented type or null</returns>
        private static Type FindImplementedType<TRepositoryInterface, TSpecificDataSessionRepository>()
        {
            #region NEW VERSION (2.0.11)
            ////Utilizzo il metodo overloaded
            //return FindImplementedType<TSpecificDataSessionRepository>(typeof(TRepositoryInterface));
            #endregion

            #region OLD VERSION (2.0.10)
            //Scorro tutte le mappature esistenti (che non hanno riferimenti all'interfaccia
            //specifica di DataSession e all'interfaccia del repository, ma solo all'implementazione)
            IList<Type> matchingTypes = new List<Type>();
            foreach (var currentMap in _RepositoryInterfaceMaps.Value)
            {
                //Recupero le interfacce
                var inters = currentMap.RepositoryImplementationType.GetInterfaces();

                //Verifico se esiste un'interfaccia nella lista che corrisponde al tipo
                //generico dell'interfaccia di repository richiesto (es. "IUserRepository") e se 
                //esiste un'interfaccia che corrisponde al topo della data session (es. "MockupDataSession")
                bool hasGeneric = inters.Any(i => i == typeof(TRepositoryInterface));
                bool hasSpecifi = inters.Any(i => i == typeof(TSpecificDataSessionRepository));

                //Se il tipo corrente non ha entrambi i match, passo al prossimo
                if (!hasGeneric || !hasSpecifi)
                    continue;

                //Aggiungo alla lista dei match e aggiorno la mappatura
                matchingTypes.Add(currentMap.RepositoryImplementationType);
                currentMap.DataSessionType = typeof(TSpecificDataSessionRepository);
                currentMap.RepositoryInterfaceType = typeof(TRepositoryInterface);
            }

            //Se ho più di un elemento, emetto eccezione
            if (matchingTypes.Count > 1)
                throw new InvalidProgramException($"Found {matchingTypes.Count} types that implements repository " +
                    $"interface '{typeof(TRepositoryInterface).FullName}' and data session interfacce " + 
                    $"'{typeof(TSpecificDataSessionRepository).FullName}'. Just one type matching specified " +
                    "criteria must be contained on application domain.");

            //Ritorno il primo elemento (o nessuno)
            return matchingTypes.SingleOrDefault();
            #endregion
        }

        /// <summary>
        /// Find implemented type using provided generic repository class and specific type
        /// </summary>
        /// <typeparam name="TSpecificDataSessionRepository">Type of interface of repository specific for data session implementation</typeparam>
        /// <returns>Returns implemented type or null</returns>
        private static Type FindImplementedType<TSpecificDataSessionRepository>(Type repositoryType)
        {
            //Validazione argomenti
            if (repositoryType == null) throw new ArgumentNullException(nameof(repositoryType));

            //Scorro tutte le mappature esistenti (che non hanno riferimenti all'interfaccia
            //specifica di DataSession e all'interfaccia del repository, ma solo all'implementazione)
            IList<Type> matchingTypes = new List<Type>();
            foreach (var currentMap in _RepositoryInterfaceMaps.Value)
            {
                //Recupero le interfacce
                var inters = currentMap.RepositoryImplementationType.GetInterfaces();

                //Verifico se esiste un'interfaccia nella lista che corrisponde al tipo
                //generico dell'interfaccia di repository richiesto (es. "IUserRepository") e se 
                //esiste un'interfaccia che corrisponde al topo della data session (es. "MockupDataSession")
                bool hasGeneric = inters.Any(i => i == repositoryType);
                bool hasSpecifi = inters.Any(i => i == typeof(TSpecificDataSessionRepository));

                //Se il tipo corrente non ha entrambi i match, passo al prossimo
                if (!hasGeneric || !hasSpecifi)
                    continue;

                //Aggiungo alla lista dei match e aggiorno la mappatura
                matchingTypes.Add(currentMap.RepositoryImplementationType);
                currentMap.DataSessionType = typeof(TSpecificDataSessionRepository);
                currentMap.RepositoryInterfaceType = repositoryType;
            }

            //Se ho più di un elemento, emetto eccezione
            if (matchingTypes.Count > 1)
                throw new InvalidProgramException($"Found {matchingTypes.Count} types that implements repository " +
                                                  $"interface '{repositoryType.FullName}' and data session interfacce '{typeof(TSpecificDataSessionRepository).FullName}'. Just one type matching specified " +
                                                  "criteria must be contained on application domain.");

            //Ritorno il primo elemento (o nessuno)
            return matchingTypes.SingleOrDefault();
        }
    }
}
