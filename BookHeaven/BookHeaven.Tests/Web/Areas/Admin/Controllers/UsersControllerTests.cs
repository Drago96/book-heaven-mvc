using AutoMapper;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Areas.Admin.Controllers;
using BookHeaven.Web.Areas.Admin.Models.Users;
using BookHeaven.Web.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Admin.Controllers
{
    public class UsersControllerTests
    {
        public UsersControllerTests()
        {
            AutoMapperInitializer.Initialize();
        }

        [Fact]
        public void UsersControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = typeof(UsersController);
            var baseController = typeof(AdminBaseController);

            //Assert
            baseController.IsAssignableFrom(controller).Should().BeTrue();
        }

        [Fact]
        public async Task AllShouldReturnCorrectResult()
        {
            //Arrange
            const string expectedSearchTerm = "TestSearchTerm";
            const int expectedPage = 123;
            const int expectedPageSize = UserServiceConstants.UserListingPageSize;
            const int expectedUsersCount = 333;
            List<UserAdminListingServiceModel> expectedUsers = this.GetExpectedUsers();

            var usersServiceMock = new Mock<IUserService>();
            usersServiceMock.Setup(u =>
                    u.AllPaginatedAsync<UserAdminListingServiceModel>(expectedSearchTerm, expectedPage, expectedPageSize))
                .ReturnsAsync(expectedUsers);

            usersServiceMock.Setup(u => u.CountBySearchTermAsync(expectedSearchTerm))
                .ReturnsAsync(expectedUsersCount);

            var controller = new UsersController(usersServiceMock.Object, null, null, null, null, null);

            //Act
            var result = await controller.All(expectedSearchTerm, expectedPage);

            //Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().Be(null);
            var resultModel = viewResult.Model;
            resultModel.Should().BeOfType<PaginatedViewModel<UserAdminListingServiceModel>>();

            var parsedResultModel = resultModel as PaginatedViewModel<UserAdminListingServiceModel>;
            parsedResultModel.Items.ShouldBeEquivalentTo(expectedUsers, options => options.WithStrictOrdering());
            parsedResultModel.TotalItems.Should().Be(expectedUsersCount);
            parsedResultModel.SearchTerm.Should().Be(expectedSearchTerm);
            parsedResultModel.CurrentPage.Should().Be(expectedPage);
            parsedResultModel.PageSize.Should().Be(expectedPageSize);
            parsedResultModel.SearchTerm.Should().Be(expectedSearchTerm);
            parsedResultModel.PageSize.Should().Be(expectedPageSize);
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundIfUserIsNull()
        {
            //Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(u => u.ByIdAsync<UserAdminDetailsServiceModel>(It.IsAny<string>()))
                .ReturnsAsync((UserAdminDetailsServiceModel)null);

            var controller = new UsersController(userServiceMock.Object, null, null, null, null, null);

            //Act
            var result = await controller.Details("");

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectResultIfUserExists()
        {
            //Arrange
            const string userId = "TestUserId";
            var expectedUser = new UserAdminDetailsServiceModel
            {
                Email = "TestEmail",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                MoneySpent = 100,
                ProfilePicture = "TestPicture",
                TotalPurchases = 10
            };
            var expectedRoles = new List<string> { "TestRole1", "TestRole2" };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(u => u.ByIdAsync<UserAdminDetailsServiceModel>(userId))
                .ReturnsAsync(expectedUser);
            userServiceMock.Setup(u => u.GetRolesByIdAsync(userId)).ReturnsAsync(expectedRoles);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<UserAdminDetailsServiceModel, UserAdminDetailsViewModel>(expectedUser))
                .Returns(new UserAdminDetailsViewModel
                {
                    Email = expectedUser.Email,
                    FirstName = expectedUser.FirstName,
                    LastName = expectedUser.LastName,
                    MoneySpent = expectedUser.MoneySpent,
                    ProfilePicture = expectedUser.ProfilePicture,
                    Roles = null,
                    TotalPurchases = expectedUser.TotalPurchases
                });

            var controller = new UsersController(userServiceMock.Object, mapperMock.Object, null, null, null, null);

            //Act
            var result = await controller.Details(userId);

            //Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().Be(null);
            var resultModel = viewResult.Model;

            resultModel.Should().BeOfType<UserAdminDetailsViewModel>();
            var parsedResultModel = resultModel as UserAdminDetailsViewModel;
            parsedResultModel.ProfilePicture.Should().Be(expectedUser.ProfilePicture);
            parsedResultModel.Email.Should().Be(expectedUser.Email);
            parsedResultModel.FirstName.Should().Be(expectedUser.FirstName);
            parsedResultModel.LastName.Should().Be(expectedUser.LastName);
            parsedResultModel.MoneySpent.Should().Be(expectedUser.MoneySpent);
            parsedResultModel.TotalPurchases.Should().Be(expectedUser.TotalPurchases);
            parsedResultModel.Roles.ShouldBeEquivalentTo(expectedRoles, options => options.WithStrictOrdering());
        }

        private List<UserAdminListingServiceModel> GetExpectedUsers()
         => new List<UserAdminListingServiceModel>
         {
             new UserAdminListingServiceModel
             {
                 Id = "TestId1",
                 Email = "Test1@gmail.com",
                 FirstName = "TestFirstName1",
                 LastName = "TestLastName1"
             },
             new UserAdminListingServiceModel
             {
                 Id = "TestId2",
                 Email = "Test2@gmail.com",
                 FirstName = "TestFirstName2",
                 LastName = "TestLastName2"
             }
         };
    }
}