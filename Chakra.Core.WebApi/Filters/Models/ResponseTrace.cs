using System;

namespace ZenProgramming.Chakra.WebApi.Filters.Models
{
    /// <summary>
    /// Represents trace for request
    /// </summary>
    public class ResponseTrace
    {
        /// <summary>
        /// Original request
        /// </summary>
        public RequestTrace Request { get; set; }

        /// <summary>
        /// Unhandled error
        /// </summary>
        public ErrorTrace Error { get; set; }

        /// <summary>
        /// Creation date of response
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Duration of request/response process
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Body type
        /// </summary>
        public string BodyType { get; internal set; }

        /// <summary>
        /// Body content
        /// </summary>
        public string BodyContent { get; internal set; }

        /// <summary>
        /// Body lenght
        /// </summary>
        public int? BodyLenght { get; internal set; }
    }
}
