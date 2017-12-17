using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Books;
using BookHeaven.Services.Models.Categories;

namespace BookHeaven.Web.Models.Books
{
    public class BookSearchListingViewModel
    {
        public PaginatedViewModel<BookListingServiceModel> Books { get; set; }

        public IEnumerable<CategoryInfoServiceModel> Categories { get; set; }
    }
}
