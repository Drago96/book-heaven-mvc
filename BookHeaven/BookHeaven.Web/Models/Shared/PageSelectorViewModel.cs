namespace BookHeaven.Web.Models.Shared
{
    public class PageSelectorViewModel
    {
        public string Action { get; set; } 

        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage { get; set; }

        public int NextPage { get; set; }

        public int TotalPages { get; set; }
    }
}
