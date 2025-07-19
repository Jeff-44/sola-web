using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;

        public CartService(ICartRepository cartRepository, IProductService productService)
        {
            _cartRepository = cartRepository;
            _productService = productService;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId)
        {
            return await _cartRepository.GetCartItemsAsync(cartId);
        }

        public async Task<CartItem> AddToCartAsync(string cartId, int productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {productId} not found.");

            if (product.Stock < quantity)
                throw new InvalidOperationException($"Not enough stock. Available: {product.Stock}");

            var cartItem = await _cartRepository.GetCartItemAsync(cartId, productId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    DateCreated = DateTime.UtcNow,
                    Product = product
                };
                await _cartRepository.AddAsync(cartItem);
            }
            else
            {
                if (product.Stock < cartItem.Quantity + quantity)
                    throw new InvalidOperationException($"Not enough stock. Available: {product.Stock}");

                cartItem.Quantity += quantity;
                await _cartRepository.UpdateAsync(cartItem);
            }

            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(string cartId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {productId} not found.");

            if (product.Stock < quantity)
                throw new InvalidOperationException($"Not enough stock. Available: {product.Stock}");

            var cartItem = await _cartRepository.GetCartItemAsync(cartId, productId);
            if (cartItem == null)
                throw new InvalidOperationException($"Cart item not found.");

            cartItem.Quantity = quantity;
            cartItem.UnitPrice = product.Price; // Update price in case it changed
            await _cartRepository.UpdateAsync(cartItem);

            return cartItem;
        }

        public async Task RemoveFromCartAsync(string cartId, int productId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartId, productId);
            if (cartItem != null)
            {
                await _cartRepository.DeleteAsync(cartItem);
            }
        }

        public async Task ClearCartAsync(string cartId)
        {
            var cartItems = await _cartRepository.GetCartItemsAsync(cartId);
            foreach (var item in cartItems)
            {
                await _cartRepository.DeleteAsync(item);
            }
        }

        public async Task<decimal> GetCartTotalAsync(string cartId)
        {
            return await _cartRepository.GetCartTotalAsync(cartId);
        }

        public async Task<int> GetCartItemCountAsync(string cartId)
        {
            return await _cartRepository.GetCartItemCountAsync(cartId);
        }
    }
}
