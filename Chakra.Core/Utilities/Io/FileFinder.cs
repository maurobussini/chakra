using System;
using System.Collections.Generic;
using System.IO;

namespace ZenProgramming.Chakra.Core.Utilities.Io
{
    /// <summary>
    /// Finder used to search specific files on 
    /// specified directory and subdirectories
    /// </summary>
    public class FileFinder
    {
        #region Public events
        /// <summary>
        /// Event raised when a file is found
        /// </summary>
        public event EventHandler<FileInfo> ElementFound;
        #endregion

        #region Public properties
        /// <summary>
        /// Base search path
        /// </summary>
        public string SearchPath { get; private set; }

        /// <summary>
        /// Search pattern
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// Results of search
        /// </summary>
        public IList<FileInfo> Results { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchPath">Search path</param>
        /// <param name="pattern">Search pattern</param>
        public FileFinder(string searchPath, string pattern)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(searchPath)) throw new ArgumentNullException(nameof(searchPath));
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException(nameof(pattern));

            //Imposto i valori nelle proprietà
            SearchPath = searchPath;
            Pattern = pattern;

            //Inizializzo i risultati
            Results = new List<FileInfo>();
        }

        /// <summary>
        /// Start search on specified path
        /// </summary>
        /// <returns>Returns result of search</returns>
        public IList<FileInfo> Search()
        {
            //Eseguo la pulizia dei risultati
            Results = new List<FileInfo>();

            //Eseguo la ricerca sulla root
            Search(SearchPath);

            //Mando in uscita i risultati della ricerca
            return Results;
        }

        /// <summary>
        /// Execute search on specified path
        /// </summary>
        /// <param name="folderPath">Folder path</param>
        private void Search(string folderPath)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));

            //Se la directory non esiste, emetto eccezione
            if (!Directory.Exists(folderPath)) 
                throw new DirectoryNotFoundException($"Unable to find directory '{folderPath}'.");

            //Recupero le informazioni della folder
            DirectoryInfo directory = new DirectoryInfo(folderPath);

            //Eseguo la ricerca dei file nella cartella corrente
            FileInfo[] files = directory.GetFiles(Pattern, SearchOption.TopDirectoryOnly);

            //Accodo i risultati in uscita
            foreach (var fileInfo in files)
            {
                //Aggiungo il risultato alla lista
                Results.Add(fileInfo);

                //Scateno l'evento di file trovato
                if (ElementFound != null)
                    ElementFound(this, fileInfo);
            }

            //Eseguo la ricerca delle directory di primo livello
            DirectoryInfo[] directories = directory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);

            //Eseguo l'iterazione delle funzione ricorsivamente
            foreach (var directoryInfo in directories)
                Search(directoryInfo.FullName);
        }
    }
}
