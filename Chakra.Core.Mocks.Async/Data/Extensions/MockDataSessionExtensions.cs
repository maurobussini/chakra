using System;
using ZenProgramming.Chakra.Core.Async.Data.Repositories;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Data;
using ZenProgramming.Chakra.Core.Mocks.Data.Repositories;

namespace ZenProgramming.Chakra.Core.Mocks.Async.Data.Extensions
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
        public static IMockDataSession AsMockDataSessionAsync(this IDataSession instance) 
        {
            //Arguments validation
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Tento il cast della sessione generica a MockDataSession
            if (instance is IMockDataSession mockSession)
                return mockSession;

            throw new InvalidCastException($"Specified data session of type '{instance.GetType().FullName}' " + 
                $"cannot be converted to type '{typeof(IMockDataSession).FullName}'.");

            //Returns instance
        }

        /// <summary>
        /// Execute resolution of repository interface using specified clas
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <returns>Returns repository instance</returns>
        public static TRepositoryInterface ResolveRepositoryAsync<TRepositoryInterface>(this IDataSession dataSession)
            where TRepositoryInterface : IRepositoryAsync
        {
            //Utilizzo il metodo presente sull'helper
            return Core.Async.Data.Repositories.Helpers.RepositoryHelper
                .Resolve<TRepositoryInterface, IMockRepositoryAsync>(dataSession);
        }
    }
}
