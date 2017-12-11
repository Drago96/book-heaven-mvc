﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface ICategoryService : IService
    {
        Task<IEnumerable<T>> AllAsync<T>();

        Task<bool> ExistsAsync(int categoryId);

        Task<bool> ExistsAsync(string categoryName);

        Task<bool> AlreadyExistsAsync(int id, string name);

        Task CreateAsync(string name);

        Task EditAsync(int id, string name);

        Task DeleteAsync(int id);

        Task<IEnumerable<string>> NamesAsync(IEnumerable<int> ids);
    }
}