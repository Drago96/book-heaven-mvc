using BookHeaven.Services.Contracts;
using BookHeaven.Web.Areas.Admin.Models.Categories;
using BookHeaven.Web.Controllers.ApiControllers;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Filters;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Web.Controllers.ApiControllers
{
    public class CategoriesControllerTests
    {
        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = typeof(CategoriesController);
            var baseController = typeof(BaseApiController);

            //Assert
            baseController.IsAssignableFrom(controller).Should().BeTrue();
        }

        [Fact]
        public void EditShouldContainAllNecessaryAttributes()
        {
            //Arrange
            var editAction = typeof(CategoriesController).GetMethod(nameof(CategoriesController.Edit));

            //Act
            var httpPutAttribute = editAction.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(HttpPutAttribute))
                as HttpPutAttribute;
            var serviceFilterAttribute = editAction.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(ServiceFilterAttribute))
                as ServiceFilterAttribute;
            var authorizeAttribute = editAction.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;
            var validateApiModelState = editAction.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(ValidateApiModelState))
                as ValidateApiModelState;

            //Assert
            httpPutAttribute.Should().NotBe(null);
            httpPutAttribute.Template.Should().Be("{id}");

            authorizeAttribute.Should().NotBe(null);
            authorizeAttribute.Roles.Should().Be(RoleConstants.Admin);

            validateApiModelState.Should().NotBe(null);

            serviceFilterAttribute.Should().NotBe(null);
            serviceFilterAttribute.ServiceType.ShouldBeEquivalentTo(typeof(ClearCategoryCache));
        }

        [Fact]
        public async Task EditShouldReturnBadRequestIfCategoryDoesntExist()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = new CategoriesController(categoryServiceMock.Object);

            //Act
            var result = await controller.Edit(1, null);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).Value.Should().Be(CategoryErrorConstants.CategoryDoesNotExist);
        }

        [Fact]
        public async Task EditShouldReturnBadRequestIfCategoryAlreadyExists()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            categoryServiceMock.Setup(c => c.AlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = new CategoriesController(categoryServiceMock.Object);

            //Act
            var result = await controller.Edit(1, new CategoryBasicInfoViewModel
            {
                Name = ""
            });

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult).Value.Should().Be(CategoryErrorConstants.CategoryAlreadyExists);
        }

        [Fact]
        public async Task EditShouldEditCategoryAndReturnOkWithCorrectData()
        {
            //Arrange
            const string newCategoryName = "TestCategory";
            const int categoryId = 1;
            string categoryToEdit = null;

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            categoryServiceMock.Setup(c => c.AlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            categoryServiceMock.Setup(c => c.EditAsync(categoryId, newCategoryName))
                .Callback((int id, string categoryName) =>
                {
                    categoryToEdit = categoryName;
                })
                .Returns(Task.CompletedTask);

            var controller = new CategoriesController(categoryServiceMock.Object);

            //Act
            var result = await controller.Edit(categoryId, new CategoryBasicInfoViewModel
            {
                Name = newCategoryName
            });

            //Assert
            result.Should().BeOfType<OkResult>();
            categoryToEdit.Should().Be(newCategoryName);
        }
    }
}