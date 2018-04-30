using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.OAuth.Entities;

namespace ZenProgramming.Chakra.OAuth.ServiceLayers
{
    /// <summary>
    /// Service layer for manage audience
    /// </summary>
    /// <typeparam name="TAudience">Type of audience entity</typeparam>
    public interface IAudienceServiceLayer<TAudience>
        where TAudience : class, IAudience, new()
    {
        /// <summary>
        /// Get single audience using clientId
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <returns>Return entity or null</returns>
        TAudience GetAudience(string clientId);

        /// <summary>
        /// Fetch list of all audiences on platform
        /// </summary>
        /// <returns>Returns list of audiences</returns>
        IList<TAudience> FetchAudiences();

        /// <summary>
        /// Save audience entity on storage
        /// </summary>
        /// <param name="entity">Entity instance</param>
        /// <returns>Returns validation result</returns>
        IList<ValidationResult> SaveAudience(TAudience entity);
    }
}
