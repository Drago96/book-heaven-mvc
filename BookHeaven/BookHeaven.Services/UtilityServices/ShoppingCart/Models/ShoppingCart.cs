using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookHeaven.Services.UtilityServices.ShoppingCart.Models
{
    public class ShoppingCart
    {
        private readonly ICollection<CartItem> books = new List<CartItem>();
        private readonly int capacity;

        public ShoppingCart(int capacity)
        {
            this.capacity = capacity;
        }

        public IReadOnlyCollection<CartItem> GetItems()
        {
            return this.books.ToList().AsReadOnly();
        }

        public void AddToCart(int bookId)
        {
            var book = this.books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                this.books.Add(new CartItem
                {
                    BookId = bookId,
                    Quantity = 1
                });
            }
            else
            {
                book.Quantity++;
            }
        }

        public void UpdateQuantity(int bookId, int quantity)
        {
            var book = this.books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                book.Quantity = quantity;
            }
        }

        public void RemoveFromCart(int bookId)
        {
            var book = this.books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                this.books.Remove(book);
            }

        }

        public void Clear()
            => this.books.Clear();

        public bool Contains(int bookId)
            => this.books.Any(b => b.BookId == bookId);

        public bool IsFull()
            => this.books.Count > this.capacity;
    }
}
