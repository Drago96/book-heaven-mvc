using BookHeaven.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Data
{
    public class BookHeavenDbContext : IdentityDbContext<User>
    {
        public DbSet<Location> Locations { get; set; }

        public DbSet<SiteVisit> Visits { get; set; }

        public BookHeavenDbContext(DbContextOptions<BookHeavenDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Location>()
                .HasIndex(l => new {l.City, l.Country})
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}