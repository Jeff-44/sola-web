namespace Sola_Web.Services
{
    public interface IPdfService
    {
        byte[] GeneratePdfFromHtml(string html);
    }
}
