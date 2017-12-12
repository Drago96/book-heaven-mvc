using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHeaven.Web.Infrastructure.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ICategoryService categories;

        public SearchViewComponent(ICategoryService categories)
        {
            this.categories = categories;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allCategories = await this.categories.AllAsync<CategoryInfoServiceModel>();

            return View(new BookSearchComponentModel
            {
                AllCategories = allCategories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            });

        }
    }
}
