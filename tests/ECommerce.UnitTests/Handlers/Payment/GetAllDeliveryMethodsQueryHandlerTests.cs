using Xunit;
using Application.Contracts.Repositories;
using Application.Payment.Queries.GetAllDeliveryMethods;
using Domain.Entites;
using Moq;

namespace ECommerce.UnitTests.Handlers.Payment
{
    public class GetAllDeliveryMethodsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAllDeliveryMethods()
        {
            var methods = new List<DeliveryMethod>
            {
                new() { Id = 1, ShortName = "UPS", Price = 10m },
                new() { Id = 2, ShortName = "FedEx", Price = 15m }
            };
            var repoMock = new Mock<IGenericRepository<DeliveryMethod>>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(methods);
            var handler = new GetAllDeliveryMethodsQueryHandler(repoMock.Object);

            var result = await handler.Handle(new GetAllDeliveryMethodsQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.ShortName == "UPS");
            Assert.Contains(result, m => m.ShortName == "FedEx");
        }
    }
}

