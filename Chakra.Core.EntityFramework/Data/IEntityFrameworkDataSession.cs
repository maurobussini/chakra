using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Data;

namespace ZenProgramming.Chakra.Core.EntityFramework.Data
{
    /// <summary>
    /// Entity framework data session interface
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IEntityFrameworkDataSession<TDbContext> : IDataSession
        where TDbContext : DbContext, new()
    {
        /// <summary>
        /// Entity framework database context
        /// </summary>
        TDbContext Context { get; }

        /// <summary>
        /// Set active transaction in current data session
        /// </summary>
        /// <param name="transaction">Active transaction</param>
        void SetActiveTransaction(IEntityFrameworkTransaction<TDbContext> transaction);
    }
}
