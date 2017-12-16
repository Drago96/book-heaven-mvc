using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IUserService : IService
    {
        Task<IEnumerable<T>> AllPaginatedAsync<T>(string search, int page, int take);

        Task<IEnumerable<T>> AllByTakeAsync<T>(string search, int usersToTake);

        Task<int> CountAsync();

        Task<int> CountBySearchTermAsync(string search);

        Task<bool> ExistsAsync(string id);

        Task<T> ByIdAsync<T>(string id);

        Task<IEnumerable<string>> GetRolesByIdAsync(string id);

        Task EditAsync(string id, string firstName, string lastName, string email, string username, IEnumerable<string> roles, string profilePicture,string profilePictureNav);

        Task<bool> AlreadyExistsAsync(string id, string username);
    }
}