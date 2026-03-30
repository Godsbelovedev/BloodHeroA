using BloodHeroA.Application.Services.Interfaces;
using BloodHeroA.Models.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BloodHeroA.Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }
        public async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart("html") { Text = htmlBody };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
