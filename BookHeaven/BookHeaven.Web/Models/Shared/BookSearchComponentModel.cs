using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Areas.Admin.Models.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHeaven.Web.Models.Shared
{
    public class BookSearchComponentModel
    {
        public string SearchTerm { get; set; }

        public IEnumerable<SelectListItem> AllCategories { get; set; } = new List<SelectListItem>();

        public IEnumerable<int> Categories { get; set; } = new List<int>();
    }
}
