using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IBookService : IService
    {
        Task<IEnumerable<T>> AllPaginatedAsync<T>(string searchTerm, int page);

        Task<IEnumerable<T>> FilterAndTakeAsync<T>(string searchTerm, int booksToTake);

        Task<IEnumerable<T>> FilterByTermAndCategoriesAsync<T>(IEnumerable<int> categories,int page,string searchTerm);

        Task<T> ByIdAsync<T>(int id);

        Task<int> CountBySearchTermAsync(string searchTerm);

        Task<int> CountBySearchTermAndCategoriesAsync(IEnumerable<int> categoryIds, string searchTerm);

        Task<bool> CreateAsync(string title, decimal price, string description, IEnumerable<int> categoryIds, byte[] picture,byte[] listingPicture, string publisherId);

        Task EditAsync(int id, string title, decimal price, string description, IEnumerable<int> categories, byte[] bookPicture, byte[] listingPicture);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

    }
}