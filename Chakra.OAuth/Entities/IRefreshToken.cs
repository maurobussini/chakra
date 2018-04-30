using System;
using System.ComponentModel.DataAnnotations;

namespace ZenProgramming.Chakra.OAuth.Entities
{
    /// <summary>
    /// Interface for RefreshToken
    /// </summary>
    public interface IRefreshToken
    {
        /// <summary>
        /// Hash for refresh token
        /// </summary>
        [Required]
        [StringLength(255)]
        string TokenHash { get; set; }

        /// <summary>
        /// ClientdId
        /// </summary>
        [Required]
        [StringLength(255)]
        string ClientId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [StringLength(255)]
        string UserName { get; set; }

        /// <summary>
        /// Issue time in UTC format
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31")]
        DateTime IssuedUtc { get; set; }

        /// <summary>
        /// Expire time in UTC format
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31")]
        DateTime ExpiresUtc { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [StringLength(2000)]
        string ProtectedTicket { get; set; }
    }
}
