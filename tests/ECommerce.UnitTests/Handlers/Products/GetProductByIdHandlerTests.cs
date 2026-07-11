using Xunit;
using Application.Contracts.Repositories;
using Application.Exceptions;
using Application.Products.Dtos;
using Application.Products.Query.GetProductById;
using AutoMapper;
using Domain.Entites;
using Moq;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IGenericRepository<Product>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Product>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ProductFound_ReturnsMappedDto()
        {
            var product = new Product { Id = 1, Description="d", Type="t", Brand="b", QuantityInStock=1, Name = "Test", Price = 10m };
            var dto = new ProductDto { Id = 1, Name = "Test", Price = "10" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(dto);

            var result = await _handler.Handle(new GetProductByIdQuery(1), CancellationToken.None);

            Assert.Same(dto, result);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsNotFoundResourceException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var exception = await Assert.ThrowsAsync<NotFoundResourceException>(() =>
                _handler.Handle(new GetProductByIdQuery(1), CancellationToken.None));

            Assert.Contains("product", exception.Message);
            Assert.Contains("1", exception.Message);
        }
    }
}


