using BookHeaven.Services.Contracts;
using BookHeaven.Web.Areas.Admin.Models.Categories;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookHeaven.Web.Controllers.ApiControllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService categories;

        public CategoriesController(ICategoryService categories)
        {
            this.categories = categories;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        [ServiceFilter(typeof(ClearCategoryCache))]
        [ValidateApiModelState]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromBody]CategoryBasicInfoViewModel model)
        {
            var exists = await this.categories.ExistsAsync(id);

            if (!exists)
            {
                return BadRequest(CategoryErrorConstants.CategoryDoesNotExist);
            }

            var nameTaken = await this.categories.AlreadyExistsAsync(id, model.Name);

            if (nameTaken)
            {
                return BadRequest(CategoryErrorConstants.CategoryAlreadyExists);
            }

            await this.categories.EditAsync(id, model.Name);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        [ServiceFilter(typeof(ClearCategoryCache))]
        [ValidateApiModelState]
        public async Task<IActionResult> Create([FromBody] CategoryBasicInfoViewModel model)
        {
            var exists = await this.categories.ExistsAsync(model.Name);

            if (exists)
            {
                return BadRequest(CategoryErrorConstants.CategoryAlreadyExists);
            }

            await this.categories.CreateAsync(model.Name);

            return Ok();
        }
    }
}