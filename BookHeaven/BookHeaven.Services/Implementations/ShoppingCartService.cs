using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
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
    }
}
