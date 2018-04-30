using System;

namespace ZenProgramming.Chakra.Core.Data
{
    /// <summary>
    /// Represents interface for data transaction
    /// </summary>
    public interface IDataTransaction : IDisposable
    {
        /// <summary>
        /// Identifies if transaction is active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Specifies if transaction was rolled back
        /// </summary>
        bool WasRolledBack { get; }

        /// <summary>
        /// Specifies if transaction was committed
        /// </summary>
        bool WasCommitted { get; }

        /// <summary>
        /// Executes commit on transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Executes rollback on transaction
        /// </summary>
        void Rollback();
    }
}
