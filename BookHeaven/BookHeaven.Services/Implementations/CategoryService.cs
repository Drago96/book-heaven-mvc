using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Services.Implementations
{
    public class CategoryService : ICategoryService
    {

        private readonly BookHeavenDbContext db;

        public CategoryService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            => await this.db
            .Categories
            .OrderByDescending(b => b.Id).ProjectTo<T>()
            .ToListAsync();

        public async Task<bool> ExistsAsync(int categoryId)
            => await this.db.Categories.FindAsync(categoryId) != null;

        public async Task<bool> ExistsAsync(string categoryName)
            => await this.db.Categories.AnyAsync(c => c.Name == categoryName);

        public async Task CreateAsync(string name)
        {
            Category category = new Category
            {
                Name = name
            };

            this.db.Add(category);
            await this.db.SaveChangesAsync();

        }
    }
}
