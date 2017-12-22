using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookHeaven.Web.Models.Shared
{
    public class BookSearchComponentModel
    {
        public string SearchTerm { get; set; }

        public IEnumerable<SelectListItem> AllCategories { get; set; } = new List<SelectListItem>();

        public IEnumerable<int> Categories { get; set; } = new List<int>();
    }
}