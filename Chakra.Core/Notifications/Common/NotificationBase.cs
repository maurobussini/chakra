using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Notifications.Common
{
    /// <summary>
    /// Represents base class for notification
    /// </summary>
    public abstract class NotificationBase : INotification
    {
        #region Public properties
        /// <summary>
        /// Sender of notification
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// List of recipients
        /// </summary>
        public IList<string> Recipients { get; set; }

        /// <summary>
        /// Get and set content of notification message
        /// </summary>
        public string Content { get; set; }
        #endregion
        
        /// <summary>
        /// Execute sending of notification
        /// </summary>
        public void Send()
        {
            //Utilizzo il metodo overloaded emettendo l'errore
            Send(true);
        }

        /// <summary>
        /// Execute sending of notification
        /// </summary>
        /// <param name="throwOnError">Flag used to contron exception throwing</param>
        public abstract void Send(bool throwOnError);
    }
}
