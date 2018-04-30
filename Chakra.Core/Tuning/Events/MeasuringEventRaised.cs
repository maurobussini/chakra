using System;

namespace ZenProgramming.Chakra.Core.Tuning.Events
{
    /// <summary>
    /// Handler for event "MeasuringEventRaised"
    /// </summary>
    /// <param name="sender">Object that raised the event</param>
    /// <param name="raisedEventArgs">Arguments</param>
    public delegate void MeasuringEventRaisedEventHandler(object sender, MeasuringEventRaisedEventArgs raisedEventArgs);

    /// <summary>
    /// Arguments for event 'MeasuringEventRaised'
    /// </summary>
    public class MeasuringEventRaisedEventArgs : EventArgs
    {        
        /// <summary>
        /// Get and set data generated
        /// </summary>
        public MeasureData Data { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data generated</param>
        public MeasuringEventRaisedEventArgs(MeasureData data)
        {
            //Inserisco le informazioni nelle proprietà
            Data = data;
        }
    }
}
