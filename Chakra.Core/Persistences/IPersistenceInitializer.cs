using System;
using System.Collections.Generic;

namespace ZenProgramming.Chakra.Core.Persistences
{
    /// <summary>
    /// Interface for persistence initializer
    /// </summary>
    public interface IPersistenceInitializer
    {
        /// <summary>
        /// Element type
        /// </summary>
        Type ElementType { get; }

        /// <summary>
        /// Initialize and fetch list of elements
        /// </summary>
        IList<TPersistence> Fetch<TPersistence>()
			where TPersistence : class, IPersistence;
    }
}
