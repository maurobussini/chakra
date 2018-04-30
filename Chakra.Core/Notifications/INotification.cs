using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Notifications
{
    /// <summary>
    /// Represents an interfacce for notification
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Sender of notification
        /// </summary>
        string Sender { get; set; }

        /// <summary>
        /// List of recipients
        /// </summary>
        IList<string> Recipients { get; set; }

        /// <summary>
        /// Content of notification message
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Execute sending of notification
        /// </summary>
        void Send();

        /// <summary>
        /// Execute sending of notification
        /// </summary>
        /// <param name="throwOnError">Flag used to contron exception throwing</param>
        void Send(bool throwOnError);
    }
}
