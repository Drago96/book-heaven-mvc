using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Books;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Services.Models.Users;
using BookHeaven.Services.UtilityServices.Contracts;
using BookHeaven.Web.Areas.Publisher.Models.Books;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Infrastructure.Filters;
using BookHeaven.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHeaven.Web.Areas.Publisher.Controllers
{
    public class BooksController : PublisherBaseController
    {

        private readonly ICategoryService categories;
        private readonly IBookService books;
        private readonly IFileService fileService;
        private readonly UserManager<User> userManager;

        public BooksController(ICategoryService categories, IBookService books, IFileService fileService, UserManager<User> userManager)
        {
            this.categories = categories;
            this.fileService = fileService;
            this.books = books;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(string searchTerm = "", int page = 1)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }

            return View(new PaginatedViewModel<BookPublisherListingServiceModel>
            {
                Items = await this.books.AllPaginatedAsync<BookPublisherListingServiceModel>(searchTerm, page),
                TotalItems = await this.books.GetCountBySearchTermAsync(searchTerm),
                CurrentPage = page,
                SearchTerm = searchTerm,
                PageSize = BookServiceConstants.BookPublisherListingPageSize
            });
        }


        public async Task<IActionResult> Create()
            => View(new BookCreateViewModel
            {
                AllCategories = await this.GetAllCategories()
            });

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCategories = await this.GetAllCategories();
                return View(model);
            }

            byte[] resizedBookPicture = null;

            if (model.BookPicture != null)
            {
                var bookPicture = await this.fileService.GetByteArrayFromFormFileAsync(model.BookPicture);
                var pictureType = model.BookPicture.ContentType.Split('/').Last();
                resizedBookPicture = this.fileService.ResizeImageAsync(bookPicture,
                    BookDataConstants.BookPictureWidth, BookDataConstants.BookPictureHeight, pictureType);
            }

            var result = await this.books.CreateAsync(model.Title,
                model.Price,
                model.Description,
                model.Categories,
                resizedBookPicture,
                userManager.GetUserId(HttpContext.User));

            if (!result)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage(string.Format(BookSuccessConstants.BookPublishedMeessage, model.Title));

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<SelectListItem>> GetAllCategories()
            => (await this.categories.GetAllAsync<CategoryInfoServiceModel>())
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
    }
}
