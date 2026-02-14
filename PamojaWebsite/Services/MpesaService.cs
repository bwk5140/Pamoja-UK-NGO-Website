namespace PamojaWebsite.Services
{
    using Microsoft.Extensions.Options;
    using System.Net.Http.Json;
    using System.Security.Cryptography;
    using System.Text;

    public class MpesaService
    {
        private readonly HttpClient _http;
        private readonly MpesaOptions _opts;

        public MpesaService(HttpClient http, MpesaOptions opts)
        {
            _http = http;
            _opts = opts;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var baseUrl = "https://sandbox.safaricom.co.ke"; // switch to live env when ready
            var byteArray = Encoding.ASCII.GetBytes($"{_opts.ConsumerKey}:{_opts.ConsumerSecret}");
            var req = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/oauth/v1/generate?grant_type=client_credentials");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var resp = await _http.SendAsync(req);
            var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return json?["access_token"] ?? "";
        }

        private static string Timestamp() => DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        private string Password(string timestamp)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_opts.ShortCode}{_opts.Passkey}{timestamp}"));

        public async Task<Dictionary<string, object>> StkPushAsync(string phoneE164, decimal amount)
        {
            var token = await GetAccessTokenAsync();
            var ts = Timestamp();
            var reqPayload = new
            {
                BusinessShortCode = _opts.ShortCode,
                Password = Password(ts),
                Timestamp = ts,
                TransactionType = "CustomerPayBillOnline",
                Amount = (int)amount, // STK Push expects integer
                PartyA = phoneE164,   // e.g., "2547XXXXXXXX"
                PartyB = _opts.ShortCode,
                PhoneNumber = phoneE164,
                CallBackURL = _opts.CallbackUrl,
                AccountReference = "ORDER-REF",
                TransactionDesc = "Payment for order"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest")
            {
                Content = JsonContent.Create(reqPayload)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var resp = await _http.SendAsync(request);
            var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return json ?? new();
        }
    }
}
