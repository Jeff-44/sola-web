using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Implementations.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly SolaContext _context;

        public CartRepository(SolaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task<CartItem> GetCartItemAsync(string cartId, int productId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<int> GetCartItemCountAsync(string cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<decimal> GetCartTotalAsync(string cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity * ci.UnitPrice);
        }

        public async Task<CartItem> AddAsync(CartItem entity)
        {
            await _context.CartItems.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(CartItem entity)
        {
            _context.CartItems.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.CartItems.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CartItem>> FindAsync(Expression<Func<CartItem, bool>> predicate)
        {
            return await _context.CartItems.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _context.CartItems.Include(ci => ci.Product).ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task UpdateAsync(CartItem entity)
        {
            _context.CartItems.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
