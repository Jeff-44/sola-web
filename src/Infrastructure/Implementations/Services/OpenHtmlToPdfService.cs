using ApplicationCore.Interfaces.IServices;
using Chilkat;
using OpenHtmlToPdf;

namespace Infrastructure.Services
{
    public class OpenHtmlToPdfService : IPdfService
    {
        public byte[] ConvertHtmlToPdf(string html) =>
            Pdf
                .From(html)
                .WithGlobalSetting("orientation", "Portrait")
                .OfSize(PaperSize.A4)
                .Content();
    }
}
