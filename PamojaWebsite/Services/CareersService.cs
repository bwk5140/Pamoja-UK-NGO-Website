using Microsoft.JSInterop;
using PamojaWebsite.Data;
using Stripe;

namespace PamojaWebsite.Services
{
    public class CareersService
    {
        private readonly Lazy<Task<List<CareerRole>>> CareerRoles;
        private readonly Lazy<Task<List<CareerField>>> CareerFields;

        private readonly MyApiService _apiService;
        public event Action? OnStateChanged;

        public CareersService(MyApiService apiService) 
        {
            _apiService = apiService;
            CareerRoles = new Lazy<Task<List<CareerRole>>>(async () => await _apiService.GetHttpClient().GetFromJsonAsync<List<CareerRole>>("api/Db/career-roles"));
            CareerFields = new Lazy<Task<List<CareerField>>>(async () => await _apiService.GetHttpClient().GetFromJsonAsync<List<CareerField>>("api/Db/career-fields"));
            NotifyStateChanged();
        }
        public async Task<List<CareerRole>> GetCareerRolesAsync()
        {
            return await CareerRoles.Value;
        }
        public async Task<List<CareerField>> GetCareerFieldsAsync()
        {
            return await CareerFields.Value;
        }
        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}
