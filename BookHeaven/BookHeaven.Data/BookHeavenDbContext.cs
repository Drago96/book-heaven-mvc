﻿using BookHeaven.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Data
{
    public class BookHeavenDbContext : IdentityDbContext<User>
    {
        public DbSet<Location> Locations { get; set; }

        public DbSet<SiteVisit> Visits { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public BookHeavenDbContext(DbContextOptions<BookHeavenDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Location>()
                .HasIndex(l => new { l.City, l.Country })
                .IsUnique();

            builder
                .Entity<User>()
                .HasMany(p => p.PublishedBooks)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder
                .Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

            builder
                .Entity<Book>()
                .HasMany(b => b.Categories)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId);

            builder
                .Entity<Category>()
                .HasMany(b => b.Books)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);

            builder
                .Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Book)
                .WithMany(b => b.ShoppingCarts)
                .HasForeignKey(sci => sci.BookId);

            builder
                .Entity<ShoppingCartItem>()
                .HasOne(sci => sci.User)
                .WithMany(b => b.ShoppingCartItems)
                .HasForeignKey(sci => sci.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}