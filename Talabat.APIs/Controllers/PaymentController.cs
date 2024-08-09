using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Service.Contract;

namespace Talabat.APIs.Controllers
{

    public class PaymentController : BaseAPIController
    {
        private readonly IPaymentService paymentService;
        private // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_edb4404d3f87e38c3f3d4fb7fb7c13ae5189390603d7dd27dd25f22de59e62c2";

        public PaymentController(IPaymentService _paymentService)
        {
            paymentService = _paymentService;
        }

        [Authorize]
        [HttpPost("{basketid}")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketid)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntentAsync(basketid);
            if (basket is null) return BadRequest(new ApiResponse(400));
            return Ok(basket);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> UpdateOrderStatus()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret,300,false);

            var PaymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            // Handle the event

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentPaymentFailed:
                    await paymentService.UpdateOrderStatus(PaymentIntent.Id, false);
                    break;
                case Events.PaymentIntentSucceeded:
                    await paymentService.UpdateOrderStatus(PaymentIntent.Id, true);
                    break;
            }

            return Ok();

        }


    }
}
