﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Orders;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteOrdersForUserAsync(string userId)
        {
            var orders = await this.db.Orders.Where(o => o.UserId == userId).ToListAsync();

            foreach (var order in orders)
            {
                this.db.Remove(order);
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> ByUserIdAsync<T>(string userId, int take = 5)
            => await this.db.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Id)
            .Take(take)
            .ProjectTo<T>()
            .ToListAsync();

        public async Task<IEnumerable<OrderPublisherSalesModel>> SalesForYear(int year, string publisherId)
        {
            ICollection<OrderPublisherSalesModel> result = new List<OrderPublisherSalesModel>();

            for (var i = 1; i <= 12; i++)
            {
                result.Add(await this.SalesForMonth(year,i,publisherId));
            }

            return result;
        }

        private async Task<OrderPublisherSalesModel> SalesForMonth(int year, int month, string publisherId)
        {
            var sales = await this.db
                .OrderItems
                .Where(o => o.Order.Date.Year == year
                    && o.Order.Date.Month == month
                    && o.BookId != null
                    && o.Book.PublisherId == publisherId)
                .SumAsync(o => o.Quantity);

            return new OrderPublisherSalesModel
            {
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1),
                Sales = sales
            };

        }


    }
}
