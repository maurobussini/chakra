using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZenProgramming.Chakra.Core.Utilities.Server.ConsoleMenu;
using ZenProgramming.Chakra.Core.Utilities.Server.Redirectors;

namespace ZenProgramming.Chakra.Core.Utilities.Server
{
    /// <summary>
    /// Utilities class used to write to standard output
    /// </summary>
    public static class ConsoleUtils
    {
        /// <summary>
        /// Execute capture
        /// </summary>
        /// <param name="outputFile">Output file</param>
        public static ConsoleFileRedirector RedirectOutputToFile(string outputFile)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(outputFile)) throw new ArgumentNullException(nameof(outputFile));

            //Creo un nuovo oggetto di cattura dell'output
            return new ConsoleFileRedirector(outputFile);
        }

        /// <summary>
        /// Execute write of a colored line on the standard console
        /// </summary>
        /// <param name="color">Color of message</param>
        /// <param name="message">Message to write</param>
        /// <param name="formatParams">Format parameters</param>
        public static void WriteColorLine(ConsoleColor color, string message, params object[] formatParams)
        {
            //Eseguo le memorizzazione del vecchio colore e imposto il nuovo
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            //Compngo il messaggio
            string fullMessage = formatParams.Length == 0
                ? message : string.Format(message, formatParams);

            //Eseguo la scrittura delle informazioni sulla console
            Console.WriteLine(fullMessage);

            //Reimposto il vecchio colore
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Execute write of a colored text on the standard console
        /// </summary>
        /// <param name="color">Color of message</param>
        /// <param name="message">Message to write</param>
        /// <param name="formatParams">Format parameters</param>
        public static void WriteColor(ConsoleColor color, string message, params object[] formatParams)
        {
            //Eseguo le memorizzazione del vecchio colore e imposto il nuovo
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            //Eseguo la scrittura delle informazioni sulla console
            Console.Write(message, formatParams);

            //Reimposto il vecchio colore
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Verify if command line arguments passed has specified command
        /// </summary>
        /// <param name="commandlineArguments">Command line arguments</param>
        /// <param name="commandToCheck">Argument to verify</param>
        /// <returns>Returns true if argument is defined</returns>
        public static bool HasCommandArgument(string[] commandlineArguments, string commandToCheck) 
        {
            //Verifico se l'argomento è contenuto nel sistema
            return commandlineArguments.Contains(commandToCheck); 
        }

        /// <summary>
        /// Pause execution of console application and wait for user input
        /// </summary>
        public static void Pause()
        {
            //Visualizzo il messaggio utente
            WriteColorLine(ConsoleColor.DarkYellow, "Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Force execution of console application
        /// </summary>
        public static void ShutDown()
        {
            //Termino forzatamente l'applicazione
            Environment.Exit(-1);
        }

        ///// <summary>
        ///// Launch console installation for service
        ///// </summary>
        ///// <param name="windowsServiceName">Windows service name</param>
        ///// <param name="executableLocation">Service executable</param>
        //public static void InstallService(string windowsServiceName, string executableLocation)
        //{
        //    //Se il servizio è già installato, esco
        //    if (ServiceControllerUtils.IsServiceInstalled(windowsServiceName))
        //    {
        //        //Visualizzo il messaggio e attendo per la terminazione
        //        WriteColorLine(ConsoleColor.Red, "Service '{0}' is already installed.", windowsServiceName);
        //    }
        //    else
        //    {
        //        //Avvio l'installazione del servizio e confermo
        //        ServiceControllerUtils.InstallService(windowsServiceName, executableLocation);
        //        WriteColorLine(ConsoleColor.Green, "Service '{0}' installation completed.", windowsServiceName);
        //    }

        //    //Metto in standby l'esecuzione
        //    Pause();
        //}

        ///// <summary>
        ///// Launch console uninstallation for service
        ///// </summary>
        ///// <param name="windowsServiceName">Windows service name</param>
        ///// <param name="executableLocation">Service executable</param>
        //public static void UninstallService(string windowsServiceName, string executableLocation)
        //{
        //    //Se il servizio non è installato, esco
        //    if (!ServiceControllerUtils.IsServiceInstalled(windowsServiceName))
        //    {
        //        //Visualizzo il messaggio e attendo per la terminazione
        //        WriteColorLine(ConsoleColor.Red, "Service '{0}' is not installed.", windowsServiceName);
                
        //    }
        //    else
        //    {
        //        //Avvio la disinstallazione del servizio
        //        ServiceControllerUtils.UninstallService(windowsServiceName, executableLocation);
        //        WriteColorLine(ConsoleColor.Green, "Service '{0}' installation completed.", windowsServiceName);
                
        //    }

        //    //Metto in standby l'esecuzione
        //    Pause();
        //}

        

        /// <summary>
        /// Executes render of a simple console menu on standard output
        /// </summary>
        /// <param name="title">Title of menu</param>
        /// <param name="menuElements">List of menu elements</param>
        public static void RenderMenu(string title, IList<ConsoleMenuElement> menuElements)
        {
            //Eseguo la validazione degli argomenti
            if (menuElements == null) throw new ArgumentNullException(nameof(menuElements));

            //Se il numero di elementi è uguale a zero, emetto eccezione
            if (menuElements.Count == 0) 
                throw new InvalidOperationException("At least one menu element must be provided.");
            
            //Variabile per il comando impartito
            string commandName;

            do
            {
                //Renderizzo la testata con il titolo dell'applicazione
                Console.WriteLine(string.Empty.PadLeft(70, '*'));
                Console.WriteLine("* Application '{0}'", title);
                Console.WriteLine(string.Empty.PadLeft(70, '*'));

                //Scorro tutti gli elementi ed eseguo la renderizzazione            
                foreach (var consoleMenuElement in menuElements)
                    consoleMenuElement.RenderSelection();

                //Renderizzo le istruzioni di gestione
                Console.Write("* - ");
                WriteColor(ConsoleColor.Cyan, "exit".PadRight(16));
                Console.WriteLine(" -> Exit program");
                Console.WriteLine(string.Empty.PadLeft(70, '*'));
                Console.WriteLine();
                Console.Write("* Command : ");

                //Eseguo la lettura del comando
                commandName = Console.ReadLine() ?? String.Empty;

                //Tento il recupero dell'elemento di console corrispondente
                ConsoleMenuElement single = menuElements.SingleOrDefault(e => e.CommandName.ToLower() == commandName.ToLower());

                //Se ho trovato l'elemento, lo eseguo
                if (single == null) continue;
                single.Execute();
            }
            while (commandName != "exit");
        }

        /// <summary>
        /// Read from console input a value, and execute cast
        /// </summary>
        /// <typeparam name="TValue">Target type</typeparam>
        /// <param name="message">Message</param>
        /// <param name="acceptCondition">Accept condition</param>
        /// <returns>Returns value</returns>
        public static TValue ReadLine<TValue>(string message, Func<TValue, bool> acceptCondition)
        {
            //Inizializzo il valore al default
            TValue value = default(TValue);
            bool isAccepted;

            do
            {
                //Scrivo il messaggio a video
                Console.Write("{0} : ", message);

                //Leggo il valore da console come stringa
                string consoleValue = Console.ReadLine() ?? String.Empty;

                //Se il tipo di uscita è nullabile, recupero il suo primitivo, altrimenti quello originale
                Type primitiveType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

                //Se il valore è un'enumerazione, tento la conversione
                if (typeof(TValue).IsEnum)
                    return (TValue)Enum.ToObject(typeof(TValue), consoleValue);

                //La conversione potrebbe non andare a buon fine
                try
                {
                    //Eseguo la conversione nel tipo target
                    value = (TValue)Convert.ChangeType(consoleValue, primitiveType);

                    //Confermo che la conversione è avvenuta
                    isAccepted = true;
                }
                catch (Exception)
                {
                    //Imposto il fallimento di accettazione
                    isAccepted = false;
                }
            }
            //Itero finchè è accettato e la condizione è soddisfatta
            while (!isAccepted || !acceptCondition(value));

            //Ritorno il valore
            return value;
        }

        /// <summary>
        /// Requests value of specified property of reference object specified
        /// </summary>
        /// <typeparam name="TTarget">Target object type</typeparam>
        /// <typeparam name="TProperty">Target property type</typeparam>
        /// <param name="targetInstance">Target instance</param>
        /// <param name="property">Property expression</param>
        /// <param name="acceptCondition">Accept condition</param>
        /// <param name="message">Custom user message</param>
        public static void ReadValue<TTarget, TProperty>(TTarget targetInstance, Expression<Func<TTarget, TProperty>> property,
            Func<TProperty, bool> acceptCondition, string message = null)
        {
            //Validazione argomenti
            if (targetInstance == null) throw new ArgumentNullException(nameof(targetInstance));
            if (property == null) throw new ArgumentNullException(nameof(property));

            //Eseguo la conversione del body dell'espressione
            MemberExpression memberExpression = property.Body as MemberExpression;
            if (memberExpression == null) throw new InvalidOperationException(
                "Specified expression is invalid. Must be something like 'c => c.Name'.");

            //Recupero il nome del membro se il 
            string messageToDisplay = message != null
                ? string.Format("[{0}] : {1}", memberExpression.Member.Name, message)
                : memberExpression.Member.Name;

            //Richiedo l'immissione del valore da console
            var value = ReadLine(messageToDisplay, acceptCondition);

            //Recupero il property info della proprietà di istanza
            PropertyInfo info = typeof(TTarget).GetProperty(memberExpression.Member.Name);
            if (info == null) throw new InvalidOperationException(string.Format("Unable to find " +
                "property '{0}' on type '{1}'.", memberExpression.Member.Name, typeof(TTarget).FullName));

            //Imposto il valore nell'oggetto
            info.SetValue(targetInstance, value, null);
        }
    }
}
