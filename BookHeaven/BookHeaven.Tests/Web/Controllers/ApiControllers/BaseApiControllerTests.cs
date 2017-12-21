using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookHeaven.Web.Controllers.ApiControllers;
using BookHeaven.Web.Infrastructure.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BookHeaven.Tests.Web.Controllers.ApiControllers
{
    public class BaseApiControllerTests
    {
        [Fact]
        public void ControllerShouldHaveCorrectRouteTag()
        {
            //Arrange
            var controller = typeof(BaseApiController);

            //Act
            var routeAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(RouteAttribute)) as RouteAttribute;

            //Assert
            routeAttribute.Should().NotBe(null);
            routeAttribute.Template.Should().Be("api/[controller]");
        }
    }
}
