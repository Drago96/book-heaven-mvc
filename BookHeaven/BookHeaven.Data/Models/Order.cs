using System;
using System.Collections.Generic;
using System.Text;

namespace BookHeaven.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
