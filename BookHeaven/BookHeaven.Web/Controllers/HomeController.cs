using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Books;
using BookHeaven.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BookHeaven.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService books;

        public HomeController(IBookService books)
        {
            this.books = books;
        }

        public async Task<IActionResult> Index()
        {
            var mostPopularBooks =
                await this.books.GetMostPopularThisWeekAsync<BookListingServiceModel>(BookServiceConstants.PopularBooksToTake);

            return View(mostPopularBooks);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}