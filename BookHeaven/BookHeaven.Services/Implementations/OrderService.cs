using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using Microsoft.Net.Http.Headers;

namespace BookHeaven.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly BookHeavenDbContext db;

        public OrderService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task FinishOrder(string userId, IDictionary<int, int> items)
        {
            var order = new Order
            {
                Date = DateTime.UtcNow,
                UserId = userId
            };

            foreach (var item in items)
            {
                var bookId = item.Key;
                var quantity = item.Value;

                var book = await this.db.Books.FindAsync(bookId);

                var orderItem = new OrderItem
                {
                    BookId = bookId,
                    BookPrice = book.Price,
                    BookTitle = book.Title,
                    Quantity = quantity
                };

                order.OrderItems.Add(orderItem);

            }
            this.db.Add(order);
            await this.db.SaveChangesAsync();
        }
    }
}
