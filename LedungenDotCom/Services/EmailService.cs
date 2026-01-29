using LedungenDotCom.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using System.Threading.Tasks;

namespace LedungenDotCom.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "no-reply@ledungen.com";
        private readonly string smtpPass = "yourpassword";

        public async Task SendEmailAsync(string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(smtpUser));
            email.To.Add(MailboxAddress.Parse("test"));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpUser, smtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
    public class GmailEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<GmailEmailService> _logger;
        public GmailEmailService(IOptions<EmailSettings> settings, ILogger<GmailEmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }
        // Send email using Gmail SMTP, Password is stored locally. 
        public async Task SendEmailAsync(string subject, string body)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_settings.From));
            mail.To.Add(MailboxAddress.Parse(_settings.From));
            mail.ReplyTo.Add(MailboxAddress.Parse(_settings.From));
            mail.Subject = $"New Contact from Ledungen.com: {subject}";
            mail.Body = new TextPart("plain") { Text = body };

            _logger.LogInformation(
                "Host: {Host}, Port: {Port}, From: {From}, Username: {Username}, Password: {Password}",
                _settings.Host, _settings.Port, _settings.From, _settings.Username, _settings.Password
            );
            
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password); // <— uses secret here
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
            
        }
    }
    public class LiveEmailService : IEmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "no-reply@ledungen.com";
        private readonly string smtpPass = "yourpassword";

        public async Task SendEmailAsync(string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(smtpUser));
            email.To.Add(MailboxAddress.Parse("test"));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpUser, smtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
