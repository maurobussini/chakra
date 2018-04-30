using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Entities
{
    public class Person: EntityBase<int>
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Surname { get; set; }

        [Required]
        public bool IsMale { get; set; }
    }
}
