using System;
using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Persistences
{
    /// <summary>
    /// Represents base class for persistence initializer
    /// </summary>
    /// <typeparam name="TElement">Type of persistence element</typeparam>
    public abstract class PersistenceInitializerBase<TElement> : IPersistenceInitializer
    {
        /// <summary>
        /// Element type
        /// </summary>
        public Type ElementType { get; private set; }

        /// <summary>
        /// List of elements
        /// </summary>
        public IList<TElement> Elements { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected PersistenceInitializerBase()
        {
            //Imposto il tipo dell'elemento
            ElementType = typeof (TElement);
        }

        /// <summary>
        /// Execute initialization of elements
        /// </summary>
        /// <param name="elements">Elements</param>
        /// <returns>Returns list of initialized elements</returns>
        protected abstract void Initialize(IList<TElement> elements);

        /// <summary>
        /// Fetch list of elements using initializer
        /// </summary>
        /// <typeparam name="TOutput">Type of output element</typeparam>
        /// <returns>Returns list of elements</returns>
        public IList<TOutput> Fetch<TOutput>()
			where TOutput: class, IPersistence
        {
            //Verifico che il tipo richiesto per il fetch sia lo stesso registrato
            if (typeof(TElement) != typeof(TOutput)) throw new InvalidProgramException(
                $"Persistence element '{typeof(TElement).FullName}' is different from type '{typeof(TOutput).FullName}' requested for fetch on initializer '{GetType().FullName}'.");

            //Se la lista non è inizializzata, procedo
            if (Elements == null)
            {
                //Inizializzo la collezione di elementi
                Elements = new List<TElement>();

                //Procedo all'inizializzazione dati
                Initialize(Elements);
            }

            //Eseguo la conversione della lista
            var outLits = Elements as IList<TOutput>;
            
            //Mando in uscita la lista
            return outLits;
        }
        
    }
}
