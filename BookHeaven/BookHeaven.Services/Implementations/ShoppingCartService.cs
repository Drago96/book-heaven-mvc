using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly BookHeavenDbContext db;

        public ShoppingCartService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(int id, string userId)
        {
            var shoppingCartItem =
                await this.db.ShoppingCartItems.FirstOrDefaultAsync(s => s.UserId == userId && s.BookId == id);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    BookId = id,
                    UserId = userId,
                    Quantity = 1

                };
                this.db.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> CartIsFullAsync(string userId)
            => await this.db.ShoppingCartItems
                   .Where(sci => sci.UserId == userId)
                   .SumAsync(sci => sci.Quantity) >= ShoppingCartServiceConstants.MaxShoppingCartItems;

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string userId)
            => await this.db.ShoppingCartItems
                .Where(s => s.UserId == userId)
                .ProjectTo<T>()
                .ToListAsync();
    }
}
