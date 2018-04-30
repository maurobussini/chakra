using System;

namespace ZenProgramming.Chakra.Core.Entities
{
    /// <summary>
    /// Interface for rich entities
    /// </summary>
    public interface IRichEntity : IEntity
    {
        /// <summary>
        /// Creation time
        /// </summary>
        DateTime? CreationTime { get; set; }

        /// <summary>
        /// Record created by
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Last update time
        /// </summary>
        DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// Record updated by
        /// </summary>
        string LastUpdateBy { get; set; }

        /// <summary>
        /// Flag for mark deleted entities
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
