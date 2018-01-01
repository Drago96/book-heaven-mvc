using BookHeaven.Services.Contracts;
using BookHeaven.Services.UtilityServices.ShoppingCart;
using BookHeaven.Tests.Mocks;
using BookHeaven.Web.Controllers;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Web.Controllers
{
    public class ShoppingCartControllerTests
    {
        private readonly ShoppingCartController sut;
        private readonly Mock<IBookService> booksMock;
        private readonly Mock<IShoppingCartManager> shoppingCartManagerMock;

        public ShoppingCartControllerTests()
        {
            this.booksMock = new Mock<IBookService>();
            this.shoppingCartManagerMock = new Mock<IShoppingCartManager>();
            this.sut = new ShoppingCartController(this.booksMock.Object, this.shoppingCartManagerMock.Object, null, null);
            this.sut.ControllerContext = new ControllerContext
            {
                HttpContext = HttpContextWithSessionMock.New().Object
            };
        }

        [Fact]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            //Arrange
            var controller = this.sut.GetType();

            //Act
            var authorizeAttribute = controller.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            //Assert
            authorizeAttribute.Should().NotBe(null);
        }

        [Fact]
        public async Task AddToShoppingCartShouldReturnBadRequestIfBookDoesNotExist()
        {
            //Arrange
            this.booksMock.Setup(b => b.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            //Act
            var result = await this.sut.AddToShoppingCart(1);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddToShoppingShouldDisplayErrorIfCartIsFull()
        {
            //Arrange
            const int bookId = 1;
            string errorMessage = null;

            this.booksMock.Setup(b => b.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.shoppingCartManagerMock.Setup(s => s.CartIsFull(It.IsAny<string>())).Returns(true);

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[DataKeyConstants.ErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            this.sut.TempData = tempData.Object;

            //Act
            var result = await this.sut.AddToShoppingCart(bookId);

            //Assert
            errorMessage.Should().Be(ShoppingCartErrorMessages.CartIsFull);
            var resultAction = result.Should().BeOfType<RedirectToActionResult>().Subject;
            resultAction.ActionName.Should().Be(nameof(BooksController.Details));
            resultAction.ControllerName.Should().Be("Books");
            resultAction.RouteValues["id"].Should().Be(bookId);
        }

        [Fact]
        public async Task AddToShoppingShouldAddToCartSuccessfullyWithCorrectData()
        {
            //Arrange
            const int bookId = 1;
            int addedBookId = 0;
            string sucessMessage = null;

            this.booksMock.Setup(b => b.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.shoppingCartManagerMock.Setup(s => s.CartIsFull(It.IsAny<string>())).Returns(false);
            this.shoppingCartManagerMock.Setup(s => s.AddToCart(It.IsAny<string>(), bookId))
                .Callback((string shoppingCartId, int addedBookIdArg) =>
                {
                    addedBookId = addedBookIdArg;
                });

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[DataKeyConstants.SuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => sucessMessage = message as string);

            this.sut.TempData = tempData.Object;

            //Act
            var result = await this.sut.AddToShoppingCart(bookId);

            //Assert
            sucessMessage.Should().Be(ShoppingCartSuccessMessages.ItemAddedSuccess);
            addedBookId.Should().Be(bookId);
            var resultAction = result.Should().BeOfType<RedirectToActionResult>().Subject;
            resultAction.ActionName.Should().Be(nameof(BooksController.Details));
            resultAction.ControllerName.Should().Be("Books");
            resultAction.RouteValues["id"].Should().Be(bookId);
        }
    }
}