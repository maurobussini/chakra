using System;
using System.IO;
using System.Reflection;

namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Represents tracer for file on disk
    /// </summary>
    public class FileTracer : TracerBase
    {
        #region Private fields
        private FileStream _File;
        private StreamWriter _Writer;        
        #endregion

        #region Public properties
        /// <summary>
        /// File name target of tracings
        /// </summary>
        public string Filename { get; private set; }
        #endregion

        /// <summary>
        /// Get format of trace message
        /// </summary>
        public override string TraceFormat
        {
            get { return "[{time}] : {kind} - {message}"; }
        }

        /// <summary>
        /// Trace an information message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceInfo(string formattedMessage)
        {
            _Writer.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Trace an warning message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceWarn(string formattedMessage)
        {
            _Writer.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceError(string formattedMessage)
        {
            _Writer.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceDebug(string formattedMessage)
        {
            _Writer.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public FileTracer()
        {
            //Determino il percorso del file di default e quello da usare
            Filename = GetFilename();
            
            //Tento di aprire il file di destinazione in scrittura e lo stream
            _File = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Write);
            _Writer = new StreamWriter(_File) { AutoFlush = true };
        }

        /// <summary>
        /// Generate target file name
        /// </summary>
        /// <returns>Returns file full path</returns>
        private string GetFilename()
        {
            //Inizio l'iterazione con il default
            string filename = GetDefaultFilename();
            int progress = 0;

            //Recupero le informazioni del file di default
            FileInfo info = new FileInfo(filename);
            if (info == null) throw new NullReferenceException(
                $"Unable to retrieve information about file '{filename}'.");

            //Eseguo l'operazione finchè esiste il file
            while (File.Exists(filename))
            {
                //Se non ho informazioni sulla directory, emetto eccezione
                if (info.Directory == null)
                    throw new NullReferenceException(
                        $"Unable to retrieve information of directory of file '{filename}'.");

                //Compongo il nuovo file utilizzando la directory del default
                //inserendo il progressivo e alla fine l'estensione
                filename = Path.Combine(info.Directory.FullName, 
                    info.Name.Replace(info.Extension, "." + progress) + info.Extension);

                //Incremento il progressivo
                progress++;
            }

            //Ritorno il file composto
            return filename;
        }

        /// <summary>
        /// Generate default file name
        /// </summary>
        /// <returns>Returns file full path</returns>
        private string GetDefaultFilename()
        {
            //Recupero il nome dell'assembly corrente
            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;

            //Compongo il nome del file di trace da emettere
            string traceFileName = $"{assemblyName}.log";

            //Utilizzo come percorso base la cartella di esecuzione
            string path = AppDomain.CurrentDomain.BaseDirectory;

            //Eseguo la creazione della cartella App_Data se non esiste;
            //questo anche se siamo in ambiente windows poichè "App_Data" 
            //sarebbe comunque locata nella root applicativa
            string appData = Path.Combine(path, "App_Data");
            if (!Directory.Exists(appData)) Directory.CreateDirectory(appData);
            
            //Cerco e creo la cartella "Logs" dove sarà archiviato il file
            string logs = Path.Combine(appData, "Logs");
            if (!Directory.Exists(logs)) Directory.CreateDirectory(logs);

            //Compongo il percorso del file
            path = Path.Combine(logs, traceFileName);

            //Mando in uscita il percorso
            return path;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                //Rilascio le risorse locali
                _Writer.Dispose();
                _File.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }
    }
}
