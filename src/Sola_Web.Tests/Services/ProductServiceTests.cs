using ApplicationCore.Models;
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using ApplicationCore.Interfaces.IRepository;

namespace Sola_Web.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.00m, Stock = 5 },
                new Product { Id = 2, Name = "Product 2", Price = 20.00m, Stock = 3 }
            };

            _productRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetProductById_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Price = 10.00m, Stock = 5 };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductById_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var productId = 1;

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddProduct_ShouldCreateNewProduct()
        {
            // Arrange
            var product = new Product { Name = "New Product", Price = 15.00m, Stock = 10 };

            _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => { p.Id = 1; return p; });

            // Act
            var result = await _productService.AddProductAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Should().BeEquivalentTo(product, opt => opt.Excluding(x => x.Id));
        }

        [Fact]
        public async Task UpdateProduct_WhenProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product", Price = 25.00m, Stock = 8 };

            _productRepositoryMock.Setup(x => x.ExistsAsync(product.Id))
                .ReturnsAsync(true);

            _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProductAsync(product);

            // Assert
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>(p => 
                p.Id == product.Id && 
                p.Name == product.Name && 
                p.Price == product.Price && 
                p.Stock == product.Stock)), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductExists_ShouldDeleteProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product" };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _productRepositoryMock.Verify(x => x.DeleteAsync(It.Is<Product>(p => p.Id == productId)), Times.Once);
        }
    }
}
