using BookHeaven.Services.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

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