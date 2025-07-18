using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<Product> AddProductAsync(Product product)
        {
            return _repository.AddAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existing = await _repository.GetByIdAsync(product.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
            }

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.ImageUrl = product.ImageUrl;

            await _repository.UpdateAsync(existing);
            return existing;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                await _repository.DeleteAsync(product);
            }
        }

        public Task<bool> ProductExistsAsync(int id)
        {
            return _repository.ExistsAsync(id);
        }
    }
}
