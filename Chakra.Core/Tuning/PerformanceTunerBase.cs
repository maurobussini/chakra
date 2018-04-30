using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ZenProgramming.Chakra.Core.Tuning.Enums;
using ZenProgramming.Chakra.Core.Tuning.Events;
using ZenProgramming.Chakra.Core.Utilities.Server;

namespace ZenProgramming.Chakra.Core.Tuning
{
    /// <summary>
    /// Represents a base class that contains a base implementation
    /// for a tuner that measure performances of an application unit of work 
    /// </summary>
    public abstract class PerformanceTunerBase : IDisposable
    {
        #region Private fields
        private bool _IsDisposed;
        private const string LogFileName = "{0}_tuner_log_{1}.txt";
        #endregion

        #region Public events
        /// <summary>
        /// Raised when an exception is raised during process
        /// </summary>
        public event ProcessingErrorRaisedEventHandler ProcessingErrorRaised;

        /// <summary>
        /// Raised when a tracing is initialized
        /// </summary>
        public event MeasuringEventRaisedEventHandler TracingInitialized;

        /// <summary>
        /// Raised when a tracing is completed
        /// </summary>
        public event MeasuringEventRaisedEventHandler TracingCompleted;

        /// <summary>
        /// Raised when a iteration is initialized
        /// </summary>
        public event MeasuringEventRaisedEventHandler IterationInitialized;

        /// <summary>
        /// Raised when a iteration is completed
        /// </summary>
        public event MeasuringEventRaisedEventHandler IterationCompleted;
        #endregion

        #region Public properties
        /// <summary>
        /// Get number of iterations completed
        /// </summary>
        public int CompletedIterations { get; private set; }

        /// <summary>
        /// Get number of total iterations
        /// </summary>
        public int TotalIterations { get; private set; }

        /// <summary>
        /// Get and set feedback output on console
        /// </summary>
        public bool IsConsoleFeedbackEnabled { get; protected set; }

        /// <summary>
        /// Get and set feedback output on log
        /// </summary>
        public bool IsLogFeedbackEnabled { get; protected set; }

        /// <summary>
        /// Get log file full path
        /// </summary>
        public string LogFilePath { get; private set; }

        /// <summary>
        /// Get list of measures for tracings
        /// </summary>
        public IList<MeasureData> TracingMeasures { get; set; }

        /// <summary>
        /// Get list of measures for iterations
        /// </summary>
        public IList<MeasureData> IterationMeasures { get; set; }

        /// <summary>
        /// Get and set delay between two iterations
        /// </summary>
        public TimeSpan DelayBetweenIterations { get; set; }
        #endregion

        protected PerformanceTunerBase() 
        {
            //Abilito tutti i log di default
            IsConsoleFeedbackEnabled = true;
            IsLogFeedbackEnabled = true;
            DelayBetweenIterations = new TimeSpan(0, 0, 0, 5);
            TracingMeasures = new List<MeasureData>();
            IterationMeasures = new List<MeasureData>();

            //Compongo il percorso completo del file di log da scrivere
            LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                string.Format(LogFileName, GetType().Name, DateTime.Now.ToString("yyyyMMdd-HHmmss")));

            //Aggancio tutti gli eventi generati dal tuner
            IterationInitialized += OnIterationInitialized;
            IterationCompleted += OnIterationCompleted;
            TracingInitialized += OnTracingInitialized;
            TracingCompleted += OnTracingCompleted;
            ProcessingErrorRaised += OnProcessingErrorRaised;
        }

        #region Methods for tracing
        /// <summary>
        /// Trace duration of specific method and raise the event
        /// </summary>
        /// <param name="actionMethod">Action method to trace</param>
        /// <param name="message">Optional message</param>
        protected void TraceDuration(Action actionMethod, string message = null) 
        {
            //Eseguo la validazione degli argomenti
            if (actionMethod == null) throw new ArgumentNullException(nameof(actionMethod));

            //Creo il dato di tracing
            var data = CreateTraceData(message);

            //Mando in esecuzione l'azione specificata come parametro della funzione
            actionMethod();

            //Aggiorno i dati da tracciare
            UpdateTraceData(data);
        }

        /// <summary>
        /// Update data of tracing
        /// </summary>
        /// <param name="data">Data to update</param>
        private void UpdateTraceData(MeasureData data)
        {
            //Eseguo la validazione degli argomenti
            if (data == null) throw new ArgumentNullException(nameof(data));

            //Aggiungo l'oggetto di misurazione con le nuove informazioni
            data.EndDate = DateTime.Now;

            //Sollevo l'evento di completamento della misurazione
            if (TracingCompleted != null)
                TracingCompleted(this, new MeasuringEventRaisedEventArgs(data));
        }

        /// <summary>
        /// Create tracing data
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Returns created data</returns>
        private MeasureData CreateTraceData(string message)
        {
            //Creo un oggetto di misurazione con le informazioni base
            MeasureData data = new MeasureData(MeasureKind.Tracing, TotalIterations,
                CompletedIterations + 1, DateTime.Now) { Message = message };

            //Sollevo l'evento di inizializzazione della misurazione
            if (TracingInitialized != null)
                TracingInitialized(this, new MeasuringEventRaisedEventArgs(data));

            //Ritorno il valore
            return data;
        }

        /// <summary>
        /// Trace duration of specific method and raise the event
        /// </summary>
        /// <typeparam name="TOutput">Type of output</typeparam>
        /// <param name="functionMethod">Function method to trace</param>
        /// <param name="message">Optional message</param>
        /// <returns>Returns result of function</returns>
        protected TOutput TraceDuration<TOutput>(Func<TOutput> functionMethod, string message = null)
        {
            //Eseguo la validazione degli argomenti
            if (functionMethod == null) throw new ArgumentNullException(nameof(functionMethod));

            //Creo il dato di tracing
            var data = CreateTraceData(message);

            //Mando in esecuzione l'azione specificata come parametro della funzione
            TOutput result = functionMethod();

            //Aggiorno i dati da tracciare
            UpdateTraceData(data);

            //Ritorno il valore
            return result;
        }

        /// <summary>
        /// Handle default behavior of event "TracingInitialized"
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments of event</param>
        private void OnTracingInitialized(object sender, MeasuringEventRaisedEventArgs e)
        {
            //Compongo il messaggio da mandare in uscita sulla console
            string feedback = string.Format("[{0}] Tracing of '{1}' initialized.\r\n",
                e.Data.StartDate.ToString("yyyy/MM/dd HH:mm:ss"), e.Data.Message);

            //Emetto il messaggio sulla console applicativa
            if (IsConsoleFeedbackEnabled)
                ConsoleUtils.WriteColorLine(ConsoleColor.Yellow, feedback);

            //Se è abilitato il log su file di testo
            if (IsLogFeedbackEnabled)
                File.AppendAllText(LogFilePath, feedback);
        }

        /// <summary>
        /// Handle default behavior of event "TracingCompleted"
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments of event</param>
        private void OnTracingCompleted(object sender, MeasuringEventRaisedEventArgs e)
        {
            //Aggiungo il pacchetto alla lista
            TracingMeasures.Add(e.Data);

            //Compongo il messaggio da mandare in uscita sulla console
            string feedback = string.Format("[{0}] Tracing of '{1}' completed.\n" + 
                "Duration : {2}. Sleeping for {3}...\r\n", e.Data.StartDate.ToString("yyyy/MM/dd HH:mm:ss"),
                e.Data.Message, e.Data.Duration, DelayBetweenIterations);

            //Emetto il messaggio sulla console applicativa
            if (IsConsoleFeedbackEnabled)
                ConsoleUtils.WriteColorLine(ConsoleColor.Yellow, feedback);

            //Se è abilitato il log su file di testo
            if (IsLogFeedbackEnabled)
                File.AppendAllText(LogFilePath, feedback);
        }
        #endregion

        #region Methods for iteration
        /// <summary>
        /// Launch execution of activity for a single iteration
        /// </summary>
        public void Execute() 
        { 
            //Utilizzo il metodo overloaded
            Execute(1);
        }

        /// <summary>
        /// Launch execution of activity of a number
        /// of defined iterations
        /// </summary>
        /// <param name="iterations">Number of iterations to execute</param>
        public void Execute(int iterations) 
        {
            //Eseguo la validazione degli argomenti
            if (iterations <= 0) throw new ArgumentOutOfRangeException(nameof(iterations), "Iterations number must be at least one.");

            //Imposto il numero di iterazioni totali da eseguire
            CompletedIterations = 0;
            TotalIterations = iterations;
            TracingMeasures = new List<MeasureData>();
            IterationMeasures = new List<MeasureData>();

            //Eseguo l'operazione un numero di volte indicato
            for (int i = 0; i < TotalIterations; i++) 
            {
                //Creo un oggetto di misurazione con le informazioni base
                MeasureData data = new MeasureData(MeasureKind.Iteration, TotalIterations,
                    CompletedIterations + 1, DateTime.Now);

                //Sollevo l'evento di inizializzazione della misurazione
                if (IterationInitialized != null)
                    IterationInitialized(this, new MeasuringEventRaisedEventArgs(data));

                try
                {
                    //Mando in esecuzione l'attiovità richiesta
                    ProcessActivity();
                }
                catch (Exception exception)
                {
                    //Sollevo l'evento di eccezione ma non lo sollevo 
                    //perchè l'iterazione deve continuare all'interno del monitor
                    if (ProcessingErrorRaised != null)
                        ProcessingErrorRaised(this, new ProcessingErrorRaisedEventArgs(exception));
                }

                //Aggiorno le iteazioni completate
                CompletedIterations = i + 1;                

                //Aggiungo l'oggetto di misurazione con le nuove informazioni
                data.EndDate = DateTime.Now;

                //Sollevo l'evento di completamento della misurazione
                if (IterationCompleted != null)
                    IterationCompleted(this, new MeasuringEventRaisedEventArgs(data));

                //Imposto uno stop di esecuzione di 10 secondi
                Thread.Sleep(DelayBetweenIterations);
            }
        }

        /// <summary>
        /// Handle default behavior of event "IterationInitialized"
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments of event</param>
        private void OnIterationInitialized(object sender, MeasuringEventRaisedEventArgs e)
        {
            //Compongo il messaggio da mandare in uscita sulla console
            string feedback = string.Format("[{0}] Iteration {1}/{2} initialized...\r\n",
                e.Data.StartDate.ToString("yyyy/MM/dd HH:mm:ss"),
                e.Data.IterationNumber, e.Data.TotalIterations);

            //Emetto il messaggio sulla console applicativa
            if (IsConsoleFeedbackEnabled)
                ConsoleUtils.WriteColorLine(ConsoleColor.Green, feedback);

            //Se è abilitato il log su file di testo
            if (IsLogFeedbackEnabled)
                File.AppendAllText(LogFilePath, feedback);
        }

        /// <summary>
        /// Handle default behavior of event "IterationCompleted"
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments of event</param>
        private void OnIterationCompleted(object sender, MeasuringEventRaisedEventArgs e)
        {
            //Aggiungo il pacchetto alla lista
            IterationMeasures.Add(e.Data);

            //Compongo il messaggio da mandare in uscita sulla console
            string feedback = string.Format("[{0}] Iteration {1}/{2} completed.\n" + 
                "Duration : {3}\r\n",
                e.Data.StartDate.ToString("yyyy/MM/dd HH:mm:ss"),
                e.Data.IterationNumber, e.Data.TotalIterations, e.Data.Duration);

            //Emetto il messaggio sulla console applicativa
            if (IsConsoleFeedbackEnabled)
                ConsoleUtils.WriteColorLine(ConsoleColor.Green, feedback);

            //Se è abilitato il log su file di testo
            if (IsLogFeedbackEnabled)
                File.AppendAllText(LogFilePath, feedback);
        }
        #endregion

        /// <summary>
        /// Execute process of activity to tune
        /// </summary>
        protected abstract void ProcessActivity();

        /// <summary>
        /// Handle default behavior of event "ProcessingErrorRaised"
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments of event</param>
        private void OnProcessingErrorRaised(object sender, ProcessingErrorRaisedEventArgs e) 
        {
            //Compongo il messaggio da mandare in uscita sulla console
            string feedback = string.Format("[{0}] Exception raised during iteration {1}/{2}.\n" +
                "Exception : {3}\r\n",
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                CompletedIterations + 1, TotalIterations, e.Error);

            //Emetto il messaggio sulla console applicativa
            if (IsConsoleFeedbackEnabled)
                ConsoleUtils.WriteColorLine(ConsoleColor.Red, feedback);

            //Se è abilitato il log su file di testo
            if (IsLogFeedbackEnabled)
                File.AppendAllText(LogFilePath, feedback);
        }

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~PerformanceTunerBase()
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
                //Sgancio tutti gli eventi della classe
                IterationInitialized -= OnIterationInitialized;
                IterationCompleted -= OnIterationCompleted;
                TracingInitialized -= OnTracingInitialized;
                TracingCompleted -= OnTracingCompleted;
                ProcessingErrorRaised -= OnProcessingErrorRaised;
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;            
        }
    }
}
