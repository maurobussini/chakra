using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using ZenProgramming.Chakra.Core.Reflection;
using ZenProgramming.Chakra.Services.Windows.Attributes;

namespace ZenProgramming.Chakra.Services.Windows.Install
{
    /// <summary>
    /// Represents base abstract class for installers of managed services
    /// </summary>
    public abstract class ManagedServiceInstallerBase : Installer
    {
        #region Private fields
        private IContainer components = null;
        private ServiceProcessInstaller _ServiceProcessInstaller;
        private ServiceInstaller _ServiceInstaller;
        #endregion

        protected ManagedServiceInstallerBase()
        {
            _ServiceProcessInstaller = new ServiceProcessInstaller();
            _ServiceInstaller = new ServiceInstaller();
            InitializeInstaller();
            Installers.AddRange(new Installer[] {
            _ServiceProcessInstaller,
            _ServiceInstaller});
        }

        /// <summary>
        /// Initialize installer
        /// </summary>
        private void InitializeInstaller() 
        {
            //Recupero l'attributo di servizio applicato sull'installer
            ManagedServiceTypeAttribute attribute = ReflectionUtils.GetSingleAttribute<ManagedServiceTypeAttribute>(GetType(), false);
            
            //Se non è stato trovato l'attributo, emetto eccezione
            if (attribute == null)
                throw new InvalidOperationException(string.Format("Unable to find attribute of type '{0}' " + 
                    "on service installer '{1}'. This attribute is required in order to associate installer " +
                    "to the windows service that needs to be installed.", typeof(ManagedServiceTypeAttribute).FullName, GetType().FullName));

            //Eseguo la generazione dell'istanza del servizio come base
            ServiceBase serviceInstance = Activator.CreateInstance(attribute.ManagedServiceType) as ServiceBase;

            //Se il tipo specificato nell'attributo non è subclasse del servizio, emetto eccezione
            if (serviceInstance == null)
                throw new InvalidOperationException(string.Format("Type '{0}', specified in attribute of type '{1}' on " +
                    "class '{2}' is not subclass of '{3}' windows service.", attribute.ManagedServiceType.FullName,
                    typeof(ManagedServiceTypeAttribute).FullName, GetType().FullName, typeof(ServiceBase)));

            //Eseguo la conversione del servizio nell'interfaccia
            IManagedService windowsServiceInstance = serviceInstance as IManagedService;

            //Se l'interfaccia del servizio windows non è implementata, imposto i default
            if (windowsServiceInstance != null)
            {
                //Imposto i valori recuperati direttamente dal servizio
                _ServiceProcessInstaller.Account = windowsServiceInstance.Account;
                _ServiceProcessInstaller.Password = windowsServiceInstance.Password;
                _ServiceProcessInstaller.Username = windowsServiceInstance.Username;
                _ServiceInstaller.ServiceName = windowsServiceInstance.ServiceName;
                _ServiceInstaller.DisplayName = windowsServiceInstance.DisplayName;
                _ServiceInstaller.Description = windowsServiceInstance.Description;
                _ServiceInstaller.StartType = windowsServiceInstance.StartType;
                _ServiceInstaller.ServicesDependedOn = windowsServiceInstance.ServicesDependedOn;
            }
            else
            {
                //Imposto i valori di default di installazione del servizio
                _ServiceProcessInstaller.Account = ServiceAccount.LocalSystem;
                _ServiceProcessInstaller.Password = null;
                _ServiceProcessInstaller.Username = null;
                _ServiceInstaller.ServiceName = serviceInstance.ServiceName;
                _ServiceInstaller.DisplayName = serviceInstance.ServiceName;
                _ServiceInstaller.StartType = ServiceStartMode.Manual;
            }            
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //Se siamo in dispose, procedo
            if (disposing && (components != null))
                components.Dispose();
            
            //Eseguo la dispose base
            base.Dispose(disposing);
        }
    }
}
