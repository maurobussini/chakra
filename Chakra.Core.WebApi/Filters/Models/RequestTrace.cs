using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.WebApi.Filters.Models;

namespace ZenProgramming.Chakra.Core.WebApi.Filters.Models
{
    /// <summary>
    /// Represents trace of single request
    /// </summary>
    public class RequestTrace
    {
        /// <summary>
        /// Request unique identifier
        /// </summary>
        public Guid UniqueId { get; set; }

        /// <summary>
        /// Authentication trace
        /// </summary>
        public AuthenticationTrace Authentication { get; set; }

        /// <summary>
        /// Http method
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Controller name
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Action name
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action parameters
        /// </summary>
        public IDictionary<string, object> ActionParameters { get; set; }

        /// <summary>
        /// Creation date of request
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
