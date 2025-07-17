using ApplicationCore.Models;
using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;


namespace Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ISolaServicesRepository _serviceRepository;

        public ServiceService(ISolaServicesRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllWithCategoriesAsync();
            return services;
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            //return service != null ? MapToDto(service) : null;
            return service != null ? service : null;
        }

        public async Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId)
        {
            var services = await _serviceRepository.GetServicesByCategoryAsync(categoryId);
            //return services.Select(MapToDto);
            return services;
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            var services = await _serviceRepository.GetActiveServicesAsync();
            //return services.Select(MapToDto);
            return services;
        }

        public async Task<Service> CreateServiceAsync(Service createService)
        {
            var createdService = await _serviceRepository.AddAsync(createService);
            return createdService;
        }

        public async Task<Service> UpdateServiceAsync(Service updateService)
        {
            var existingService = await _serviceRepository.GetByIdAsync(updateService.Id);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Service with ID {updateService.Id} not found.");
            }

            existingService.Name = updateService.Name;
            existingService.Description = updateService.Description;
            existingService.IconUrl = updateService.IconUrl;
            existingService.IsActive = updateService.IsActive;
            existingService.ServiceCategoryId = updateService.ServiceCategoryId;

            await _serviceRepository.UpdateAsync(existingService);
            return existingService;
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service != null)
            {
                await _serviceRepository.DeleteAsync(service);
            }
        }

        public async Task<bool> ServiceExistsAsync(int id)
        {
            return await _serviceRepository.ExistsAsync(id);
        }

        //private static ServiceDto MapToDto(Service service)
        //{
        //    return new ServiceDto
        //    {
        //        Id = service.Id,
        //        Name = service.Name,
        //        Description = service.Description,
        //        IconUrl = service.IconUrl,
        //        IsActive = service.IsActive,
        //        ServiceCategoryId = service.ServiceCategoryId,
        //        ServiceCategoryName = service.ServiceCategory?.Name ?? string.Empty
        //    };
        //}
    }
}
