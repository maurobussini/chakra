using System;

namespace ZenProgramming.Chakra.Services.Windows.Events
{
    /// <summary>
    /// Delegate used to handle 'ServiceMessageRaised' event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServiceMessageRaisedEventHandler(object sender, ServiceMessageRaisedEventArgs e);

    /// <summary>
    /// Event arguments for 'ServiceMessageRaised'
    /// </summary>
    public class ServiceMessageRaisedEventArgs : EventArgs
    {
        /// <summary>
        /// Message raised
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public ServiceMessageRaisedEventArgs(string message) 
        {
            //Inserisco il valore nella proprietà
            Message = message; 
        }
    }
}
