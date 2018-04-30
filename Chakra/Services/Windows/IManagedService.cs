using System.ServiceProcess;
using ZenProgramming.Chakra.Services.Windows.Events;

namespace ZenProgramming.Chakra.Services.Windows
{
    /// <summary>
    /// Interface for managed services
    /// </summary>
    public interface IManagedService
    {
        #region Public events
        /// <summary>
        /// Message raised by windows service
        /// </summary>
        event ServiceMessageRaisedEventHandler MessageRaised;

        /// <summary>
        /// Error raised by windows service
        /// </summary>
        event ServiceErrorRaisedEventHandler ErrorRaised;
        #endregion

        #region Public properties
        /// <summary>
        /// Get service name
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Get display name
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Get service description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get service start mode
        /// </summary>
        ServiceStartMode StartType { get; }

        /// <summary>
        /// Get service execution account
        /// </summary>
        ServiceAccount Account { get; }

        /// <summary>
        /// Get account username
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Get account password
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Get dependencies of windows service
        /// </summary>
        string[] ServicesDependedOn { get; }
        #endregion
    }
}
