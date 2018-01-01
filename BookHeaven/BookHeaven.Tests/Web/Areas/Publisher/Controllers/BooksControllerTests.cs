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
        private readonly BooksController sut;
        private readonly Mock<ICategoryService> categoriesMock;

        public BooksControllerTests()
        {
            AutoMapperInitializer.Initialize();

            this.categoriesMock = new Mock<ICategoryService>();
            this.sut = new BooksController(this.categoriesMock.Object, null, null, null, null, null);
        }

        [Fact]
        public void CategoriesControllerShouldExtendAdminBaseController()
        {
            //Arrange
            var controller = this.sut.GetType();
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

            this.categoriesMock.Setup(c => c.AllAsync<CategoryInfoServiceModel>())
                .ReturnsAsync(categoriesToReturn);

            //Act
            var result = await this.sut.Create();

            //Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be(null);
            var resultModel = viewResult.Model.Should().BeOfType<BookCreateViewModel>().Subject;
            resultModel.AllCategories.ShouldBeEquivalentTo(expectedCategories, options => options.WithStrictOrdering());
            resultModel.Categories.Count().Should().Be(0);
        }
    }
}