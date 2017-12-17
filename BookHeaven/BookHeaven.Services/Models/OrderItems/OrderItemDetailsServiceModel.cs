using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.OrderItems
{
    public class OrderItemDetailsServiceModel : IMapFrom<OrderItem>
    {
        public string BookTitle { get; set; }

        public decimal BookPrice { get; set; }

        public int Quantity { get; set; }
    }
}
