using ApplicationCore.Models;
using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ServiceCategoryService : IServiceCategoryService
    {
        private readonly IServiceCategoryRepository _repository;
        public ServiceCategoryService(IServiceCategoryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public Task<ServiceCategory> AddServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            if (serviceCategory == null)
            {
                throw new ArgumentNullException(nameof(serviceCategory));
            }
            return _repository.AddAsync(serviceCategory);
        }

        public async Task DeleteServiceCategoryAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            }

            var serviceCategory = await _repository.GetByIdAsync(id);
            if (serviceCategory == null)
            {
                throw new KeyNotFoundException($"Service category with ID {id} not found.");
            }

            await _repository.DeleteAsync(serviceCategory);
        }

        public Task<IEnumerable<ServiceCategory>> GetAllServiceCategoriesAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<ServiceCategory?> GetServiceCategoryByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
            }
            return _repository.GetByIdAsync(id);
        }

        public async Task UpdateServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            var result = await _repository.GetByIdAsync(serviceCategory.Id);
            if (result == null)
            {
                throw new Exception("Invalid request.");
            }

            result.Name = serviceCategory.Name;
            result.Description = serviceCategory.Description;
            await _repository.UpdateAsync(result);
        }
    }
}
