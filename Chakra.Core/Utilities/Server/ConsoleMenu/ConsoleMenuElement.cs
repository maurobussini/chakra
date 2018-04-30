using System;

namespace ZenProgramming.Chakra.Core.Utilities.Server.ConsoleMenu
{
    /// <summary>
    /// Represents console menu element
    /// </summary>
    public class ConsoleMenuElement
    {
        /// <summary>
        /// Command name
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Command description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Action that needs to be executed
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandName">Command name</param>
        /// <param name="description">Description</param>
        /// <param name="action">Action to execute</param>
        public ConsoleMenuElement(string commandName, string description, Action action)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrEmpty(commandName)) throw new ArgumentNullException(nameof(commandName));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
            if (action == null) throw new ArgumentNullException(nameof(action));

            //Imposto le proprietà
            CommandName = commandName;
            Description = description;
            Action = action;
        }

        /// <summary>
        /// Execute render of current element selection to standard output
        /// </summary>
        internal void RenderSelection()
        {
            //Renderizzo sulla console la selezione
            Console.Write("* - ");
            ConsoleUtils.WriteColor(ConsoleColor.Cyan, CommandName.PadRight(16));
            Console.WriteLine(" -> {0}", Description);
        }

        /// <summary>
        /// Execute render of current element execution to standard output
        /// </summary>
        internal void Execute()
        {
            //Renderizzo sulla console l'inizio esecuzione
            Console.Clear();
            Console.WriteLine(string.Empty.PadLeft(70, '*'));
            Console.Write("* Executing command ");
            ConsoleUtils.WriteColor(ConsoleColor.Yellow, CommandName);
            Console.WriteLine("...");            
            Console.WriteLine();

            try
            {
                //Mando in esecuzione l'azione associata
                Action.Invoke();

                //Renderizzo sulla console il completamento dell'esecuzione            
                Console.Write("* Command ");
                ConsoleUtils.WriteColor(ConsoleColor.Green, CommandName);
                Console.WriteLine(" completed.");
                Console.WriteLine(string.Empty.PadLeft(70, '*'));
                Console.WriteLine();
            }
            catch (Exception exc)
            {
                //Renderizzo sulla console il completamento dell'esecuzione            
                Console.Write("* Command ");
                ConsoleUtils.WriteColor(ConsoleColor.Red, CommandName);
                Console.WriteLine(" completed with errors.");
                ConsoleUtils.WriteColorLine(ConsoleColor.Red, exc.ToString());
                Console.WriteLine(string.Empty.PadLeft(70, '*'));
                Console.WriteLine();
            }                       
        }
    }
}
