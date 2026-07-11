using Xunit;
using API.Controllers;
using Application.Payment.Commands.CreateOrUpdatePaymentIntent;
using Application.Payment.Queries.GetAllDeliveryMethods;
using Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.UnitTests.Controllers
{
    public class PaymentControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PaymentController _controller;

        public PaymentControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PaymentController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateOrUpdatePaymentInit_Returns_Ok_With_Cart()
        {
            var cart = new ShoppingCart
            {
                Id = "cart1",
                Items = new List<CartItem>(),
                ClientSecret = "secret",
                PaymentIntentId = "pi_123"
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrUpdatePaymentIntentCommand>(), default))
                .ReturnsAsync(cart);

            var result = await _controller.CreateOrUpdatePaymentInit("cart1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(cart, okResult.Value);
        }

        [Fact]
        public async Task GetDeliveryMethods_Returns_Ok_With_Methods()
        {
            var methods = new List<DeliveryMethod>
            {
                new() { Id = 1, ShortName = "UPS", Price = 10m },
                new() { Id = 2, ShortName = "FedEx", Price = 15m }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllDeliveryMethodsQuery>(), default))
                .ReturnsAsync(methods);

            var result = await _controller.GetDeliveryMethods();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(methods, okResult.Value);
        }
    }
}

