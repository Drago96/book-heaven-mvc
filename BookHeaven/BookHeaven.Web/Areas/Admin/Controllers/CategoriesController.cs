﻿using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        private readonly ICategoryService categories;

        public CategoriesController(ICategoryService categories)
        {
            this.categories = categories;
        }

        public async Task<IActionResult> All()
            => View(await this.categories.AllAsync<CategoryAdminListingServiceModel>());

        [HttpPost]
        [ServiceFilter(typeof(RefreshCategoryCache))]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await this.categories.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest();
            }

            await this.categories.DeleteAsync(id);

            TempData.AddSuccessMessage(CategorySuccessConstants.CategoryDeletedMessage);
            return RedirectToAction(nameof(All));
        }
    }
}