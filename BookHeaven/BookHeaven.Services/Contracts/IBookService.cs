﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IBookService : IService
    {
        Task<IEnumerable<T>> AllPaginatedAsync<T>(string searchTerm, int page, int take);

        Task<IEnumerable<T>> AllByPublisherPaginatedAsync<T>(string userId, string searchTerm, int page, int take);

        Task<IEnumerable<T>> FilterAndTakeAsync<T>(string searchTerm, int booksToTake);

        Task<IEnumerable<T>> FilterByTermAndCategoriesAsync<T>(IEnumerable<int> categories, int page, string searchTerm, int take);

        Task<T> ByIdAsync<T>(int id);

        Task<IEnumerable<T>> ByIdsAsync<T>(IEnumerable<int> ids);

        Task<int> CountBySearchTermAsync(string searchTerm);

        Task<int> CountBySearchTermAndCategoriesAsync(IEnumerable<int> categoryIds, string searchTerm);

        Task<int> CreateAsync(string title, decimal price, string description, IEnumerable<int> categoryIds, string picture, string listingPicture, string publisherId);

        Task EditAsync(int id, string title, decimal price, string description, IEnumerable<int> categories, string bookPicture, string listingPicture);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> IsPublisherAsync(int id, string userId);

        Task<IEnumerable<T>> GetMostPopularThisWeekAsync<T>(int popularBooksToTake);
    }
}