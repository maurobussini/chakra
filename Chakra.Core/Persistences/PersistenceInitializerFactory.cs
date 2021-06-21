using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Reflection;

namespace ZenProgramming.Chakra.Core.Persistences
{
    /// <summary>
    /// Static 
    /// </summary>
    public static class PersistenceInitializerFactory
    {
		/// <summary>
		/// Element cache
		/// </summary>
        private static readonly IDictionary<Type, IPersistenceInitializer> _Cache = new ConcurrentDictionary<Type, IPersistenceInitializer>();

        private static IList<Type> _AllPersistenceInitializerTypes;
        
        /// <summary>
        /// Resolves initializer for provided type
        /// </summary>
        /// <typeparam name="TElement">Type of element</typeparam>
        /// <returns>Returns initializer instance</returns>
        private static IPersistenceInitializer ResolveInitializer<TElement>()
        {
            //Se non ho la lista dei tipi, la genero
            if (_AllPersistenceInitializerTypes == null)
                _AllPersistenceInitializerTypes = ExportableTypeDiscoverer.Instance
                    .FetchExportedTypesImplementingInterface<IPersistenceInitializer>();

            //Creo un'istanza dei tipi inizializzati
            IList<IPersistenceInitializer> initializers = new List<IPersistenceInitializer>();

            //Istanzio ogni tipo
            foreach (var currentType in _AllPersistenceInitializerTypes)
            {
                //Creo l'istanza
                object instance = Activator.CreateInstance(currentType);

                //Se l'istanza non è convertibile a Ipers, emetto eccezione
                if (!(instance is IPersistenceInitializer))
                    throw new InvalidCastException($"Unable to cast type of '{instance.GetType().FullName}' to " +
                                                   $"interface '{typeof(IPersistenceInitializer).FullName}'.");

                //Accodo in lista
                initializers.Add(instance as IPersistenceInitializer);
            }

            //Scorro tutti i tipi e procedo alla creazione delle istanze
            foreach (var currentPersistenceInitializer in initializers)
            {
                //Accodo l'inizializzatore nella cache
                if (!_Cache.ContainsKey(currentPersistenceInitializer.ElementType))
                    _Cache.Add(currentPersistenceInitializer.ElementType, currentPersistenceInitializer);

                //Se il tipo di elemento è quello richiesto, lo emetto ed esco
                if (typeof(TElement) == currentPersistenceInitializer.ElementType)
                    return currentPersistenceInitializer;
            }

            //Se ho scorso tutti gli elementi ma non ho trovato il tipo, esco
            return null;
        }

        /// <summary>
        /// Fetch and project list of elements registered with initializer
        /// </summary>
        /// <typeparam name="TElement">Type of element</typeparam>
        /// <returns>Returns list of elements</returns>
        public static IList<TElement> Fetch<TElement>()
			where TElement: class, IPersistence
        {
            //Predispongo l'inizializzatore
            IPersistenceInitializer initializer = null;

            //Se l'elemento richiesto è già presente in cache
            if (_Cache.ContainsKey(typeof (TElement)))
                initializer = _Cache[typeof (TElement)];

            //Se l'inizializzatore non è stato trovato
            if (initializer == null)
            {
                //Tento la sua risoluzione
                initializer = ResolveInitializer<TElement>();
            }

            //Se l'inizializzatore non è stato trovato, emetto eccezione
            if (initializer == null) throw new InvalidProgramException(
                $"Unable to find a persistence initializer for type '{typeof(TElement).FullName}'. Please verify that " +
                "the initializer (if exists) has been marked with attribute " + "[PersistenceInitializer].");

            //Ritorno il valore
            return initializer.Fetch<TElement>();
        }
    }
}
