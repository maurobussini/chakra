using System;
using System.IO;

namespace ZenProgramming.Chakra.Core.Utilities.Server.Redirectors
{
    /// <summary>
    /// Represents override of standard stream that add tracing data to output
    /// </summary>
    internal class TraceStreamWriter : StreamWriter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">Active stream</param>
        public TraceStreamWriter(Stream stream) 
            : base(stream) { }

        /// <summary>
        /// Execute format of messagge adding date/time
        /// </summary>
        /// <param name="message">Messagge to format</param>
        /// <returns>Returns formatted message</returns>
        private string FormatMessage(string message)
        {
            DateTime registrationDate = DateTime.Now;

            //Compongo la stringa di uscita comprensiva di data, ora e messaggio da tracciare
            string result =
                $"[{registrationDate.ToString("yyyy/MM/dd")} {registrationDate.ToString("HH:mm:ss")}.{registrationDate.Millisecond.ToString().PadLeft(3, '0')}] {message}";

            //Ritorno il messaggio formattato
            return result;
        }

        /// <summary>
        /// Write a text on stream output
        /// </summary>
        /// <param name="value">Text to write</param>
        public override void WriteLine(string value)
        {
            //Formatto il messaggio e uso la funzione base
            string message = FormatMessage(value);
            base.WriteLine(message);
        }
    }
}
