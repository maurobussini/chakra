using System;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.DataAnnotations.Enums;

namespace ZenProgramming.Chakra.Core.DataAnnotations
{
    /// <summary>
    /// Represents a validator attribute for max date/time field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TimeLimitAttribute : ValidationAttribute
    {
        #region Public properties
        /// <summary>
        /// Range limit
        /// </summary>
        public RangeLimit Limit { get; private set; }

        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// Month
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// Day
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        /// Hour
        /// </summary>
        public int Hour { get; private set; }

        /// <summary>
        /// Minute
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// Second
        /// </summary>
        public int Second { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="limit">Limit of range (max/min)</param>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <param name="hour">Hour</param>
        /// <param name="minute">Minute</param>
        /// <param name="second">Second</param>
        public TimeLimitAttribute(RangeLimit limit, int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            //Inserisco i valori nelle proprietà
            Limit = limit;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// Format specified message using attribute value
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <returns>Returns formatted message</returns>
        public override string FormatErrorMessage(string name)
        {
            //Eseguo la formattazione del messaggio
            return string.Format(Strings.TimeRangeAttribute_Message, name, Limit);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>Returns true if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            //Se non è presente alcun valore (es date/time null), è valido
            if (value == null) 
                return true;

            //Se il valore passato non è una data, emetto eccezione
            if (!(value is DateTime))
                throw new FormatException(string.Format("Specified value '{0}' can not be recognized as a valid date/time.", value));

            //Eseguo la composizione della data di riferimento (limite) ed 
            //eseguo la conversione, poi eseguo il cast dell'elemento
            DateTime referenceValue = new DateTime(Year, Month, Day, Hour, Minute, Second);
            DateTime dateValue = (DateTime) value;

            //Se il valore passato è minore del riferimento, ritorno successo
            return Limit == RangeLimit.Min ? 
                dateValue >= referenceValue : 
                dateValue <= referenceValue;
        }  
    }
}
