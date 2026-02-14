using Microsoft.AspNetCore.Mvc;
using PamojaWebsite.Data.Contexts;

namespace PamojaWebsite.Services
{
    public class MyApiService
    {
        private readonly HttpClient _httpClient;

        public MyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:44343/");
            _httpClient.BaseAddress = new Uri("https://www.pamojasafeguardingnetwork.co.uk/");
        }

        public async Task<HttpClient> GetClient()
        {
            return _httpClient;
        }
    }
}
