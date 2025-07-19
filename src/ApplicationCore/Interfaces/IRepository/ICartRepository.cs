using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.IRepository
{
    public interface ICartRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId);
        Task<CartItem> GetCartItemAsync(string cartId, int productId);
        Task<int> GetCartItemCountAsync(string cartId);
        Task<decimal> GetCartTotalAsync(string cartId);
    }
}
