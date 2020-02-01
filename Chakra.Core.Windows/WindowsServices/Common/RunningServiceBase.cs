using System;
using System.ServiceProcess;
using ZenProgramming.Chakra.Core.Windows.WindowsServices.Events;

namespace ZenProgramming.Chakra.Core.Windows.WindowsServices.Common
{
    /// <summary>
    /// Represents base class for windows service used
    /// for continous running
    /// </summary>
    public abstract class RunningServiceBase : ServiceBase, IManagedWindowsService
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

        #region Public properties
        /// <summary>
        /// Display name
        /// </summary>
        public virtual string DisplayName => $"{ServiceName} service";

	    /// <summary>
        /// Service description
        /// </summary>
        public virtual string Description => $"Running service for '{ServiceName}'.";

	    /// <summary>
        /// Get a flag that specify if an internal error
        /// of service must be thrown on external application
        /// </summary>
        public virtual bool ThrowException => false;
	    #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceName">Service name</param>
        protected RunningServiceBase(string serviceName)
        { 
            //Avvio l'inizializzazione dei componenti
            ServiceName = serviceName;

            //Se non è presente un nome valido, emetto eccezione
            if (string.IsNullOrWhiteSpace(ServiceName))
                throw new InvalidOperationException("Unable to create instance of windows service " +
                    "without a valid name set in application configuration or specified as parameter of service.");
        }

        /// <summary>
        /// Execute manual start of service 
        /// </summary>
        public void Activate()
        {
			//Visualizzo il messaggio utente 
			RaiseMessage($"Running service of type '{GetType().FullName}' starting...");

			try
            {
                //Lancio la funzione di startup dell'iteratore
                OnActivation();
            }
            catch (Exception exc)
            {
                //Forzo il sollevamento dell'errore e recupero la "gestione" dello stesso
                bool isHandled = RaiseServiceErrorRaised(exc, "On activation");

                //Se non è stato gestito, scateno l'eccezione
                if (!isHandled)
                    throw;
            }			

            //Scrivo il log dell'operazione completata
            RaiseMessage($"Running service of type '{GetType().FullName}' started.");
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
            Activate();
        }
		
        /// <summary>
        /// Execute manual stop of iterator
        /// </summary>
        public void Terminate()
        {
            //Emetto il messaggio per l'azione corrente
            RaiseMessage($"Running service of type '{GetType().FullName}' terminating...");

            try
            {
				//Richiamo la funzione di terminazione
				OnTermination();
            }
            catch (Exception exc)
            {
                //Forzo il sollevamento dell'errore e recupero la "gestione" dello stesso
                bool isHandled = RaiseServiceErrorRaised(exc, "On termination");

                //Se non è stato gestito, scateno l'eccezione
                if (!isHandled)
                    throw;
            }

            //Scrivo il log dell'operazione completata
            RaiseMessage($"Running service of type '{GetType().FullName}' terminated.");
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent 
        /// to the service by the Service Control Manager (SCM). Specifies actions 
        /// to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            //Arresto lo iterator
            Terminate();
        }

        /// <summary>
        /// Execute custom operations on running service initialize
        /// </summary>
        protected virtual void OnActivation()
        {
            //********************************************************
            //* Questa funzione non esegue alcuna operazione perchè 
            //* viene eventualmente sovrascritta nella classe derivata
            //********************************************************
        }

        /// <summary>
        /// Execute custom operations on service termination
        /// </summary>
        protected virtual void OnTermination()
        {
            //********************************************************
            //* Questa funzione non esegue alcuna operazione perchè 
            //* viene eventualmente sovrascritta nella classe derivata
            //********************************************************
        }		

        /// <summary>
        /// Force raise of event 'ServiceMessageRaised'
        /// </summary>
        /// <param name="raisedMessage">Raised message</param>
        protected void RaiseMessage(string raisedMessage)
        {
			//Se il gestore è stato collegatio
			MessageRaised?.Invoke(this, new ServiceMessageRaisedEventArgs(raisedMessage));
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
			ErrorRaised?.Invoke(this, arguments);

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
                //Nessun rilascio in questo caso
            }

            //Eseguo la dispose base
            base.Dispose(disposing);
        }
    }
}
