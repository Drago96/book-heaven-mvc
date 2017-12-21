using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookHeaven.Web.Areas.Admin.Controllers;
using BookHeaven.Web.Areas.Publisher.Controllers;
using BookHeaven.Web.Infrastructure.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Publisher.Controllers
{
    public class PublisherBaseControllerTests
    {
        [Fact]
        public void AdminBaseControllerShouldBeInAreaForAdmin()
        {
            //Arrange
            var controller = typeof(PublisherBaseController);

            //Act
            var areaAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute)) as AreaAttribute;

            //Assert
            areaAttribute.Should().NotBe(null);
            areaAttribute.RouteValue.Should().Be(AreaConstants.Publisher);
        }

        [Fact]
        public void AdminBaseControllerShouldBeAuthorizedForAdminOnly()
        {
            //Arrange
            var controller = typeof(PublisherBaseController);

            //Act
            var authorizeAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            //Assert
            authorizeAttribute.Should().NotBe(null);
            authorizeAttribute.Roles.Should().Be(RoleConstants.Publisher);
        }
    }
}
