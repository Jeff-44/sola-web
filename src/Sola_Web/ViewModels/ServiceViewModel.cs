using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sola_Web.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string IconUrl { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int ServiceCategoryId { get; set; }

        public IFormFile? IconImage { get; set; }
    }
}
