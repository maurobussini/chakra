using System;
using System.Threading.Tasks;

namespace ZenProgramming.Chakra.Core.Bus
{
    /// <summary>
    /// Abstract class for generic service bus
    /// </summary>
    public abstract class ServiceBusBase<TMessage>: IServiceBus<TMessage>
        where TMessage : class, new()
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        /// <summary>
        /// Message received on channel
        /// </summary>
        public event EventHandler<TMessage> MessageReceived;

        #region Public properties
        /// <summary>
        /// Bus address
        /// </summary>
        public string BusAddress { get; set; }

        /// <summary>
        /// Channel (queue) name
        /// </summary>
        public string ChannelName { get; set; }
        #endregion        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="busAddress">Bus address</param>
        /// <param name="channelName">Channel name</param>
        protected ServiceBusBase(string busAddress, string channelName)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(busAddress)) throw new ArgumentNullException(nameof(busAddress));
            if (string.IsNullOrEmpty(channelName)) throw new ArgumentNullException(nameof(channelName));

            //Imposto i valori
            BusAddress = busAddress;
            ChannelName = channelName;
        }

        /// <summary>
        /// Sends a single message on service bus
        /// </summary>
        /// <param name="message">Message to send</param>
        public abstract void Send(TMessage message);

        /// <summary>
        /// Sends a single message on service bus (async)
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>Returns async task</returns>
        public abstract Task SendAsync(TMessage message);

        /// <summary>
        /// Start listening incoming messages on channel
        /// </summary>
        public abstract void StartListening();

        /// <summary>
        /// Stop listening incoming messages on channel
        /// </summary>
        public abstract void StopListening();

        /// <summary>
        /// Raise event "MessageReceived" with provided message
        /// </summary>
        /// <param name="message">Message context</param>
        protected void RaiseMessageReceived(TMessage message)
        {
            //Sollevo solo se l'evento è stato gestito
            MessageReceived?.Invoke(this, message);
        }

        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
        /// </summary>
        ~ServiceBusBase()
        {
            //Richiamo i dispose implicito
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Eseguo una dispose esplicita
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        protected virtual void Dispose(bool isDisposing)
        {
            //Se l'oggetto è già rilasciato, esco
            if (_IsDisposed)
                return;

            //Se è richiesto il rilascio esplicito
            if (isDisposing)
            {
                //RIlascio della logica non finalizzabile
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;
        }
    }
}