﻿// <auto-generated />
using BookHeaven.Data;
using BookHeaven.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BookHeaven.Data.Migrations
{
    [DbContext(typeof(BookHeavenDbContext))]
    [Migration("20171219201528_AddedVotes")]
    partial class AddedVotes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookHeaven.Data.Models.Book", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("BookListingPicture");

                b.Property<string>("BookPicture");

                b.Property<string>("Description")
                    .IsRequired();

                b.Property<decimal>("Price");

                b.Property<DateTime>("PublishedDate");

                b.Property<string>("PublisherId")
                    .IsRequired();

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(30);

                b.HasKey("Id");

                b.HasIndex("PublisherId");

                b.ToTable("Books");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.BookCategory", b =>
            {
                b.Property<int>("BookId");

                b.Property<int>("CategoryId");

                b.HasKey("BookId", "CategoryId");

                b.HasIndex("CategoryId");

                b.ToTable("BookCategory");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Category", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(20);

                b.HasKey("Id");

                b.HasIndex("Name")
                    .IsUnique();

                b.ToTable("Categories");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Location", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("City")
                    .HasMaxLength(100);

                b.Property<string>("Country")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<int>("SiteVisits");

                b.HasKey("Id");

                b.HasIndex("City", "Country")
                    .IsUnique()
                    .HasFilter("[City] IS NOT NULL");

                b.ToTable("Locations");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Order", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("Date");

                b.Property<string>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("Orders");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.OrderItem", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("BookId");

                b.Property<decimal>("BookPrice");

                b.Property<string>("BookTitle")
                    .IsRequired()
                    .HasMaxLength(30);

                b.Property<int>("OrderId");

                b.Property<int>("Quantity");

                b.HasKey("Id");

                b.HasIndex("BookId");

                b.HasIndex("OrderId");

                b.ToTable("OrderItems");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.SiteVisit", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("Date");

                b.Property<int>("Visits");

                b.HasKey("Id");

                b.ToTable("Visits");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.User", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Email")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<bool>("LockoutEnabled");

                b.Property<DateTimeOffset?>("LockoutEnd");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256);

                b.Property<string>("PasswordHash");

                b.Property<string>("PhoneNumber");

                b.Property<bool>("PhoneNumberConfirmed");

                b.Property<string>("ProfilePicture");

                b.Property<string>("ProfilePictureNav");

                b.Property<string>("SecurityStamp");

                b.Property<bool>("TwoFactorEnabled");

                b.Property<string>("UserName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

                b.ToTable("AspNetUsers");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Vote", b =>
            {
                b.Property<int>("BookId");

                b.Property<string>("UserId");

                b.Property<int?>("VoteValue");

                b.HasKey("BookId", "UserId");

                b.HasIndex("UserId");

                b.ToTable("Votes");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Name")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasName("RoleNameIndex")
                    .HasFilter("[NormalizedName] IS NOT NULL");

                b.ToTable("AspNetRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("RoleId")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("UserId")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider");

                b.Property<string>("ProviderKey");

                b.Property<string>("ProviderDisplayName");

                b.Property<string>("UserId")
                    .IsRequired();

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId");

                b.Property<string>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId");

                b.Property<string>("LoginProvider");

                b.Property<string>("Name");

                b.Property<string>("Value");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Book", b =>
            {
                b.HasOne("BookHeaven.Data.Models.User", "Publisher")
                    .WithMany("PublishedBooks")
                    .HasForeignKey("PublisherId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("BookHeaven.Data.Models.BookCategory", b =>
            {
                b.HasOne("BookHeaven.Data.Models.Book", "Book")
                    .WithMany("Categories")
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("BookHeaven.Data.Models.Category", "Category")
                    .WithMany("Books")
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Order", b =>
            {
                b.HasOne("BookHeaven.Data.Models.User", "User")
                    .WithMany("Orders")
                    .HasForeignKey("UserId");
            });

            modelBuilder.Entity("BookHeaven.Data.Models.OrderItem", b =>
            {
                b.HasOne("BookHeaven.Data.Models.Book", "Book")
                    .WithMany("Orders")
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne("BookHeaven.Data.Models.Order", "Order")
                    .WithMany("OrderItems")
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("BookHeaven.Data.Models.Vote", b =>
            {
                b.HasOne("BookHeaven.Data.Models.Book", "Book")
                    .WithMany("Votes")
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("BookHeaven.Data.Models.User", "User")
                    .WithMany("Votes")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("BookHeaven.Data.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("BookHeaven.Data.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("BookHeaven.Data.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("BookHeaven.Data.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}
