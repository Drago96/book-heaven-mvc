using AutoMapper;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Users;
using BookHeaven.Services.UtilityServices.Contracts;
using BookHeaven.Web.Areas.Admin.Models.Users;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Common.Extensions;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService users;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService users, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IFileService fileService)
        {
            this.users = users;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.fileService = fileService;
        }

        public async Task<IActionResult> All(string searchTerm = "", int page = 1)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }

            return View(new PaginatedViewModel<UserAdminListingServiceModel>
            {
                Items = await this.users.AllPaginatedAsync<UserAdminListingServiceModel>(searchTerm, page, UserServiceConstants.UserListingPageSize),
                TotalItems = await this.users.CountBySearchTermAsync(searchTerm),
                CurrentPage = page,
                SearchTerm = searchTerm,
                PageSize = UserServiceConstants.UserListingPageSize
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.users.ByIdAsync<UserDetailsServiceModel>(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = this.mapper.Map<UserDetailsServiceModel, UserDetailsViewModel>(user);

            model.Roles = await this.users.GetRolesByIdAsync(id);
            
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.users.ByIdAsync<UserDetailsServiceModel>(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = this.mapper.Map<UserDetailsServiceModel, UserEditViewModel>(user);

            model.Roles = await this.users.GetRolesByIdAsync(id);
            model.AllRoles = await this.roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllRoles = await this.roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            var exists = await this.users.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            foreach (var role in model.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    return BadRequest();
                }
            }

            var usernameAlreadyTaken = await this.users.AlreadyExistsAsync(id, model.Email);

            if (usernameAlreadyTaken)
            {
                TempData.AddErrorMessage(UserErrorConstants.UserAlreadyExists);
                model.AllRoles = await this.roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            string profilePictureUrl = null;
            string profilePictureNavUrl = null;

            if (model.NewProfilePicture != null)
            {
                var profilePictureFromFormFile =
                    await this.fileService.GetByteArrayFromFormFileAsync(model.NewProfilePicture);
                var pictureType = model.NewProfilePicture.ContentType.Split('/').Last();
                var profilePicture = this.fileService.ResizeImage(profilePictureFromFormFile,
                    UserDataConstants.ProfilePictureWidth, UserDataConstants.ProfilePictureHeight, pictureType);
                profilePictureUrl = this.fileService.UploadImage(profilePicture);

                var profilePictureNav = this.fileService.ResizeImage(profilePicture,
                    UserDataConstants.ProfilePictureNavWidth, UserDataConstants.ProfilePictureNavHeight, pictureType);
                profilePictureNavUrl = this.fileService.UploadImage(profilePictureNav);

            }

            await this.users.EditAsync(id, model.FirstName, model.LastName, model.Email, model.Email, model.Roles, profilePictureUrl, profilePictureNavUrl);

            TempData.AddSuccessMessage(UserSuccessMessages.EditMessage);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var exists = await this.users.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            var user = await this.userManager.FindByIdAsync(id);

            if (user.ProfilePicture != null)
            {
                this.fileService.DeleteImage(user.ProfilePicture);
            }
            if (user.ProfilePictureNav != null)
            {
                this.fileService.DeleteImage(user.ProfilePictureNav);
            }

            await this.userManager.DeleteAsync(user);

            TempData.AddSuccessMessage(UserSuccessMessages.DeleteMessage);
            return RedirectToAction(nameof(All));
        }
    }
}