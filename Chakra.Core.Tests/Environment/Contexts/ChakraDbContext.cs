using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Contexts
{
    /// <summary>
    /// Entity Framework DbContext
    /// </summary>
    public class ChakraDbContext: DbContext
    {
        /// <summary>
        /// Persons
        /// </summary>
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Add SQL configuration
            optionsBuilder.UseSqlServer("Data Source=ICUBEDNB01;Initial Catalog=CHAKRA;User ID=sa;Password=P@ssw0rd;Pooling=True");

            //Base configuration
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mappo le entità
            modelBuilder.Entity<Person>().ToTable("icCHAKRA_Persons");
        }
    }
}
