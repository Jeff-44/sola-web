using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.IServices
{
    public interface IEmailAttachmentSender
    {
        Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentData, string attachmentName, string mimeType = "application/pdf");
    }
}
