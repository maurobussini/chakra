using System;

namespace ZenProgramming.Chakra.Services.Windows.Events
{
    /// <summary>
    /// Delegate used to handle 'ServiceErrorRaised' event
    /// </summary>
    /// <param name="sender">Object that raised the event</param>
    /// <param name="e">Arguments of event</param>
    public delegate void ServiceErrorRaisedEventHandler(object sender, ServiceErrorRaisedEventArgs e);

    /// <summary>
    /// Event arguments for 'ServiceErrorRaised'
    /// </summary>
    public class ServiceErrorRaisedEventArgs : EventArgs
    {
        /// <summary>
        /// Get exception of raised
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Get context where error is raised
        /// </summary>
        public string Context { get; private set; }

        /// <summary>
        /// Get and set a flag that mark error has handled
        /// </summary>
        public bool MarkAsHandled { get; set; }

        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="error">Error raised</param>
        public ServiceErrorRaisedEventArgs(Exception error) : this(error, null) {  }

        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="error">Error raised</param>
        /// <param name="context">Content of processing</param>
        public ServiceErrorRaisedEventArgs(Exception error, string context) 
        {
            //Inserisco il valore nella proprietà
            Error = error;
            Context = context; 
        }
    }
}
