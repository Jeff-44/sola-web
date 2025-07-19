using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId)
        {
            return await _cartRepository.GetCartItemsAsync(cartId);
        }

        public async Task<CartItem> AddToCartAsync(string cartId, int productId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartId, productId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    DateCreated = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                await _cartRepository.UpdateAsync(cartItem);
            }

            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(string cartId, int productId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartId, productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _cartRepository.UpdateAsync(cartItem);
            }

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
