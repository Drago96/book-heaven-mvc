using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface ICategoryService : IService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<bool> ExistsAsync(int categoryId);

        Task<bool> ExistsAsync(string categoryName);

        Task CreateAsync(string name);
    }
}
