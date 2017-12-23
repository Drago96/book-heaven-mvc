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

        public CategoriesControllerTests()
        {
            AutoMapperInitializer.Initialize();
        }

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = typeof(CategoriesController);
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

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(c => c.AllAsync<CategoryAdminListingServiceModel>()).ReturnsAsync(categoriesToReturn);
            var controller = new CategoriesController(categoryService.Object);

            //Act
            var result = await controller.All();

            //Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().Be(null);
            var model = viewResult.Model;
            model.Should().BeOfType<List<CategoryAdminListingServiceModel>>();
            var modelResult = model as List<CategoryAdminListingServiceModel>;
            modelResult.ShouldBeEquivalentTo(categoriesToReturn, options => options.WithStrictOrdering());
        }

        [Fact]
        public void DeleteShouldContainCorrectAttributes()
        {
            //Arrange
            var deleteAction = typeof(CategoriesController).GetMethod(nameof(CategoriesController.Delete));

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
            serviceFilterAttribute.ServiceType.ShouldBeEquivalentTo(typeof(ClearCategoryCache));
        }

        [Fact]
        public async Task DeleteShouldReturnBadRequestIfCategoryDoesntExist()
        {
            //Arrange
            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var categoryController = new CategoriesController(categoryService.Object);

            //Act
            var result = await categoryController.Delete(int.MaxValue);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteShouldDeleteCorrectCategoryAndReturnCorrectResultIfExists()
        {
            //Arrange
            const int categoryToDelete = 1;
            int deletedCategory = 0;
            string successMessage = null;

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            categoryService.Setup(c => c.DeleteAsync(It.IsAny<int>()))
                .Callback((int categoryId) =>
                {
                    deletedCategory = categoryId;
                })
                .Returns(Task.CompletedTask);

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[DataKeyConstants.SuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);

            var categoryController = new CategoriesController(categoryService.Object);
            categoryController.TempData = tempData.Object;

            //Act
            var result = await categoryController.Delete(categoryToDelete);

            //Assert
            deletedCategory.Should().Be(categoryToDelete);
            successMessage.Should().Be(CategorySuccessConstants.CategoryDeletedMessage);
            result.GetType().ShouldBeEquivalentTo(typeof(RedirectToActionResult));
            (result as RedirectToActionResult).ActionName.ShouldBeEquivalentTo(nameof(CategoriesController.All));
        }
    }
}