﻿using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Books;
using BookHeaven.Web.Models;
using BookHeaven.Web.Models.Books;
using BookHeaven.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly ICategoryService categories;

        public BooksController(IBookService books, ICategoryService categories)
        {
            this.books = books;
            this.categories = categories;
        }

        public async Task<IActionResult> Search(BookSearchComponentModel model, int page=1)
        {
            foreach (var categoryId in model.Categories)
            {
                if (!await this.categories.ExistsAsync(categoryId))
                {
                    return BadRequest();
                }
            }

            var searchTerm = model.SearchTerm ?? "";

            var searchResult = await this.books.FilterByTermAndCategoriesAsync<BookSearchListingServiceModel>( model.Categories,page, searchTerm);

            var listingModel = new BookSearchListingViewModel
            {
                Categories = await this.categories.NamesAsync(model.Categories),
                Books = new PaginatedViewModel<BookSearchListingServiceModel>
                {
                    CurrentPage = page,
                    Items = searchResult,
                    PageSize = BookServiceConstants.BookUserListingPageSize,
                    SearchTerm = searchTerm,
                    TotalItems =
                        await this.books.CountBySearchTermAndCategoriesAsync(model.Categories, model.SearchTerm)
                }
            };

            return View(listingModel);
}
    }
}
