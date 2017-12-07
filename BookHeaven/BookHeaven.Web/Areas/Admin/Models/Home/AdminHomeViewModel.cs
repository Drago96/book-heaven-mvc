using System.Collections.Generic;
using BookHeaven.Services.Models.Locations;

namespace BookHeaven.Web.Areas.Admin.Models.Home
{
    public class AdminHomeViewModel
    {
        public int TotalUsers { get; set; }

        public int VisitsToday { get; set; }

        public int MostVisits { get; set; }

        public IEnumerable<LocationVisitsServiceModel> Locations { get; set; }
    }
}
