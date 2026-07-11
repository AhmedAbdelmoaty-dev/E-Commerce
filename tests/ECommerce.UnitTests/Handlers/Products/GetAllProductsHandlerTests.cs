using Xunit;
using Application.Contracts.Repositories;
using Application.Products.Dtos;
using Application.Products.Query.GetAllProducts;
using AutoMapper;
using Domain.Entites;
using Moq;
using Skinet.RequestHelpers;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IGenericRepository<Product>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Product>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllProductsHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPaginatedProductDtos()
        {
            var specParams = new Application.Specification.ProductSpecParams
            {
                PageIndex = 1,
                PageSize = 10,
                sort = "priceAsc"
            };
            var products = new List<Product>
            {
                new() { Id = 1, Name = "P1", Price = 10m, Description = "d1", Type = "t1", Brand = "b1", QuantityInStock = 5 },
                new() { Id = 2, Name = "P2", Price = 20m, Description = "d2", Type = "t2", Brand = "b2", QuantityInStock = 10 }
            };
            var dtos = new List<ProductDto>
            {
                new() { Id = 1, Name = "P1", Price = "10" },
                new() { Id = 2, Name = "P2", Price = "20" }
            };

            _repositoryMock.Setup(r => r.GetAllWithSpecAsync(It.IsAny<Application.Specification.ProductSpecification>()))
                .ReturnsAsync(products);
            _repositoryMock.Setup(r => r.CountAsync(It.IsAny<Application.Specification.ProductSpecification>()))
                .ReturnsAsync(2);
            _mapperMock.Setup(m => m.Map<IReadOnlyList<ProductDto>>(products)).Returns(dtos);

            var result = await _handler.Handle(new GetAllProductsQuery(specParams), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.PageIndex);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, d => d.Name == "P1");
        }
    }
}


