using System;
using System.Collections.Generic;
using System.Linq;

namespace ZenProgramming.Chakra.Core.Utilities.Server.ConsoleMenu
{
    /// <summary>
    /// Abstract implementation for console menu container
    /// </summary>
    /// <typeparam name="TContainer">Type of container for self injection</typeparam>
    public abstract class ConsoleMenuContainerBase<TContainer> : IConsoleMenuContainer
        where TContainer : ConsoleMenuContainerBase<TContainer>, new()
    {
        #region Singleton initialization
        /// <summary>
        /// Holder for static instance
        /// </summary>
        private static TContainer _Current;

        /// <summary>
        /// Current instance
        /// </summary>
        public static TContainer Current
        {
            get { return _Current ?? (_Current = new TContainer()); }
        }
        #endregion

        #region Private fields
        private IList<ConsoleMenuElement> _Elements;
        #endregion

        #region Public properties
        /// <summary>
        /// List of elements on menu container
        /// </summary>
        public IList<ConsoleMenuElement> Elements
        {
            get { return _Elements ?? (_Elements = GenerateElements()); }
        }
        #endregion

        /// <summary>
        /// Generates elements of current console container
        /// </summary>
        /// <returns></returns>
        protected abstract IList<ConsoleMenuElement> GenerateElements();

        /// <summary>
        /// Renders summary of current container elements
        /// </summary>
        public void Summary()
        {
            //Execute render of elements
            ConsoleUtils.RenderMenu(typeof(TContainer).Name, Elements);
        }

        /// <summary>
        /// Launch element matching command criteria
        /// </summary>
        /// <param name="commandCriteria">Command criteria (es. main>import>products)</param>
        /// <returns>Returns element matching criteria or null</returns>
        public ConsoleMenuElement Launch(string commandCriteria)
        {
            //Validazione argomenti
            if (string.IsNullOrEmpty(commandCriteria)) throw new ArgumentNullException(nameof(commandCriteria));

            //Tento lo split del criterio per ">"
            string[] segments = commandCriteria.Split('>');

            //Male che vada ho un solo elemento; tento quindi la selezione
            //dell'elemento nel contesto corrente che corrisponde
            ConsoleMenuElement single = Elements.SingleOrDefault(e => 
                e.CommandName.Equals(segments[0], StringComparison.InvariantCultureIgnoreCase));

            //Se non ho trovato nulla, ritorno null
            if (single == null)
                return null;

            throw new NotImplementedException();
        }
    }
}
