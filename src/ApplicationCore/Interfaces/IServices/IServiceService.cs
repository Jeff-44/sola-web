using ApplicationCore.Models;

namespace ApplicationCore.Interfaces.IServices
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<Service?> GetServiceByIdAsync(int id);
        Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
        Task<Service> CreateServiceAsync(Sola_Web.ViewModels.ServiceViewModel createService);
        Task<Service> UpdateServiceAsync(Sola_Web.ViewModels.ServiceViewModel updateService);
        Task DeleteServiceAsync(int id);
        Task<bool> ServiceExistsAsync(int id);
    }
}
