using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Books;
using BookHeaven.Services.UtilityServices.ShoppingCart;
using BookHeaven.Services.UtilityServices.ShoppingCart.Models;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Models.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Edm.Validation;

namespace BookHeaven.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IBookService books;
        private readonly IShoppingCartManager shoppingCarts;
        private readonly IOrderService orders;
        private readonly IMapper mapper;

        public ShoppingCartController(IBookService books, IShoppingCartManager shoppingCarts, IMapper mapper, IOrderService orders)
        {
            this.books = books;
            this.shoppingCarts = shoppingCarts;
            this.mapper = mapper;
            this.orders = orders;
        }

        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            var cartIsFull =  this.shoppingCarts.CartIsFull(shoppingCartId);

            if (cartIsFull)
            {
                TempData.AddErrorMessage(ShoppingCartErrorMessages.CartIsFull);
                return RedirectToAction("Details", "Books", new {id});
            }

            this.shoppingCarts.AddToCart(shoppingCartId,id);

            TempData.AddSuccessMessage(ShoppingCartSuccessMessages.ItemAddedSuccess);

            return RedirectToAction("Details", "Books", new {id});

        }

        public async Task<IActionResult> CartContent()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();
            var shoppingCartItems = this.shoppingCarts.GetItems(shoppingCartId);
            var shoppingCartItemIds = shoppingCartItems.Select(b => b.BookId);

            foreach (var bookId in shoppingCartItemIds)
            {
                var bookExists = await this.books.ExistsAsync(bookId);

                if (!bookExists)
                {
                    this.shoppingCarts.RemoveFromCart(shoppingCartId,bookId);
                    TempData.AddWarningMessage(ShoppingCartErrorMessages.ErrorFindingItem);
                }
            }

            var books = await this.books.ByIdsAsync<BookShoppingCartServiceModel>(shoppingCartItemIds);

            var bookQuantities = shoppingCartItems.ToDictionary(b => b.BookId, b => b.Quantity);

            var bookViewModels = this.mapper.Map<IEnumerable<BookShoppingCartServiceModel>,IEnumerable<ShoppingCartBookViewModel>>(books).ToList();
            bookViewModels.ForEach(b => b.Quantity = bookQuantities[b.Id]);

            return View(bookViewModels);
        }

        [HttpPost]
        public IActionResult DeleteCartItem(int bookId)
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();
            var contains = this.shoppingCarts.Contains(shoppingCartId, bookId);

            if (!contains)
            {
                return BadRequest();
            }

            this.shoppingCarts.RemoveFromCart(shoppingCartId,bookId);
            TempData.AddSuccessMessage(ShoppingCartSuccessMessages.ItemDeletedSuccess);
            return RedirectToAction(nameof(CartContent));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOutCart()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();
            var shoppingCartItems = this.shoppingCarts.GetItems(shoppingCartId);

            if (shoppingCartItems.Count == 0)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = shoppingCartItems.ToDictionary(i => i.BookId, i => i.Quantity);
            await this.orders.FinishOrder(userId, items);
            this.shoppingCarts.ClearCart(shoppingCartId);

            TempData.AddSuccessMessage(ShoppingCartSuccessMessages.OrderFinishedSuccess);

            return RedirectToAction("Index", "Home", new { area = "Home" });
        }


    }
}
