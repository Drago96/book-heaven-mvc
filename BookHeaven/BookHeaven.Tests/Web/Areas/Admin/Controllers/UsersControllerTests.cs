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
        private readonly UsersController sut;
        private readonly Mock<IUserService> usersMock;
        private readonly Mock<IMapper> mapperMock;

        public UsersControllerTests()
        {
            AutoMapperInitializer.Initialize();

            this.usersMock = new Mock<IUserService>();
            this.mapperMock = new Mock<IMapper>();
            this.sut = new UsersController(this.usersMock.Object, this.mapperMock.Object, null, null, null, null);
        }

        [Fact]
        public void UsersControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = this.sut.GetType();
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

            this.usersMock.Setup(u =>
                    u.AllPaginatedAsync<UserAdminListingServiceModel>(expectedSearchTerm, expectedPage, expectedPageSize))
                .ReturnsAsync(expectedUsers);
            this.usersMock.Setup(u => u.CountBySearchTermAsync(expectedSearchTerm))
                .ReturnsAsync(expectedUsersCount);

            //Act
            var result = await this.sut.All(expectedSearchTerm, expectedPage);

            //Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be(null);
            var resultModel = viewResult.Model.Should().BeOfType<PaginatedViewModel<UserAdminListingServiceModel>>().Subject;

            resultModel.Items.ShouldBeEquivalentTo(expectedUsers, options => options.WithStrictOrdering());
            resultModel.TotalItems.Should().Be(expectedUsersCount);
            resultModel.SearchTerm.Should().Be(expectedSearchTerm);
            resultModel.CurrentPage.Should().Be(expectedPage);
            resultModel.PageSize.Should().Be(expectedPageSize);
            resultModel.SearchTerm.Should().Be(expectedSearchTerm);
            resultModel.PageSize.Should().Be(expectedPageSize);
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundIfUserIsNull()
        {
            //Arrange
            this.usersMock.Setup(u => u.ByIdAsync<UserAdminDetailsServiceModel>(It.IsAny<string>()))
                .ReturnsAsync((UserAdminDetailsServiceModel)null);

            //Act
            var result = await this.sut.Details("");

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

            this.usersMock.Setup(u => u.ByIdAsync<UserAdminDetailsServiceModel>(userId))
                .ReturnsAsync(expectedUser);
            this.usersMock.Setup(u => u.GetRolesByIdAsync(userId)).ReturnsAsync(expectedRoles);
            this.mapperMock.Setup(m => m.Map<UserAdminDetailsServiceModel, UserAdminDetailsViewModel>(expectedUser))
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

            //Act
            var result = await this.sut.Details(userId);

            //Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be(null);
            var resultModel = viewResult.Model.Should().BeOfType<UserAdminDetailsViewModel>().Subject;

            resultModel.ProfilePicture.Should().Be(expectedUser.ProfilePicture);
            resultModel.Email.Should().Be(expectedUser.Email);
            resultModel.FirstName.Should().Be(expectedUser.FirstName);
            resultModel.LastName.Should().Be(expectedUser.LastName);
            resultModel.MoneySpent.Should().Be(expectedUser.MoneySpent);
            resultModel.TotalPurchases.Should().Be(expectedUser.TotalPurchases);
            resultModel.Roles.ShouldBeEquivalentTo(expectedRoles, options => options.WithStrictOrdering());
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