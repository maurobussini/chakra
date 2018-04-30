using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenProgramming.Chakra.Core.Messaging
{
    /// <summary>
    /// Broker for event bus
    /// </summary>
    public class EventBus
    {
        #region Static initialization
        private static Lazy<EventBus> _Instance = new Lazy<EventBus>(() => new EventBus());

        /// <summary>
        /// Default instance
        /// </summary>
        public static EventBus Default => _Instance.Value;

        #endregion

        #region Public properties
        /// <summary>
        /// Registrations
        /// </summary>
        public IList<EventMapping> EventMappings { get; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        private EventBus()
        {
            //Per evitare l'inizializzazione

            //Inizializzo la lista delle registrazioni
            EventMappings = new List<EventMapping>();
        }

	    /// <summary>
	    /// Broadcasts a message on event bus
	    /// </summary>
	    /// <param name="message">Message</param>
	    /// <param name="runSynchronously">Run broadcast in sync mode</param>
	    public void Broadcast(IEventMessage message, bool runSynchronously = false)
        {
            //Validazione argomenti
            if (message == null) throw new ArgumentNullException(nameof(message));

            //Scorro tutta la lista delle mappature
            foreach (var currentMapping in EventMappings)
            {
                //Se il tipo del messaggio corrente NON è una subclass
                //del tipo specificato per il listener corrente, passo al prossimo
                if (message.GetType() != currentMapping.MessageType)
                    continue;

                //Scorro tutte le registrazioni per il tipo di messaggio corrente
                foreach (var currentRegistration in currentMapping.Registrations)
                {
                    //Scorro tutte le azioni
                    foreach (var currentAction in currentRegistration.Actions)
                    {
						//Se siamo in modalità sincrona
	                    if (runSynchronously)
	                    {
							//Mando in esecuzione l'azione con il messaggio
							currentAction(message);							
	                    }
	                    else
	                    {
							//Esegui in async
							Task.Run(() =>
							{
								//Mando in esecuzione l'azione con il messaggio
								currentAction(message);
							});
						}	                    
                    }
                }
            }
        }

        /// <summary>
        /// Register for message
        /// </summary>
        /// <typeparam name="TEventMessage">Type of message</typeparam>
        /// <param name="recipient">Recipient</param>
        /// <param name="action">Action on event</param>
        public void Register<TEventMessage>(object recipient, Action<TEventMessage> action)
            where TEventMessage : class, IEventMessage
        {
            //Validazione argomenti
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (action == null) throw new ArgumentNullException(nameof(action));

            //Verifico la presenza della mappatura per il tipo di messaggio
            EventMapping mapping = EventMappings
                .SingleOrDefault(e => e.MessageType == typeof(TEventMessage));

            //Se non è stato trovato, lo creo
            if (mapping == null)
            {
                //Creo al nuova mappatura
                mapping = new EventMapping
                {
                    MessageType = typeof(TEventMessage),
                    Registrations = new List<EventRegistration>()
                };

                //Accodo la mappatura
                EventMappings.Add(mapping);
            }

            //Verifico la presenza del recipient
            EventRegistration registration = mapping.Registrations
                .SingleOrDefault(r => r.RecipientType == recipient.GetType());

            //Se non è stato trovato, lo creo
            if (registration == null)
            {
                //Creo l'oggetto e inizializzo
                registration = new EventRegistration
                {
                    RecipientType = recipient.GetType(),
                    Actions = new List<Action<IEventMessage>>()
                };

                //Accodo la registrazione
                mapping.Registrations.Add(registration);
            }

            //Wrappo l'azione in una generica
            Action<IEventMessage> genericAction = msg =>
            {
                //Eseguo il cast dell'argomento
                TEventMessage argument = msg as TEventMessage;

                //Invoco l'azione
                action(argument);
            };

            //Accodo l'azione
            registration.Actions.Add(genericAction);
        }
    }
}
