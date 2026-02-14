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
    public class EmailService(ILogger<EmailSender> logger, IConfiguration configuration, IStringLocalizer<SharedResource> Loc)
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

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
    }
}
