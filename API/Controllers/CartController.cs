using Application.Contracts.Services;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cservice)
        {
            _cartService = cservice;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var cart = await _cartService.GetCartAsync(id);
            return cart == null ? NotFound("Cart not found") : Ok(cart);
        }
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var result = await _cartService.SetCartAsync(cart);
            if (result == null) return BadRequest("Failed to update the cart");
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var result = await _cartService.DeleteCartAsync(id);
            if (!result) return BadRequest("Failed to delete the cart");
            return Ok();
        }
    }
}
