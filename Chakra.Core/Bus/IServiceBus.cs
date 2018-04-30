using System;
using System.Threading.Tasks;

namespace ZenProgramming.Chakra.Core.Bus
{
    /// <summary>
    /// Interface for generic service bus on a specific message
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    public interface IServiceBus<TMessage>: IDisposable
        where TMessage: class, new()
    {
        /// <summary>
        /// Bus address
        /// </summary>
        string BusAddress { get; set; }

        /// <summary>
        /// Channel (queue) name
        /// </summary>
        string ChannelName { get; set; }

        /// <summary>
        /// Message received on channel
        /// </summary>
        event EventHandler<TMessage> MessageReceived;

        /// <summary>
        /// Sends a single message on service bus
        /// </summary>
        /// <param name="message">Message to send</param>
        void Send(TMessage message);

        /// <summary>
        /// Sends a single message on service bus (async)
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>Returns async task</returns>
        Task SendAsync(TMessage message);

        /// <summary>
        /// Start listening incoming messages on channel
        /// </summary>
        void StartListening();

        /// <summary>
        /// Stop listening incoming messages on channel
        /// </summary>
        void StopListening();
    }
}
