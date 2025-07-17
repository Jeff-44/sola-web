using ApplicationCore.Models;
using ApplicationCore.Interfaces.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SolaServicesRepository : GenericRepository<Service>, ISolaServicesRepository
    {
        public SolaServicesRepository(SolaContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Service>> GetAllWithCategoriesAsync()
        {
            return await _context.Services
                //.Include(s => s.ServiceCategory)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId)
        {
            return await _context.Services
                //.Include(s => s.ServiceCategory)
                .Where(s => s.ServiceCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            return await _context.Services
                //.Include(s => s.ServiceCategory)
                .Where(s => s.IsActive)
                .ToListAsync();
        }
    }
}
