using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Users;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get([FromQuery] string searchTerm = "")
            => Ok(await this.users
                .AllByTakeAsync<UserSearchDto>(searchTerm, UserServiceConstants.UsersSearchCount));
    }
}
