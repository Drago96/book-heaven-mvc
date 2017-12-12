﻿using AutoMapper.QueryableExtensions;
using BookHeaven.Common.Extensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;

namespace BookHeaven.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly BookHeavenDbContext db;
        private readonly ICategoryService categories;

        public BookService(BookHeavenDbContext db, ICategoryService categories)
        {
            this.db = db;
            this.categories = categories;
        }

        public async Task<IEnumerable<T>> AllPaginatedAsync<T>(string searchTerm = "", int page = 1)
            => await this.FindBookBySearchTerm(searchTerm)
                .Skip((page - 1) * BookServiceConstants.BookPublisherListingPageSize)
                .Take(BookServiceConstants.BookPublisherListingPageSize)
                .OrderByDescending(b => b.Id)
                .ProjectTo<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> FilterAndTakeAsync<T>(string searchTerm = "", int booksToTake = 10)
            => await this.db.Books.Where(b => b.Title.ContainsInsensitive(searchTerm))
                .Take(booksToTake)
                .ProjectTo<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> FilterByTermAndCategoriesAsync<T>(IEnumerable<int> categories,int page=1, string searchTerm = "")
        {
            var books = this.FindBookBySearchTerm(searchTerm);

            if (!categories.NullOrEmpty())
            {
                books = books.Where(b => b.Categories.Any(c => categories.Contains(c.CategoryId)));
            }

            books = books.Skip((page - 1) * BookServiceConstants.BookUserListingPageSize)
                .Take(BookServiceConstants.BookUserListingPageSize)
                .OrderByDescending(b => b.Id);

            return await books.ProjectTo<T>().ToListAsync();
        }

        public async Task<T> ByIdAsync<T>(int id)
            => await this.db.Books.Where(b => b.Id == id).ProjectTo<T>().FirstOrDefaultAsync();


        public async Task<int> CountBySearchTermAsync(string searchTerm = "")
            => await this.FindBookBySearchTerm(searchTerm).CountAsync();

        public async Task<int> CountBySearchTermAndCategoriesAsync(IEnumerable<int> categoryIds, string searchTerm)
        {
            var books = this.FindBookBySearchTerm(searchTerm);

            if (!categoryIds.NullOrEmpty())
            {
                books = books.Where(b => b.Categories.Any(c => categoryIds.Contains(c.CategoryId)));
            }

            return await books.CountAsync();
        }

        public async Task<bool> CreateAsync(string title, decimal price, string description, IEnumerable<int> categoryIds, byte[] picture,byte[] listingPicture, string publisherId)
        {
            foreach (var categoryId in categoryIds)
            {
                var categoryExsits = await this.categories.ExistsAsync(categoryId);
                if (!categoryExsits)
                {
                    return false;
                }
            }

            Book book = new Book
            {
                Title = title,
                Price = price,
                Description = description,
                BookPicture = picture,
                BookListingPicture = listingPicture,
                PublishedDate = DateTime.Today.Date,
                PublisherId = publisherId
            };

            foreach (var categoryId in categoryIds)
            {
                book
                     .Categories
                     .Add(new BookCategory
                     {
                         CategoryId = categoryId
                     });
            }

            this.db.Add(book);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
            => await this.db.Books.FindAsync(id) != null;

        public async Task DeleteAsync(int id)
        {
            var book = await this.db.Books.FindAsync(id);
            this.db.Remove(book);
            await this.db.SaveChangesAsync();
        }


        public async Task EditAsync(int id, string title, decimal price, string description, IEnumerable<int> categories, byte[] bookPicture, byte[] listingPicture)
        {
            var book = await this.db.Books.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);

            book.Title = title;
            book.Price = price;
            book.Description = description;
            book.BookPicture = bookPicture ?? book.BookPicture;
            book.BookListingPicture = listingPicture ?? book.BookListingPicture;

            foreach (var category in book.Categories)
            {
                if (!categories.Contains(category.CategoryId))
                {
                    this.db.Remove(category);
                }
            }

            foreach (var categoryId in categories)
            {
                if (book.Categories.All(c => c.CategoryId != categoryId))
                {
                    book.Categories.Add(new BookCategory
                    {
                        CategoryId = categoryId
                    });
                }
            }

            await this.db.SaveChangesAsync();
        }

        private IQueryable<Book> FindBookBySearchTerm(string searchTerm)
            => this.db.Books.Where(b => b.Title.ContainsInsensitive(searchTerm));
    }
}