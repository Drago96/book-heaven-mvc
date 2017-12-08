using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IUserService : IServce
    {
        Task<IEnumerable<T>> AllPaginatedAsync<T>(string search , int page);

        Task<IEnumerable<T>> AllByTakeAsync<T>(string search, int usersToTake);

        Task<int> GetCountAsync();

        Task<int> GetCountBySearchTermAsync(string search);

        Task<bool> ExistsAsync(string id);

        Task<T> GetByIdAsync<T>(string id);
    }
}
