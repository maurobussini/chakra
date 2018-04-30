using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Persistences;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Persistences
{
    /// <summary>
    /// Credential
    /// </summary>
    public class Credential : IPersistence
    {
        /// <summary>
        /// Unique key (username)
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Key { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
