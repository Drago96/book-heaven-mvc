using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
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

            await this.shoppingCarts.AddAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

            TempData.AddSuccessMessage(ShoppingCartSuccessMessages.ItemAddedSuccess);

            return RedirectToAction("Details", "Books", new {id});

        }
    }
}
