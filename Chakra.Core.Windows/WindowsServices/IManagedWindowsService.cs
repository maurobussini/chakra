using ZenProgramming.Chakra.Core.Windows.WindowsServices.Events;

namespace ZenProgramming.Chakra.Core.Windows.WindowsServices
{
    /// <summary>
    /// Interface for managed windows services
    /// </summary>
    public interface IManagedWindowsService
    {
        /// <summary>
        /// Message raised by windows service
        /// </summary>
        event ServiceMessageRaisedEventHandler MessageRaised;

        /// <summary>
        /// Error raised by windows service
        /// </summary>
        event ServiceErrorRaisedEventHandler ErrorRaised;

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
    }
}
