using Core.Application.Mocks;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Persistence
{
    public class PersistenceContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public PersistenceContext(DbContextOptions<PersistenceContext> options) : base(options)
        {
            // Only in azure are migrations set to be applied automatically:
            if ((Environment.GetEnvironmentVariable("MIGRATIONS")?.Equals("1")) == true) 
            {
                Database.Migrate(); // automatic apply any pending migrations to db schema.
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);
        }
    }
}
