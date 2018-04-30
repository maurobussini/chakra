using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ZenProgramming.Chakra.Core.Reflection;

namespace ZenProgramming.Chakra.Core.Diagnostic
{
    /// <summary>
    /// Tracer for execute diagnostic of application
    /// </summary>
    public static class Tracer
    {
        #region Private fields
        private static IList<ITracer> _Tracers = new List<ITracer>();
        #endregion

        /// <summary>
        /// Clear all appended tracers
        /// </summary>
        public static void Clear() 
        {
            //Eseguo la dispose di tutti i tracer
            foreach (ITracer current in _Tracers)
                current.Dispose();

            //Eseguo la rimozione di tutti i tracers
            _Tracers.Clear();
        }

        /// <summary>
        /// Append a new tracer using its type
        /// </summary>
        public static void Append(Type tracerType)
        {
            //Validazione argomenti
            if (tracerType == null) throw new ArgumentNullException(nameof(tracerType));

            //Eseguo la generazione del tipo del tracer
            object value = Activator.CreateInstance(tracerType);
            ITracer instance = value as ITracer;

            //Se l'oggetto è nullo, emetto eccezione
            if (instance == null) throw new NullReferenceException(string.Format("Unable to generate a valid " +
                "instance of type '{0}' that implements interface '{1}'.", tracerType.FullName, typeof(ITracer).FullName));

            //Ricerco un tracer dello stesso tipo nella lista
            ITracer duplicateTracer = _Tracers.SingleOrDefault(t => t.GetType() == instance.GetType());

            //Se già esiste un tracer dello stesso tipo, emetto eccezione
            if (duplicateTracer != null)
                throw new InvalidProgramException(string.Format("Found " +
                    "another tracer of the same type of '{0}'.", tracerType.FullName));

            //Aggiungo il tracer alla lista
            _Tracers.Add(instance);
        }

        /// <summary>
        /// Append a new tracer using its type
        /// </summary>
        /// <typeparam name="TTracer">Type of tracer</typeparam>
        public static void Append<TTracer>()
            where TTracer: class, ITracer, new()
        {
            //Utilizzo il metodo overloaded
            Append(typeof(TTracer));            
        }

        /// <summary>
        /// Append a new tracer using its type full 
        /// name or alias (only for build-in tracers)
        /// </summary>
        /// <param name="tracerName">Tracer full type name or alias</param>
        public static void Append(string tracerName)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(tracerName)) throw new ArgumentNullException(nameof(tracerName));

            //Predispongo il tipo del tracer
            Type type;

            //Verifico che il tracer invocato sia uno di quelli di base
            switch (tracerName) 
            { 
                //*******************************************************
                case "auto": 

                //Se sono in debug in Visual Studio aggiungo il tracer del debug
                    //altrimenti verifico se sono in modalità interattiva: se lo sono
                    //utilizzo la console, altrimenti utilizzo il file
                    type = Debugger.IsAttached 
                        ? typeof(DebugTracer)
                        : Environment.UserInteractive 
                            ? typeof(ConsoleTracer) 
                            : typeof(FileTracer);
                    break;

                //*******************************************************
                case "console": 
                    type = typeof(ConsoleTracer);
                    break;

                //*******************************************************
                case "file": 
                    type = typeof(FileTracer);
                    break;

                //*******************************************************
                default:

                    //Recupero il tipo del tracer passando dal nome
                    type = ReflectionUtils.GenerateType(tracerName);

                    //Se il tipo non è stato generato emetto eccezione
                    if (type == null) throw new TypeLoadException(string.
                        Format("Unable to generate type '{0}'.", tracerName));

                    break;
            }

            //Utilizzo il metodo overloaded
            Append(type);   
        }

        /// <summary>
        /// Verify that configuration was run
        /// </summary>
        private static void EnsureConfiguration()
        {
            //Se non ci sono tracers in lista, aggiungo quello automatico
            if (_Tracers.Count == 0)
            {
                //Aggiungo quello automatico
                Append("auto");

                //Traccio che è stato utilizzato quello "auto" perchè non ne ho uno impostato
                Info("No tracers configured. Appending 'auto' tracer...");
            }
        }

        /// <summary>
        /// Write an information message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public static void Info(string message, params object[] parameters) 
        {
            //Verifico che sia stata impostata la configurazione
            EnsureConfiguration();

            //Scorro tutti i tracer ed eseguo l'invocazione della funzione
            foreach (ITracer current in _Tracers)
                current.Info(message, parameters);
        }

        /// <summary>
        /// Write a warning message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public static void Warn(string message, params object[] parameters)
        {
            //Verifico che sia stata impostata la configurazione
            EnsureConfiguration();

            //Scorro tutti i tracer ed eseguo l'invocazione della funzione
            foreach (ITracer current in _Tracers)
                current.Warn(message, parameters);
        }

        /// <summary>
        /// Write an error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public static void Error(string message, params object[] parameters)
        {
            //Verifico che sia stata impostata la configurazione
            EnsureConfiguration();

            //Scorro tutti i tracer ed eseguo l'invocazione della funzione
            foreach (ITracer current in _Tracers)
                current.Error(message, parameters);
        }

        /// <summary>
        /// Write a debug message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        public static void Debug(string message, params object[] parameters)
        {
            //Verifico che sia stata impostata la configurazione
            EnsureConfiguration();

            //Scorro tutti i tracer ed eseguo l'invocazione della funzione
            foreach (ITracer current in _Tracers)
                current.Debug(message, parameters);
        }

        /// <summary>
        /// Trace duration of operation closed inside "using" construct
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="parameters">Format parameters</param>
        /// <returns>Returns trace duration object</returns>
        public static TraceDuration Duration(string message, params object[] parameters)
        {
            //Verifico che sia stata impostata la configurazione
            EnsureConfiguration();

            //Creo l'oggetto di trace duration
            return new TraceDuration(_Tracers, message, parameters);
        }

        /// <summary>
        /// Format message applying components of time and kind
        /// </summary>
        /// <param name="tracer">Tracer to use</param>
        /// <param name="time">Current time</param>
        /// <param name="kind">Kind</param>        
        /// <param name="message">Message</param>
        /// <param name="parameters">Format paramenter for message</param>
        /// <returns>Returns formatted message</returns>
        public static string FormatMessage(ITracer tracer, DateTime time, string kind, string message, params object[] parameters)
        {
            //Eseguo la validazione degli argomenti
            if (tracer == null) throw new ArgumentNullException(nameof(tracer));
            if (string.IsNullOrEmpty(kind)) throw new ArgumentNullException(nameof(kind));

            //Definisco le costanti accettate nel formato
            const string TimeToken = "{time}";
            const string KindToken = "{kind}";
            const string MessageToken = "{message}";

            //Recupero il formato impostato nel tracer, impostando il default nel caso in cui
            //non sia stato possibile determinare un formato custom per il tracer
            string format = string.IsNullOrEmpty(tracer.TraceFormat) ?
                "[{time}] {kind} - {message}"  : tracer.TraceFormat;

			//Eseguo la formattazione del messaggio solo se necessario
			//poichè ci potrebbe essere un JSON che di fatto fa fallire la formattazione stessa
	        string formattedMessage = parameters != null && parameters.Length > 0
		        ? string.Format(message, parameters)
		        : message;

			//Eseguo il parsing delle varie componenti e mando in uscita
			return format.Replace(TimeToken, time.ToString("yyyy/MM/dd HH:mm:ss")).
                Replace(KindToken, kind.PadRight(5, ' ')).
                Replace(MessageToken, formattedMessage);
        }
    }
}
