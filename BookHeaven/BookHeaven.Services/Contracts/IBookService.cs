using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IBookService : IService
    {
        Task<bool> CreateAsync(string title, decimal price, string description, IEnumerable<int> categoryIds,byte[] picture, string publisherId);

        Task<IEnumerable<T>> AllPaginatedAsync<T>(string searchTerm, int page);

        Task<int> GetCountBySearchTermAsync(string searchTerm);
    }
}
