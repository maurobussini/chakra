using System;
using System.Collections.Generic;
using System.Configuration;

namespace ZenProgramming.Chakra.Configurations.Utils
{
	/// <summary>
	/// Utilities for manage app settings
	/// </summary>
	public class AppSettingsUtils
	{
		/// <summary>
		/// Execute switch of actions using value of provided key, invoking
		/// the action specified on mapping if value matches
		/// </summary>
		/// <param name="appSettingsKey">App settings ket</param>
		/// <param name="mappings">Mappings</param>
		public static void Switch(string appSettingsKey, Dictionary<string, Action> mappings)
		{
			//Validazione argomenti
			if (string.IsNullOrEmpty(appSettingsKey))
				throw new ArgumentNullException(nameof(appSettingsKey));

			//Recupero la key dalla configurazione
			var config = ConfigurationManager.AppSettings[appSettingsKey];
			if (config == null)
				throw new ConfigurationErrorsException(string.Format("Unable to find " +
					"app settings entry with value '{0}'", appSettingsKey));

			//Verifico se nel dizionario esiste il valore richiesto
			if (!mappings.ContainsKey(config))
				throw new ConfigurationErrorsException(string.Format("Unable to find " +
					"suitable key on mappings matching value '{0}' of key '{1}'", config, appSettingsKey));

			//Recupero l'elemento di mappatura
			var mappingValue = mappings[config];

			//Invoco la funzione
			mappingValue();
		}
	}
}
