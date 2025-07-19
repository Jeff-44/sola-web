using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Implementations.Services
{
    public class EmailAttachmentSender : IEmailAttachmentSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailAttachmentSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentData, string attachmentName, string mimeType = "application/pdf")
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.UseStartTls,
                Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password)
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            // Create and attach the PDF
            using var ms = new MemoryStream(attachmentData);
            var attachment = new Attachment(ms, attachmentName, mimeType);
            mailMessage.Attachments.Add(attachment);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow
                // You might want to add proper logging here
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}
