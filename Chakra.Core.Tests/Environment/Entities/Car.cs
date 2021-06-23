using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Entities
{
    public class Car: ModernEntityBase
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Brand { get; set; }
    }
}