using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
    public class Service
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
        public int ServiceCategoryId { get; set; }
        [NotMapped]
        public ServiceCategory ServiceCategory { get; set; } = new ServiceCategory();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        //public string CreatedBy { get; set; } = string.Empty;
        //public string UpdatedBy { get; set; } = string.Empty;
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
