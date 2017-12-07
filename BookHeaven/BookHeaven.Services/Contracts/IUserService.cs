using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IUserService : IServce
    {
        Task<IEnumerable<T>> AllAsync<T>(string search , int page);

        Task<int> GetCountAsync();

        Task<int> GetCountBySearchTermAsync(string search);
    }
}
