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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll([FromQuery] ProductSpecParams specParams)
        {
            var products = await _mediator.Send(new GetAllProductsQuery(specParams));
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetAllTypes()
        {
            var types = await _mediator.Send(new GetAllTypesQuery());
            return Ok(types);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetAllBrands()
        {
            var brands = await _mediator.Send(new GetAllBrandsQuery());
            return Ok(brands);
        }
    }
}
