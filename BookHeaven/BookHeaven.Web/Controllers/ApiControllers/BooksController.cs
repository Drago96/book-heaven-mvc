using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookHeaven.Web.Controllers.ApiControllers
{
    public class BooksController : BaseApiController
    {
        private readonly IBookService books;

        public BooksController(IBookService books)
        {
            this.books = books;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] string searchTerm = "")
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }

            var allBooks = await this.books.FilterAndTakeAsync<BookSearchDto>(searchTerm, BookServiceConstants.BookPublisherSearchCount);

            return Ok(allBooks);
        }
    }
}