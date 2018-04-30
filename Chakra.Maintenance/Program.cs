using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Utilities.Server;
using ZenProgramming.Chakra.Core.Utilities.Server.ConsoleMenu;

namespace Chakra.Maintenance
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleUtils.RenderMenu("Chakra Maintenance", new List<ConsoleMenuElement>
            {
                new ConsoleMenuElement("t", "Test .NET Core on Docker", TestDotNetCoreOnDocker)
            });

            
        }

        private static void TestDotNetCoreOnDocker()
        {
            Console.WriteLine("Hello World Docker!");
        }
    }
}
