using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Locations;
using BookHeaven.Web.Areas.Admin.Models.Home;
using BookHeaven.Web.Infrastructure.Constants.Areas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IUserService users;
        private readonly ISiteVisitService siteVisits;
        private readonly ILocationService locations;

        public HomeController(IUserService users, ISiteVisitService siteVisits, ILocationService locations)
        {
            this.users = users;
            this.siteVisits = siteVisits;
            this.locations = locations;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsers = await this.users.CountAsync();
            var visitsToday = await this.siteVisits.VisitsByDateAsync(DateTime.Today);
            var mostVisitsInADay = await this.siteVisits.MostSiteVisitsInADayAsync();

            var locationsWithMostVisits = await this.locations.LocationsWithMostVisitsAsync<LocationVisitsServiceModel>(AdminConstants.CountryVisitsToDisplay);

            return View(new AdminHomeViewModel
            {
                TotalUsers = totalUsers,
                MostVisits = mostVisitsInADay,
                VisitsToday = visitsToday,
                Locations = locationsWithMostVisits
            });
        }
    }
}