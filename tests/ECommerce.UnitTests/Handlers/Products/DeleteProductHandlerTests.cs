using Xunit;
using Application.Contracts.Repositories;
using Application.Exceptions;
using Application.Products.Commands.DeleteProduct;
using Domain.Entites;
using Moq;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IGenericRepository<Product>> _repositoryMock;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Product>>();
            _handler = new DeleteProductHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_DeletesAndSaves()
        {
            var product = new Product { Id = 1, Description="d", Type="t", Brand="b", QuantityInStock=1, Name = "ToDelete" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            await _handler.Handle(new DeleteProductCommand(1), CancellationToken.None);

            _repositoryMock.Verify(r => r.Delete(product), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsNotFoundResourceException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var exception = await Assert.ThrowsAsync<NotFoundResourceException>(() =>
                _handler.Handle(new DeleteProductCommand(999), CancellationToken.None));

            Assert.Contains("Product", exception.Message);
            Assert.Contains("999", exception.Message);
        }
    }
}


