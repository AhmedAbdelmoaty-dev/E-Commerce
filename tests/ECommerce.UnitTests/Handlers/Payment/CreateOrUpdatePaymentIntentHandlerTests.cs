using Xunit;
using Application.Contracts.Services;
using Application.Exceptions;
using Application.Payment.Commands.CreateOrUpdatePaymentIntent;
using Domain.Entites;
using Moq;

namespace ECommerce.UnitTests.Handlers.Payment
{
    public class CreateOrUpdatePaymentIntentHandlerTests
    {
        [Fact]
        public async Task Handle_PaymentServiceReturnsCart_ReturnsCart()
        {
            var cart = new ShoppingCart
            {
                Id = "cart1",
                Items = new List<CartItem>(),
                ClientSecret = "secret",
                PaymentIntentId = "pi_123"
            };
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(p => p.CreateOrUpdatePaymentInit("cart1"))
                .ReturnsAsync(cart);
            var handler = new CreateOrUpdatePaymentIntentHandler(paymentServiceMock.Object);

            var result = await handler.Handle(
                new CreateOrUpdatePaymentIntentCommand { CartId = "cart1" },
                CancellationToken.None);

            Assert.Same(cart, result);
        }

        [Fact]
        public async Task Handle_PaymentServiceReturnsNullTask_ReturnsNullCart()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(p => p.CreateOrUpdatePaymentInit("bad_cart"))
                .ReturnsAsync((ShoppingCart?)null);
            var handler = new CreateOrUpdatePaymentIntentHandler(paymentServiceMock.Object);

            var result = await handler.Handle(
                new CreateOrUpdatePaymentIntentCommand { CartId = "bad_cart" },
                CancellationToken.None);

            Assert.Null(result);
        }
    }
}

