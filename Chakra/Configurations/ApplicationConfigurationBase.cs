using System;
using System.Configuration;
using ZenProgramming.Chakra.Configurations.Attributes;
using ZenProgramming.Chakra.Core.Reflection;

namespace ZenProgramming.Chakra.Configurations
{
    /// <summary>
    /// Represents base class for create application configuration
    /// </summary>
    /// <typeparam name="TApplicationConfiguration">Type of configuration class for self injection</typeparam>
    public abstract class ApplicationConfigurationBase<TApplicationConfiguration> : ConfigurationSection
        where TApplicationConfiguration : ApplicationConfigurationBase<TApplicationConfiguration>
    {
        #region Private fields
        private static readonly Lazy<TApplicationConfiguration> CurrentInstance = new Lazy<TApplicationConfiguration>(GetConfiguration);
        #endregion

        #region Public properties
        /// <summary>
        /// Current instance of configuration
        /// </summary>
        public static TApplicationConfiguration Current 
        {
            get { return CurrentInstance.Value; } 
        }
        #endregion        

        /// <summary>
        /// Generate a lazy-load configurazion section using provided data
        /// </summary>
        /// <returns>Returns instance of configuration section</returns>
        private static TApplicationConfiguration GetConfiguration()
        {
            //Eseguo il recupero dell'attributo con il nome della configurazione
            ConfigurationSectionAttribute attribute = ReflectionUtils.
                GetSingleAttribute<ConfigurationSectionAttribute>(typeof(TApplicationConfiguration), false);

            //Se l'attributo non è stato specificato, emetto eccezione
            if (attribute == null)
                throw new InvalidProgramException(string.Format("Attribute of type '{0}' is required on type '{1}'.", 
                    typeof(ConfigurationSectionAttribute).FullName, typeof(TApplicationConfiguration).FullName));

            //Procedo alla lettura del file di configurazione applicativo
            TApplicationConfiguration local = ConfigurationManager.GetSection(attribute.TagName) as TApplicationConfiguration;

            //Se non ho recuperaro un oggetto valido, emetto un'eccezione
            if (local == null)
                throw new ConfigurationErrorsException(string.Format("Unable to recognize a " +
                    "valid configuration section for '{0}' in application configuration file.", attribute.TagName));
            
            //Ritorno la configurazione
            return local;
        }

		/// <summary>
		/// Get custom value from application configuration, converts the value to the 
		/// specified type and throw an exception if key does not exist or can not be converted
		/// </summary>
		/// <typeparam name="TResult">Output type</typeparam>
		/// <param name="key">Key for the value to recover</param>
		/// <param name="throwOnError">If false the exception are supressed</param>
		/// <param name="defaultValue">Default value if configuration is not specified</param>
		/// <returns>Returns the recovered value</returns>
		public TResult GetSettingsValue<TResult>(string key, bool throwOnError, TResult defaultValue)
		{
			try
			{
				//Imposto nell'oggetto la chiave recuperata dall'AppSettings
				object configValue = ConfigurationManager.AppSettings[key];

				//Se la chiave non è nulla, converto il valore e lo ritorno
				if (configValue != null)
					return (TResult)Convert.ChangeType(configValue, typeof(TResult));

				//Se è stato chiesto di scatenare l'errore avvio l'eccezione
				//di chiave non trovata, altrimenti ritorno il default del valore
				if (throwOnError)
					throw new ConfigurationErrorsException("Configuration key '" + key + "' cannot be found in section 'appSettings' " +
						"or contained value cannot be converted to type '" + typeof(TResult).FullName + "'.");

				//Ritorno il default
				return default(TResult);
			}
			catch (ConfigurationErrorsException)
			{
				//Se è stata impostata l'opzione di "throw" scateno all'esterno l'errore
				//altrimenti ritorno il valore di default del tipo specificato
				if (throwOnError)
					throw;

				//Ritorno il valore
				return defaultValue;
			}
		}
	}
}
