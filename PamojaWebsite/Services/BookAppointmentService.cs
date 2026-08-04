using PamojaWebsite.Data;
using Stripe;

namespace PamojaWebsite.Services
{
    public class BookAppointmentService
    {
        private readonly Lazy<Task<List<CountryCode>>> CountryCodes;

        private readonly MyApiService _apiService;
        public event Action? OnStateChanged;
        public BookAppointmentService(MyApiService apiService) 
        {
            _apiService = apiService;
            CountryCodes = new Lazy<Task<List<CountryCode>>>(async () => await (_apiService.GetHttpClient().GetFromJsonAsync<List<CountryCode>>("api/Db/country-codes")));
            NotifyStateChanged();
        }
        public async Task<List<CountryCode>> GetCountryCodesAsync()
        {
            return await CountryCodes.Value;
        }
        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}
