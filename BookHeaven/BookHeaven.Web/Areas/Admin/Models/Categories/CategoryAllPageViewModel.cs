using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Categories;

namespace BookHeaven.Web.Areas.Admin.Models.Categories
{
    public class CategoryAllPageViewModel
    {
        public CategoryCreateViewModel CategoryCreateModel { get; set; }

        public IEnumerable<CategoryAdminListingServiceModel> AllCategories { get; set; }
    }
}
