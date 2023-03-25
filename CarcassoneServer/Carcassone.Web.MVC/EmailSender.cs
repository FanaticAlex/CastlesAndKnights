using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Carcassone.Server.Services
{
    class EmailSettings
    {
        public string Pass { get; set; }
        public string Login { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }

    public class EmailSender : IEmailSender
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

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            await SendByOldSmtpClient(_emailSettings, message, subject, toEmail);
        }

        private async Task SendByOldSmtpClient(EmailSettings settings, string body, string subject, string to)
        {
            MailMessage message = new();
            message.From = new MailAddress(settings.Login);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
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
