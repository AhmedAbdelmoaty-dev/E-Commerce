using Xunit;
using Application.Contracts.Repositories;
using Application.Products.Commands.CreateProduct;
using AutoMapper;
using Domain.Entites;
using Moq;

namespace ECommerce.UnitTests.Handlers.Products
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IGenericRepository<Product>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Product>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateProductHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_MapsAndAddsProduct_ReturnsSavedId()
        {
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Type = "Electronics",
                Brand = "TestBrand",
                QuantityInStock = 10,
                PictureUrl = "test.jpg"
            };
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Type = command.Type,
                Brand = command.Brand,
                QuantityInStock = command.QuantityInStock,
                PictureUrl = command.PictureUrl
            };
            _mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            _repositoryMock.Setup(r => r.Add(product)).Returns(1);
            _repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(42);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(42, result);
            _mapperMock.Verify(m => m.Map<Product>(command), Times.Once);
            _repositoryMock.Verify(r => r.Add(product), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}

