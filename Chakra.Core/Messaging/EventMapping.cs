using System;
using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Messaging
{
    /// <summary>
    /// Event mapping
    /// </summary>
    public class EventMapping
    {
        /// <summary>
        /// Type of message
        /// </summary>
        public Type MessageType { get; set; }

        /// <summary>
        /// List of registrations
        /// </summary>
        public IList<EventRegistration> Registrations { get; set; }
    }
}
