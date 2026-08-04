using Microsoft.JSInterop;

namespace PamojaWebsite.Services
{
    public class CookiesService
    {
        public string? functional_consent;
        public string? analytics_consent;
        public string? performance_consent;
        public string? advertisement_consent;

        public bool PolicyIsVisible = false;
        public bool IsCookiesVisible = false;
        public bool IsCookieButtonVisible = false;

        public bool FunctionalAccepted;
        public bool PerformanceAccepted;
        public bool AnalyticsAccepted;
        public bool AdvertisementAccepted;

        public event Action? OnStateChanged;
        public bool IsInitialized { get; private set; } = false;

        private readonly IJSRuntime _jsRuntime;

        public CookiesService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            try
            {
                functional_consent = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "functional");
                analytics_consent = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "analytics");
                performance_consent = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "performance");
                advertisement_consent = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "advertisement");

                if (string.IsNullOrEmpty(functional_consent) && string.IsNullOrEmpty(analytics_consent) && 
                    string.IsNullOrEmpty(performance_consent) && string.IsNullOrEmpty(advertisement_consent))
                {
                    IsCookiesVisible = true;
                }
                else
                {
                    FunctionalAccepted = bool.TryParse(functional_consent, out var parsed_consent) && parsed_consent;
                    PerformanceAccepted = bool.TryParse(performance_consent, out var parsed_performance) && parsed_performance;
                    AnalyticsAccepted = bool.TryParse(analytics_consent, out var parsed_analytics) && parsed_analytics;
                    AdvertisementAccepted = bool.TryParse(advertisement_consent, out var parsed_advertisement) && parsed_advertisement;
                    IsCookiesVisible = false;
                    IsCookieButtonVisible = true;
                }

                IsInitialized = true;
                NotifyStateChanged();
            }
            catch (JSException ex)
            {
                Console.WriteLine($"LocalStorage error: {ex.Message}");
            }
        }

        public async Task SetFunctionalConsent(bool consent)
        {
            await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "functional", consent);
            FunctionalAccepted = consent;
            functional_consent = consent.ToString();
            NotifyStateChanged();
        }

        public async Task SetAnalyticsConsent(bool consent)
        {
            await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "analytics", consent);
            AnalyticsAccepted = consent;
            analytics_consent = consent.ToString();
            NotifyStateChanged();
        }

        public async Task SetPerformanceConsent(bool consent)
        {
            await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "performance", consent);
            PerformanceAccepted = consent;
            performance_consent = consent.ToString();
            NotifyStateChanged();
        }

        public async Task SetAdvertisementConsent(bool consent)
        {
            await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "advertisement", consent);
            AdvertisementAccepted = consent;
            advertisement_consent = consent.ToString();
            NotifyStateChanged();
        }
        public void SetCookiePolicyBannerVisibility(bool visible)
        {
            if (PolicyIsVisible != visible)
            {
                PolicyIsVisible = visible;
                NotifyStateChanged();
            }
        }
        public void SetCookieBannerVisibility(bool visible)
        {
            if (IsCookiesVisible != visible)
            {
                IsCookiesVisible = visible;
                NotifyStateChanged();
            }
        }
        public void SetCookieButtonVisibility(bool visible)
        {
            if (IsCookieButtonVisible != visible)
            {
                IsCookieButtonVisible = visible;
                NotifyStateChanged();
            }
        }
        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}
