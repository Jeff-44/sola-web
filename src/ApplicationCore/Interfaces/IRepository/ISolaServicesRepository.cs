using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.IRepository
{
    public interface ISolaServicesRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetAllWithCategoriesAsync();
        Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
    }
}
