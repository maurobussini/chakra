using System;
using ZenProgramming.Chakra.Core.Tuning.Enums;

namespace ZenProgramming.Chakra.Core.Tuning
{
    /// <summary>
    /// Represents measure data on tuner
    /// </summary>
    public class MeasureData
    {
        /// <summary>
        /// Get and set measure kind
        /// </summary>
        public MeasureKind Kind { get; private set; }

        /// <summary>
        /// Get total iterations scheduled
        /// </summary>
        public int TotalIterations { get; private set; }

        /// <summary>
        /// Get and set start date
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Get current iteration number
        /// </summary>
        public int IterationNumber { get; private set; }

        /// <summary>
        /// Get custom message
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Get and set end date
        /// </summary>
        public DateTime? EndDate { get; internal set; }

        /// <summary>
        /// Get duration of operation
        /// </summary>
        public TimeSpan Duration 
        {
            get 
            {
                //Se non ho ancora la data di fine, ritorno quella corrente
                return EndDate == null ? 
                    DateTime.Now.Subtract(StartDate) : 
                    EndDate.Value.Subtract(StartDate);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kind">Kind of data</param>
        /// <param name="totalIterations">Total iterations</param>
        /// <param name="iterationNumber">Number of current iteration</param>
        /// <param name="startDate">Start date</param>
        public MeasureData(MeasureKind kind, int totalIterations, int iterationNumber, DateTime startDate)
        {
            //Inserisco i valori nelle proprietà
            Kind = kind;
            TotalIterations = totalIterations;
            IterationNumber = iterationNumber;
            StartDate = startDate;
        }
    }
}
