using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Reflection;

namespace ZenProgramming.Chakra.Core.Utilities.Data
{
    /// <summary>
    /// Utilities for enumerations
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Get list of all possibile values in enum
        /// </summary>
        /// <typeparam name="TEnum">Type of enumeration</typeparam>
        /// <returns>Returns enumeration values</returns>
        public static IEnumerable<TEnum> Enumerate<TEnum>()
            where TEnum : struct
        {
            //Recupero l'elenco dei valori nell'enumerazione
            Array values = Enum.GetValues(typeof (TEnum));

            //Itero tutti i valori dell'enumerazione ed emetto il singolo valore
            foreach (TEnum current in values)
                yield return current;
        }

        /// <summary>
        /// Get name of enum element using "DisplayAttribute" applied on it
        /// </summary>
        /// <typeparam name="TEnum">Type of enum</typeparam>
        /// <param name="value">Value of enum</param>
        /// <returns>Returns name or null</returns>
        public static string GetName<TEnum>(TEnum value)
            where TEnum : struct
        {
            //Recupero l'istanza dell'attributo
            DisplayAttribute displayAttribute = ReflectionUtils.GetSingleAttribute<TEnum, DisplayAttribute>(value);

            //Se non ho l'attributo, ritorno il "ToString" del valore
            return displayAttribute == null
                ? value.ToString()
                : displayAttribute.Name;
        }
    }
}
