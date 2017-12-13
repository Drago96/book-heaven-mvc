using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.ShoppingCart;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Models.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IBookService books;
        private readonly IShoppingCartService shoppingCarts;

        public ShoppingCartController(IBookService books, IShoppingCartService shoppingCarts)
        {
            this.books = books;
            this.shoppingCarts = shoppingCarts;
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartIsFull = await this.shoppingCarts.CartIsFullAsync(userId);

            if (cartIsFull)
            {
                TempData.AddErrorMessage(ShoppingCartErrorMessages.CartIsFull);
                return RedirectToAction("Details", "Books", new {id});
            }

            await this.shoppingCarts.AddAsync(id,userId);

            TempData.AddSuccessMessage(ShoppingCartSuccessMessages.ItemAddedSuccess);

            return RedirectToAction("Details", "Books", new {id});

        }

        public async Task<IActionResult> CartContent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new ShoppingCartViewModel
            {
                Items = await this.shoppingCarts.GetItemsAsync<ShoppingCartItemServiceModel>(userId)
            });
        }
            
    }
}
