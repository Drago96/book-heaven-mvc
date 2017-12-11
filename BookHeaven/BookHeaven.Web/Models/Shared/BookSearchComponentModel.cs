using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Areas.Admin.Models.Categories;

namespace BookHeaven.Web.Models.Shared
{
    public class BookSearchComponentModel
    {
        public string SearchTerm { get; set; }

        public IEnumerable<CategoryInfoServiceModel> AllCategories { get; set; } = new List<CategoryInfoServiceModel>();

        public IEnumerable<int> Categories { get; set; } = new List<int>();
    }
}
