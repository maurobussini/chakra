using System;
using ZenProgramming.Chakra.Core.Data.Repositories;

namespace ZenProgramming.Chakra.Core.Data
{
    /// <summary>
    /// Represents inteface for session of data
    /// </summary>
    public interface IDataSession : IDisposable
    {
        /// <summary>
        /// Begin new transaction on active session
        /// </summary>
        /// <returns>Returns data transaction instance</returns>
        IDataTransaction BeginTransaction();

        /// <summary>
        /// Executes convert of session instance on specified type
        /// </summary>
        /// <typeparam name="TOutput">Target type</typeparam>
        /// <returns>Returns converted instance</returns>
        TOutput As<TOutput>()
            where TOutput : class;

        /// <summary>
        /// Execute resolve of repository interface on concrete
        /// type that matchh data provider defined by data session
        /// </summary>
        /// <typeparam name="TRepositoryInterface">Type of repository interface</typeparam>
        /// <returns>Returns repository instance</returns>
        TRepositoryInterface ResolveRepository<TRepositoryInterface>()
            where TRepositoryInterface : IRepository;

        /// <summary>
        /// Active transaction on data session
        /// </summary>
        IDataTransaction Transaction { get; }
    }
}
