using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Utilities.Server.ConsoleMenu
{
    /// <summary>
    /// Console menu interface
    /// </summary>
    public interface IConsoleMenuContainer
    {
        /// <summary>
        /// List of elements on menu container
        /// </summary>
        IList<ConsoleMenuElement> Elements { get; }

        /// <summary>
        /// Launch element matching command criteria
        /// </summary>
        /// <param name="commandCriteria">Command criteria (es. main>import>products)</param>
        /// <returns>Returns element matching criteria or null</returns>
        ConsoleMenuElement Launch(string commandCriteria);
    }
}
