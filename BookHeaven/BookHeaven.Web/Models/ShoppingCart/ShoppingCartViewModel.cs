using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Models.ShoppingCart;

namespace BookHeaven.Web.Models.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCartItemServiceModel> Items { get; set; }

        public int CartItemsCount => Items.Sum(i => i.Quantity);

        public decimal CartTotalSum => Items.Sum(i => (i.BookPrice * i.Quantity));
    }
}
