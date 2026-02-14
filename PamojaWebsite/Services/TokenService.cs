namespace PamojaWebsite.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config) => _config = config;

        public async Task<(string accessToken, string refreshToken)> ExchangeCodeForTokens(string code)
        {
            var values = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _config["GoogleOAuth:ClientId"] },
            { "client_secret", _config["GoogleOAuth:ClientSecret"] },
            { "redirect_uri", _config["GoogleOAuth:RedirectUri"] },
            { "grant_type", "authorization_code" }
        };

            using var client = new HttpClient();
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(values));
            var json = await response.Content.ReadAsStringAsync();

            var doc = System.Text.Json.JsonDocument.Parse(json);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString();
            var refreshToken = doc.RootElement.TryGetProperty("refresh_token", out var rt) ? rt.GetString() : null;

            return (accessToken, refreshToken);
        }

    }
}
