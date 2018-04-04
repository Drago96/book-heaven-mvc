using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Areas.Admin.Controllers;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Filters;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Admin.Controllers
{
    public class CategoriesControllerTests
    {
        private const int TestCategoryIdOne = 1;
        private const string TestCategoryNameOne = "TestNameOne";

        private const int TestCategoryIdTwo = 2;
        private const string TestCategoryNameTwo = "TestNameTwo";

        private readonly CategoriesController sut;
        private readonly Mock<ICategoryService> categoriesMock;

        public CategoriesControllerTests()
        {
            AutoMapperInitializer.Initialize();

            this.categoriesMock = new Mock<ICategoryService>();
            this.sut = new CategoriesController(this.categoriesMock.Object);
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
        public async Task AllShouldReturnCorrectResult()
        {
            //Arrange
            List<CategoryAdminListingServiceModel> categoriesToReturn =
                new List<CategoryAdminListingServiceModel>
                {
                    new CategoryAdminListingServiceModel
                    {
                        Id = TestCategoryIdOne,
                        Name = TestCategoryNameOne
                    },
                    new CategoryAdminListingServiceModel
                    {
                        Id = TestCategoryIdTwo,
                        Name = TestCategoryNameTwo
                    }
                };

            this.categoriesMock.Setup(c => c.AllAsync<CategoryAdminListingServiceModel>()).ReturnsAsync(categoriesToReturn);

            //Act
            var result = await this.sut.All();

            //Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be(null);
            var model = viewResult.Model.Should().BeOfType<List<CategoryAdminListingServiceModel>>().Subject;
            model.ShouldBeEquivalentTo(categoriesToReturn, options => options.WithStrictOrdering());
            this.categoriesMock.Verify(c => c.AllAsync<CategoryAdminListingServiceModel>(), Times.Once);
        }

        [Fact]
        public void DeleteShouldContainCorrectAttributes()
        {
            //Arrange
            var deleteAction = this.sut.GetType().GetMethod(nameof(CategoriesController.Delete));

            //Act
            var httpPostAttribute = deleteAction.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(HttpPostAttribute))
                as HttpPostAttribute;
            var serviceFilterAttribute = deleteAction.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(ServiceFilterAttribute))
                as ServiceFilterAttribute;

            //Assert
            httpPostAttribute.Should().NotBe(null);
            serviceFilterAttribute.Should().NotBe(null);
            serviceFilterAttribute.ServiceType.ShouldBeEquivalentTo(typeof(RefreshCategoryCache));
        }

        [Fact]
        public async Task DeleteShouldReturnBadRequestIfCategoryDoesntExist()
        {
            //Arrange
            this.categoriesMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            //Act
            var result = await this.sut.Delete(int.MaxValue);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
            this.categoriesMock.Verify(c => c.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteShouldDeleteCorrectCategoryAndReturnCorrectResultIfExists()
        {
            //Arrange
            const int categoryToDelete = 1;
            int deletedCategory = 0;
            string successMessage = null;

            this.categoriesMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.categoriesMock.Setup(c => c.DeleteAsync(It.IsAny<int>()))
                .Callback((int categoryId) =>
                {
                    deletedCategory = categoryId;
                })
                .Returns(Task.CompletedTask);

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[DataKeyConstants.SuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);
            this.sut.TempData = tempData.Object;

            //Act
            var result = await this.sut.Delete(categoryToDelete);

            //Assert
            deletedCategory.Should().Be(categoryToDelete);
            successMessage.Should().Be(CategorySuccessConstants.CategoryDeletedMessage);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.ShouldBeEquivalentTo(nameof(CategoriesController.All));
            this.categoriesMock.Verify(c => c.ExistsAsync(It.IsAny<int>()), Times.Once);
            this.categoriesMock.Verify(c => c.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}