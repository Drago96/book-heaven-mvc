using System.Collections.Generic;
using BookHeaven.Services.UtilityServices.ShoppingCart.Models;

namespace BookHeaven.Services.UtilityServices.ShoppingCart
{
    public interface IShoppingCartManager
    {
        void AddToCart(string id, int bookId);

        void UpdateQuantity(string id, int bookId, int quantity);

        void RemoveFromCart(string id, int bookId);

        IReadOnlyCollection<CartItem> GetItems(string id);

        bool CartIsFull(string id);

        bool Contains(string id, int bookId);

        void ClearCart(string id);
    }
}
