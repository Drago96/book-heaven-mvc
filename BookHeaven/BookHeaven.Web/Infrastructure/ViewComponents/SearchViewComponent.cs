using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookHeaven.Web.Infrastructure.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ICategoryService categories;
        private readonly IMemoryCache cache;

        public SearchViewComponent(ICategoryService categories, IMemoryCache cache)
        {
            this.categories = categories;
            this.cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allCategories = this.cache.Get<IEnumerable<CategoryInfoServiceModel>>(CacheConstants.CategoriesCacheKey);

            if (allCategories == null)
            {
                allCategories = await this.categories.AllAsync<CategoryInfoServiceModel>();
                this.cache.Set(CacheConstants.CategoriesCacheKey, allCategories,
                    DateTimeOffset.UtcNow.AddDays(CacheConstants.CategoriesCacheInDays));
            }

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