using BookHeaven.Data.Models.Enums;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IVoteService : IService
    {
        Task<VoteValue?> GetUserVoteAsync(string userId, int bookId);

        Task VoteAsync(string userId, int id, VoteValue vote);

        Task DeleteVotesForUserAsync(string id);
    }
}