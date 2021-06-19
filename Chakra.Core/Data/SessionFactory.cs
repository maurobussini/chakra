using System;

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
        private readonly static object lockObj = new object();

        /// <summary>
        /// Execute register of default data session type
        /// </summary>
        /// <typeparam name="TDataSession">Type of data session</typeparam>
        public static void RegisterDefaultDataSession<TDataSession>()
            where TDataSession : class, IDataSession, new()
        {
            lock (lockObj) 
            {
                //Eseguo la registrazione del tipo di default
                DefaultDataSessionType = typeof(TDataSession);
            }
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
            //Verifica della presenza della sessione di default
            if (DefaultDataSessionType == null)
                throw new InvalidProgramException("Default data session not found." + 
                    "Please register with 'RegisterDefaultDataSession<TDataSession>'");

            //Eseguo la creazione dell'istanza
            var instance = Activator.CreateInstance(DefaultDataSessionType) as IDataSession;
            if (instance == null) throw new InvalidCastException("Unable to cast " +
                                                                 $"instance of type '{DefaultDataSessionType.FullName}' to target type '{typeof(IDataSession).FullName}'.");

            //Ritorno l'istanza
            return instance;
        }
    }
}
