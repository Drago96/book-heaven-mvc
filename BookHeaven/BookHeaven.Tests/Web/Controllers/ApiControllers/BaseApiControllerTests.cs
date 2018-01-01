using BookHeaven.Web.Controllers.ApiControllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace BookHeaven.Tests.Web.Controllers.ApiControllers
{
    public class BaseApiControllerTests
    {
        private readonly BaseApiController sut;

        public BaseApiControllerTests()
        {
            this.sut = new BaseApiController();
        }

        [Fact]
        public void ControllerShouldHaveCorrectRouteTag()
        {
            //Arrange
            var controller = this.sut.GetType();

            //Act
            var routeAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(RouteAttribute)) as RouteAttribute;

            //Assert
            routeAttribute.Should().NotBe(null);
            routeAttribute.Template.Should().Be("api/[controller]");
        }
    }
}