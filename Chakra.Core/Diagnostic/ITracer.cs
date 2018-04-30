using System;

namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Represents public interface for a tracer
    /// </summary>
    public interface ITracer : IDisposable
    {
        /// <summary>
        /// Get format of trace message
        /// </summary>
        string TraceFormat { get; }

        /// <summary>
        /// Trace an information message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        void Info(string message, params object[] parameters);

        /// <summary>
        /// Trace a warning message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        void Warn(string message, params object[] parameters);

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        void Error(string message, params object[] parameters);

        /// <summary>
        /// Trace a debug message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        void Debug(string message, params object[] parameters);        
    }
}
