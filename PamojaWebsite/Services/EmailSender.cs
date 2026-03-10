using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using MimeKit;
using PamojaWebsite.Data;
using PamojaWebsite.Localization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace PamojaWebsite.Services
{
    public class EmailSender(ILogger<EmailSender> logger, IConfiguration configuration, IStringLocalizer<SharedResource> Loc) : IEmailSender<ApplicationUser>
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        => SendLinkEmailAsync(email, "Confirm your email by ", confirmationLink);

        Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        => SendPasswordResetCodeEmailAsync(email, "pamojamentalhealth@pamojasafeguarding.com", "Reset your password", resetCode);

        Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        => SendPasswordResetEmailAsync(email, "pamojamentalhealth@pamojasafeguarding.com", "Reset your password",
               resetLink);


        public async Task SendLinkEmailAsync(string toEmail, string subject, string message)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            byte[] logo_image_Bytes = File.ReadAllBytes("wwwroot/Images/PamojaLogo.png");
            string base64Image = Convert.ToBase64String(logo_image_Bytes);
            string logoImageSrc = $"data:image/png;base64,{base64Image}";

            byte[] second_logo_image_Bytes = File.ReadAllBytes("wwwroot/Images/PamojaLogo.png");
            string base64Image2 = Convert.ToBase64String(second_logo_image_Bytes);
            string secondLogoImageSrc = $"data:image/png;base64,{base64Image2}";

            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <style>
                    @media only screen and (max-width: 600px) {{
                        .responsive-table {{
                            width: 100% !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table class=""responsive-table"" width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" 
                                   style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd; text-align:center;"">
                                        <p style=""margin:20px 0 10px; font-size:24px; font-weight:bold; color:#333;"">
                                            Welcome to Pamoja Counselling, Mental Wellbeing and Safeguarding Services
                                        </p>
                                        <p class=""ms-3"" style=""margin:20px 0 10px; padding-bottom: 40px; font-size:16px; color:#555;"">
                                            Please confirm your account by <a style=""text-decoration: none;"" href=""{0}"">clicking here</a>.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {1}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);

            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", "pamojamentalhealth@pamojasafeguarding.com"));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = builder.HtmlBody };

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
        public async Task SendWelcomeEmailAsync(string toEmail, string subject, string message)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
                        <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <style>
                    @media only screen and (max-width: 600px) {{
                        .responsive-table {{
                            width:100% !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table class=""responsive-table"" width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" 
                                   style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd; text-align:center;"">
                                        <p style=""margin:20px 0 10px; font-size:24px; font-weight:bold; color:#333;"">
                                            Welcome to Pamoja Counselling, Mental Wellbeing and Safeguarding Services
                                        </p>
                                        <p class=""ms-3"" style=""margin:20px 0 10px; padding-bottom: 40px; font-size:16px; color:#555;"">
                                            Please confirm your account by <a style=""text-decoration: none;"" href=""{0}"">clicking here</a>.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {1}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", "pamojamentalhealth@pamojasafeguarding.com"));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = builder.HtmlBody };

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
        public async Task SendPasswordResetEmailAsync(string toEmail, string fromEmail, string subject, string message)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
                        <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <style>
                    @media only screen and (max-width: 600px) {{
                        .responsive-table {{
                            width: 100% !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table class=""responsive-table"" width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" 
                                   style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd; text-align:center;"">
                                        <p class=""ms-3"" style=""margin:20px 0 10px; padding-bottom: 40px; font-size:16px; color:#555;"">
                                            Looks like you're having trouble accessing your account. Please reset your password by  
                                            <a style=""text-decoration: none;"" href=""{0}"">
                                                clicking here
                                            </a>.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {1}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", fromEmail));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = builder.HtmlBody };

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
                await client.AuthenticateAsync(fromEmail, Password);
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
                await client.AuthenticateAsync(fromEmail, Password);
            }
            await client.SendAsync(Message);
            await client.DisconnectAsync(true);

        }
        public async Task SendPasswordResetEmailAsync(string toEmail, string subject, string message)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
                        <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <style>
                    @media only screen and (max-width: 600px) {{
                        .responsive-table {{
                            width:100% !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table class=""responsive-table"" width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" 
                                   style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd; text-align:center;"">
                                        <p class=""ms-3"" style=""margin:20px 0 10px; padding-bottom: 40px; font-size:16px; color:#555;"">
                                            Looks like you're having trouble accessing your account. Please reset your password by  
                                            <a style=""text-decoration: none;"" href=""{0}"">
                                                clicking here
                                            </a>.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {1}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", "pamojamentalhealth@pamojasafeguarding.com"));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = builder.HtmlBody };

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
        public async Task SendWelcomeEmailAsync(string toEmail, string fromEmail, string subject, string message)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
                        <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <style>
                    @media only screen and (max-width: 600px) {{
                        .responsive-table {{
                            width: 100% !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table class=""responsive-table"" width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" 
                                   style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd; text-align:center;"">
                                        <p style=""margin:20px 0 10px; font-size:24px; font-weight:bold; color:#333;"">
                                            Welcome to Pamoja Counselling, Mental Wellbeing and Safeguarding Services
                                        </p>
                                        <p class=""ms-3"" style=""margin:20px 0 10px; padding-bottom: 40px; font-size:16px; color:#555;"">
                                            Please confirm your account by <a style=""text-decoration: none;"" href=""{0}"">clicking here</a>.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {1}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", fromEmail));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = builder.HtmlBody };

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
                await client.AuthenticateAsync(fromEmail, Password);
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
                await client.AuthenticateAsync(fromEmail, Password);
            }
            await client.SendAsync(Message);
            await client.DisconnectAsync(true);

        }
        public async Task SendPasswordResetCodeEmailAsync(string toEmail, string fromEmail, string subject, string message)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format(@"
                        <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <style>
                    @media only screen and (max-width: 600px) {{
                        .responsive-table {{
                            width: 100% !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f4f4;"">
                    <tr>
                        <td align=""center"" valign=""top"">
                            <table class=""responsive-table"" width=""50%"" cellpadding=""0"" cellspacing=""0"" border=""0"" 
                                   style=""background-color:#ffffff; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1); font-family:Arial, sans-serif;"">
                                <tr>
                                    <td style=""padding:40px; text-align:center;"">
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             alt=""Pamoja Logo"" width=""100"" height=""120"" style=""display:block; margin:0 auto;""/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; border-bottom:1px solid #ddd; text-align:center;"">
                                        <p class=""ms-3"" style=""margin:20px 0 10px; padding-bottom: 40px; font-size:16px; color:#555;"">
                                            Looks like you're having trouble accessing your account. Please reset your password by using 
                                            the following code: {0}
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:20px; text-align:center; font-size:12px; color:#555; background-color:#f0f0f0;"">
                                        &copy;2025 pamojasafeguardingnetwork.co.uk<br/>
                                        {1}<br/>
                                        <img src=""https://res.cloudinary.com/dzmfpxcwu/image/upload/v1763748093/PamojaLogo_iu9ryg.png"" 
                                             width=""90"" height=""120"" alt=""Pamoja Logo"" style=""margin-top:10px;""/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>",
            message,
            Loc["PamojaNetworkAndAllRelatedMarks"]);
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Pamoja Counselling, Mental Wellbeing & Safeguarding Services", fromEmail));
            Message.To.Add(new MailboxAddress("", toEmail));
            Message.Subject = subject;
            Message.Body = new TextPart("html") { Text = builder.HtmlBody };

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
                await client.AuthenticateAsync(fromEmail, Password);
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
                await client.AuthenticateAsync(fromEmail, Password);
            }
            await client.SendAsync(Message);
            await client.DisconnectAsync(true);

        }

        Task IEmailSender<ApplicationUser>.SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            return SendConfirmationLinkAsync(user, email, confirmationLink);
        }

        Task IEmailSender<ApplicationUser>.SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            return SendPasswordResetLinkAsync(user, email, resetLink);
        }

        Task IEmailSender<ApplicationUser>.SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            return SendPasswordResetCodeAsync(user, email, resetCode);
        }
    }
}
