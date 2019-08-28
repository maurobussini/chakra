using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Chakra.Core.Windows.WindowsServices.Utils
{
    /// <summary>
    /// Installer class for windows services
    /// </summary>
    public static class WindowsServiceInstaller
    {
        private const string ServiceCommandExecutable = "sc.exe";
        private const string ServiceCommandCreate = "create {0} displayname=\"{1}\" binpath=\"{2}\"";
        private const string ServiceCommandDescription = "description {0} \"{1}\"";
        private const string ServiceCommandDelete = "delete {0}";
        private const string ServiceCommandStart = "start {0}";
        private const string ServiceCommandStop = "stop {0}";

        /// <summary>
        /// Install provided service on current platform (Windows only)
        /// </summary>
        /// <param name="serviceInstance">Instance of service class</param>
        /// <returns>Returns list of validations; no validation on correct</returns>
        public static IList<ValidationResult> Install(IManagedWindowsService serviceInstance)
        {
            //Validazione argomenti
            if (serviceInstance == null) throw new ArgumentNullException(nameof(serviceInstance));

            //Funzione base passando i valori estratti dal builder
            var installValidations = LaunchServiceCommand(serviceInstance, (file, name, displayName, description) 
                => string.Format(ServiceCommandCreate, name, displayName, file));

            //Se ho validazioni fallite, esco
            if (installValidations.Count > 0)
                return installValidations;

            //In caso contrario lancio il comando per la descrizione
            return LaunchServiceCommand(serviceInstance, (file, name, displayName, description)
                => string.Format(ServiceCommandDescription, name, description));
        }

        /// <summary>
        /// Uninstall provided service on current platform (Windows only)
        /// </summary>
        /// <param name="serviceInstance">Instance of service class</param>
        /// <returns>Returns list of validations; no validation on correct</returns>
        public static IList<ValidationResult> Uninstall(IManagedWindowsService serviceInstance)
        {
            //Validazione argomenti
            if (serviceInstance == null) throw new ArgumentNullException(nameof(serviceInstance));

            //Funzione base passando i valori estratti dal builder
            return LaunchServiceCommand(serviceInstance, (file, name, displayName, description)
                => string.Format(ServiceCommandDelete, name));
        }

        /// <summary>
        /// Start provided service on current platform (Windows only)
        /// </summary>
        /// <param name="serviceInstance">Instance of service class</param>
        /// <returns>Returns list of validations; no validation on correct</returns>
        public static IList<ValidationResult> Start(IManagedWindowsService serviceInstance)
        {
            //Validazione argomenti
            if (serviceInstance == null) throw new ArgumentNullException(nameof(serviceInstance));

            //Funzione base passando i valori estratti dal builder
            return LaunchServiceCommand(serviceInstance, (file, name, displayName, description)
                => string.Format(ServiceCommandStart, name));
        }

        /// <summary>
        /// Stop provided service on current platform (Windows only)
        /// </summary>
        /// <param name="serviceInstance">Instance of service class</param>
        /// <returns>Returns list of validations; no validation on correct</returns>
        public static IList<ValidationResult> Stop(IManagedWindowsService serviceInstance)
        {
            //Validazione argomenti
            if (serviceInstance == null) throw new ArgumentNullException(nameof(serviceInstance));

            //Funzione base passando i valori estratti dal builder
            return LaunchServiceCommand(serviceInstance, (file, name, displayName, description)
                => string.Format(ServiceCommandStop, name));
        }

        /// <summary>
        /// Launch service command (sc.exe) with provided arguments 
        /// </summary>
        /// <param name="serviceInstance">Service instance</param>
        /// <param name="commandBuilder">Command builder</param>
        /// <returns>Returns validations</returns>
        private static IList<ValidationResult> LaunchServiceCommand(IManagedWindowsService serviceInstance, 
            Func<string, string, string, string, string> commandBuilder)
        {
            //Validazione argomenti
            if (serviceInstance == null) throw new ArgumentNullException(nameof(serviceInstance));
            if (commandBuilder == null) throw new ArgumentNullException(nameof(commandBuilder));

            //Lista delle validazioni di uscita
            var validations = new List<ValidationResult>();

            //Se il sistema operativo non è Windows, esco
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //Aggiunta della validazione ed uscita
                validations.Add(new ValidationResult("Install of service is supported on Windows only"));
                return validations;
            }

            //Recupero il tipo dell'istanza del servizio
            Type serviceType = serviceInstance.GetType();

            //Recupero l'assembly e il suo percorso
            var assembly = serviceType.Assembly;
            var folder = assembly.Location;
            var file = $"{folder.Remove(folder.Length - 4)}.exe";

            //Se il file non è stato trovato, esco
            if (!File.Exists(file))
            {
                //Aggiunta della validazione ed uscita
                validations.Add(new ValidationResult($"Executable service file '{file}' could not be found"));
                return validations;
            }

            //Richiamo il builder per il comando
            var command = commandBuilder(file, serviceInstance.ServiceName, 
                serviceInstance.DisplayName, serviceInstance.Description);

            //Esecuzione con processo esterno
            var process = new Process
            {
                //Informazioni di avvio
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = ServiceCommandExecutable,
                    Arguments = command
                }
            };

            //Avvio dell'esecuzione e attesa completamento
            process.Start();
            process.WaitForExit();

            //Lista di validazioni vuote (tutto ok)
            return validations;
        }
    }
}
