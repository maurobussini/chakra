using System;

namespace ZenProgramming.Chakra.Core.Tuning.Events
{
    /// <summary>
    /// Handler for event "ProcessingErrorRaised"
    /// </summary>
    /// <param name="sender">Object that raised the event</param>
    /// <param name="raisedEventArgs">Arguments</param>
    public delegate void ProcessingErrorRaisedEventHandler(object sender, ProcessingErrorRaisedEventArgs raisedEventArgs);

    /// <summary>
    /// Arguments for event 'MeasuringEventRaised'
    /// </summary>
    public class ProcessingErrorRaisedEventArgs : EventArgs
    {        
        /// <summary>
        /// Get and set error raised
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">Error raised</param>
        public ProcessingErrorRaisedEventArgs(Exception error)
        {
            //Inserisco le informazioni nelle proprietà
            Error = error;
        }
    }
}
