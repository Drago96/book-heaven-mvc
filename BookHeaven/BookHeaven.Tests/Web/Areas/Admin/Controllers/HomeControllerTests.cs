using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Locations;
using BookHeaven.Web.Areas.Admin.Controllers;
using BookHeaven.Web.Areas.Admin.Models.Home;
using BookHeaven.Web.Infrastructure.Constants.Areas;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Admin.Controllers
{
    public class HomeControllerTests
    {
        private readonly HomeController sut;
        private readonly Mock<IUserService> usersMock;
        private readonly Mock<ISiteVisitService> siteVisitsMock;
        private readonly Mock<ILocationService> locationsMock;

        public HomeControllerTests()
        {
            AutoMapperInitializer.Initialize();

            this.usersMock = new Mock<IUserService>();
            this.siteVisitsMock = new Mock<ISiteVisitService>();
            this.locationsMock = new Mock<ILocationService>();
            this.sut = new HomeController(this.usersMock.Object, this.siteVisitsMock.Object, this.locationsMock.Object);
        }

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = this.sut.GetType();
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

            this.usersMock.Setup(u => u.CountAsync()).ReturnsAsync(usersToReturn);
            this.siteVisitsMock.Setup(s => s.VisitsByDateAsync(DateTime.Today)).ReturnsAsync(visitsToday);
            this.siteVisitsMock.Setup(s => s.MostSiteVisitsInADayAsync()).ReturnsAsync(mostVisitsInADay);
            this.locationsMock.Setup(l =>
                    l.LocationsWithMostVisitsAsync<LocationVisitsServiceModel>(AdminConstants.CountryVisitsToDisplay))
                .ReturnsAsync(locationsWithMostVisits);

            //Act
            var result = await this.sut.Index();

            //Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be(null);
            var model = viewResult.Model.Should().BeOfType<AdminHomeViewModel>().Subject;
            model.Locations.ShouldBeEquivalentTo(locationsWithMostVisits, options => options.WithStrictOrdering());
            model.MostVisits.Should().Be(mostVisitsInADay);
            model.TotalUsers.Should().Be(usersToReturn);
            model.VisitsToday.Should().Be(visitsToday);
        }
    }
}