using System;
using ZenProgramming.Chakra.Core.Data.Mockups;

namespace ZenProgramming.Chakra.Core.Data
{
    /// <summary>
    /// Represents factory class for 
    /// </summary>
    public static class SessionFactory
    {
        /// <summary>
        /// Type of default data session registered
        /// </summary>
        public static Type DefaultDataSessionType { get; private set; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static SessionFactory()
        {
            //Eseguo la registrazione del tipo della data session di default
            RegisterDefaultDataSession<MockupDataSession>();
        }        

        /// <summary>
        /// Execute register of default data session type
        /// </summary>
        /// <typeparam name="TDataSession">Type of data session</typeparam>
        public static void RegisterDefaultDataSession<TDataSession>()
            where TDataSession : class, IDataSession, new()
        {
            //Eseguo la registrazione del tipo di default
            DefaultDataSessionType = typeof (TDataSession);
        }

        /// <summary>
        /// Open a new session on storage
        /// </summary>
        /// <returns>Returns an open, usable session</returns>
        public static TDataSession OpenSession<TDataSession>()
            where TDataSession : class, IDataSession, new()
        {
            //Eseguo la creazione di una istanza di data session
            var instance = new TDataSession();

            //Mando in uscita l'istanza
            return instance;
        }

        /// <summary>
        /// Open data session configured as default
        /// </summary>
        /// <returns>Returns data session instance</returns>
        public static IDataSession OpenSession()
        {
            //Eseguo la creazione dell'istanza
            var instance = Activator.CreateInstance(DefaultDataSessionType) as IDataSession;
            if (instance == null) throw new InvalidCastException(string.Format("Unable to cast " +
                "instance of type '{0}' to target type '{1}'.", DefaultDataSessionType.FullName, typeof(IDataSession).FullName));

            //Ritorno l'istanza
            return instance;
        }
    }
}
