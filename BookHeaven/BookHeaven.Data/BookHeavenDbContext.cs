namespace BookHeaven.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BookHeavenDbContext : IdentityDbContext<User>
    {
        public BookHeavenDbContext(DbContextOptions<BookHeavenDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
