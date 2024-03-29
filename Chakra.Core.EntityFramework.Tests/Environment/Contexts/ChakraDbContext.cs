﻿using Microsoft.EntityFrameworkCore;
using ZenProgramming.Chakra.Core.Tests.Environment.Entities;

namespace ZenProgramming.Chakra.Core.EntityFramework.Tests.Environment.Contexts
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

        /// <summary>
        /// Departments
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Add SQL configuration
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CHAKRA;User ID=sa;Password=password;Pooling=True");

            //Base configuration
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Map of entities
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Department>().ToTable("Departments");
        }
    }
}
