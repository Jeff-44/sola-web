using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.IServices
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<Service?> GetServiceByIdAsync(int id);
        Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
        Task<Service> CreateServiceAsync(Service createService);
        Task<Service> UpdateServiceAsync(Service updateService);
        Task DeleteServiceAsync(int id);
        Task<bool> ServiceExistsAsync(int id);
    }
}
