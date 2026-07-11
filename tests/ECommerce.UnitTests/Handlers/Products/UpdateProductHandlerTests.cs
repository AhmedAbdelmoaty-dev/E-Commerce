using Xunit;
using Application.Contracts.Repositories;
using Application.Exceptions;
using Application.Products.Commands.UpdateProduct;
using AutoMapper;
using Domain.Entites;
using Moq;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IGenericRepository<Product>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Product>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateProductHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_MapsAndSaves()
        {
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Updated",
                Description = "Updated Desc",
                Price = 49.99m,
                QuantityInStock = 5,
                PictureUrl = "updated.jpg"
            };
            var existingProduct = new Product { Id = 1, Description="d", Type="t", Brand="b", QuantityInStock=1, Name = "Original" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProduct);

            await _handler.Handle(command, CancellationToken.None);

            _mapperMock.Verify(m => m.Map(command, existingProduct), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsNotFoundResourceException()
        {
            var command = new UpdateProductCommand { Id = 999 };
            _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var exception = await Assert.ThrowsAsync<NotFoundResourceException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Product", exception.Message);
            Assert.Contains("999", exception.Message);
        }
    }
}


