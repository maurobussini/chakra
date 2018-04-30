using System.ComponentModel.DataAnnotations;

namespace ZenProgramming.Chakra.OAuth.Entities
{
    /// <summary>
    /// Interface for Audience entity
    /// </summary>
    public interface IAudience
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(255)]
        string Name { get; set; }

        /// <summary>
        /// Client id
        /// </summary>
        [Required]
        [StringLength(255)]
        string ClientId { get; set; }

        /// <summary>
        /// Client secret
        /// </summary>
        [Required]
        [StringLength(255)]
        string ClientSecret { get; set; }

        /// <summary>
        /// Is native application
        /// </summary>
        [Required]
        bool IsNative { get; set; }

        /// <summary>
        /// Is enabled
        /// </summary>
        [Required]
        bool IsEnabled { get; set; }

        /// <summary>
        /// Refresh token life time (in minutes)
        /// </summary>
        [Required]
        int RefreshTokenLifeTime { get; set; }

        /// <summary>
        /// Allowed origin
        /// </summary>
        [Required]
        [StringLength(255)]
        string AllowedOrigin { get; set; }

        /// <summary>
        /// Current audience has administrative access
        /// </summary>
        [Required]
        bool HasAdministrativeAccess { get; set; }
    }
}
