using System;
using System.Collections.Generic;
using System.Linq;

namespace ZenProgramming.Chakra.Core.Persistences.Extensions
{
	/// <summary>
	/// Extensions for enumerables
	/// </summary>
	public static class PersistenceEnumerableExtensions
	{
		/// <summary>
		/// Get single element using its key (throws exception if not found)
		/// </summary>
		/// <typeparam name="TPersistence">Type of persistence</typeparam>
		/// <param name="instance">Instance</param>
		/// <param name="key">Key</param>
		/// <returns>Returns instance</returns>
		public static TPersistence SingleByKey<TPersistence>(this IEnumerable<TPersistence> instance, string key)
			where TPersistence : class, IPersistence
		{
			//Validazione argomenti
			if (instance == null) throw new ArgumentNullException(nameof(instance));
			if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

			//Utilizzo il metodo base
			var result = SingleOrDefaultByKey(instance, key);

			//Se non è stato trovato, eccezione
			if (result == null)
				throw new NullReferenceException(string.Format("Unable to find " + 
					"persistence of type '{0}' with key '{1}'", 
					typeof(TPersistence).FullName, key));

			//Ritorno il valore
			return result;
		}

		/// <summary>
		/// Get single element or null using its key (null if not found)
		/// </summary>
		/// <typeparam name="TPersistence">Type of persistence</typeparam>
		/// <param name="instance">Instance</param>
		/// <param name="key">Key</param>
		/// <returns>Returns instance or null</returns>
		public static TPersistence SingleOrDefaultByKey<TPersistence>(this IEnumerable<TPersistence> instance, string key)
			where TPersistence : class, IPersistence
		{
			//Validazione argomenti
			if (instance == null) throw new ArgumentNullException(nameof(instance));
			if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

			//Recupero l'elemento per codice (se esiste)
			return instance.SingleOrDefault(p => p.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
