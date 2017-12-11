using System;
using System.Collections.Generic;

namespace BookHeaven.Web.Models
{
    public class PaginatedViewModel<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalItems / this.PageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage >= this.TotalPages
                ? this.TotalPages
                : this.CurrentPage + 1;

        public string SearchTerm { get; set; }
    }
}