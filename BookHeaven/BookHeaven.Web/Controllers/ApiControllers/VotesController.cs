using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Votes;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookHeaven.Web.Controllers.ApiControllers
{
    public class VotesController : BaseApiController
    {
        private readonly IVoteService votes;
        private readonly IBookService books;

        public VotesController(IVoteService votes, IBookService books)
        {
            this.votes = votes;
            this.books = books;
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]VoteDto model, int id)
        {
            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest(VoteErrorConstants.ErrorProcessingVoteMessage);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.votes.VoteAsync(userId, id, model.Vote);
            return Ok();
        }
    }
}