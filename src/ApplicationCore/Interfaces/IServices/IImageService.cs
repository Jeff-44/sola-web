using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Interfaces.IServices
{
    public interface IImageService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task DeleteAsync(string filePath);
    }
}
