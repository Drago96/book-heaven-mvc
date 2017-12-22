using BookHeaven.Services.Models.Books;
using BookHeaven.Services.Models.Categories;
using System.Collections.Generic;

namespace BookHeaven.Web.Models.Books
{
    public class BookSearchListingViewModel
    {
        public PaginatedViewModel<BookListingServiceModel> Books { get; set; }

        public IEnumerable<CategoryInfoServiceModel> Categories { get; set; }
    }
}