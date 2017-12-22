using BookHeaven.Common.Mapping;
using BookHeaven.Services.Models.Books;

namespace BookHeaven.Web.Models.ShoppingCart
{
    public class ShoppingCartBookViewModel : IMapFrom<BookShoppingCartServiceModel>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string BookListingPicture { get; set; }
    }
}