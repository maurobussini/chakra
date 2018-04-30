using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.ServicesLayers
{
    /// <summary>
    /// Represents interface for service layer that interact with entities
    /// </summary>
    public interface IEntityServiceLayer
    {
        /// <summary>
        /// Get single entity using primary identifier
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <param name="id">Primary identifier</param>
        /// <returns>Returns instance or null value</returns>
        TEntity GetSingle<TEntity, TRepository>(int id)
            where TEntity : class, IEntity, new()
            where TRepository : IRepository<TEntity>;

        /// <summary>
        /// Fetch list of all entities
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <returns>Returns list of entity</returns>
        IList<TEntity> FetchAll<TEntity, TRepository>()
            where TEntity : class, IEntity, new()
            where TRepository : IRepository<TEntity>;

        /// <summary>
        /// Count all entities
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <returns>Count all entities</returns>
        int CountAll<TEntity, TRepository>()
            where TEntity : class, IEntity, new()
            where TRepository : IRepository<TEntity>;
    }
}
