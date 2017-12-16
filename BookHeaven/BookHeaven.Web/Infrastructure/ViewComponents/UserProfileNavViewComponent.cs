using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace BookHeaven.Web.Infrastructure.ViewComponents
{
    public class UserProfileNavViewComponent : ViewComponent
    {
        private readonly IUserService users;

        public UserProfileNavViewComponent(IUserService users)
        {
            this.users = users;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.users.ByIdAsync<UserNavServiceModel>(userId);
            return View(user);
        }
    }
}
