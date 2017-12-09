using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Areas.Admin.Models.Users;
using BookHeaven.Web.Models;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService users;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(IUserService users, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            this.users = users;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> All(string searchTerm = "", int page = 1)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }

            return View(new PaginatedViewModel<UserAdminListingServiceModel>
            {
                Items = await this.users.AllPaginatedAsync<UserAdminListingServiceModel>(searchTerm, page),
                TotalItems = await this.users.GetCountBySearchTermAsync(searchTerm),
                CurrentPage = page,
                SearchTerm = searchTerm,
                PageSize = UserServiceConstants.UserListingPageSize
            });
        }
        

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.users.GetByIdAsync<UserDetailsServiceModel>(id);

            if (user==null)
            {
                return NotFound();
            }

            var model = this.mapper.Map<UserDetailsServiceModel, UserDetailsViewModel>(user);

            model.AllRoles = await this.roleManager.Roles.Select(r => r.Name).ToListAsync();
            model.Roles = await this.users.GetRolesByIdAsync(id);

            return View(model);
        }
    }
}
