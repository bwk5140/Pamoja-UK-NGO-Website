using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PamojaWebsite.Data;
using PamojaWebsite.Data.Contexts;
using System.Text.RegularExpressions;

namespace PamojaWebsite.Services
{
    public class MyApiService
    {
        private HttpClient _httpClient;

        public MyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:44343/");
            _httpClient.BaseAddress = new Uri("https://www.pamojasafeguardingnetwork.co.uk/");
        }
        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
        private string GetShortExtension(string contentType)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "doc" },
            { "application/msword", "doc" },
            { "application/pdf", "pdf" },
            { "text/plain", "txt" },
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xls" },
            { "image/png", "png" }
        };

            return mapping.TryGetValue(contentType, out var extension) ? extension : "unknown";
        }
    }
}
