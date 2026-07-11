using Xunit;
using API.Controllers;
using Application.Contracts.Services;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.UnitTests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _controller = new CartController(_cartServiceMock.Object);
        }

        [Fact]
        public async Task GetCartById_CartFound_Returns_Ok()
        {
            var cart = new ShoppingCart { Id = "cart1", Items = new List<CartItem>() };
            _cartServiceMock.Setup(s => s.GetCartAsync("cart1")).ReturnsAsync(cart);

            var result = await _controller.GetCartById("cart1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(cart, okResult.Value);
        }

        [Fact]
        public async Task GetCartById_CartNotFound_Returns_NotFound()
        {
            _cartServiceMock.Setup(s => s.GetCartAsync("missing")).ReturnsAsync((ShoppingCart?)null);

            var result = await _controller.GetCartById("missing");

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateCart_Success_Returns_Ok()
        {
            var cart = new ShoppingCart { Id = "cart1", Items = new List<CartItem>() };
            _cartServiceMock.Setup(s => s.SetCartAsync(cart)).ReturnsAsync(cart);

            var result = await _controller.UpdateCart(cart);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(cart, okResult.Value);
        }

        [Fact]
        public async Task UpdateCart_Failure_Returns_BadRequest()
        {
            var cart = new ShoppingCart { Id = "cart1" };
            _cartServiceMock.Setup(s => s.SetCartAsync(cart)).ReturnsAsync((ShoppingCart?)null);

            var result = await _controller.UpdateCart(cart);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteCart_Success_Returns_Ok()
        {
            _cartServiceMock.Setup(s => s.DeleteCartAsync("cart1")).ReturnsAsync(true);

            var result = await _controller.DeleteCart("cart1");

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteCart_Failure_Returns_BadRequest()
        {
            _cartServiceMock.Setup(s => s.DeleteCartAsync("cart1")).ReturnsAsync(false);

            var result = await _controller.DeleteCart("cart1");

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

