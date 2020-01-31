using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Entities;

namespace Chakra.Core.Tests.Environment.Entities
{
    /// <summary>
    /// Entity for department
    /// </summary>
    public class Department: ModernEntityBase
    {
        /// <summary>
        /// Code
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Code { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
