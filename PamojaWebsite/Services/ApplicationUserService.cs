using Microsoft.AspNetCore.Identity;
using PamojaWebsite.Data;
using System.Security.Claims;

namespace PamojaWebsite.Services
{
    public class ApplicationUserService
    {
        private ClaimsPrincipal? claimsPrincipal { get; set; }
        private ApplicationUser? ApplicationUser { get; set; }
        private readonly IServiceProvider? _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser>? UserManager { get; set; }

        public ApplicationUserService(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }
        private async Task InitializeUser()
        {
            using var scope = _serviceProvider.CreateScope();
            UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            claimsPrincipal = GetUser();
            if (claimsPrincipal is not null)
            {
                ApplicationUser = await UserManager.GetUserAsync(claimsPrincipal);
            }
            else
            {
                ApplicationUser = null;
            }
        }
        private ClaimsPrincipal GetUser()
        {
            return _httpContextAccessor.HttpContext?.User;
        }
        public async Task<ApplicationUser> GetApplicationUser()
        {
            await InitializeUser();
            return ApplicationUser;
        }
        public void SignOut()
        {
            ApplicationUser = null;
        }
        public void UpdateUser(ApplicationUser user)
        {
            this.ApplicationUser = user;
        }
    }
}
