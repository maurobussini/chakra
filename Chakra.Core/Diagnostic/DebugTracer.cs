namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Tracer for write in Visual Studio debug window
    /// </summary>
    public class DebugTracer : TracerBase
    {
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
            //Utilizzo il tracciamento del debugger
            System.Diagnostics.Debug.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Trace an warning message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceWarn(string formattedMessage)
        {
            //Utilizzo il tracciamento del debugger
            System.Diagnostics.Debug.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceError(string formattedMessage)
        {
            //Utilizzo il tracciamento del debugger
            System.Diagnostics.Debug.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected override void TraceDebug(string formattedMessage)
        {
            //Utilizzo il tracciamento del debugger
            System.Diagnostics.Debug.WriteLine(formattedMessage);
        }
    }
}
