using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ZenProgramming.Chakra.Core.Utilities.Data
{
    /// <summary>
    /// Contains utilities for manage app domain
    /// </summary>
    public static class AppDomainUtils
    {
        /// <summary>
        /// Fetch list of deployed assemblies in "bin" folder for current 
        /// app domain; the methods uses a new app domain in order to 
        /// avoid unwanted loading of assemblies on current domain
        /// </summary>
        /// <returns>Returns list of deployed assemblyies</returns>
        public static IList<Assembly> FetchDeployedAssemblies()
        {
            //Eseguo la creazione di un nuovo appdomain
            AppDomain domain = AppDomain.CreateDomain("TemporaryAppDomain");

            //Recupero tutti i percorsi delle dll dell'app domain
            IList<string> paths = FetchBinFolders();
            IList<AssemblyName> assemblyNames = new List<AssemblyName>();

            //Scorro tutti i percorsi dei "bin"
            foreach (var currentPath in paths)
            {
                //Recupero gli assemblynames trovati
                var names = FetchAssemblyNames(currentPath);

                //Accodo i nomi se non già presenti
                foreach (var currentName in names)
                    if (assemblyNames.All(c => c.FullName != currentName.FullName))
                        assemblyNames.Add(currentName);
            }

            //Predispongo la lista di assemblies di uscita
            var allAssemblies = new List<Assembly>();

            //Eseguo il caricamento di tutti gli assembly nel nuovo app domain
            foreach (var currentAssemblyName in assemblyNames)
            {
                //Carico l'assembly utilizzando il nome
                var loadedAssembly = domain.Load(currentAssemblyName);

                //Accodo l'assembly alla lista
                allAssemblies.Add(loadedAssembly);
            }

            //Emetto tutti gli assembly ordinati per nome
            return allAssemblies
                .OrderBy(a => a.FullName)
                .ToList();
        }

        /// <summary>
        /// Fetch list of assembly names contained in specified folder
        /// </summary>
        /// <param name="path">Source folder</param>
        /// <returns>Returns list of assembly names</returns>
        public static IList<AssemblyName> FetchAssemblyNames(string path)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            //Recupero la lista dei file ".dll" e ".exe" contenuti nella directory corrente
            FileInfo[] files = new DirectoryInfo(path)
                .GetFiles("*.*", SearchOption.AllDirectories)
                .Where(file => file.Name.ToLower().EndsWith(".dll") || 
                               file.Name.ToLower().EndsWith(".exe"))
                .ToArray();

            //Predispongo la lista di uscita
            IList<AssemblyName> outList = new List<AssemblyName>();

            //Scorro tutti i file individuati nella folder
            foreach (var currentFile in files)
                outList.Add(AssemblyName.GetAssemblyName(currentFile.FullName));

            //Mando un uscita gli elementi
            return outList;
        }

        /// <summary>
        /// Fetch list of "bin" folders of current application domain
        /// </summary>
        /// <returns>Returns list of bin folders</returns>
        public static IList<string> FetchBinFolders()
        {
            //Predispongo la lista di uscita
            List<string> toReturn = new List<string>();

            //Recupero la cartella di esecuzione dell'appdomain
            string appDomainBinFolder = AppDomain.CurrentDomain.BaseDirectory;

            //Se la cartella esiste, la aggiungo
            if (Directory.Exists(appDomainBinFolder))
                toReturn.Add(appDomainBinFolder);

            //Se siamo in contesto web, viene tornata la "root"
            //del web site, quindi devi includere la "bin" 
            string binWebApp = Path.Combine(appDomainBinFolder, "Bin");

            //Se la cartella esiste, la aggiungo in uscita
            if (Directory.Exists(binWebApp))
                toReturn.Add(appDomainBinFolder);

            //Emetto la lista di elementi
            return toReturn;
        }
    }
}
