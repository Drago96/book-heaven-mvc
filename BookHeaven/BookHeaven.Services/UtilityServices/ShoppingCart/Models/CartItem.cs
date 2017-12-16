using System;
using System.Collections.Generic;
using System.Text;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.UtilityServices.ShoppingCart.Models
{
    public class CartItem
    {
        public int BookId { get; set; }

        public int Quantity { get; set; }
       
    }
}
