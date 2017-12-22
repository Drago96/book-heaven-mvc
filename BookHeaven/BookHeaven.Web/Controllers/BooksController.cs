using AutoMapper;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Books;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Models;
using BookHeaven.Web.Models.Books;
using BookHeaven.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookHeaven.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly ICategoryService categories;
        private readonly IVoteService votes;
        private readonly IMapper mapper;

        public BooksController(IBookService books, ICategoryService categories, IVoteService votes, IMapper mapper)
        {
            this.books = books;
            this.categories = categories;
            this.mapper = mapper;
            this.votes = votes;
        }

        public async Task<IActionResult> Search(BookSearchComponentModel model, int page = 1)
        {
            foreach (var categoryId in model.Categories)
            {
                if (!await this.categories.ExistsAsync(categoryId))
                {
                    return BadRequest();
                }
            }

            var searchTerm = model.SearchTerm ?? "";

            var searchResult = await this.books.FilterByTermAndCategoriesAsync<BookListingServiceModel>(model.Categories, page, searchTerm, BookServiceConstants.BookUserListingPageSize);

            var listingModel = new BookSearchListingViewModel
            {
                Categories = await this.categories.ByIdsAsync<CategoryInfoServiceModel>(model.Categories),
                Books = new PaginatedViewModel<BookListingServiceModel>
                {
                    CurrentPage = page,
                    Items = searchResult,
                    PageSize = BookServiceConstants.BookUserListingPageSize,
                    SearchTerm = searchTerm,
                    TotalItems =
                        await this.books.CountBySearchTermAndCategoriesAsync(model.Categories, searchTerm)
                }
            };

            return View(listingModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return NotFound();
            }

            var book = await this.books.ByIdAsync<BookDetailsServiceModel>(id);

            var model = this.mapper.Map<BookDetailsServiceModel, BookDetailsViewModel>(book);

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.UserVote = await this.votes.GetUserVoteAsync(userId, id);
            }

            return View(model);
        }
    }
}