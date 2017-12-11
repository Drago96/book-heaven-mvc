using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookHeaven.Web.Controllers.ApiControllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
        {
            this.users = users;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Get([FromQuery] string searchTerm = "")
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            return Ok(await this.users
                .AllByTakeAsync<UserSearchDto>(searchTerm, UserServiceConstants.UsersSearchCount));
        }
    }
}