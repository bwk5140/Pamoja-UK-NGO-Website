namespace PamojaWebsite.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using PamojaWebsite.Services;

    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly StripePaymentService _stripe;
        private readonly StripeOptions _stripeOptions;

        public PaymentsController(StripePaymentService stripe, StripeOptions stripeOptions)
        {
            _stripe = stripe;
            _stripeOptions = stripeOptions;
        }

        [HttpPost("stripe/intent")]
        public async Task<IActionResult> CreateStripeIntent([FromBody] CreateIntentRequest req)
        {
            var intent = await _stripe.CreatePaymentIntentAsync(req.AmountCents, req.Currency, req.Email);
            return Ok(new { clientSecret = intent.ClientSecret, publishableKey = _stripeOptions.PublishableKey });
        }
    }

    public record CreateIntentRequest(long AmountCents, string Currency, string Email);
}
