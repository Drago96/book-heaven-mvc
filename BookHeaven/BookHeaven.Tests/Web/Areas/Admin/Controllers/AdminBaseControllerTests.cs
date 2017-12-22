using BookHeaven.Web.Areas.Admin.Controllers;
using BookHeaven.Web.Infrastructure.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Admin.Controllers
{
    public class AdminBaseControllerTests
    {
        [Fact]
        public void AdminBaseControllerShouldBeInAreaForAdmin()
        {
            //Arrange
            var controller = typeof(AdminBaseController);

            //Act
            var areaAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute)) as AreaAttribute;

            //Assert
            areaAttribute.Should().NotBe(null);
            areaAttribute.RouteValue.Should().Be(AreaConstants.Admin);
        }

        [Fact]
        public void AdminBaseControllerShouldBeAuthorizedForAdminOnly()
        {
            //Arrange
            var controller = typeof(AdminBaseController);

            //Act
            var authorizeAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            //Assert
            authorizeAttribute.Should().NotBe(null);
            authorizeAttribute.Roles.Should().Be(RoleConstants.Admin);
        }
    }
}