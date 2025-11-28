using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using PamojaWebsite.Components;
using PamojaWebsite.Components.Account;
using PamojaWebsite.Data;
using PamojaWebsite.Data.Contexts;
using PamojaWebsite.Services;
using System.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddSingleton<EmailEncryptor>();
builder.Services.AddScoped<ApplicationUserService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
//options.UseSqlServer(connectionString));
options.UseMySql(
    connectionString,
    new MySqlServerVersion(new Version(11, 8, 3)) // adjust to your MariaDB version
));


builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddAuthentication()
   .AddCookie()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection =
       builder.Configuration.GetSection("Authentication:Google");
       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
       options.SignInScheme = IdentityConstants.ExternalScheme;
       options.AdditionalAuthorizationParameters.Add("prompt", "select_account");
   })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        IConfigurationSection microsoftAuthNSection =
        builder.Configuration.GetSection("Authentication:Microsoft");
        microsoftOptions.ClientId = microsoftAuthNSection["ClientId"];
        microsoftOptions.ClientSecret = microsoftAuthNSection["ClientSecret"];
        microsoftOptions.CallbackPath = "/signin-oidc";
    });

builder.Services.AddLocalization();

builder.Services.Configure<CircuitOptions>(
    builder.Configuration.GetSection("CircuitOptions"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();

var supportedCultures = new[] { "en-KE", "en-US", "en-GB", "es-US", "es-ES", "fr-FR", "fr-CA", "ar-SA", "zh-Hant", "de-DE", "ja-JP", "it-IT", "sw-KE" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-KE"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
