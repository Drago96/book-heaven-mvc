using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.UtilityServices.ShoppingCart;
using BookHeaven.Web.Controllers;
using BookHeaven.Web.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using BookHeaven.Web.Infrastructure.Constants.SuccessMessages;
using BookHeaven.Web.Infrastructure.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace BookHeaven.Tests.Web.Controllers
{
    public class ShoppingCartControllerTests
    {
        [Fact]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            //Arrange
            var controller = typeof(ShoppingCartController);

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
            var bookServiceMock = new Mock<IBookService>();
            bookServiceMock.Setup(b => b.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var controller = new ShoppingCartController(bookServiceMock.Object, null,null,null);
            controller.ControllerContext = this.GetControllerContextMock();

            //Act
            var result = await controller.AddToShoppingCart(1);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddToShoppingShouldDisplayErrorIfCartIsFull()
        {
            //Arrange
            const int bookId = 1;
            string errorMessage = null;

            var bookServiceMock = new Mock<IBookService>();
            bookServiceMock.Setup(b => b.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var shoppingCartMock = new Mock<IShoppingCartManager>();
            shoppingCartMock.Setup(s => s.CartIsFull(It.IsAny<string>())).Returns(true);

            var controller = new ShoppingCartController(bookServiceMock.Object,shoppingCartMock.Object,null,null);
            controller.ControllerContext = this.GetControllerContextMock();

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[DataKeyConstants.ErrorMessage] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            controller.TempData = tempData.Object;

            //Act
            var result = await controller.AddToShoppingCart(bookId);

            //Assert
            errorMessage.Should().Be(ShoppingCartErrorMessages.CartIsFull);
            result.Should().BeOfType<RedirectToActionResult>();
            var resultAction = result as RedirectToActionResult;
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

            var bookServiceMock = new Mock<IBookService>();
            bookServiceMock.Setup(b => b.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            var shoppingCartMock = new Mock<IShoppingCartManager>();
            shoppingCartMock.Setup(s => s.CartIsFull(It.IsAny<string>())).Returns(false);
            shoppingCartMock.Setup(s => s.AddToCart(It.IsAny<string>(), bookId))
                .Callback((string shoppingCartId, int addedBookIdArg) =>
                {
                    addedBookId = addedBookIdArg;
                });

            var controller = new ShoppingCartController(bookServiceMock.Object, shoppingCartMock.Object, null, null);
            controller.ControllerContext = this.GetControllerContextMock();

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[DataKeyConstants.SuccessMessage] = It.IsAny<string>())
                .Callback((string key, object message) => sucessMessage = message as string);

            controller.TempData = tempData.Object;

            //Act
            var result = await controller.AddToShoppingCart(bookId);

            //Assert
            sucessMessage.Should().Be(ShoppingCartSuccessMessages.ItemAddedSuccess);
            addedBookId.Should().Be(bookId);
            result.Should().BeOfType<RedirectToActionResult>();
            var resultAction = result as RedirectToActionResult;
            resultAction.ActionName.Should().Be(nameof(BooksController.Details));
            resultAction.ControllerName.Should().Be("Books");
            resultAction.RouteValues["id"].Should().Be(bookId);
        }

        private ControllerContext GetControllerContextMock()
        {
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(h => h.Session).Returns(this.GetSessionMock().Object);
            return new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };
        }

        private Mock<ISession> GetSessionMock()
        {
            var sessionMock = new Mock<ISession>();
  
            var value = new byte[0];

            sessionMock.Setup(_ => _.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((k, v) => value = v);

            sessionMock.Setup(_ => _.TryGetValue(It.IsAny<string>(), out value))
                .Returns(true);

            return sessionMock;
        }
    }
}
