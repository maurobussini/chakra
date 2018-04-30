using System;
using System.Collections.Generic;
using System.Linq;

namespace ZenProgramming.Chakra.Core.Extensions
{
    /// <summary>
    /// Contains extensions for enumerable elements
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Set paging on specific enumerable
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <returns>Returns overloaded enumerable</returns>
        public static IEnumerable<TEntity> Paging<TEntity>(this IEnumerable<TEntity> instance, int? startRowIndex, int? maximumRows)
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Predispongo la lista di uscita
            var outList = instance;

            //Se ho impostato la riga di inizio, la aggiungo
            if (startRowIndex != null)
                outList = outList.Skip(startRowIndex.Value);

            //Se ho impostato la riga di fine, la aggiungo
            if (maximumRows != null)
                outList = outList.Take(maximumRows.Value);

            //Ritorno la query paginata
            return outList;
        }        

        /// <summary>
        /// Executes action for each element on list
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="instance">Input elements</param>
        /// <param name="action">Action to execute</param>
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Scorro tutti gli elementi nella lista ed eseguo
            //l'operazione specificata sul singolo elemento
            foreach (var currentElement in instance)
                action(currentElement);
        }

        /// <summary>
        /// Converts all elements from source list to target
        /// </summary>
        /// <typeparam name="TInput">Type of source</typeparam>
        /// <typeparam name="TOutput">Type of target</typeparam>
        /// <param name="instance">Source instance</param>
        /// <param name="convertFunction">Convert function</param>
        /// <returns>Returns output list</returns>
        public static IList<TOutput> As<TInput, TOutput>(this IEnumerable<TInput> instance, Func<TInput, TOutput> convertFunction)
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (convertFunction == null) throw new ArgumentNullException(nameof(convertFunction));

            //Creo una lista di uscita
            IList<TOutput> outList = new List<TOutput>();

            //Itero su ciascun elemento nella lista sorgente e lo converto
            instance.Each(s => outList.Add(convertFunction(s)));

            //Mando in uscita la lista
            return outList;
        }

    }
}
