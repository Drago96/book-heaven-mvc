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
        private readonly CategoriesController sut;
        private readonly Mock<ICategoryService> categoriesMock;

        public CategoriesControllerTests()
        {
            this.categoriesMock = new Mock<ICategoryService>();
            this.sut = new CategoriesController(this.categoriesMock.Object);
        }

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = this.sut.GetType();
            var baseController = typeof(BaseApiController);

            //Assert
            baseController.IsAssignableFrom(controller).Should().BeTrue();
        }

        [Fact]
        public void EditShouldContainAllNecessaryAttributes()
        {
            //Arrange
            var editAction = this.sut.GetType().GetMethod(nameof(CategoriesController.Edit));

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
            serviceFilterAttribute.ServiceType.ShouldBeEquivalentTo(typeof(RefreshCategoryCache));
        }

        [Fact]
        public async Task EditShouldReturnBadRequestIfCategoryDoesntExist()
        {
            //Arrange
            this.categoriesMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            //Act
            var result = await this.sut.Edit(1, null);

            //Assert
            var viewResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            viewResult.Value.Should().Be(CategoryErrorConstants.CategoryDoesNotExist);
        }

        [Fact]
        public async Task EditShouldReturnBadRequestIfCategoryAlreadyExists()
        {
            //Arrange
            this.categoriesMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.categoriesMock.Setup(c => c.AlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var result = await this.sut.Edit(1, new CategoryBasicInfoViewModel
            {
                Name = ""
            });

            //Assert
            var viewResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            viewResult.Value.Should().Be(CategoryErrorConstants.CategoryAlreadyExists);
        }

        [Fact]
        public async Task EditShouldEditCategoryAndReturnOkWithCorrectData()
        {
            //Arrange
            const string newCategoryName = "TestCategory";
            const int categoryId = 1;
            string categoryToEdit = null;

            this.categoriesMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.categoriesMock.Setup(c => c.AlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            this.categoriesMock.Setup(c => c.EditAsync(categoryId, newCategoryName))
                .Callback((int id, string categoryName) =>
                {
                    categoryToEdit = categoryName;
                })
                .Returns(Task.CompletedTask);

            //Act
            var result = await this.sut.Edit(categoryId, new CategoryBasicInfoViewModel
            {
                Name = newCategoryName
            });

            //Assert
            result.Should().BeOfType<OkResult>();
            categoryToEdit.Should().Be(newCategoryName);
        }
    }
}