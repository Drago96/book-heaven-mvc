using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookHeaven.Data.Models
{
    public class ShoppingCartItem
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public int Quantity { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }


    }
}
