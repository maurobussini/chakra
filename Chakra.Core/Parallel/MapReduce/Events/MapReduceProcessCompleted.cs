using System;

namespace ZenProgramming.Chakra.Core.Parallel.MapReduce.Events
{
    /// <summary>
    /// Handler for event "MapReduceProcessCompleted"
    /// </summary>
    /// <typeparam name="TOutput">Type of result</typeparam>
    /// <param name="sender">Object that raised the event</param>
    /// <param name="args">Arguments</param>
    public delegate void MapReduceProcessCompletedEventHandler<TOutput>(object sender, MapReduceProcessCompletedEventArgs<TOutput> args); 

    /// <summary>
    /// Arguments for event 'MapReduceProcessCompleted'
    /// </summary>
    /// <typeparam name="TOutput">Type of result</typeparam>
    public class MapReduceProcessCompletedEventArgs<TOutput> : EventArgs
    {
        /// <summary>
        /// Get result of process
        /// </summary>
        public TOutput Result { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result of process</param>
        public MapReduceProcessCompletedEventArgs(TOutput result) 
        {
            //Inserisco le informazioni nelle proprietà
            Result = result;
        }
    }
}
