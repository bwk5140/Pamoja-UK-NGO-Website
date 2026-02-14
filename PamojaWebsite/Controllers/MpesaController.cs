namespace PamojaWebsite.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PamojaWebsite.Services;

    [ApiController]
    [Route("api/mpesa")]
    public class MpesaController : ControllerBase
    {
        private readonly MpesaService _mpesa;
        public MpesaController(MpesaService mpesa) => _mpesa = mpesa;

        [HttpPost("stkpush")]
        public async Task<IActionResult> StkPush([FromBody] MpesaRequest req)
            => Ok(await _mpesa.StkPushAsync(req.PhoneE164, req.Amount));

        // Safaricom will POST transaction results here
        [HttpPost("callback")]
        public IActionResult Callback([FromBody] object payload)
        {
            // TODO: validate, store result, update order status
            return Ok();
        }
    }

    public record MpesaRequest(string PhoneE164, decimal Amount);
}
