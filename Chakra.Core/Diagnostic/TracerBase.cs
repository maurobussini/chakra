using System;

namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Represents base abstract class for tracing
    /// </summary>
    public abstract class TracerBase : ITracer
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        /// <summary>
        /// Get format of trace message
        /// </summary>
        public abstract string TraceFormat { get; }

        /// <summary>
        /// Trace an information message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public void Info(string message, params object[] parameters)
        {
            //Eseguo la formattazione del messaggio, quindi uso il metodo astratto
            string trace = Tracer.FormatMessage(this, DateTime.Now, "INFO", message, parameters);
            TraceInfo(trace);
        }

        /// <summary>
        /// Trace an information message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected abstract void TraceInfo(string formattedMessage);

        /// <summary>
        /// Trace a warning message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public void Warn(string message, params object[] parameters)
        {
            //Eseguo la formattazione del messaggio, quindi uso il metodo astratto
            string trace = Tracer.FormatMessage(this, DateTime.Now, "WARN", message, parameters);
            TraceWarn(trace);
        }

        /// <summary>
        /// Trace an warning message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected abstract void TraceWarn(string formattedMessage);

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public void Error(string message, params object[] parameters)
        {
            //Eseguo la formattazione del messaggio, quindi uso il metodo astratto
            string trace = Tracer.FormatMessage(this, DateTime.Now, "ERROR", message, parameters);
            TraceError(trace);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected abstract void TraceError(string formattedMessage);

        /// <summary>
        /// Trace a debug message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public void Debug(string message, params object[] parameters)
        {
            //Eseguo la formattazione del messaggio, quindi uso il metodo astratto
            string trace = Tracer.FormatMessage(this, DateTime.Now, "DEBUG", message, parameters);
            TraceDebug(trace);
        }

        /// <summary>
        /// Trace an error message
        /// </summary>
        /// <param name="formattedMessage">Formatted message</param>
        protected abstract void TraceDebug(string formattedMessage);

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~TracerBase()
		{
            //Richiamo i dispose implicito
			Dispose(false);
		}

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Eseguo una dispose esplicita
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        protected virtual void Dispose(bool isDisposing)
        {
            //Se l'oggetto è già rilasciato, esco
            if (_IsDisposed)
                return;

            //Se è richiesto il rilascio esplicito
            if (isDisposing)
            {
                //RIlascio della logica non finalizzabile
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }
    }
}
