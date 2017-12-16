using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.UtilityServices.ShoppingCart.Models;

namespace BookHeaven.Services.UtilityServices.ShoppingCart
{
    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly ConcurrentDictionary<string, Models.ShoppingCart> shoppingCarts = new ConcurrentDictionary<string, Models.ShoppingCart>();

        public void AddToCart(string id, int bookId)
        {
            var shoppingCart = this.GetShoppingCart(id);

           shoppingCart.AddToCart(bookId);
           
        }

        public void UpdateQuantity(string id, int bookId, int quantity)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.UpdateQuantity(bookId,quantity);
        }

        public void RemoveFromCart(string id, int bookId)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.RemoveFromCart(bookId);
        }

        public IReadOnlyCollection<CartItem> GetItems(string id)
        {
            var shoppingCart = this.GetShoppingCart(id);

            return shoppingCart.GetItems();
        }

        public bool CartIsFull(string id)
            => this.GetShoppingCart(id).IsFull();

        public bool Contains(string id, int bookId)
            => this.GetShoppingCart(id).GetItems().Any(i => i.BookId == bookId);

        public void ClearCart(string id)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.Clear();
        }


        private Models.ShoppingCart GetShoppingCart(string id)
        {
            return this.shoppingCarts.GetOrAdd(id, new Models.ShoppingCart(ShoppingCartServiceConstants.MaxShoppingCartItems));
        }
    }
}
