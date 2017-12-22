using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Categories;
using BookHeaven.Web.Areas.Publisher.Controllers;
using BookHeaven.Web.Areas.Publisher.Models.Books;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Web.Areas.Publisher.Controllers
{
    public class BooksControllerTests
    {
        public BooksControllerTests()
        {
            AutoMapperInitializer.Initialize();
        }

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = typeof(BooksController);
            var baseController = typeof(PublisherBaseController);

            //Assert
            baseController.IsAssignableFrom(controller).Should().BeTrue();
        }

        [Fact]
        public async Task GetCreateShouldReturnViewWithCorrectModel()
        {
            //Arrange
            var categoriesToReturn = new List<CategoryInfoServiceModel>()
            {
                new CategoryInfoServiceModel
                {
                    Name = "TestName1",
                    Id = 1
                },
                new CategoryInfoServiceModel
                {
                    Name = "TestName2",
                    Id = 2
                }
            };

            var expectedCategories = categoriesToReturn.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var categoriesMock = new Mock<ICategoryService>();
            categoriesMock.Setup(c => c.AllAsync<CategoryInfoServiceModel>())
                .ReturnsAsync(categoriesToReturn);

            var controller = new BooksController(categoriesMock.Object, null, null, null, null, null);

            //Act
            var result = await controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
            var resultModel = (result as ViewResult).Model;
            resultModel.Should().BeOfType<BookCreateViewModel>();
            (resultModel as BookCreateViewModel).AllCategories.ShouldBeEquivalentTo(expectedCategories, options => options.WithStrictOrdering());
            (resultModel as BookCreateViewModel).Categories.Count().Should().Be(0);
        }
    }
}