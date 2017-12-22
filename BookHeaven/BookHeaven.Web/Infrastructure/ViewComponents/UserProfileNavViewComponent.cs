using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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