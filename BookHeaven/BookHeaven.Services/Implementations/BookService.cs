using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Common.Extensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> CreateAsync(string title, decimal price, string description, IEnumerable<int> categoryIds, byte[] picture, string publisherId)
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

        public async Task<IEnumerable<T>> AllPaginatedAsync<T>(string searchTerm = "", int page = 1)
         => await this.FindBookBySearchTerm(searchTerm)
                .Skip((page - 1) * BookServiceConstants.BookPublisherListingPageSize)
                .Take(BookServiceConstants.BookPublisherListingPageSize)
                .OrderByDescending(b => b.Id)
                .ProjectTo<T>()
                .ToListAsync();

        public async Task<int> GetCountBySearchTermAsync(string searchTerm = "")
            => await this.FindBookBySearchTerm(searchTerm).CountAsync();

        private IQueryable<Book> FindBookBySearchTerm(string searchTerm)
            => this.db.Books.Where(b => b.Title.ContainsInsensitive(searchTerm));
    }
}
