using System;
using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Trace duration of operations between costructor and dispose of current class
    /// </summary>
    public class TraceDuration : IDisposable
    {
        #region Private fields
        private IList<ITracer> _Tracers;
        private string _Message;
        private object[] _Parameters;
        #endregion

        #region Public properties
        /// <summary>
        /// Start time
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// End time
        /// </summary>
        public DateTime End { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tracers">List of tracers that must be used</param>
        /// <param name="message">Message to trace</param>
        /// <param name="parameters">Optional parameters</param>
        internal TraceDuration(IList<ITracer> tracers, string message, params object[] parameters)
        {
            //Eseguo la validazione degli argomenti
            if (tracers == null) throw new ArgumentNullException(nameof(tracers));

            //Valorizzo l'orario di inizio
            Start = DateTime.Now;

            //Imposto il tracer da utilizzare
            _Tracers = tracers;
            _Message = message;
            _Parameters = parameters;

            //Modifico il messaggio aggiungendo la durata
            string outMessage = string.Format("{0} -> Duration start...", message);

            //Eseguo il tracciamento dell'inizio operazione
            foreach (ITracer current in _Tracers)
                current.Info(outMessage, parameters);
        }

        /// <summary>
        /// Release local resources 
        /// </summary>
        public void Dispose()
        {
            //Valorizzo l'orario di fine
            End = DateTime.Now;

            //Modifico il messaggio aggiungendo la durata
            string outMessage = string.Format("{0} -> Duration end. {1}", _Message, End.Subtract(Start));

            //Eseguo il tracciamento della fine operazione
            foreach (ITracer current in _Tracers)
                current.Info(outMessage, _Parameters);
        }
    }
}
