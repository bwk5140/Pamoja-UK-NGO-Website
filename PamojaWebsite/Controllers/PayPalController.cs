namespace PamojaWebsite.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PamojaWebsite.Services;

    [ApiController]
    [Route("api/paypal")]
    public class PayPalController : ControllerBase
    {
        private readonly PayPalService _paypal;
        public PayPalController(PayPalService paypal) => _paypal = paypal;

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreatePaypalOrder req)
            => Ok(new { orderId = await _paypal.CreateOrderAsync(req.Amount, req.Currency) });

        [HttpPost("capture")]
        public async Task<IActionResult> Capture([FromBody] CapturePaypalOrder req)
            => Ok(new { success = await _paypal.CaptureOrderAsync(req.OrderId) });
    }

    public record CreatePaypalOrder(decimal Amount, string Currency);
    public record CapturePaypalOrder(string OrderId);
}
