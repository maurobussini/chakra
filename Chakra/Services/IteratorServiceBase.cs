using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using ZenProgramming.Chakra.Services.Windows;
using ZenProgramming.Chakra.Services.Windows.Events;

namespace ZenProgramming.Chakra.Services
{
    /// <summary>
    /// Represents base class for windows services base on iteration
    /// </summary>
    public abstract class IteratorServiceBase : ServiceBase, IManagedService
    {
        #region Public events
        /// <summary>
        /// Message raised by managed service
        /// </summary>
        public event ServiceMessageRaisedEventHandler MessageRaised;

        /// <summary>
        /// Error raised by managed service
        /// </summary>
        public event ServiceErrorRaisedEventHandler ErrorRaised;
        #endregion

        #region Private fields
        private Timer _IterationTimer;
        private IContainer _Components;
        private const int IterationDueTime = 5000;
        private int _IterationPeriod;
        private bool _IsPaused;

	    #endregion

        #region Public properties
        /// <summary>
        /// Display name
        /// </summary>
        public virtual string DisplayName => string.Format("{0} service", ServiceName);

	    /// <summary>
        /// Service description
        /// </summary>
        public virtual string Description => string.Format("Iteration service for '{0}'.", ServiceName);

	    /// <summary>
        /// Get service start mode
        /// </summary>
        public virtual ServiceStartMode StartType => ServiceStartMode.Manual;

	    /// <summary>
        /// Get service execution account
        /// </summary>
        public virtual ServiceAccount Account => ServiceAccount.LocalSystem;

	    /// <summary>
        /// Get account username
        /// </summary>
        public virtual string Username => null;

	    /// <summary>
        /// Get account password
        /// </summary>
        public virtual string Password => null;

	    /// <summary>
        /// Get dependencies of windows service
        /// </summary>
        public virtual string[] ServicesDependedOn => new string[] { };

	    /// <summary>
        /// Get a flag that specify if an internal error
        /// of service must be thrown on external application
        /// </summary>
        public virtual bool ThrowException => false;

	    /// <summary>
        /// Get a flag that specify if parallel execution is enabled
        /// </summary>
        public bool IsParallelExecutionEnabled { get; }

	    #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <param name="isParallelExecutionEnabled">Enable parallel execution</param>
        protected IteratorServiceBase(string serviceName, bool isParallelExecutionEnabled)             
        { 
                //Avvio l'inizializzazione dei componenti
            _Components = new Container();
            ServiceName = serviceName;
            IsParallelExecutionEnabled = isParallelExecutionEnabled;

            //Se non è presente un nome valido, emetto eccezione
            if (string.IsNullOrWhiteSpace(ServiceName))
                throw new InvalidOperationException("Unable to create instance of windows service " +
                    "without a valid name set in application configuration or specified as parameter of service.");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceName">Service name</param>
        protected IteratorServiceBase(string serviceName) : this(serviceName, false) { }

        /// <summary>
        /// Execute manual start of iterator
        /// </summary>
        public abstract void IterationStart();

        /// <summary>
        /// Execute manual start of iterator with specified delay
        /// </summary>
        /// <param name="minutesInterval"></param>
        public void IterationStart(int minutesInterval)
        {
            //Se il valore passato è minore di 1, emetto eccezione
            if (minutesInterval <= 0)
                throw new InvalidOperationException("Unable to set time interval on scheduled service with value less then 1 minute.");

            try
            {
                //Lancio la funzione di startup dell'iteratore
                OnIterationStart(minutesInterval);
            }
            catch (Exception exc)
            {
                //Forzo il sollevamento dell'errore e recupero la "gestione" dello stesso
                bool isHandled = RaiseServiceErrorRaised(exc, "On iterator start");

                //Se non è stato gestito, scateno l'eccezione
                if (!isHandled)
                    throw;
            }

            //Visualizzo il messaggio utente 
            RaiseMessage(string.Format("Scheduled service of type '{0}' starting. " +
                "Minutes inteval : {1}", GetType().FullName, minutesInterval));

            //Imposto il periodo di intervallo del timer
            _IterationPeriod = minutesInterval * 1000 * 60;
            _IsPaused = false;

            //Inizializzo il timer con le impostazioni
            _IterationTimer = new Timer(o => BeforeExecuteActivity(),
                null, IterationDueTime, _IterationPeriod);

            //Scrivo il log dell'operazione completata
            RaiseMessage(string.Format("Scheduled service of type '{0}' started.", GetType().FullName));
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to 
        /// the service by the Service Control Manager (SCM) or when the operating system 
        /// starts (for a service that starts automatically). Specifies actions to take 
        /// when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            //Avvio lo iterator
            IterationStart();
        }

        /// <summary>
        /// Apply pause state on iterator
        /// </summary>
        protected void PauseIterator() 
        {
            //Utilizzo il metodo overloaded
            PauseIterator(false);
        }

        /// <summary>
        /// Apply pause state on iterator
        /// </summary>
        private void PauseIterator(bool forceActivity) 
        {
            //Se l'attività non è forzata internamente
            if (!forceActivity)
            {
                //Se non è permessa la parallelizzazione e chiamo esplicitamente
                //il metodo di pause dell'esecuzione, emetto eccezione
                if (!IsParallelExecutionEnabled)
                    throw new InvalidOperationException("Unable to pause a iterator configured with parallel execution disabled.");

                //Se siamo in pausa, emetto eccezione
                if (_IsPaused)
                    throw new InvalidOperationException("Cannot pause current iterator. It is not in running state.");
            }

            //Se lo iterator non è ancora stato istanziato, emetto eccezione
            if (_IterationTimer == null)
                throw new InvalidOperationException("Cannot pause current iterator. It is not yet initialized.");

            //Imposto il timer al valore di base (arrestato)
            _IterationTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _IsPaused = true;

            //Visualizzo il messaggio utente 
            RaiseMessage(string.Format("Iterator service of type '{0}' paused.", GetType().FullName));
        }

        /// <summary>
        /// Remove pause from iterator
        /// </summary>
        protected void ResumeIterator() 
        {
            //Utilizzo il metodo overloaded
            ResumeIterator(false);
        }

        /// <summary>
        /// Remove pause from iterator
        /// </summary>
        private void ResumeIterator(bool forceActivity)
        {
            //Se l'attività non è forzata internamente
            if (!forceActivity)
            {
                //Se non è permessa la parallelizzazione e chiamo esplicitamente
                //il metodo di pause dell'esecuzione, emetto eccezione
                if (!IsParallelExecutionEnabled)
                    throw new InvalidOperationException("Unable to resume a iterator configured with parallel execution disabled.");

                //Se non siamo in pausa, emetto eccezione
                if (!_IsPaused)
                    throw new InvalidOperationException("Cannot resume current iterator. It is not in pause state.");
            }

            //Se lo iterator non è ancora stato istanziato, emetto eccezione
            if (_IterationTimer == null)
                throw new InvalidOperationException("Cannot resume current iterator. It is not yet initialized.");

            //Imposto il timer al valore di base (arrestato)
            _IterationTimer.Change(_IterationPeriod, _IterationPeriod);
            _IsPaused = false;

            //Visualizzo il messaggio utente 
            RaiseMessage(string.Format("Scheduled service of type '{0}' resumed.", GetType().FullName));
        }

        /// <summary>
        /// Execute manual stop of iterator
        /// </summary>
        public void ScheduledStop()
        {
            //Emetto il messaggio per l'azione corrente
            RaiseMessage(string.Format("Scheduled service of type '{0}' stopping.", GetType().FullName));

            //Se il timer è istanziato, lo rilascio
            if (_IterationTimer != null)
            {
                //Eseguo la dispose e lo annullo
                _IterationTimer.Dispose();
                _IterationTimer = null;
                _IsPaused = false;
            }

            try
            {
                //Richiamo la funzione di shutdown
                OnScheduledStop();
            }
            catch (Exception exc)
            {
                //Forzo il sollevamento dell'errore e recupero la "gestione" dello stesso
                bool isHandled = RaiseServiceErrorRaised(exc, "On iterator stop");

                //Se non è stato gestito, scateno l'eccezione
                if (!isHandled)
                    throw;
            }

            //Scrivo il log dell'operazione completata
            RaiseMessage(string.Format("Scheduled service of type '{0}' stopped.", GetType().FullName));
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent 
        /// to the service by the Service Control Manager (SCM). Specifies actions 
        /// to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            //Arresto lo iterator
            ScheduledStop();
        }

        /// <summary>
        /// Execute custom operations on iterator service initialize
        /// </summary>
        /// <param name="minutesInterval">Minutes of scheduled interval</param>
        protected virtual void OnIterationStart(int minutesInterval)
        {
            //********************************************************
            //* Questa funzione non esegue alcuna operazione perchè 
            //* viene eventualmente sovrascritta nella classe derivata
            //********************************************************
        }

        /// <summary>
        /// Execute custom operations on iterator service terminate
        /// </summary>
        protected virtual void OnScheduledStop()
        {
            //********************************************************
            //* Questa funzione non esegue alcuna operazione perchè 
            //* viene eventualmente sovrascritta nella classe derivata
            //********************************************************
        }

        /// <summary>
        /// Executes activity and raised specified event
        /// </summary>
        private void BeforeExecuteActivity()
        {
            //Se non è abilitata la parallelizzazione del'esecuzione, arresto il timer
            if (!IsParallelExecutionEnabled)
                PauseIterator(true);

            //Sollevo l'evento del servizio
            RaiseMessage("Execute activity initializing...");

            try
            {
                //Mando in esecuzione l'azione
                ExecuteActivity();
            }
            catch (Exception exc)
            {
                //Forzo il sollevamento dell'errore e recupero la "gestione" dello stesso
                bool isHandled = RaiseServiceErrorRaised(exc, "Execute activity");

                //Se non è stato gestito, scateno l'eccezione
                if (!isHandled)
                    throw;
            }

            //Confermo l'esecuzione dell'azione
            RaiseMessage("Execute activity completed.");

            //Se non è abilitata la parallelizzazione del'esecuzione, riavvio il timer
            if (!IsParallelExecutionEnabled)
                ResumeIterator(true);
        }

        /// <summary>
        /// Execure activity related with execution
        /// </summary>
        protected abstract void ExecuteActivity();

        /// <summary>
        /// Execute main activity immediately one time
        /// </summary>
        public void ExecuteImmediate()
        {
            //Richiamo il metodo di esecuzione
            ExecuteActivity();
        }

        /// <summary>
        /// Force raise of event 'ServiceMessageRaised'
        /// </summary>
        /// <param name="raisedMessage">Raised message</param>
        protected void RaiseMessage(string raisedMessage)
        {
            //Se il gestore è stato collegatio
            if (MessageRaised != null)
                MessageRaised(this, new ServiceMessageRaisedEventArgs(raisedMessage));
        }

        /// <summary>
        /// Force raise of event 'ServicErrorRaised'
        /// </summary>
        /// <param name="error">Raised error</param>
        /// <param name="context">Context of error</param>
        protected bool RaiseServiceErrorRaised(Exception error, string context)
        {
            //Dichiaro l'argormento dell'evento
            ServiceErrorRaisedEventArgs arguments = new ServiceErrorRaisedEventArgs(error, context);

            //Se il gestore è stato collegatio
            if (ErrorRaised != null)
                ErrorRaised(this, arguments);

            //Ritorno il flag di handled dell'evento
            return arguments.MarkAsHandled;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //Se siamo in dispose esplicito
            if (disposing)
            {
                //Se ci sono componenti, li rilascio
                if (_Components != null)
                    _Components.Dispose();

                //Se il timer è instanziato, lo rilascio
                if (_IterationTimer != null)
                    _IterationTimer.Dispose();
            }

            //Eseguo la dispose base
            base.Dispose(disposing);
        }
    }
}
