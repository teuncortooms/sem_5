using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
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
        }
    }
}
