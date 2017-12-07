using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
        {
            this.users = users;
        }

        public async Task<IActionResult> All(string searchTerm = "", int page = 1)
            => View(new PaginatedViewModel<UserAdminListingServiceModel>
            {
                Items = await this.users.AllAsync<UserAdminListingServiceModel>(searchTerm, page),
                TotalItems = await this.users.GetCountBySearchTermAsync(searchTerm),
                CurrentPage = page,
                SearchTerm = searchTerm,
                PageSize = UserServiceConstants.UserListingPageSize
            });
    }
}
