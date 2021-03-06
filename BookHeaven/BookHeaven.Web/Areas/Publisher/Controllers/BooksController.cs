﻿using AutoMapper;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Books;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Services.UtilityServices.Contracts;
using BookHeaven.Web.Areas.Publisher.Models.Books;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookHeaven.Web.Areas.Publisher.Controllers
{
    public class BooksController : PublisherBaseController
    {
        private readonly ICategoryService categories;
        private readonly IBookService books;
        private readonly IOrderService orders;
        private readonly IFileService fileService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public BooksController(ICategoryService categories, IBookService books, IFileService fileService, UserManager<User> userManager, IMapper mapper, IOrderService orders)
        {
            this.categories = categories;
            this.fileService = fileService;
            this.books = books;
            this.userManager = userManager;
            this.mapper = mapper;
            this.orders = orders;
        }

        public async Task<IActionResult> All(string searchTerm = "", int page = 1)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(new PaginatedViewModel<BookPublisherListingServiceModel>
            {
                Items = await this.books.AllByPublisherPaginatedAsync<BookPublisherListingServiceModel>(userId, searchTerm, page, BookServiceConstants.BookPublisherListingPageSize),
                TotalItems = await this.books.CountBySearchTermAsync(searchTerm),
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

            foreach (var categoryId in model.Categories)
            {
                var categoryExsits = await this.categories.ExistsAsync(categoryId);
                if (!categoryExsits)
                {
                    return BadRequest();
                }
            }

            string resizedBookPictureUrl = model.BookFilePicture != null ? await this.fileService.UploadImageAndGetUrlAsync(
                model.BookFilePicture, BookDataConstants.BookPictureWidth, BookDataConstants.BookPictureHeight) : null;
            string bookListingPictureUrl = model.BookFilePicture != null ? await this.fileService.UploadImageAndGetUrlAsync(
                model.BookFilePicture, BookDataConstants.BookPictureListingWidth, BookDataConstants.BookPictureListingHeight) : null;

            await this.books.CreateAsync(model.Title,
                model.Price,
                model.Description,
                model.Categories,
                resizedBookPictureUrl,
                bookListingPictureUrl,
                userManager.GetUserId(HttpContext.User));

            TempData.AddSuccessMessage(string.Format(BookSuccessConstants.BookPublishedMeessage, model.Title));

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(int id)
        {
            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isPublisher = await this.books.IsPublisherAsync(id, userId);

            if (!isPublisher)
            {
                return BadRequest();
            }

            var model = await this.books.ByIdAsync<BookPublisherDetailsServiceModel>(id);

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isPublisher = await this.books.IsPublisherAsync(id, userId);

            if (!isPublisher)
            {
                return BadRequest();
            }

            var book = await this.books.ByIdAsync<BookEditServiceModel>(id);
            var model = this.mapper.Map<BookEditServiceModel, BookEditViewModel>(book);
            model.AllCategories = await this.GetAllCategories();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCategories = await this.GetAllCategories();
                return View(model);
            }

            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isPublisher = await this.books.IsPublisherAsync(id, userId);

            if (!isPublisher)
            {
                return BadRequest();
            }

            foreach (var category in model.Categories)
            {
                if (!await this.categories.ExistsAsync(category))
                {
                    return BadRequest();
                }
            }

            string bookPictureUrl = model.BookFilePicture != null ? await this.fileService.UploadImageAndGetUrlAsync(
                model.BookFilePicture, BookDataConstants.BookPictureWidth, BookDataConstants.BookPictureHeight) : null;
            string listingPictureUrl = model.BookFilePicture != null ? await this.fileService.UploadImageAndGetUrlAsync(
                model.BookFilePicture, BookDataConstants.BookPictureListingWidth, BookDataConstants.BookPictureListingHeight) : null;

            await this.books.EditAsync(id, model.Title, model.Price, model.Description, model.Categories, bookPictureUrl, listingPictureUrl);

            TempData.AddSuccessMessage(BookSuccessConstants.EditMessage);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await this.books.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isPublisher = await this.books.IsPublisherAsync(id, userId);

            if (!isPublisher)
            {
                return BadRequest();
            }

            await this.books.DeleteAsync(id);

            TempData.AddSuccessMessage(BookSuccessConstants.BookDeletedSuccessfully);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Sales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<int> yearsWithSales = await this.orders.GetYearsWithSalesAsync(userId);

            var yearsModel = yearsWithSales.Select(y => new SelectListItem
            {
                Value = y.ToString(),
                Text = y.ToString()
            });

            return View(yearsModel);
        }

        private async Task<IEnumerable<SelectListItem>> GetAllCategories()
            => (await this.categories.AllAsync<CategoryInfoServiceModel>())
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
    }
}