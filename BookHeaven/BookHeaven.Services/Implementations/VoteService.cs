using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Data.Models.Enums;
using BookHeaven.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookHeaven.Services.Implementations
{
    public class VoteService : IVoteService
    {
        private readonly BookHeavenDbContext db;

        public VoteService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task<VoteValue?> GetUserVoteAsync(string userId, int bookId)
            => await this.db.Votes
                .Where(v => v.BookId == bookId && v.UserId == userId)
                .Select(v => v.VoteValue)
                .FirstOrDefaultAsync();

        public async Task VoteAsync(string userId, int id, VoteValue vote)
        {
            var userVote = await this.db.Votes.FirstOrDefaultAsync(v => v.UserId == userId && v.BookId == id);

            if (userVote == null)
            {
                this.db.Add(new Vote
                {
                    BookId = id,
                    UserId = userId,
                    VoteValue = vote
                });
            }
            else if (userVote.VoteValue == vote)
            {
                userVote.VoteValue = null;
            }
            else
            {
                userVote.VoteValue = vote;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteVotesForUserAsync(string id)
        {
            var votes = this.db.Votes.Where(v => v.UserId == id);
            this.db.RemoveRange(votes);
            await this.db.SaveChangesAsync();
        }
    }
}