using System;
using ZenProgramming.Chakra.Core.Utilities.Server;

namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Represents tracer for standard console
    /// </summary>
    public class ConsoleTracer : TracerBase
    {
        /// <summary>
        /// Get format of trace message
        /// </summary>
        public override string TraceFormat
        {
            get { return "[{time}] {message}"; }
        }
        
        /// <summary>
        /// Trace an information message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceInfo(string formattedMessage)
        {
            //Utilizzo il tracciamento indicando il colore
            ConsoleUtils.WriteColorLine(ConsoleColor.White, formattedMessage);
        }

        /// <summary>
        /// Trace an warning message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceWarn(string formattedMessage)
        {
            //Utilizzo il tracciamento indicando il colore
            ConsoleUtils.WriteColorLine(ConsoleColor.Yellow, formattedMessage);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceError(string formattedMessage)
        {
            //Utilizzo il tracciamento indicando il colore
            ConsoleUtils.WriteColorLine(ConsoleColor.Red, formattedMessage);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceDebug(string formattedMessage)
        {
            //Utilizzo il tracciamento indicando il colore
            ConsoleUtils.WriteColorLine(ConsoleColor.Cyan, formattedMessage);
        }
    }
}
