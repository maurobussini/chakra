using System;
using System.Threading.Tasks;

namespace ZenProgramming.Chakra.Core.Data
{
    /// <summary>
    /// Represents interface for data transaction
    /// </summary>
    public interface IDataTransactionAsync : IDataTransaction
    {
        /// <summary>
        /// Executes commit async on transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Executes commit async on transaction
        /// </summary>
        Task RollBackAsync();
    }
}
