using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.IServices
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId);
        Task<CartItem> AddToCartAsync(string cartId, int productId, int quantity);
        Task<CartItem> UpdateCartItemAsync(string cartId, int productId, int quantity);
        Task RemoveFromCartAsync(string cartId, int productId);
        Task ClearCartAsync(string cartId);
        Task<decimal> GetCartTotalAsync(string cartId);
        Task<int> GetCartItemCountAsync(string cartId);
    }
}
