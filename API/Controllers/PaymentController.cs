using Application.Payment.Commands.CreateOrUpdatePaymentIntent;
using Application.Payment.Queries.GetAllDeliveryMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [HttpPost("{cartId}")]

        public async Task<IActionResult> CreateOrUpdatePaymentInit(string cartId)
        {
           var cart =  await _mediator.Send(new CreateOrUpdatePaymentIntentCommand { CartId = cartId });
            return Ok(cart);
        }

        [HttpGet("delivery-methods")]
         public async Task<IActionResult> GetDeliveryMethods()
        {
           var deliveryMethods= await _mediator.Send(new GetAllDeliveryMethodsQuery());
            return Ok( deliveryMethods);   
        }
    }
}
