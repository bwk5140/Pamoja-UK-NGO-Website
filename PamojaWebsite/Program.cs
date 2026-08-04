using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Blazor.Analytics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using PamojaWebsite.Components;
using PamojaWebsite.Components.Account;
using PamojaWebsite.Data;
using PamojaWebsite.Data.Contexts;
using PamojaWebsite.Services;
using Stripe;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);
bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
SecretClient client = null;


if (isLinux)
{
    builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/var/www/PamojaWebsite/keys"))
    .SetApplicationName("PamojaWebsite");


    var credential = new ClientCertificateCredential(
    tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
    clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
    new X509Certificate2("/etc/ssl/pamoja/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));
    client = new SecretClient(
    new Uri("https://pamojakeyvault.vault.azure.net/"),
    credential);
    KeyVaultSecret connectionstring_secret = client.GetSecret("Remote-DefaultConnection");

    var connectionString = connectionstring_secret.Value ?? throw new InvalidOperationException("Connection string 'Remote-DefaultConnection' not found.");
    builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(
            connectionString,
            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
        );
    });
}
if (isWindows)
{
    builder.Configuration.AddAzureKeyVault(
    new Uri("https://pamojakeyvault.vault.azure.net/"),
    new DefaultAzureCredential());
    var credential = new ClientCertificateCredential(
    tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
    clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
    new X509Certificate2("/users/brian/Desktop/apps/PamojaWebsiteUK/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));

    if (credential is not null)
    {
        client = new SecretClient(
        new Uri("https://pamojakeyvault.vault.azure.net/"),
        credential);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    }
}

if (client is not null)
{
    KeyVaultSecret auth_google_client_id_secret = client.GetSecret("Authentication-Google-ClientId");
    KeyVaultSecret auth_google_client_secret_secret = client.GetSecret("Authentication-Google-Secret");
    KeyVaultSecret auth_microsoft_client_id_secret = client.GetSecret("Authentication-Microsoft-ClientId");
    KeyVaultSecret auth_microsoft_client_secret_secret = client.GetSecret("Authentication-Microsoft-Secret");
    KeyVaultSecret auth_microsoft_tenant_id_secret = client.GetSecret("Authentication-Microsoft-TenantId");
    KeyVaultSecret auth_microsoft_secret_id_secret = client.GetSecret("Authentication-Microsoft-SecretId");
    KeyVaultSecret auth_stripe_secret_id_secret = client.GetSecret("Authentication-Stripe-SecretId");
    KeyVaultSecret auth_stripe_publishable_key = client.GetSecret("Authentication-Stripe-PublishableKey");
    KeyVaultSecret auth_paypal_client_id = client.GetSecret("Authentication-PayPal-ClientId");
    KeyVaultSecret auth_paypal_client_secret = client.GetSecret("Authentication-PayPal-ClientSecret");
    KeyVaultSecret auth_paypal_environment = client.GetSecret("Authentication-PayPal-Environment");
    KeyVaultSecret auth_mpesa_consumer_key = client.GetSecret("Authentication-Mpesa-ConsumerKey");
    KeyVaultSecret auth_mpesa_consumer_secret = client.GetSecret("Authentication-Mpesa-ConsumerSecret");
    KeyVaultSecret auth_mpesa_shortcode = client.GetSecret("Authentication-Mpesa-Shortcode");
    KeyVaultSecret auth_mpesa_passkey = client.GetSecret("Authentication-Mpesa-Passkey");
    KeyVaultSecret auth_mpesa_callbackurl = client.GetSecret("Authentication-Mpesa-CallbackUrl");

    if (auth_stripe_secret_id_secret is not null && auth_stripe_publishable_key is not null)
    {
        builder.Services.AddSingleton(new StripeOptions
        {
            SecretKey = auth_stripe_secret_id_secret.Value,
            PublishableKey = auth_stripe_publishable_key.Value
        });
    }
    if (auth_paypal_client_id is not null && auth_paypal_client_secret is not null && auth_paypal_environment is not null)
    {
        builder.Services.AddSingleton(new PayPalOptions
        {
            ClientId = auth_paypal_client_id.Value,
            ClientSecret = auth_paypal_client_secret.Value,
            Environment = auth_paypal_environment.Value
        });
    }
    if (auth_mpesa_consumer_key is not null && auth_mpesa_consumer_secret is not null 
        && auth_mpesa_shortcode is not null && auth_mpesa_passkey is not null
        && auth_mpesa_callbackurl is not null)
    {
        builder.Services.AddSingleton(new MpesaOptions
        {
            ConsumerKey = auth_mpesa_consumer_key.Value,
            ConsumerSecret = auth_mpesa_consumer_secret.Value,
            ShortCode = auth_mpesa_shortcode.Value,
            Passkey = auth_mpesa_passkey.Value,
            CallbackUrl = auth_mpesa_callbackurl.Value
        });
    }
    if (auth_google_client_id_secret is not null && auth_google_client_secret_secret is not null)
    {
        builder.Services.AddAuthentication()
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = auth_google_client_id_secret.Value;
            options.ClientSecret = auth_google_client_secret_secret.Value;
            options.SignInScheme = IdentityConstants.ExternalScheme;
            options.AdditionalAuthorizationParameters.Add("prompt", "select_account");
        });
    }
    if (auth_microsoft_client_id_secret is not null && auth_microsoft_client_secret_secret is not null)
    {
        builder.Services.AddAuthentication()
        .AddMicrosoftAccount(microsoftOptions =>
        {
            microsoftOptions.ClientId = auth_microsoft_client_id_secret.Value;
            microsoftOptions.ClientSecret = auth_microsoft_client_secret_secret.Value;
            microsoftOptions.CallbackPath = "/signin-oidc";
        });
    }
}

// Configuration binding
builder.Services.AddScoped<CountryCodeService>();
builder.Services.AddScoped<Radzen.DialogService>();
builder.Services.AddScoped<Radzen.NotificationService>();
builder.Services.AddScoped<Radzen.TooltipService>();
builder.Services.AddScoped<Radzen.ContextMenuService>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, EmailSender>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<CookiesService>();
builder.Services.AddScoped<BookAppointmentService>();

// Add Google Analytics with your Measurement ID
builder.Services.AddGoogleAnalytics("G-XXXXXXXXXX"); // replace with your GA ID

builder.Services.AddControllers();

//Services
builder.Services.AddScoped<StripePaymentService>();
builder.Services.AddScoped<CareersService>();
builder.Services.AddScoped<ResourcesService>();
builder.Services.AddHttpClient<PayPalService>();
builder.Services.AddHttpClient<MpesaService>();
builder.Services.AddHttpClient<MyApiService>();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // optional, for AJAX
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMemoryCache();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddSingleton<EmailEncryptor>();
builder.Services.AddSingleton<ApplicationUserService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddLocalization();

builder.Services.Configure<CircuitOptions>(
    builder.Configuration.GetSection("CircuitOptions"));

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Status307TemporaryRedirect;
    options.HttpsPort = 5001;
});

var app = builder.Build();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

var supportedCultures = new[] { "en-KE", "en-US", "en-GB", "es-US", "es-ES", "fr-FR", "fr-CA", "ar-SA", "zh-Hant", "de-DE", "ja-JP", "it-IT", "sw-KE" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-KE"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

app.UseStaticFiles();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});

app.UseRouting();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

// Options classes
public class StripeOptions { public string SecretKey { get; set; } = ""; public string PublishableKey { get; set; } = ""; }
public class PayPalOptions
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string Environment { get; set; } = "sandbox";
}

public class MpesaOptions
{
    public string ConsumerKey { get; set; } = "";
    public string ConsumerSecret { get; set; } = "";
    public string ShortCode { get; set; } = "";
    public string Passkey { get; set; } = "";
    public string CallbackUrl { get; set; } = "";
}

