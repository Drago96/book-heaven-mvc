using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Orders;
using BookHeaven.Services.UtilityServices.ShoppingCart.Models;

namespace BookHeaven.Services.Contracts
{
    public interface IOrderService : IService
    {
        Task FinishOrder(string userId, IDictionary<int, int> items);

        Task<IEnumerable<T>> ByUserIdAsync<T>(string userId, int take);

        Task<IEnumerable<OrderPublisherSalesServiceModel>> SalesForYear(int year, string publisherId);

        Task<IEnumerable<int>> GetYearsWithSalesAsync(string userId);
    }
}
