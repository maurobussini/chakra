using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.Async.Data
{
    /// TIPS: customizzare classe dataTransaction
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
