using System;

namespace ZenProgramming.Chakra.Core.Parallel.MapReduce.Events
{
    /// <summary>
    /// Handler for event "MapReduceProcessCompleted"
    /// </summary>
    /// <typeparam name="TOutput">Type of result</typeparam>
    /// <param name="sender">Object that raised the event</param>
    /// <param name="args">Arguments</param>
    public delegate void MapReducePartialResultProcessedEventHandler<TOutput>(object sender, MapReducePartialResultProcessedEventArgs<TOutput> args); 

    /// <summary>
    /// Arguments for event 'MapReduceProcessCompleted'
    /// </summary>
    /// <typeparam name="TOutput">Type of result</typeparam>
    public class MapReducePartialResultProcessedEventArgs<TOutput> : EventArgs
    {
        /// <summary>
        /// Get result of process
        /// </summary>
        public TOutput Result { get; private set; }

        /// <summary>
        /// Get processor identifier
        /// </summary>
        public int ProcessorId { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result of process</param>
        /// <param name="processorId">Identifier of processor</param>
        public MapReducePartialResultProcessedEventArgs(TOutput result, int processorId) 
        {
            //Inserisco le informazioni nelle proprietà
            Result = result;
            ProcessorId = processorId;
        }
    }
}
