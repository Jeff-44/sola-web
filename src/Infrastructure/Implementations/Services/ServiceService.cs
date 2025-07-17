using ApplicationCore.Models;
using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using Sola_Web.ViewModels;

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

        public async Task<Service> CreateServiceAsync(ServiceViewModel createServiceVM)
        {
            var service = new Service
            {
                Name = createServiceVM.Name,
                Description = createServiceVM.Description,
                IconUrl = createServiceVM.IconUrl,
                IsActive = createServiceVM.IsActive,
                ServiceCategoryId = createServiceVM.ServiceCategoryId
            };

            var createdService = await _serviceRepository.AddAsync(service);
            //return MapToDto(updateService);
            return createdService;
        }

        public async Task<Service> UpdateServiceAsync(ServiceViewModel updateServiceVM)
        {
            var updateService = await _serviceRepository.GetByIdAsync(updateServiceVM.Id);
            if (updateService == null)
            {
                throw new KeyNotFoundException($"Service with ID {updateServiceVM.Id} not found.");
            }

            updateService.Name = updateServiceVM.Name;
            updateService.Description = updateServiceVM.Description;
            updateService.IconUrl = updateServiceVM.IconUrl;
            updateService.IsActive = updateServiceVM.IsActive;
            updateService.ServiceCategoryId = updateServiceVM.ServiceCategoryId;

            await _serviceRepository.UpdateAsync(updateService);
            //return MapToDto(updateService);
            return updateService;
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
