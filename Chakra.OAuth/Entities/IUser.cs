using System;
using System.ComponentModel.DataAnnotations;

namespace ZenProgramming.Chakra.OAuth.Entities
{
    /// <summary>
    /// Interface for User entity
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.]+$")]
        string UserName { get; set; }

        /// <summary>
        /// Password hash (SHA-384)
        /// </summary>
        [StringLength(1024)]
        string PasswordHash { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [StringLength(255)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.'\+&=]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        string Email { get; set; }

        /// <summary>
        /// Person name
        /// </summary>
        [StringLength(255)]
        string PersonName { get; set; }

        /// <summary>
        /// Person surname
        /// </summary>
        [StringLength(255)]
        string PersonSurname { get; set; }

        /// <summary>
        /// Flag for enable user
        /// </summary>
        [Required]
        bool IsEnabled { get; set; }

        /// <summary>
        /// Last access date
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31")]
        DateTime? LastAccessDate { get; set; }

        /// <summary>
        /// Flag for locked user (ex. too much tentatives)
        /// </summary>
        [Required]
        bool IsLocked { get; set; }
    }
}
