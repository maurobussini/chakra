using System;
using System.IO;

namespace ZenProgramming.Chakra.Core.Utilities.Server.Redirectors
{
    /// <summary>
    /// Represents a redirector for console output
    /// </summary>
    public class ConsoleFileRedirector : IDisposable
    {
        #region Private fields
        private FileStream _File;
        private StreamWriter _Writer;
        private TextWriter _StandardWriter;
        #endregion

        #region Public properties
        /// <summary>
        /// Get path of output file
        /// </summary>
        public string OutputFile { get; private set; }
        #endregion

        /// <summary>
        /// Redirect console output to a target file
        /// </summary>
        /// <param name="outputFile">Output file</param>
        public ConsoleFileRedirector(string outputFile)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(outputFile)) throw new ArgumentNullException(nameof(outputFile));

            //Imposto i valori nelle mirror
            OutputFile = outputFile;

            //Tento di aprire il file di destinazione in scrittura e lo stream
            _File = new FileStream(OutputFile, FileMode.OpenOrCreate, FileAccess.Write);
            _Writer = new TraceStreamWriter(_File) { AutoFlush = true };

            //Segnalo che l'output sarà ridirezionato sul file
            ConsoleUtils.WriteColorLine(ConsoleColor.Blue, "Console output will be redirected to file '{0}'...", OutputFile);

            //Salvo il writer di default
            _StandardWriter = Console.Out;

            //Imposto l'output sul file
            Console.SetOut(_Writer);
        }

        /// <summary>
        /// Execute release of current resources
        /// redirecting console out to standard
        /// </summary>
        public void Dispose()
        {
            //Eseguo la chiusura dello stream            
            _Writer.Close();
            _File.Close();

            //Imposto l'output standard
            Console.SetOut(_StandardWriter);

            //Segnalo all'utente che il file è stato salvato nella lo
            ConsoleUtils.WriteColorLine(ConsoleColor.Blue, "Output file save in '{0}'.", OutputFile);
        }
    }
}
