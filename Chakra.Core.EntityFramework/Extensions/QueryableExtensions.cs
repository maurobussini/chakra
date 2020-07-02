using System;
using System.Linq;

namespace ZenProgramming.Chakra.Core.EntityFramework.Extensions
{
    /// <summary>
    /// Contains extensions for IQueryable elements
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Set paging on specific enumerable
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="startRowIndex">Start row index</param>
        /// <param name="maximumRows">Maximum rows</param>
        /// <returns>Returns overloaded enumerable</returns>
        public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> instance, int? startRowIndex, int? maximumRows)
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
    }
}
