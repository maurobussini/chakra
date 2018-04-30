using System;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using ZenProgramming.Chakra.Security;

namespace ZenProgramming.Chakra.Utilities.Server
{
    /// <summary>
    /// Server utilities for service controller
    /// </summary>
    public static class ServiceControllerUtils
    {
        #region Install/uninstall
        /// <summary>
        /// Verify if specified windows service is installed
        /// </summary>
        /// <param name="windowsServiceName">Windows service name</param>
        /// <returns>Returns true if service is installed</returns>
        public static bool IsServiceInstalled(string windowsServiceName)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));

            //Ritorno la verifica se il servizio è installato
            return ServiceController.GetServices().
                Count(s => s.ServiceName == windowsServiceName) > 0;
        }

        /// <summary>
        /// Execute installation of service specified
        /// </summary>
        /// <param name="windowsServiceName">Windows service name</param>
        /// <param name="executableLocation">Executable file location</param>
        public static void InstallService(string windowsServiceName, string executableLocation)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));
            if (string.IsNullOrWhiteSpace(executableLocation))
                throw new ArgumentNullException(nameof(executableLocation));

            //Eseguo la validazione dell'eseguibile
            ValidateServiceExecutable(executableLocation);

            //Eseguo l'installazione del corrente tramite il tool
            ManagedInstallerClass.InstallHelper(new[] { executableLocation });
        }

        /// <summary>
        /// Execute uninstallation of service specified
        /// </summary>
        /// <param name="windowsServiceName">Windows service name</param>
        /// <param name="executableLocation">Executable file location</param>
        public static void UninstallService(string windowsServiceName, string executableLocation)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));
            if (string.IsNullOrWhiteSpace(executableLocation))
                throw new ArgumentNullException(nameof(executableLocation));

            //Eseguo la validazione dell'eseguibile
            ValidateServiceExecutable(executableLocation);

            //Eseguo l'installazione del corrente tramite il tool
            ManagedInstallerClass.InstallHelper(new[] { "/u", executableLocation });
        }

        /// <summary>
        /// Execute validation of specified executable file
        /// </summary>
        /// <param name="executableLocation">Executable file full path</param>
        private static void ValidateServiceExecutable(string executableLocation)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(executableLocation))
                throw new ArgumentNullException(nameof(executableLocation));

            //Verifico se il file in esecuzione è un eseguibile
            FileInfo file = new FileInfo(executableLocation);

            //Se il file specificato non è un eseguibile, emetto eccezione
            if (!file.Extension.ToLower().Contains("exe"))
                throw new InvalidOperationException(string.Format("Unable to install/uninstall " +
                    "as windows service executing file '{0}'.", executableLocation));

            //Se il file non esiste, emetto eccezione
            if (!file.Exists)
                throw new FileNotFoundException(string.Format("Unable to find windows " +
                    "service executable file at '{0}'.", executableLocation));
        }
        #endregion

        #region Start/stop/restart
        /// <summary>
        /// Execute stopping of specified windows service
        /// </summary>
        /// <param name="windowsServiceName">Name of windows service</param>
        public static void StopService(string windowsServiceName)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));

            //Se l'utente corrente non ha i permessi amministrativi, impedisco l'esecuzione
            if (!WindowsIdentityUtils.CurrentUserInRole(WindowsBuiltInRole.Administrator))
                throw new InstallException("Current user has not privileges to execute install of windows services.");

            //Recupero l'istanza del controller del servizio
            using (var controller = new ServiceController(windowsServiceName))
            {
                //Se il servizio è in esecuzione
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    //Eseguo l'operazione e attendo lo stato
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
        }

        /// <summary>
        /// Execute starting of specified windows service
        /// </summary>
        /// <param name="windowsServiceName">Name of windows service</param>
        public static void StartService(string windowsServiceName)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));

            //Se l'utente corrente non ha i permessi amministrativi, impedisco l'esecuzione
            if (!WindowsIdentityUtils.CurrentUserInRole(WindowsBuiltInRole.Administrator))
                throw new InstallException("Current user has not privileges to execute install of windows services.");

            //Recupero l'istanza del controller del servizio
            using (var controller = new ServiceController(windowsServiceName))
            {
                //Se il servizio non è in esecuzione
                if (controller.Status != ServiceControllerStatus.Running)
                {
                    //Eseguo l'operazione e attendo lo stato
                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
        }

        /// <summary>
        /// Execute restart of specified windows service
        /// </summary>
        /// <param name="windowsServiceName">Name of windows service</param>
        public static void RestartService(string windowsServiceName)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));

            //Se l'utente corrente non ha i permessi amministrativi, impedisco l'esecuzione
            if (!WindowsIdentityUtils.CurrentUserInRole(WindowsBuiltInRole.Administrator))
                throw new InstallException("Current user has not privileges to execute install of windows services.");

            //Recupero l'istanza del controller del servizio
            using (var controller = new ServiceController(windowsServiceName))
            {
                //Se il servizio è in esecuzione
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    //Eseguo l'operazione e attendo lo stato
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped);
                }

                //Se il servizio non è in esecuzione
                if (controller.Status != ServiceControllerStatus.Running)
                {
                    //Eseguo l'operazione e attendo lo stato
                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
        }

        /// <summary>
        /// Get current status for specified windows service
        /// </summary>
        /// <param name="windowsServiceName">Name of windows service</param>
        /// <returns>Returns windows service status</returns>
        public static ServiceControllerStatus GetServiceStatus(string windowsServiceName)
        {
            //Se non è stato specificato un tipo, emetto eccezione
            if (string.IsNullOrWhiteSpace(windowsServiceName))
                throw new ArgumentNullException(nameof(windowsServiceName));

            //Se il servizio non è installato, emetto eccezione
            if (!IsServiceInstalled(windowsServiceName))
                throw new NullReferenceException(string.Format("Unable to find service with " +
                    "name '{0}' on current machine.", windowsServiceName));

            //Recupero l'istanza del controller del servizio
            using (var controller = new ServiceController(windowsServiceName))
            {
                //Ritorno lo stato del servizio
                return controller.Status;
            }
        }
        #endregion
    }
}
