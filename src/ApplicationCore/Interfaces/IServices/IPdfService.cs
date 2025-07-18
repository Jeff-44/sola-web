namespace ApplicationCore.Interfaces.IServices
{
    public interface IPdfService
    {
        byte[] ConvertHtmlToPdf(string html);
    }
}
