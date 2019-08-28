using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Chakra.Core.Configurations
{
    /// <summary>
    /// Factory for read configuration of current application
    /// </summary>
    /// <typeparam name="TApplicationConfiguration">Type of configuration typized structure</typeparam>
    public static class ConfigurationFactory<TApplicationConfiguration>
        where TApplicationConfiguration: class, IApplicationConfigurationRoot, new()
    {
        /// <summary>
        /// Lazy initializer for application configuration
        /// </summary>
        private static readonly Lazy<TApplicationConfiguration> _Instance = new Lazy<TApplicationConfiguration>(Initialize);

        /// <summary>
        /// Forced valued used instead of lazy loading
        /// </summary>
        private static TApplicationConfiguration _ForcedValue = null;

        /// <summary>
        /// Singleton instance for configuration
        /// </summary>
        public static TApplicationConfiguration Instance => _Instance.Value;

        /// <summary>
        /// Set value of configuration using provided instance
        /// </summary>
        /// <param name="instance">Instance</param>
        public static void Set(TApplicationConfiguration instance)
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Imposto il valore nella variabile di forzatura
            _ForcedValue = instance;
        }

        /// <summary>
        /// Initializes application configuration
        /// </summary>
        /// <returns>Returns instance</returns>
        private static TApplicationConfiguration Initialize()
        {
            //Se il valore "forced" è inizializzato, utilizzo quello
            if (_ForcedValue != null)
                return _ForcedValue;

            //Recupero le variabili di ambiente possibili
            var aspNetCore = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var dotNetCore = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT");

            //Se ho quella asp.net utilizzo quella
            string environmentName = !string.IsNullOrEmpty(aspNetCore)
                ? aspNetCore
                : dotNetCore;

            //Default settings file
            const string DefaultAppSettings = "appsettings.json";
            const string TemplatedAppSettings = "appsettings.{0}.json";

            //Nome del file di settings
            string settingsFileName = string.IsNullOrEmpty(environmentName)
                ? DefaultAppSettings
                : string.Format(TemplatedAppSettings, environmentName).ToLower();

            //Composizione del percorso del file
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settingsFileName);

            //Se il file non esiste, imposto il default
            if (!File.Exists(path))
                settingsFileName = DefaultAppSettings;

            //Creo il builder della configurazione
            Debug.WriteLine($"Using setting file '{settingsFileName}' with environment '{environmentName}'...");
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(settingsFileName, optional: true, reloadOnChange: true);

            //Buildo la configurazione
            IConfigurationRoot configuration = builder.Build();

            //Creo una nuova instanza della classe strongly-types
            TApplicationConfiguration instance = new TApplicationConfiguration();

            //Eseguo il binding delle informazioni sulla classe statica
            configuration.Bind(instance);

            //Imposto il nome dell'environment usato
            instance.EnvironmentName = environmentName;

            //Ritorno l'istanza
            return instance;
        }
    }    
}
