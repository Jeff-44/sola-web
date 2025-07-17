using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.IServices
{
    public interface IServiceCategoryService
    {
        /// <summary>
        /// Retrieves all service categories.
        /// </summary>
        /// <returns>A list of service categories.</returns>
        Task<IEnumerable<ServiceCategory>> GetAllServiceCategoriesAsync();
        /// <summary>
        /// Retrieves a service category by its ID.
        /// </summary>
        /// <param name="id">The ID of the service category.</param>
        /// <returns>The service category with the specified ID, or null if not found.</returns>
        Task<ServiceCategory?> GetServiceCategoryByIdAsync(int id);
        /// <summary>
        /// Adds a new service category.
        /// </summary>
        /// <param name="serviceCategory">The service category to add.</param>
        /// <returns>The added service category.</returns>
        Task<ServiceCategory> AddServiceCategoryAsync(ServiceCategory serviceCategory);
        /// <summary>
        /// Updates an existing service category.
        /// </summary>
        /// <param name="serviceCategory">The service category to update.</param>
        Task UpdateServiceCategoryAsync(ServiceCategory serviceCategory);
        /// <summary>
        /// Deletes a service category by its ID.
        /// </summary>
        /// <param name="id">The ID of the service category to delete.</param>
        Task DeleteServiceCategoryAsync(int id);
    }
}
