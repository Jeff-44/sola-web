using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
}
