using AutoMapper.QueryableExtensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<T>> ByUserIdAsync<T>(string userId, int take = 5)
            => await this.db.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Id)
            .Take(take)
            .ProjectTo<T>()
            .ToListAsync();

        public async Task<IEnumerable<OrderPublisherSalesServiceModel>> SalesForYear(int year, string publisherId)
        {
            ICollection<OrderPublisherSalesServiceModel> result = new List<OrderPublisherSalesServiceModel>();

            for (var i = 1; i <= 12; i++)
            {
                result.Add(await this.SalesForMonth(year, i, publisherId));
            }

            return result;
        }

        public async Task<IEnumerable<int>> GetYearsWithSalesAsync(string userId)
            => await this.db
                .OrderItems
                .Where(oi => oi.Book.PublisherId == userId)
                .Select(oi => oi.Order.Date.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

        private async Task<OrderPublisherSalesServiceModel> SalesForMonth(int year, int month, string publisherId)
        {
            var sales = await this.db
                .OrderItems
                .Where(o => o.Order.Date.Year == year
                    && o.Order.Date.Month == month
                    && o.BookId != null
                    && o.Book.PublisherId == publisherId)
                .SumAsync(o => o.Quantity);

            return new OrderPublisherSalesServiceModel
            {
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                Sales = sales
            };
        }
    }
}