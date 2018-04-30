using System;
using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Messaging
{
    /// <summary>
    /// Registration of event
    /// </summary>
    public class EventRegistration
    {
        /// <summary>
        /// Type of recipient
        /// </summary>
        public Type RecipientType { get; set; }

        /// <summary>
        /// List of actions
        /// </summary>
        public IList<Action<IEventMessage>> Actions { get; set; }
    }
}
