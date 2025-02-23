using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Carcassone.Web.Blazor.Data;
using Carcassone.Web.Blazor.Components.Account.Pages.Manage;

namespace Carcassone.Web.Blazor.Components.Account
{
    class EmailSettings
    {
        public string Pass { get; set; }
        public string Login { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }

    public class EmailSender : IEmailSender<CarcassoneUser>
    {
        private readonly ILogger _logger;
        private readonly EmailSettings _emailSettings;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _emailSettings = new EmailSettings()
            {
                Pass = "ylrgeacywshgbdms",
                Login = "Madyson8787@gmail.com",
                Server = "smtp.gmail.com",
                Port = 587
            };

            _logger = logger;
        }

        public Task SendConfirmationLinkAsync(CarcassoneUser user, string email, string confirmationLink)
        {
            return SendByOldSmtpClient(_emailSettings, confirmationLink, user, email);
        }

        public Task SendPasswordResetCodeAsync(CarcassoneUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(CarcassoneUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }

        private async Task SendByOldSmtpClient(EmailSettings settings, string body, CarcassoneUser user, string to)
        {
            MailMessage message = new();
            message.From = new MailAddress(settings.Login);
            message.To.Add(new MailAddress(to));
            message.Subject = "Подтверждение регистрации Carcassone";
            message.Body = body;
            message.IsBodyHtml = true;

            using SmtpClient client = new();
            client.Host = settings.Server;
            client.Port = settings.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(settings.Login, settings.Pass);
            await client.SendMailAsync(message);
        }
    }
}
