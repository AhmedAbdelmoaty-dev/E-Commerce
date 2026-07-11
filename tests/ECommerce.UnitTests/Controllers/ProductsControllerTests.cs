using Xunit;
using API.Controllers;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Dtos;
using Application.Products.Query.GetAllBrands;
using Application.Products.Query.GetAllProducts;
using Application.Products.Query.GetAllTypes;
using Application.Products.Query.GetProductById;
using Application.Specification;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Skinet.RequestHelpers;

namespace ECommerce.UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetById_Returns_Ok_With_ProductDto()
        {
            var dto = new ProductDto { Id = 1, Name = "Test" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
                .ReturnsAsync(dto);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(dto, okResult.Value);
        }

        [Fact]
        public async Task GetAll_Returns_Ok_With_Pagination()
        {
            var pagination = new Pagination<ProductDto>(10, 1, 2, new List<ProductDto>
            {
                new() { Id = 1, Name = "P1" },
                new() { Id = 2, Name = "P2" }
            });
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), default))
                .ReturnsAsync(pagination);

            var result = await _controller.GetAll(new ProductSpecParams());

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(pagination, okResult.Value);
        }

        [Fact]
        public async Task CreateProduct_Returns_CreatedAtAction()
        {
            var command = new CreateProductCommand { Name = "New" };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(42);

            var result = await _controller.CreateProduct(command);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ProductsController.GetById), createdResult.ActionName);
            Assert.Equal(42, createdResult.RouteValues?["id"]);
        }

        [Fact]
        public async Task DeleteProduct_Returns_NoContent()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.DeleteProduct(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_Returns_NoContent()
        {
            var command = new UpdateProductCommand();
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var result = await _controller.UpdateProduct(1, command);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(1, command.Id);
        }

        [Fact]
        public async Task GetAllTypes_Returns_Ok_With_Types()
        {
            var types = new List<string> { "TypeA", "TypeB" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTypesQuery>(), default))
                .ReturnsAsync(types);

            var result = await _controller.GetAllTypes();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(types, okResult.Value);
        }

        [Fact]
        public async Task GetAllBrands_Returns_Ok_With_Brands()
        {
            var brands = new List<string> { "BrandA", "BrandB" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllBrandsQuery>(), default))
                .ReturnsAsync(brands);

            var result = await _controller.GetAllBrands();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(brands, okResult.Value);
        }
    }
}
