using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Areas.Admin.Models.Categories;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using BookHeaven.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

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
            => View(new CategoryAllPageViewModel
            {
                AllCategories = await this.categories.GetAllAsync<CategoryAdminListingServiceModel>()
            });

        [HttpPost]
        public async Task<IActionResult> Create(CategoryAllPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCategories = await this.categories.GetAllAsync<CategoryAdminListingServiceModel>();
                return View(nameof(All), model);
            }

            var exists = await this.categories.ExistsAsync(model.CategoryCreateModel.Name);
            if (exists)
            {
                TempData.AddErrorMessage(string.Format(CommonErrorConstants.AlreadyExists,nameof(Category)));
                model.AllCategories = await this.categories.GetAllAsync<CategoryAdminListingServiceModel>();
                return View(nameof(All), model);
            }

            await this.categories.CreateAsync(model.CategoryCreateModel.Name);
            TempData.AddSuccessMessage(string.Format(CategorySuccessConstants.CategoryCreateMessage,model.CategoryCreateModel.Name));

            return RedirectToAction(nameof(All));
        }
    }
}
