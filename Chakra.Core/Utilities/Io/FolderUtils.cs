using System;
using System.IO;

namespace ZenProgramming.Chakra.Core.Utilities.Io
{
    /// <summary>
    /// Utilities for manage folders
    /// </summary>
    public static class FolderUtils
    {
        /// <summary>
        /// Check if specified folder exists, create it if needed and return to caller
        /// </summary>
        /// <param name="target">Target folder</param>
        /// <returns>Returns folder full path</returns>
        public static string GetOrCreateFolder(string target)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(target)) throw new ArgumentNullException(nameof(target));

            //Recupero il valore della configurazione
            string folder = target;

            //Se il percorso è assoluto, lo utilizzo nativamente, altrimenti
            //lo combino con il folder di esecuzione per ottenere quello assocluto
            if (!Path.IsPathRooted(folder))
                folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);

            //Se la folder non esiste, eseguo la sua creazione
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            //Ritorno la directory creata o composta
            return folder;
        }
    }
}
