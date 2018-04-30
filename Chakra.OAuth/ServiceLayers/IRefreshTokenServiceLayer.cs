using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ZenProgramming.Chakra.OAuth.Entities;

namespace ZenProgramming.Chakra.OAuth.ServiceLayers
{
    /// <summary>
    /// Service layer for manage refresh token
    /// </summary>
    /// <typeparam name="TRefreshToken">Type of refresh token entity</typeparam>
    public interface IRefreshTokenServiceLayer<TRefreshToken>
        where TRefreshToken : class, IUser, new()
    {
        /// <summary>
        /// Get single refresh token using token hash
        /// </summary>
        /// <param name="tokenHash">Token hash</param>
        /// <returns>Returns instance or null</returns>
        TRefreshToken GetRefreshToken(string tokenHash);

        /// <summary>
        /// Get single refresh token using clientId and user name
        /// </summary>
        /// <param name="clientId">ClientId</param>
        /// <param name="userName">User name</param>
        /// <returns>Returns instance or null</returns>
        TRefreshToken GetRefreshToken(string clientId, string userName);

        /// <summary>
        /// Fetch list of all refresh tokens
        /// </summary>
        /// <param name="filter">Filter on data</param>
        /// <returns>Returns list of entities</returns>
        IList<TRefreshToken> FetchRefreshTokens(Expression<Func<TRefreshToken, bool>> filter = null);

        /// <summary>
        /// Save provided refresh token
        /// </summary>
        /// <param name="entity">Refresh toke</param>
        /// <returns>Returns validations</returns>
        IList<ValidationResult> SaveRefreshToken(TRefreshToken entity);

        /// <summary>
        /// Delete single refresh token
        /// </summary>
        /// <param name="entity">Entity</param>
        void DeleteRefreshToken(TRefreshToken entity);
    }
}
