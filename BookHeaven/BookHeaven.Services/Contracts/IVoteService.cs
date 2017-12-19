using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Data.Models.Enums;

namespace BookHeaven.Services.Contracts
{
    public interface IVoteService : IService
    {
        Task<VoteValue?> GetUserVoteAsync(string userId, int bookId);

        Task VoteAsync(string userId, int id, VoteValue vote);

        Task DeleteVotesForUserAsync(string id);
    }
}
