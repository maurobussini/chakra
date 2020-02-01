using System;
using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.Mocks.Data.Extensions
{
    /// <summary>
    /// Extensions for data session
    /// </summary>
    public static class DataSessionExtensions
    {
        /// <summary>
        /// Convert provided data session in "IMockDataSession"
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <returns>Returns converted data session</returns>
        public static IMockDataSession AsMockDataSession(this IDataSession instance) 
        {
            //Arguments validation
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Tento il cast della sessione generica a MockDataSession
            IMockDataSession mockSession = instance as IMockDataSession;
            if (mockSession == null)
                throw new InvalidCastException($"Specified data session of type '{instance.GetType().FullName}' " + 
                    $"cannot be converted to type '{typeof(IMockDataSession).FullName}'.");

            //Returns instance
            return mockSession;
        }
    }
}
