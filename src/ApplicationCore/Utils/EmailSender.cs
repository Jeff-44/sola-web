using ApplicationCore.Settings;
using ApplicationCore.Interfaces.IServices;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ApplicationCore.Utils
{
    public class EmailSender : IEmailSender, IEmailAttachmentSender
    {
        //public Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    throw new NotImplementedException();
        //}

        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> options, ILogger<EmailSender> logger)
        {
            _emailSettings = options.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Sola", _emailSettings.FromEmail));
            emailMessage.ReplyTo.Add(new MailboxAddress("", email));
            emailMessage.To.Add(new MailboxAddress("", _emailSettings.FromEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = $"From: {email} " + " " + " " + htmlMessage };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentData, string attachmentName, string mimeType = "application/pdf")
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Sola", _emailSettings.FromEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;

            var builder = new BodyBuilder { TextBody = message };
            var typeParts = mimeType.Split('/');
            var contentType = new ContentType(typeParts[0], typeParts.Length > 1 ? typeParts[1] : string.Empty);
            builder.Attachments.Add(attachmentName, attachmentData, contentType);
            mimeMessage.Body = builder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
