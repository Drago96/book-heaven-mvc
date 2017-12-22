using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace BookHeaven.Web.Infrastructure.Filters
{
    public class ClearCategoryCache : ActionFilterAttribute
    {
        private readonly ICategoryService categories;
        private readonly IMemoryCache cache;

        public ClearCategoryCache(ICategoryService categories, IMemoryCache cache)
        {
            this.categories = categories;
            this.cache = cache;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            var allCategories = await this.categories.AllAsync<CategoryInfoServiceModel>();
            this.cache.Set(CacheConstants.CategoriesCacheKey, allCategories,
                DateTime.UtcNow.AddDays(CacheConstants.CategoriesCacheInDays));
        }
    }
}