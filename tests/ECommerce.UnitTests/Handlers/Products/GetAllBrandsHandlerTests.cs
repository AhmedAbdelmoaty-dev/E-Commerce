using Xunit;
using Application.Contracts.Repositories;
using Application.Products.Query.GetAllBrands;
using Moq;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class GetAllBrandsHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsBrandsFromRepository()
        {
            var brands = new List<string> { "BrandA", "BrandB" };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetBrandsAsync()).ReturnsAsync(brands);
            var handler = new GetAllBrandsHandler(repoMock.Object);

            var result = await handler.Handle(new GetAllBrandsQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains("BrandA", result);
            Assert.Contains("BrandB", result);
        }
    }
}

