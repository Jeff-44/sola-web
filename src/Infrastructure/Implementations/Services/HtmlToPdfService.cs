using ApplicationCore.Interfaces.IServices;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Infrastructure.Services
{
    public class HtmlToPdfService : IPdfService
    {
        private readonly IConverter _converter;

        public HtmlToPdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] ConvertHtmlToPdf(string html)
        {
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = html
                    }
                }
            };

            return _converter.Convert(doc);
        }
    }
}
