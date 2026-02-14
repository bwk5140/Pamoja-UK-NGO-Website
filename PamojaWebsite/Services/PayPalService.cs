namespace PamojaWebsite.Services
{
    using Microsoft.Extensions.Options;
    using System.Net.Http.Json;

    public class PayPalService
    {
        private readonly HttpClient _http;
        private readonly PayPalOptions _opts;
        private Dictionary<string, decimal> ExchangeRates = new()
        {
            { "USD", 1m },
            { "EUR", 0.92m },
            { "GBP", 0.78m },
            { "JPY", 145m },
            { "CAD", 1.35m },
            { "AUD", 1.50m },
            { "KES", 153m }
        };
        public PayPalService(HttpClient http, PayPalOptions opts)
        {
            _http = http;
            _opts = opts;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var authClient = new HttpClient();
            var baseUrl = _opts.Environment == "live" ? "https://api-m.paypal.com" : "https://api-m.sandbox.paypal.com";
            var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_opts.ClientId}:{_opts.ClientSecret}");
            authClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var tokenResp = await authClient.PostAsync($"{baseUrl}/v1/oauth2/token", new FormUrlEncodedContent(new[] {
            new KeyValuePair<string,string>("grant_type","client_credentials")
        }));
            var tokenJson = await tokenResp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return tokenJson?["access_token"]?.ToString() ?? "";
        }
        private decimal ChangeCurrency(string toCurrency, string fromCurrency, decimal amount)
        {
            if (!ExchangeRates.ContainsKey(fromCurrency) || !ExchangeRates.ContainsKey(toCurrency))
                throw new ArgumentException("Unsupported currency");

            // Step 1: normalize to USD
            decimal amountInUsd = amount / ExchangeRates[fromCurrency];

            // Step 2: convert USD → target
            decimal converted = amountInUsd * ExchangeRates[toCurrency];

            return converted;
        }
        public async Task<string> CreateOrderAsync(decimal amount, string currency)
        {
            if (currency == "KES")
            {
                currency = "USD";
                ChangeCurrency("USD", "KES", amount);
            }
            var baseUrl = _opts.Environment == "live" ? "https://api-m.paypal.com" : "https://api-m.sandbox.paypal.com";
            var token = await GetAccessTokenAsync();
            var req = new
            {
                intent = "CAPTURE",
                purchase_units = new[] {
                new { amount = new { currency_code = currency, value = amount.ToString("0.00") } }
            }
            };
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/v2/checkout/orders")
            {
                Content = JsonContent.Create(req)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resp = await _http.SendAsync(request);
            var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return json?["id"]?.ToString() ?? "";
        }

        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            var baseUrl = _opts.Environment == "live" ? "https://api-m.paypal.com" : "https://api-m.sandbox.paypal.com";
            var token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resp = await _http.SendAsync(request);
            return resp.IsSuccessStatusCode;
        }
    }
}
