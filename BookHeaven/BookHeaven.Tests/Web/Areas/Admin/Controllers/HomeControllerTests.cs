using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Locations;
using BookHeaven.Web.Areas.Admin.Controllers;
using BookHeaven.Web.Areas.Admin.Models.Home;
using BookHeaven.Web.Infrastructure.Constants.Areas;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Admin.Controllers
{
    public class HomeControllerTests
    {
        public HomeControllerTests()
        {
            AutoMapperInitializer.Initialize();

        }

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = typeof(HomeController);
            var baseController = typeof(AdminBaseController);

            //Assert
            baseController.IsAssignableFrom(controller).Should().BeTrue();

        }

        [Fact]
        public async Task IndexShouldReturnCorrectResult()
        {
            //Arrange
            const int usersToReturn = 10;
            const int visitsToday = 20;
            const int mostVisitsInADay = 30;
            List<LocationVisitsServiceModel> locationsWithMostVisits = new List<LocationVisitsServiceModel>
            {
                new LocationVisitsServiceModel
                {
                    Country = "TestCountry1",
                    Visits = 1
                },
                new LocationVisitsServiceModel
                {
                    Country = "TestCountry2",
                    Visits = 2
                }
            };

            var users = new Mock<IUserService>();
            users.Setup(u => u.CountAsync()).ReturnsAsync(usersToReturn);
            var siteVisits = new Mock<ISiteVisitService>();
            siteVisits.Setup(s => s.VisitsByDateAsync(DateTime.Today)).ReturnsAsync(visitsToday);
            siteVisits.Setup(s => s.MostSiteVisitsInADayAsync()).ReturnsAsync(mostVisitsInADay);
            var locations = new Mock<ILocationService>();
            locations.Setup(l =>
                    l.LocationsWithMostVisitsAsync<LocationVisitsServiceModel>(AdminConstants.CountryVisitsToDisplay))
                .ReturnsAsync(locationsWithMostVisits);

            var controller = new HomeController(users.Object,siteVisits.Object,locations.Object);

            //Act
            var result = await controller.Index();

            //Assert
            result.Should().BeOfType<ViewResult>();
            var model = (result as ViewResult).Model;
            model.Should().BeOfType<AdminHomeViewModel>();
            var modelResult = model as AdminHomeViewModel;
            modelResult.Locations.ShouldBeEquivalentTo(locationsWithMostVisits);
            modelResult.MostVisits.Should().Be(mostVisitsInADay);
            modelResult.TotalUsers.Should().Be(usersToReturn);
            modelResult.VisitsToday.Should().Be(visitsToday);
        }
    }
}
