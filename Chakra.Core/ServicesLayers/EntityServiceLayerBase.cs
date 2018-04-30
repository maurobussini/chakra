using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.ServicesLayers
{
    /// <summary>
    /// Represents abstract class for service layer that interact with entities
    /// </summary>
    public abstract class EntityServiceLayerBase: IEntityServiceLayer
    {
        /// <summary>
        /// Get single entity using primary identifier
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <param name="id">Primary identifier</param>
        /// <returns>Returns instance or null value</returns>
        public TEntity GetSingle<TEntity, TRepository>(int id)
            where TEntity : class, IEntity, new()
            where TRepository : IRepository<TEntity>
        {
            //Eseguo la validazione degli argomenti
            if (id == 0) throw new ArgumentOutOfRangeException(nameof(id));


            throw new NotImplementedException();
            ////Istanzio il repository da utilizzare per l'operazione
            //using (TRepository repository = RepositoryFactory.Create<TRepository>())
            //{
            //    //Lancio l'operazione sul repository
            //    return repository.GetSingle(id);
            //}
        }

        /// <summary>
        /// Fetch list of all entities
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <returns>Returns list of entity</returns>
        public IList<TEntity> FetchAll<TEntity, TRepository>()
            where TEntity : class, IEntity, new()
            where TRepository : IRepository<TEntity>
        {
            throw new NotImplementedException();
            ////Istanzio il repository da utilizzare per l'operazione
            //using (TRepository repository = RepositoryFactory.Create<TRepository>())
            //{
            //    //Lancio l'operazione sul repository
            //    return repository.FetchAll();
            //}
        }

        /// <summary>
        /// Count all entities
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <returns>Count all entities</returns>
        public int CountAll<TEntity, TRepository>()
            where TEntity : class, IEntity, new()
            where TRepository : IRepository<TEntity>
        {
            throw new NotImplementedException();
            ////Istanzio il repository da utilizzare per l'operazione
            //using (TRepository repository = RepositoryFactory.Create<TRepository>())
            //{
            //    //Lancio l'operazione sul repository
            //    return repository.CountAll();
            //}
        }
    }
}
