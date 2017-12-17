using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Services.UtilityServices.ShoppingCart.Models;

namespace BookHeaven.Services.Contracts
{
    public interface IOrderService : IService
    {
        Task FinishOrder(string userId, IDictionary<int, int> items);

        Task DeleteOrdersForUserAsync(string userId);

        Task<IEnumerable<T>> ByUserIdAsync<T>(string userId, int take);
    }
}
