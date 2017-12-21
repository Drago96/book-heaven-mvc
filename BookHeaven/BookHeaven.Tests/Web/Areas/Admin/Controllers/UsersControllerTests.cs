using System;
using System.Collections.Generic;
using System.Text;
using BookHeaven.Web.Areas.Admin.Controllers;
using FluentAssertions;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Admin.Controllers
{
    public class UsersControllerTests
    {

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = typeof(UsersController);
            var baseController = typeof(AdminBaseController);

            //Assert
            baseController.IsAssignableFrom(controller).Should().BeTrue();

        }
    }
}
