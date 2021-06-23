using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ZenProgramming.Chakra.Core.Diagnostic;
using ZenProgramming.Chakra.Core.Extensions;
using ZenProgramming.Chakra.Core.Reflection.Attributes;

namespace ZenProgramming.Chakra.Core.Reflection
{
    /// <summary>
    /// Exportable type discoverer
    /// </summary>
    public class ExportableTypeDiscoverer
    {
        ///// <summary>
        ///// Get exportable catalog
        ///// </summary>
        //public static CompositionHost Catalog => _Catalog.Value;

        ///// <summary>
        ///// Catalog holder
        ///// </summary>
        //private static readonly Lazy<CompositionHost> _Catalog = new Lazy<CompositionHost>(InitializeDiscoverer);

        /// <summary>
        /// Get singleton instance
        /// </summary>
        public static ExportableTypeDiscoverer Instance => _Instance.Value;

        /// <summary>
        /// Catalog holder
        /// </summary>
        private static readonly Lazy<ExportableTypeDiscoverer> _Instance = new Lazy<ExportableTypeDiscoverer>(InitializeDiscoverer);

        /// <summary>
        /// Initialize discoverer
        /// </summary>
        /// <returns>Returns instance</returns>
        private static ExportableTypeDiscoverer InitializeDiscoverer()
        {
            //Directory base
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //Lista di tutti i files da caricare
            var allFiles = new List<string>();

            //Lista degli assembly caricati
            var allAssemblies = new List<Assembly>
            {
                //...aggiungendo l'assembly corrente
                Assembly.GetExecutingAssembly()
            };

            //Directory "bin" per applicativi web
            var binDirectory = Path.Combine(baseDirectory, "bin");

            //Lista degli assembly caricati

            //Se esiste la cartella bin
            if (Directory.Exists(binDirectory))
            {
                //Aggiungo i file dll nella "bin" corrente
                Directory.GetFiles(binDirectory, "*.dll")
                    .Each(f => allFiles.Add(f));

                //Aggiungo i file exe nella "bin" corrente
                Directory.GetFiles(binDirectory, "*.exe")
                    .Each(f => allFiles.Add(f));
            }
            else
            {
                //Altrimenti sono nella cartella di esecuzione
                //quindi aggiungo tutti i file .dll della directory
                Directory.GetFiles(baseDirectory, "*.dll")
                    .Each(f => allFiles.Add(f));

                //Aggiungo anche gli exe
                Directory.GetFiles(baseDirectory, "*.exe")
                    .Each(f => allFiles.Add(f));
            }

            //Carico tutti i files
            foreach (var currentFile in allFiles)
            {
                try
                {
                    //Carico il file nel domain context corrente e lo aggiungo
                    var currentAssembly = Assembly.LoadFrom(currentFile);
                    allAssemblies.Add(currentAssembly);
                }
                catch (Exception exc)
                {
                    //Traccio l'eccezione di caricamento
                    Tracer.Warn($"Unable to load file {currentFile}: {exc}");
                }
            }

            ////Percorsi assemblies nella directory base
            //var baseAssemblies = Directory.GetFiles(baseDirectory, "*.dll")
            //    .Select(Assembly.LoadFrom)
            //    .ToList();

            ////Directory "bin" per applicativi web
            //var binDirectory = Path.Combine(baseDirectory, "bin");

            ////Se il percorso esiste, aggiungo
            //if (Directory.Exists(binDirectory))
            //{
            //    //Percorsi assemply nella directory "bin"
            //    var binAssemblies = Directory.GetFiles(binDirectory, "*.dll")
            //        .Select(Assembly.LoadFrom)
            //        .ToList();

            //    //Unione di tutti gli assemblies
            //    baseAssemblies.AddRange(binAssemblies);
            //}

            ////Aggiunta dell'esecutore
            //baseAssemblies.Add(Assembly.LoadFrom(
            //    Assembly.GetExecutingAssembly().Location));

            IList<Type> allTypes = new List<Type>();

            //Estraggo i tipi esportati
            foreach (var currentAssembly in allAssemblies)
            {
                try
                {
                    //Recupero i tipi esportabili dell'assembly corrente e li aggiungo
                    var currentAssemblyTypes = currentAssembly.GetExportedTypes();
                    currentAssemblyTypes.Each(t => allTypes.Add(t));
                }
                catch (Exception exc)
                {
                    //Traccio l'eccezione di caricamento
                    Tracer.Debug($"Unable to get exported types on {currentAssembly.FullName}: {exc}");
                }
            }

            ////Recupero di tutti i tipi
            //IList<Type> allTypes = allAssemblies
            //    .SelectMany(a => a.ExportedTypes)
            //    .ToList();

            //Estrazione di tutte le interfacce implementate
            var exporteTypes = allTypes
                .Where(t => ReflectionUtils
                    .FetchMultipleAttributes<ExportableAttribute>(t, false)
                    .Any())
                .ToList();

            //Creo un'istanza della classe iniettando i tipi esportati
            return new ExportableTypeDiscoverer(exporteTypes);

            #region OLD VERSION
            //var regisBuilder = new ConventionBuilder();
            //regisBuilder.ForType<IRepository>().SelectConstructor(cInfo => cInfo[0]).Export<IRepository>();
            //assemblyCatelog = new AssemblyCatalog(Assembly.GetExecutingAssembly(), regisBuilder);
            //agrCatelog = new AggregateCatalog(assemblyCatelog);


            //container = new CompositionContainer(agrCatelog, CompositionOptions.DisableSilentRejection);
            //container.ComposeExportedValue("Method", "MethodValue");
            //container.ComposeExportedValue("Version", "2.0");
            //container.ComposeParts();
            //var anniGreeting = container.GetExportedValue<Contract>();



            //var conventions = new ConventionBuilder();
            //conventions
            //    .ForTypesDerivedFrom<IRepository>()
            //    .Export<IRepository>()
            //    .SelectConstructor(ctorInfos => 
            //    {
            //        var parameterLessCtor = ctorInfos.FirstOrDefault(ci => ci.GetParameters().Length == 0);
            //        if (parameterLessCtor != null)
            //        return parameterLessCtor;
            //        else
            //            return ctorInfos.First();
            //    });


            //    //.Min(d => d.GetParameters().Length)

            ////Configuration e container
            //var configuration = new ContainerConfiguration().WithAssemblies(baseAssemblies, conventions);
            //configuration.
            //var container = configuration.CreateContainer();

            //return container;
            #endregion
        }

        /// <summary>
        /// List of exported types
        /// </summary>
        private IList<Type> ExportedTypes { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        private ExportableTypeDiscoverer(IList<Type> exporteTypes)
        {
            //Inizializzazione lista di tipi
            ExportedTypes = exporteTypes;
        }

        /// <summary>
        /// FetchAndProject list of exported types implementing interface
        /// </summary>
        /// <typeparam name="TInterface">Type of interface</typeparam>
        /// <returns></returns>
        public IList<Type> FetchExportedTypesImplementingInterface<TInterface>()
        {
            //Recupero tutti i tipi che implementano l'interfaccia specificata
            return ExportedTypes
                .Where(t => t.GetInterfaces()
                    .Any(i => i == typeof(TInterface)))
                .ToList();
        }
    }
}
