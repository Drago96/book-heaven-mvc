using AutoMapper.QueryableExtensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookHeaven.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly BookHeavenDbContext db;

        public CategoryService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<T>> AllAsync<T>()
            => await this.db
            .Categories
            .OrderByDescending(b => b.Id).ProjectTo<T>()
            .ToListAsync();

        public async Task<bool> ExistsAsync(int categoryId)
            => await this.db.Categories.FindAsync(categoryId) != null;

        public async Task<bool> ExistsAsync(string categoryName)
            => await this.db.Categories.AnyAsync(c => c.Name == categoryName);


        public Task<bool> AlreadyExistsAsync(int id, string name)
            => this.db.Categories.AnyAsync(c => c.Id != id && c.Name == name);

        public async Task EditAsync(int id, string name)
        {
            var category = await this.db.Categories.FindAsync(id);
            category.Name = name;
            await this.db.SaveChangesAsync();
        }

        public async Task CreateAsync(string name)
        {
            Category category = new Category
            {
                Name = name
            };

            this.db.Add(category);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await this.db.Categories.FindAsync(id);

            this.db.Remove(category);

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> NamesAsync(IEnumerable<int> ids)
            => await this.db.Categories
                .Where(c => ids.Contains(c.Id))
                .Select(c => c.Name)
                .ToListAsync();
    }
}