using Xunit;
using Application.Contracts.Repositories;
using Application.Products.Query.GetAllTypes;
using Moq;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class GetAllTypesHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsTypesFromRepository()
        {
            var types = new List<string> { "TypeX", "TypeY" };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetAllTypesAsync()).ReturnsAsync(types);
            var handler = new GetAllTypesHandler(repoMock.Object);

            var result = await handler.Handle(new GetAllTypesQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains("TypeX", result);
            Assert.Contains("TypeY", result);
        }
    }
}

