using ApplicationCore.Models;
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;

namespace Sola_Web.Tests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productServiceMock = new Mock<IProductService>();
            _cartService = new CartService(_cartRepositoryMock.Object, _productServiceMock.Object);
        }

        [Fact]
        public async Task AddToCart_WithValidProduct_ShouldAddItemToCart()
        {
            // Arrange
            var cartId = "test-cart";
            var productId = 1;
            var quantity = 2;
            var product = new Product { Id = productId, Name = "Test Product", Price = 10.00m, Stock = 5 };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            _cartRepositoryMock.Setup(x => x.GetCartItemAsync(cartId, productId))
                .ReturnsAsync((CartItem)null);

            _cartRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CartItem>()))
                .ReturnsAsync((CartItem cart) => cart);

            // Act
            var result = await _cartService.AddToCartAsync(cartId, productId, quantity);

            // Assert
            result.Should().NotBeNull();
            result.CartId.Should().Be(cartId);
            result.ProductId.Should().Be(productId);
            result.Quantity.Should().Be(quantity);
            result.UnitPrice.Should().Be(product.Price);
        }

        [Fact]
        public async Task AddToCart_WhenProductNotFound_ShouldThrowException()
        {
            // Arrange
            var cartId = "test-cart";
            var productId = 1;
            var quantity = 2;

            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await _cartService.Invoking(x => x.AddToCartAsync(cartId, productId, quantity))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Product with ID {productId} not found.");
        }

        [Fact]
        public async Task AddToCart_WhenInsufficientStock_ShouldThrowException()
        {
            // Arrange
            var cartId = "test-cart";
            var productId = 1;
            var quantity = 5;
            var product = new Product { Id = productId, Name = "Test Product", Price = 10.00m, Stock = 3 };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            // Act & Assert
            await _cartService.Invoking(x => x.AddToCartAsync(cartId, productId, quantity))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Not enough stock. Available: {product.Stock}");
        }

        [Fact]
        public async Task UpdateCartItem_WithValidQuantity_ShouldUpdateItem()
        {
            // Arrange
            var cartId = "test-cart";
            var productId = 1;
            var quantity = 2;
            var product = new Product { Id = productId, Name = "Test Product", Price = 10.00m, Stock = 5 };
            var cartItem = new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 1,
                UnitPrice = product.Price
            };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            _cartRepositoryMock.Setup(x => x.GetCartItemAsync(cartId, productId))
                .ReturnsAsync(cartItem);

            _cartRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CartItem>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _cartService.UpdateCartItemAsync(cartId, productId, quantity);

            // Assert
            result.Should().NotBeNull();
            result.Quantity.Should().Be(quantity);
            result.UnitPrice.Should().Be(product.Price);
        }

        [Fact]
        public async Task GetCartTotal_ShouldCalculateCorrectTotal()
        {
            // Arrange
            var cartId = "test-cart";
            var items = new List<CartItem>
            {
                new CartItem { CartId = cartId, ProductId = 1, Quantity = 2, UnitPrice = 10.00m },
                new CartItem { CartId = cartId, ProductId = 2, Quantity = 1, UnitPrice = 20.00m }
            };

            _cartRepositoryMock.Setup(x => x.GetCartTotalAsync(cartId))
                .ReturnsAsync(40.00m);

            // Act
            var total = await _cartService.GetCartTotalAsync(cartId);

            // Assert
            total.Should().Be(40.00m);
        }
    }
}
