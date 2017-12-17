using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.UtilityServices.Contracts;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Orders;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Models.Account;

namespace BookHeaven.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IFileService fileService;
        private readonly IUserService users;
        private readonly IOrderService orders;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFileService fileService,
            IUserService users,
            IOrderService orders)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.fileService = fileService;
            this.users = users;
            this.orders = orders;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddErrorMessage(UserErrorConstants.LogOutFirst);
                return RedirectToAction("Index", "Home");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData.SetReturnUrl(returnUrl);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            ViewData.SetReturnUrl(returnUrl);

            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            TempData.AddErrorMessage(UserErrorConstants.InvalidLoginAttempt);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddErrorMessage(UserErrorConstants.LogOutFirst);
                return RedirectToAction("Index", "Home");
            }
            ViewData.SetReturnUrl(returnUrl);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            ViewData.SetReturnUrl(returnUrl);

            var userExists = await this.userManager.FindByNameAsync(model.Email) != null;

            if (userExists)
            {
                TempData.AddErrorMessage(UserErrorConstants.UserExists);
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (model.ProfilePicture != null)
            {
                var profilePicture = await this.fileService.GetByteArrayFromFormFileAsync(model.ProfilePicture);
                var pictureType = model.ProfilePicture.ContentType.Split('/').Last();

                var profilePictureData = this.fileService.ResizeImage(profilePicture,
                    UserDataConstants.ProfilePictureWidth, UserDataConstants.ProfilePictureHeight, pictureType);
                user.ProfilePicture= this.fileService.UploadImage(profilePictureData);

                var profilePictureNavData = this.fileService.ResizeImage(profilePicture,
                    UserDataConstants.ProfilePictureNavWidth, UserDataConstants.ProfilePictureNavHeight, pictureType);
                user.ProfilePictureNav= this.fileService.UploadImage(profilePictureNavData);

            }

            var result = await this.userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                TempData.AddErrorMessage(UserErrorConstants.ErrorCreatingUser);
                return View(model);
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);
            TempData.AddSuccessMessage(string.Format(UserSuccessMessages.RegisterMessage, model.FirstName, model.LastName));
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddErrorMessage(UserErrorConstants.LogOutFirst);
                return RedirectToAction("Index", "Home");
            }

            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/", string remoteError = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddErrorMessage(UserErrorConstants.LogOutFirst);
                return RedirectToAction("Index", "Home");
            }

            if (remoteError != null)
            {
                TempData.AddErrorMessage(string.Format(UserErrorConstants.ErrorFromExternalProvider, remoteError));
                return RedirectToAction(nameof(Login));
            }
            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (!result.Succeeded)
            {
                ViewData.SetReturnUrl(returnUrl);
                ViewData[DataKeyConstants.LoginProvider] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                var userPersonalNames = info.Principal.Identity.Name;
                var firstName = "";
                var lastName = "";

                if (userPersonalNames != null)
                {
                    var names = userPersonalNames.Split();
                    firstName = names[0];
                    lastName = names[names.Length - 1];
                }

                return View("ExternalLogin", new ExternalLoginViewModel
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                });
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewData.SetReturnUrl(returnUrl);
                return View(nameof(ExternalLogin), model);
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData.AddErrorMessage(UserErrorConstants.ExternalLoginInformation);
                return RedirectToLocal(returnUrl);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var userExists = await this.userManager.FindByNameAsync(model.Email) != null;

            if (userExists)
            {
                TempData.AddErrorMessage(UserErrorConstants.UserExists);
                ViewData.SetReturnUrl(returnUrl);
                return View(nameof(ExternalLogin), model);
            }

            var result = await this.userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                TempData.AddErrorMessage(UserErrorConstants.ErrorCreatingUser);
                ViewData.SetReturnUrl(returnUrl);
                return View(nameof(ExternalLogin), model);
            }

            result = await this.userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                TempData.AddErrorMessage(UserErrorConstants.ErrorCreatingUser);
                ViewData.SetReturnUrl(returnUrl);
                return View(nameof(ExternalLogin), model);
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);
            TempData.AddSuccessMessage(string.Format(UserSuccessMessages.RegisterMessage, model.FirstName, model.LastName));
            return RedirectToLocal(returnUrl);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.users.ByIdAsync<UserDetailsServiceModel>(userId);
            var userOrders = await this.orders.ByUserIdAsync<OrderDetailsServiceModel>(userId);

            return View(new UserProfileViewModel
            {
                User = user,
                Orders = userOrders
            });
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}