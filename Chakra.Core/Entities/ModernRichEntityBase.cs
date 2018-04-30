using System;

namespace ZenProgramming.Chakra.Core.Entities
{
    /// <summary>
    /// Basic implementation for rich modern entity
    /// </summary>
    public abstract class ModernRichEntityBase: ModernEntityBase, IRichEntity
    {
        /// <summary>
        /// Creation time
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }

        /// <summary>
        /// Record created by
        /// </summary>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// Last update time
        /// </summary>
        public virtual DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// Record updated by
        /// </summary>
        public virtual string LastUpdateBy { get; set; }

        /// <summary>
        /// Flag for mark deleted entities
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}
