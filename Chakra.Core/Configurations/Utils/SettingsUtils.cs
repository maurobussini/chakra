using System;
using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Configurations.Utils
{
    /// <summary>
    /// Contains utilities for settings
    /// </summary>
    public static class SettingsUtils
    {
        /// <summary>
        /// Execute switch of actions using value of provided key, invoking
        /// the action specified on mapping if value matches
        /// </summary>
        /// <param name="settingValue">Setting value</param>
        /// <param name="mappings">Mappings</param>
        public static void Switch<TValue>(TValue settingValue, Dictionary<TValue, Action> mappings)
        {
            //Validazione argomenti
            if (mappings == null) throw new ArgumentNullException(nameof(mappings));

            //Se il valore è nullo, esco
            if (settingValue == null)
                return;

            //Recupero l'elemento di mappatura
            var mappingValue = mappings[settingValue];

            //Invoco la funzione
            mappingValue();
        }
    }
}
