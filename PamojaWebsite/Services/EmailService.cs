using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using MimeKit;
using PamojaWebsite.Data;
using PamojaWebsite.Localization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace PamojaWebsite.Services
{
    public class EmailService(ILogger<EmailSender> logger, IConfiguration configuration, IStringLocalizer<SharedResource> Loc, ApplicationUserService applicationUserService, IMemoryCache Cache)
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        Dictionary<string, string> CurrencySymbols = new()
            {
                { "USD", "$" },
                { "EUR", "€" },
                { "GBP", "£" },
                { "JPY", "¥" },
                { "CAD", "C$" },
                { "AUD", "A$" },
                { "KES", "KSh" }
            };
        public async Task SendLinkEmailAsync(string toEmail, string subject, string message)
        {
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", "pamojamentalhealth@pamojasafeguarding.com"));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

            if (isLinux)
            {
                var credential = new ClientCertificateCredential(
                tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
                clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
                new X509Certificate2("/etc/ssl/pamoja/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));
                var KeyVaultClient = new SecretClient(
                new Uri("https://pamojakeyvault.vault.azure.net/"),
                credential);
                KeyVaultSecret gmailPassword = KeyVaultClient.GetSecret("Gmail-Server-Password");
                var Password = gmailPassword.Value ?? throw new InvalidOperationException("'Gmail-Server-Password' not found.");
                await client.AuthenticateAsync("pamojamentalhealth@pamojasafeguarding.com", Password);
            }
            if (isWindows)
            {
                var credential = new ClientCertificateCredential(
                tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
                clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
                new X509Certificate2("/users/brian/Desktop/apps/PamojaWebsiteUK/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));

                var KeyVaultClient = new SecretClient(
                new Uri("https://pamojakeyvault.vault.azure.net/"),
                credential);
                KeyVaultSecret gmailPassword = KeyVaultClient.GetSecret("Gmail-Server-Password");

                var Password = gmailPassword.Value ?? throw new InvalidOperationException("'Gmail-Server-Password' not found.");
                await client.AuthenticateAsync("pamojamentalhealth@pamojasafeguarding.com", Password);
            }
            await client.SendAsync(Message);
            await client.DisconnectAsync(true);
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", "pamojamentalhealth@pamojasafeguarding.com"));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

            if (isLinux)
            {
                var credential = new ClientCertificateCredential(
                tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
                clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
                new X509Certificate2("/etc/ssl/pamoja/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));
                var KeyVaultClient = new SecretClient(
                new Uri("https://pamojakeyvault.vault.azure.net/"),
                credential);
                KeyVaultSecret gmailPassword = KeyVaultClient.GetSecret("Gmail-Server-Password");
                var Password = gmailPassword.Value ?? throw new InvalidOperationException("'Gmail-Server-Password' not found.");
                await client.AuthenticateAsync("pamojamentalhealth@pamojasafeguarding.com", Password);
            }
            if (isWindows)
            {
                var credential = new ClientCertificateCredential(
                tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
                clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
                new X509Certificate2("/users/brian/Desktop/apps/PamojaWebsiteUK/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));

                var KeyVaultClient = new SecretClient(
                new Uri("https://pamojakeyvault.vault.azure.net/"),
                credential);
                KeyVaultSecret gmailPassword = KeyVaultClient.GetSecret("Gmail-Server-Password");

                var Password = gmailPassword.Value ?? throw new InvalidOperationException("'Gmail-Server-Password' not found.");
                await client.AuthenticateAsync("pamojamentalhealth@pamojasafeguarding.com", Password);
            }
            await client.SendAsync(Message);
            await client.DisconnectAsync(true);
        }
        public async Task SendEmailAsync(string toEmail, string fromEmail, string subject, string message)
        {
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", fromEmail));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

            if (isLinux)
            {
                var credential = new ClientCertificateCredential(
                tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
                clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
                new X509Certificate2("/etc/ssl/pamoja/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));
                var KeyVaultClient = new SecretClient(
                new Uri("https://pamojakeyvault.vault.azure.net/"),
                credential);
                KeyVaultSecret gmailPassword = KeyVaultClient.GetSecret("Gmail-Server-Password");
                var Password = gmailPassword.Value ?? throw new InvalidOperationException("'Gmail-Server-Password' not found.");
                await client.AuthenticateAsync("pamojamentalhealth@pamojasafeguarding.com", Password);
            }
            if (isWindows)
            {
                var credential = new ClientCertificateCredential(
                tenantId: "cbe773e0-1982-4f06-a69a-8fa5df2e58ba",
                clientId: "69f0d61e-7599-44e1-b87c-9d7b4a27f14d",
                new X509Certificate2("/users/brian/Desktop/apps/PamojaWebsiteUK/PamojaWebsite.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD")));

                var KeyVaultClient = new SecretClient(
                new Uri("https://pamojakeyvault.vault.azure.net/"),
                credential);
                KeyVaultSecret gmailPassword = KeyVaultClient.GetSecret("Gmail-Server-Password");

                var Password = gmailPassword.Value ?? throw new InvalidOperationException("'Gmail-Server-Password' not found.");
                await client.AuthenticateAsync("pamojamentalhealth@pamojasafeguarding.com", Password);
            }
            await client.SendAsync(Message);
            await client.DisconnectAsync(true);

        }
        public async Task SendClientPaymentEmailAsync(Payment payment)
        {
            var user = await applicationUserService.GetApplicationUser();
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                        <h1 style=""margin:20px 0 10px; font-size:24px; font-weight:bold; color:#333;"">Payment Invoice</h1>
                                        <p style=""margin:0; font-size:16px; color:#555;"">Thank you!</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd;"">
                                        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                            <tr>
                                                <td style=""text-align:left; font-size:14px; color:#333;"">
                                                    <h2 style=""margin:0 0 10px; font-size:16px; font-weight:bold;"">
                                                        Pamoja Counselling, Mental Wellbeing and Safeguarding Services
                                                    </h2>
                                                    <p style=""margin:0; font-size:12px; color:#555;"">
                                                        Unit 3, Office A, 1st Floor, 6-7 St Mary At Hill<br/>
                                                        London, EC3R 8EE, England
                                                    </p>
                                                </td>
                                                <td style=""text-align:right; font-size:12px; color:#555;"">
                                                    Invoice <span id=""invoice-number"">{0}</span><br/>
                                                    <span id=""invoice-date"">{1}</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px;"">
                                        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border-collapse:collapse;"">
                                            <thead>
                                                <tr style=""background-color:#f9f9f9;"">
                                                    <th align=""left"" style=""padding:8px; font-size:12px; color:#555; text-transform:uppercase;"">Description</th>
                                                    <th align=""left"" style=""padding:8px; font-size:12px; color:#555; text-transform:uppercase;"">Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td style=""padding:8px; font-size:14px; color:#333;"">Appointment Payment</td>
                                                    <td style=""padding:8px; font-size:14px; color:#555;"" id=""donation-amount"">{2}{3}</td>
                                                </tr>
                                                <tr>
                                                    <td style=""padding:8px; font-size:14px; color:#333;"">Processing Fee</td>
                                                    <td style=""padding:8px; font-size:14px; color:#555;"" id=""processing-fee"">{2}{4}</td>
                                                </tr>
                                                <tr style=""background-color:#f9f9f9;"">
                                                    <td style=""padding:8px; font-size:14px; font-weight:bold; color:#333;"">Total</td>
                                                    <td style=""padding:8px; font-size:14px; color:#555;"" id=""total-amount"">{2}{5}</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; background-color:#e6f4ea; border-radius:6px;"">
                                        <h3 style=""margin:0 0 10px; font-size:14px; font-weight:bold; color:#2f855a;"">Your payment is tax deductible</h3>
                                        <p style=""margin:0; font-size:12px; color:#276749;"">
                                            Pamoja Counselling, Mental Wellbeing and Safeguarding Services is a registered 501(c)(3) nonprofit organization. Your donation is tax-deductible to the extent allowed by law.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {6}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            payment?.InvoiceNumber,
            DateOnly.FromDateTime(DateTime.UtcNow).ToLongDateString(),
            CurrencySymbols[payment?.Currency],
            payment.Amount,
            payment.ProcessingFee,
            payment.Total,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            Cache.TryGetValue("ClientPaymentEmailSent", out var emailSent);
            if (emailSent is null)
            {
                await SendEmailAsync(payment.Email, "Payment Invoice", builder.HtmlBody);
                var EmailSent = await Cache.GetOrCreateAsync("ClientPaymentEmailSent", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    var emailSent = true;
                    return emailSent;
                });
            }
        }
        public async Task SendClientAppointmentEmailAsync(Appointment appointment)
        {
            var user = await applicationUserService.GetApplicationUser();
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:20px; background-color:#ffffff; border-radius:6px;"">
                                        <!-- Appointment Confirmation Block -->
                                        <div style=""max-width:100%; margin:0 auto; padding:16px;"">
                                            <div style=""max-width:100%; margin:0 auto; background-color:#ffffff; border-radius:12px; box-shadow:0 4px 6px rgba(0,0,0,0.1);"">
                                                <div style=""padding:8px 16px; border-bottom:1px solid #ddd;"">
                                                    <strong>Appointment Confirmation</strong>
                                                </div>
                                                <div style=""padding:16px;"">
                                                    <div style=""margin:16px 0; max-width:100%; border:2px solid #16a34a; border-left:8px solid #16a34a; background-color:#ffffff; border-radius:8px;"">
                                                        <div style=""padding:8px; display:flex; flex-direction:column; align-items:center; gap:24px;"">
                                                            <div style=""background-color:#ffffff; padding:24px; border-radius:8px; box-shadow:0 2px 4px rgba(0,0,0,0.1); width:90%;"">
                                                                <h1 style=""font-size:18px; font-weight:bold; text-align:center; color:#1f2937; margin:16px 0;"">Appointment Details</h1>
                                                                <div style=""background-color:#ffffff; padding:24px; border-radius:8px; box-shadow:0 2px 4px rgba(0,0,0,0.1);"">
                                                                    <div style=""display:flex; flex-direction:row; justify-content:space-between; gap:16px;"">
                                                                        <div style=""flex:1; display:flex; flex-direction:row; gap:16px;"">
                                                                            <div style=""margin:12px 0; padding-right: 12px;"">
                                                                                <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                                                            </div>
                                                                            <div style=""margin:12px 0;"">
                                                                                <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:16px;"">
                                                                                    Consultation with Pamoja Mental Health and Safeguarding Team
                                                                                </p>
                                                                                <p style=""font-size:16px; color:#4b5563; margin-bottom:16px;"">20 minutes</p>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {0} at {1}
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        Description of needs
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:normal; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {2}
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- End Appointment Block -->
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {3}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            DateOnly.FromDateTime(appointment.Date.ToLocalTime()).ToLongDateString(),
            appointment.Time.ToString("hh:mm tt"),
            appointment.Message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            Cache.TryGetValue("ClientAppointmentEmailSent", out var emailSent);
            if (emailSent is null)
            {
                await SendEmailAsync(appointment.Email, "Appointment Confirmation", builder.HtmlBody);
                var EmailSent = await Cache.GetOrCreateAsync("ClientAppointmentEmailSent", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    var emailSent = true;
                    return emailSent;
                });
            }
        }
        public async Task SendManagementEmailAsync(Appointment appointment)
        {
            var user = await applicationUserService.GetApplicationUser();
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:20px; background-color:#ffffff; border-radius:6px;"">
                                        <!-- Appointment Confirmation Block -->
                                        <div style=""max-width:100%; margin:0 auto; padding:16px;"">
                                            <div style=""max-width:100%; margin:0 auto; background-color:#ffffff; border-radius:12px; box-shadow:0 4px 6px rgba(0,0,0,0.1);"">
                                                <div style=""padding:8px 16px; border-bottom:1px solid #ddd;"">
                                                    <strong>Appointment Confirmation</strong>
                                                </div>
                                                <div style=""padding:16px;"">
                                                    <div style=""margin:16px 0; max-width:100%; border:2px solid #16a34a; border-left:8px solid #16a34a; background-color:#ffffff; border-radius:8px;"">
                                                        <div style=""padding:8px; display:flex; flex-direction:column; align-items:center; gap:24px;"">
                                                            <div style=""background-color:#ffffff; padding:24px; border-radius:8px; box-shadow:0 2px 4px rgba(0,0,0,0.1); width:90%;"">
                                                                <h1 style=""font-size:18px; font-weight:bold; text-align:center; color:#1f2937; margin:16px 0;"">Appointment Details</h1>
                                                                <div style=""background-color:#ffffff; padding:24px; border-radius:8px; box-shadow:0 2px 4px rgba(0,0,0,0.1);"">
                                                                    <div style=""display:flex; flex-direction:row; justify-content:space-between; gap:16px;"">
                                                                        <div style=""flex:1; display:flex; flex-direction:row; gap:16px;"">
                                                                            <div style=""margin:12px 0; padding-right: 12px;"">
                                                                                <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                                                            </div>
                                                                            <div style=""margin:12px 0;"">
                                                                                <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:16px;"">
                                                                                    Consultation with Pamoja Mental Health and Safeguarding Team
                                                                                </p>
                                                                                <p style=""font-size:16px; color:#4b5563; margin-bottom:16px;"">20 minutes</p>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {0} at {1}
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        Description of needs
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:normal; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {2}
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <h4 style=""font-weight:bold; font-size:18px; color:#1f2937; margin-bottom:12px;"">
                                                                                        Client Information
                                                                                    </h4>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        Name: 
                                                                                    </p>
                                                                                    <p style=""margin-left: 5px; font-weight:normal; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {3} {4}
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        Email: 
                                                                                    </p>
                                                                                    <p style=""margin-left: 5px; font-weight:normal; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {5}
                                                                                    </p>
                                                                                </div>
                                                                                <div style=""display:flex; flex-direction:column;"">
                                                                                    <p style=""font-weight:bold; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        Phone: 
                                                                                    </p>
                                                                                    <p style=""margin-left: 5px; font-weight:normal; font-size:16px; color:#1f2937; margin-bottom:12px;"">
                                                                                        {6}
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- End Appointment Block -->
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {7}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            DateOnly.FromDateTime(appointment.Date.ToLocalTime()).ToLongDateString(),
            appointment.Time.ToString("hh:mm tt"),
            appointment.Message,
            appointment.FirstName,
            appointment.LastName,
            appointment.Email,
            appointment.Phone,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            Cache.TryGetValue("ManagementEmailSent", out var emailSent);
            if (emailSent is null)
            {
                await SendEmailAsync("pamojamentalhealth@pamojasafeguarding.com", "Appointment Confirmation", builder.HtmlBody);
                var EmailSent = await Cache.GetOrCreateAsync("ManagementEmailSent", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    var emailSent = true;
                    return emailSent;
                });
            }
        }
    }
}
