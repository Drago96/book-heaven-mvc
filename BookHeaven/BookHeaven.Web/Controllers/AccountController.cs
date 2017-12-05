using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
namespace BookHeaven.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.AccountViewModels;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IFileService fileService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFileService fileService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.fileService = fileService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            TempData.AddErrorMessage("Invalid login attempt.");

            return View(model);


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            if (model.ProfilePicture != null)
            {
                user.ProfilePicture = await this.fileService.GetByteArrayFromFormFileAsync(model.ProfilePicture);
            }

            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }

            TempData.AddErrorMessage("User already exists.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //{
        //    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        //    var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        //{
        //    if (remoteError != null)
        //    {
        //        ErrorMessage = $"Error from external provider: {remoteError}";
        //        return RedirectToAction(nameof(Login));
        //    }
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Login));
        //    }

        //    // Sign in the user with this external login provider if the user already has a login.
        //    var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //    if (result.Succeeded)
        //    {                
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then ask the user to create an account.
        //        ViewData["ReturnUrl"] = returnUrl;
        //        ViewData["LoginProvider"] = info.LoginProvider;
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await this.signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            throw new ApplicationException("Error loading external login information during confirmation.");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await _userManager.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View(nameof(ExternalLogin), model);
        //}

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
